using OrderManagementSystemServer.Cache;
using OrderManagementSystemServer.Repository;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace OrderManagementSystemServer.Components.Classes
{
    public enum MessageType
    {
        Category,
        Order,
        Product,
        User,
        Heartbeat,
        Error
    }
    public enum MessageAction
    {
        Add,
        Update,
        Delete,
        Load,
        Ping,
        Error
    }
    public class Request
    {
        public MessageType MessageType { get; set; }
        public MessageAction MessageAction { get; set; }
        public object Data { get; set; }
    }

    public class Response
    {
        public MessageType MessageType { get; set; }
        public MessageAction MessageAction { get; set; }
        public object? Data { get; set; }
        public string? Error { get; set; }
    }

    public class ServerManager
    {
        private CacheManager? m_objCacheManager;
        private TcpListener? m_objListener;
        private CancellationTokenSource? m_objCts;
        private List<TcpClient> m_objClients = new List<TcpClient>();
        private readonly object m_objLock = new object();
        private CancellationToken m_objToken;

        public CacheManager CacheManager
        {
            get { return m_objCacheManager; }
            set { m_objCacheManager = value; }
        }

        public async Task Init()
        {
            CacheManager = CacheManager.Instance;

            AppDomain.CurrentDomain.ProcessExit += OnProcessExit;
            Console.CancelKeyPress += OnCancelKeyPress;

            Console.WriteLine("Server starting...");
            await StartServer();
        }

        private void OnProcessExit(object? sender, EventArgs e)
        {
            try
            {
                CacheManager.SaveData(false);
                StopServer();
                Console.WriteLine("Cache data saved successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving cache data: {ex.Message}");
            }

        }

        private void OnCancelKeyPress(object? sender, ConsoleCancelEventArgs e)
        {
            try
            {
                CacheManager.SaveData(false);
                StopServer();
                Console.WriteLine("Cache data saved successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving cache data: {ex.Message}");
            }

        }

        private async Task StartServer()
        {
            m_objCts = new CancellationTokenSource();
            m_objToken = m_objCts.Token;

            m_objListener = new TcpListener(IPAddress.Parse(Constants.IPAddress), Constants.Port);
            m_objListener.Start();
            Console.WriteLine($"Server listening on port {Constants.Port}.");

            TcpClient? client = default(TcpClient);
            while (!m_objToken.IsCancellationRequested)
            {
                try
                {
                    client = await m_objListener.AcceptTcpClientAsync();
                    Console.WriteLine($"Client {client.Client.RemoteEndPoint} connected.");
                    lock (m_objLock)
                    {
                        m_objClients.Add(client);
                        Console.WriteLine($"Number of clients after adding: {m_objClients.Count}");
                    }
                    _ = HandleClient(client);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Listener error: {ex.Message}");
                }
            }
        }

        private async Task HandleClient(TcpClient client)
        {
            using (client)
            {
                try
                {
                    NetworkStream clientStream = client.GetStream();
                    byte[] buffer = new byte[Constants.BufferSize];
                    string messageBuffer = string.Empty;

                    while (client.Connected)
                    {

                        int bytesRead = await clientStream.ReadAsync(buffer, 0, buffer.Length);
                        if (bytesRead == 0) { client.Close(); break; }

                        messageBuffer += Encoding.UTF8.GetString(buffer, 0, bytesRead);


                        while (TryExtractJson(ref messageBuffer, out string jsonMessage))
                        {
                            Console.WriteLine($"Received from {client.Client.RemoteEndPoint}: {jsonMessage}");
                            ProcessRequest(jsonMessage, clientStream);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Client handling error: {ex.Message}");
                }
                finally
                {
                    lock (m_objLock) m_objClients.Remove(client);
                    Console.WriteLine($"Number of clients after removing: {m_objClients.Count}");
                    Console.WriteLine("Client disconnected.");
                }
            }
        }

        private async void SendResponse(Response resp, NetworkStream networkStream)
        {
            string jsonResponse = SerializeJson(resp);
            if (resp.MessageType == MessageType.Error || resp.MessageType == MessageType.Heartbeat || resp.MessageAction == MessageAction.Load)
            {
                byte[] responseBytes = Encoding.UTF8.GetBytes(jsonResponse);
                await networkStream.WriteAsync(responseBytes, 0, responseBytes.Length);
                Console.WriteLine($"Sent: {jsonResponse}");
            }
            else
            {
                lock (m_objLock)
                {
                    foreach (TcpClient client in m_objClients)
                    {
                        try
                        {
                            if (client.Connected)
                            {
                                byte[] responseBytes = Encoding.UTF8.GetBytes(jsonResponse);
                                client.GetStream().WriteAsync(responseBytes, 0, responseBytes.Length);
                                Console.WriteLine($"Sent to {client.Client.RemoteEndPoint}: {jsonResponse}\n");
                            }
                            else
                            {
                                m_objClients.Remove(client);
                            }
                        }
                        catch
                        {
                            m_objClients.Remove(client);
                        }

                    }
                }
            }
        }

        public void StopServer()
        {
            m_objCts.Cancel();
            m_objListener.Stop();
            lock (m_objLock)
            {
                foreach (TcpClient client in m_objClients)
                {
                    client.Close();
                }
                m_objClients.Clear();
            }
            Console.WriteLine("Server stopped.");
        }


        private bool TryExtractJson(ref string buffer, out string json)
        {
            json = null;

            try
            {
                int openBraces = 0, closeBraces = 0;

                for (int i = 0; i < buffer.Length; i++)
                {
                    if (buffer[i] == '{') openBraces++;
                    if (buffer[i] == '}') closeBraces++;

                    // When we find matching braces
                    if (openBraces > 0 && openBraces == closeBraces)
                    {
                        json = buffer.Substring(0, i + 1);
                        buffer = buffer.Substring(i + 1);
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error extracting JSON: {ex.Message}");
            }

            return false;
        }

        //private static void ProcessRequest(string request, NetworkStream networkStream)
        //{
        //    Response responseObject = null;
        //    try
        //    {
        //        Request requestObject = DeserializeJson<Request>(request);
        //        if (requestObject == null)
        //        {
        //            throw new JsonException("Request object is null");
        //        }

        //        switch (requestObject.MessageType)
        //        {
        //            case MessageType.Category:
        //                responseObject = MessageProcessor.ProcessCategoryMessage(requestObject);
        //                break;
        //            case MessageType.Order:
        //                responseObject = MessageProcessor.ProcessOrderMessage(requestObject);
        //                break;
        //            case MessageType.Product:
        //                responseObject = MessageProcessor.ProcessProductMessage(requestObject);
        //                break;
        //            case MessageType.User:
        //                responseObject = MessageProcessor.ProcessUserMessage(requestObject);
        //                break;
        //            case MessageType.Error:
        //                break;
        //            case MessageType.Heartbeat:
        //                responseObject = MessageProcessor.ProcessHeartbeatMessage(requestObject);
        //                break;
        //            default:
        //                break;
        //        }
        //        SendResponse(responseObject, networkStream);
        //    }
        //    catch (JsonException ex)
        //    {
        //        Console.WriteLine($"JSON Error: {ex.Message}");
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Error: {ex.Message}");
        //    }
        //}


        public static string? SerializeJson(Response response)
        {
            try
            {
                if (response != null)
                    return JsonSerializer.Serialize(response);
                return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Unexpected error occurred when trying to serialize data.");
                return null;
            }
        }
        public static T? DeserializeJson<T>(object requestData)
        {
            try
            {
                string data = requestData.ToString();
                if (requestData != null || string.IsNullOrEmpty(data))
                {
                    return (T)JsonSerializer.Deserialize<T>(data);
                }
                return default;
            }
            catch (Exception)
            {
                Console.WriteLine("Unexpected error occurred when trying to deserialize data.");
                return default;
            }
        }




        private void ProcessRequest(string request, NetworkStream networkStream)
        {
            Response responseObject = null;
            try
            {
                Request requestObject = DeserializeJson<Request>(request);
                if (requestObject == null)
                {
                    throw new JsonException("Request object is null");
                }

                switch (requestObject.MessageType)
                {
                    case MessageType.Category:
                        responseObject = ProcessCategoryMessage(requestObject);
                        break;
                    case MessageType.Order:
                        responseObject = ProcessOrderMessage(requestObject);
                        break;
                    case MessageType.Product:
                        responseObject = ProcessProductMessage(requestObject);
                        break;
                    case MessageType.User:
                        responseObject = ProcessUserMessage(requestObject);
                        break;
                    case MessageType.Error:
                        break;
                    case MessageType.Heartbeat:
                        responseObject = ProcessHeartbeatMessage(requestObject);
                        break;
                    default:
                        break;
                }
                SendResponse(responseObject, networkStream);
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"JSON Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        public Response ProcessCategoryMessage(Request request)
        {
            try
            {
                switch (request.MessageAction)
                {
                    case MessageAction.Add:
                        {
                            Category categoryData = ServerManager.DeserializeJson<Category>(request.Data);

                            return new Response
                            {
                                MessageAction = MessageAction.Add,
                                MessageType = MessageType.Category,
                                Data = CacheManager.Instance.AddCategory(categoryData),
                                Error = null
                            };
                        }

                    case MessageAction.Update:
                        {
                            Category categoryData = ServerManager.DeserializeJson<Category>(request.Data?.ToString() ?? null);

                            return new Response
                            {
                                MessageAction = MessageAction.Update,
                                MessageType = MessageType.Category,
                                Data = CacheManager.Instance.UpdateCategory(categoryData),
                                Error = null
                            };

                        }

                    case MessageAction.Delete:
                        {
                            string categoryId = request.Data.ToString();

                            return new Response
                            {
                                MessageAction = MessageAction.Delete,
                                MessageType = MessageType.Category,
                                Data = CacheManager.Instance.DeleteCategory(categoryId),
                                Error = null
                            };
                        }

                    case MessageAction.Load:
                        {
                            return new Response
                            {
                                MessageAction = MessageAction.Load,
                                MessageType = MessageType.Category,
                                Data = CacheManager.Instance.Categories,
                                Error = null
                            };

                        }
                    default:
                        return null;
                }
            }
            catch (Exception ex)
            {
                return new Response
                {
                    MessageAction = MessageAction.Error,
                    MessageType = MessageType.Category,
                    Data = null,
                    Error = $"Error trying to {request.MessageAction} {request.MessageType}: {ex.Message}"
                };
            }
        }

        public Response ProcessOrderMessage(Request request)
        {
            try
            {
                switch (request.MessageAction)
                {
                    case MessageAction.Add:
                        {
                            Order orderData = ServerManager.DeserializeJson<Order>(request.Data?.ToString() ?? string.Empty);

                            return new Response
                            {
                                MessageAction = MessageAction.Add,
                                MessageType = MessageType.Order,
                                Data = CacheManager.Instance.AddOrder(orderData),
                                Error = null
                            };
                        }

                    case MessageAction.Update:
                        {
                            Order orderData = ServerManager.DeserializeJson<Order>(request.Data?.ToString() ?? string.Empty);

                            return new Response
                            {
                                MessageAction = MessageAction.Update,
                                MessageType = MessageType.Order,
                                Data = CacheManager.Instance.UpdateOrder(orderData),
                                Error = null
                            };

                        }

                    case MessageAction.Delete:
                        {
                            string orderId = request.Data.ToString();

                            return new Response
                            {
                                MessageAction = MessageAction.Delete,
                                MessageType = MessageType.Order,
                                Data = CacheManager.Instance.DeleteOrder(orderId),
                                Error = null
                            };

                        }

                    case MessageAction.Load:
                        {
                            return new Response
                            {
                                MessageAction = MessageAction.Load,
                                MessageType = MessageType.Order,
                                Data = CacheManager.Instance.Orders,
                                Error = null
                            };
                        }
                    default:
                        return null;
                }
            }
            catch (Exception ex)
            {
                return new Response
                {
                    MessageAction = MessageAction.Error,
                    MessageType = MessageType.Order,
                    Data = null,
                    Error = $"Error trying to {request.MessageAction} {request.MessageType}: {ex.Message}"
                };
            }
        }

        public Response ProcessProductMessage(Request request)
        {
            try
            {
                switch (request.MessageAction)
                {
                    case MessageAction.Add:
                        {
                            Product productData = ServerManager.DeserializeJson<Product>(request.Data?.ToString() ?? string.Empty);

                            return new Response
                            {
                                MessageAction = MessageAction.Add,
                                MessageType = MessageType.Product,
                                Data = CacheManager.Instance.AddProduct(productData),
                            };
                        }

                    case MessageAction.Update:
                        {
                            Product productData = ServerManager.DeserializeJson<Product>(request.Data);

                            return new Response
                            {
                                MessageAction = MessageAction.Update,
                                MessageType = MessageType.Product,
                                Data = CacheManager.Instance.UpdateProduct(productData),
                            };
                        }

                    case MessageAction.Delete:
                        {
                            string productId = request.Data.ToString();

                            return new Response
                            {
                                MessageAction = MessageAction.Delete,
                                MessageType = MessageType.Product,
                                Data = CacheManager.Instance.DeleteProduct(productId),
                            };
                        }

                    case MessageAction.Load:
                        {
                            return new Response
                            {
                                MessageAction = MessageAction.Load,
                                MessageType = MessageType.Product,
                                Data = CacheManager.Instance.Products,
                            };
                        }
                    default:
                        return null;
                }
            }
            catch (Exception ex)
            {
                return new Response
                {
                    MessageAction = MessageAction.Error,
                    MessageType = MessageType.Product,
                    Data = null,
                    Error = $"Error trying to {request.MessageAction} {request.MessageType}: {ex.Message}"
                };
            }
        }

        public Response ProcessUserMessage(Request request)
        {
            try
            {
                switch (request.MessageAction)
                {
                    case MessageAction.Add:
                        {
                            User userData = ServerManager.DeserializeJson<User>(request.Data);

                            return new Response
                            {
                                MessageAction = MessageAction.Add,
                                MessageType = MessageType.User,
                                Data = CacheManager.Instance.AddUser(userData),
                                Error = null
                            };
                        }

                    case MessageAction.Update:
                        {
                            User userData = ServerManager.DeserializeJson<User>(request.Data);

                            return new Response
                            {
                                MessageAction = MessageAction.Update,
                                MessageType = MessageType.User,
                                Data = CacheManager.Instance.UpdateUser(userData),
                                Error = null
                            };
                        }

                    case MessageAction.Delete:
                        {
                            string userId = request.Data.ToString();

                            return new Response
                            {
                                MessageAction = MessageAction.Delete,
                                MessageType = MessageType.User,
                                Data = CacheManager.Instance.DeleteUser(userId),
                                Error = null
                            };
                        }

                    case MessageAction.Load:
                        {
                            return new Response
                            {
                                MessageAction = MessageAction.Load,
                                MessageType = MessageType.User,
                                Data = CacheManager.Instance.Users,
                                Error = null
                            };
                        }
                    default:
                        return null;
                }
            }
            catch (Exception ex)
            {
                return new Response
                {
                    MessageAction = MessageAction.Error,
                    MessageType = MessageType.User,
                    Data = null,
                    Error = $"Error trying to {request.MessageAction} {request.MessageType}: {ex.Message}"
                };
            }

        }

        public Response ProcessHeartbeatMessage(Request request)
        {
            try
            {
                switch (request.MessageAction)
                {
                    case MessageAction.Ping:
                        {
                            return new Response
                            {
                                MessageAction = MessageAction.Ping,
                                MessageType = MessageType.Heartbeat,
                                Data = "PONG",
                                Error = null
                            };
                        }
                    default:
                        return null;
                }
            }
            catch (Exception ex)
            {
                return new Response
                {
                    MessageAction = MessageAction.Ping,
                    MessageType = MessageType.Heartbeat,
                    Data = null,
                    Error = $"Error trying to send heartbeat response: {ex.Message}"
                };
            }

        }
    }
}
