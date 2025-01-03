//using OrderManagementSystemServer.Cache;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Text.Json;
using OrderManagementSystemServer.Cache;
using System.IO;

namespace OrderManagementSystemServer.Components.Classes
{
    public class ServerManager
    {
        private static CacheManager _cacheManager;
        private static TcpListener listener;
        private static CancellationTokenSource cts;
        private static List<TcpClient> clients = new List<TcpClient>();
        private static readonly object _lock = new object();
        private static CancellationToken token;
        //private static NetworkStream stream;

        public async Task Init()
        {
            _cacheManager = CacheManager.Instance;

            AppDomain.CurrentDomain.ProcessExit += OnProcessExit;
            Console.CancelKeyPress += OnCancelKeyPress;

            Console.WriteLine("Server starting...");
            await StartServer();
        }

        private static void OnProcessExit(object? sender, EventArgs e)
        {
            try
            {
                _cacheManager.SaveData(false);
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
                _cacheManager.SaveData(false);
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
            cts = new CancellationTokenSource();
            token = cts.Token;

            listener = new TcpListener(IPAddress.Parse(Constants.IPAddress), Constants.Port);
            listener.Start();
            Console.WriteLine($"Server listening on port {Constants.Port}.");

            var client = default(TcpClient);
            while (!token.IsCancellationRequested)
            {
                try
                {
                    client = await listener.AcceptTcpClientAsync().ConfigureAwait(false);
                    Console.WriteLine($"Client {client.Client.RemoteEndPoint} connected.");
                    lock (_lock)
                    {
                        clients.Add(client);
                        Console.WriteLine($"Number of clients after adding: {clients.Count}");
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

                        // Append newly read data to the buffer
                        messageBuffer += Encoding.UTF8.GetString(buffer, 0, bytesRead);

                        // Process the message buffer
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
                    lock (_lock) clients.Remove(client);
                    Console.WriteLine($"Number of clients after removing: {clients.Count}");
                    Console.WriteLine("Client disconnected.");
                }
            }
        }

        private static async void SendResponse(Response resp, NetworkStream networkStream)
        {
            string jsonResponse = JsonSerializer.Serialize(resp);
            if (resp.MessageType == Enums.MessageType.Error || resp.MessageType == Enums.MessageType.Heartbeat || resp.MessageAction == Enums.MessageAction.Load)
            {
                byte[] responseBytes = Encoding.UTF8.GetBytes(jsonResponse);
                await networkStream.WriteAsync(responseBytes, 0, responseBytes.Length);
                Console.WriteLine($"Sent: {jsonResponse}");
            }
            else
            {
                //BroadcastClients(jsonResponse);
                lock (_lock)
                {
                    foreach (TcpClient client in clients)
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
                                clients.Remove(client);
                            }
                        }
                        catch
                        {
                            clients.Remove(client);
                        }

                    }
                }
            }

        }

        public static void StopServer()
        {
            cts.Cancel();
            listener.Stop();
            lock (_lock)
            {
                foreach (var client in clients)
                {
                    client.Close();
                }
                clients.Clear();
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
                Request requestObject = JsonSerializer.Deserialize<Request>(request);
                if (requestObject == null)
                {
                    throw new JsonException("Request object is null");
                }

                switch (requestObject.MessageType)
                {
                    case Enums.MessageType.Category:
                        responseObject = MessageProcessor.ProcessCategoryMessage(requestObject);
                        break;
                    case Enums.MessageType.Order:
                        responseObject = MessageProcessor.ProcessOrderMessage(requestObject);
                        break;
                    case Enums.MessageType.Product:
                        responseObject = MessageProcessor.ProcessProductMessage(requestObject);
                        break;
                    case Enums.MessageType.User:
                        responseObject = MessageProcessor.ProcessUserMessage(requestObject);
                        break;
                    case Enums.MessageType.Error:
                        break;
                    case Enums.MessageType.Heartbeat:
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

    }
}
