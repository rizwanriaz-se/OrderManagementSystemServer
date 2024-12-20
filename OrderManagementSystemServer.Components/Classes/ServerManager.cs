//using OrderManagementSystemServer.Cache;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Text.Json;
using OrderManagementSystemServer.Cache;

namespace OrderManagementSystemServer.Components.Classes
{
    public class ServerManager
    {
        private static CacheManager _cacheManager;

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
                Console.WriteLine("Cache data saved successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving cache data: {ex.Message}");
            }

        }

        private static async Task StartServer()
        {
            TcpListener listener = new TcpListener(IPAddress.Parse(Constants.IPAddress), Constants.Port);
            listener.Start();
            Console.WriteLine($"Server listening on port {Constants.Port}.");

            while (true)
            {
                TcpClient client = await listener.AcceptTcpClientAsync();
                Console.WriteLine("Client connected.");
                _ = Task.Run(() => HandleClient(client));
            }
        }

        private static async Task HandleClient(TcpClient client)
        {
            using var stream = client.GetStream();
            byte[] buffer = new byte[Constants.BufferSize];
            string messageBuffer = string.Empty;

            while (true)
            {
                try
                {
                    int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                    if (bytesRead == 0) break;

                    // Append newly read data to the buffer
                    messageBuffer += Encoding.UTF8.GetString(buffer, 0, bytesRead);

                    // Process the message buffer
                    while (TryExtractJson(ref messageBuffer, out string jsonMessage))
                    {
                        Console.WriteLine($"Received JSON: {jsonMessage}");
                        string response = ProcessRequest(jsonMessage);
                        byte[] responseBytes = Encoding.UTF8.GetBytes(response);
                        await stream.WriteAsync(responseBytes, 0, responseBytes.Length);
                        Console.WriteLine($"Sent: {response}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Client handling error: {ex.Message}");
                    break;
                }
            }
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

        private static string ProcessRequest(string request)
        {
            try
            {
                Console.WriteLine($"Processing request: {request}");

                Request requestObject = JsonSerializer.Deserialize<Request>(request);
                if (requestObject == null)
                {
                    throw new JsonException("Request object is null");
                }
                Console.WriteLine($"Parsed request: {requestObject}");

                Response responseObject = null;

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

                return JsonSerializer.Serialize(responseObject);
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"JSON Error: {ex.Message}");
                return JsonSerializer.Serialize(new { Status = "Error", Message = "Invalid JSON" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return JsonSerializer.Serialize(new { Status = "Error", Message = "An error occurred" });
            }
        }

    }
}
