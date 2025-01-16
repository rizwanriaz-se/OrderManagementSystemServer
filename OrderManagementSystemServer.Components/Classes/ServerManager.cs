using OrderManagementSystemServer.Cache;
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

    public class ServerManager
    {
        private static CacheManager? m_objCacheManager;
        private static TcpListener? m_objListener;
        private static CancellationTokenSource? m_objCts;
        private static List<TcpClient> m_objClients = new List<TcpClient>();
        private static readonly object m_objLock = new object();
        private static CancellationToken m_objToken;

        public static CacheManager CacheManager
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

        private static void OnProcessExit(object? sender, EventArgs e)
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

        private static void OnCancelKeyPress(object? sender, ConsoleCancelEventArgs e)
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

        private static async Task StartServer()
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

        private static async Task HandleClient(TcpClient client)
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
                            //SendResponse(response, clientStream);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Client handling error: {ex.Message}");
                    //break;
                }
                finally
                {
                    lock (m_objLock) m_objClients.Remove(client);
                    Console.WriteLine($"Number of clients after removing: {m_objClients.Count}");
                    Console.WriteLine("Client disconnected.");
                }
            }
        }

        private static async void SendResponse(Response resp, NetworkStream networkStream)
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

        public static void StopServer()
        {
            m_objCts.Cancel();
            m_objListener.Stop();
            lock (m_objLock)
            {
                foreach (var client in m_objClients)
                {
                    client.Close();
                }
                m_objClients.Clear();
            }
            Console.WriteLine("Server stopped.");
        }
        

        private static bool TryExtractJson(ref string buffer, out string json)
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

        private static void ProcessRequest(string request, NetworkStream networkStream)
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
                        responseObject = MessageProcessor.ProcessCategoryMessage(requestObject);
                        break;
                    case MessageType.Order:
                        responseObject = MessageProcessor.ProcessOrderMessage(requestObject);
                        break;
                    case MessageType.Product:
                        responseObject = MessageProcessor.ProcessProductMessage(requestObject);
                        break;
                    case MessageType.User:
                        responseObject = MessageProcessor.ProcessUserMessage(requestObject);
                        break;
                    case MessageType.Error:
                        break;
                    case MessageType.Heartbeat:
                        responseObject = MessageProcessor.ProcessHeartbeatMessage(requestObject);
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
    }
}
