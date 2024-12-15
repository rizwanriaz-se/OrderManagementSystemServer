// See https://aka.ms/new-console-template for more information


using OrderManagementSystemServer.Cache;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Text.Json;
using OrderManagementSystemServer.Cache.Models;
using OrderManagementSystemServer;
using System.Diagnostics;

namespace OrderManagementSystem.Server
{
    //public class Program
    //{

    //    private static CacheManager m_CacheManager;

    //    public static async Task Main(string[] args)
    //    {

    //        m_CacheManager = CacheManager.Instance;
    //        await StartServer();

    //    }

    //    private static async Task StartServer()
    //    {
    //        TcpListener listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 4444);
    //        listener.Start();

    //        Console.WriteLine($"Server has started on {listener.LocalEndpoint}");

    //        while (true)
    //        {
    //            TcpClient client = await listener.AcceptTcpClientAsync();
    //            _ = Task.Run(() => HandleClientAsync(client));
    //        }
    //    }

    //    private static async Task HandleClientAsync(TcpClient tcpClient)
    //    {
    //        string data = null;
    //        byte[] bytes = new byte[2048];
    //        NetworkStream stream = tcpClient.GetStream();

    //        try
    //        {
    //            while (true)
    //            {
    //                int i = await stream.ReadAsync(bytes, 0, bytes.Length);
    //                if (i > 0)
    //                {
    //                    data = Encoding.ASCII.GetString(bytes, 0, i);
    //                    Console.WriteLine($"Received on server: {data}");

    //                    if (data == "PING")
    //                    {
    //                        byte[] pongMessage = Encoding.ASCII.GetBytes("PONG");
    //                        await stream.WriteAsync(pongMessage, 0, pongMessage.Length);
    //                        Console.WriteLine("Sent by server: PONG");
    //                    }
    //                    else
    //                    {
    //                        byte[] msg = Encoding.ASCII.GetBytes(data);
    //                        await stream.WriteAsync(msg, 0, msg.Length);
    //                        Console.WriteLine($"Sent by server: {data}");
    //                    }
    //                }
    //                else
    //                {
    //                    Console.WriteLine("Client disconnected.");
    //                    break;
    //                }
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            Console.WriteLine($"Client error: {ex.Message}");
    //        }
    //    }

    //}
    //public class Request
    //{
    //    public string Type { get; set; }
    //    public string Data { get; set; }
    //}
    //public class Response
    //{
    //    public string Type
    //    {
    //        get; set;
    //    }
    //    public string Data
    //    {
    //        get; set;
    //    }
    //}
    public class Program
    {
        private static CacheManager _cacheManager;

        public static async Task Main(string[] args)
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
            TcpListener listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 4444);
            listener.Start();
            Console.WriteLine("Server listening on port 4444...");

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
            byte[] buffer = new byte[2048];

            while (true)
            {
                try
                {
                    int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                    if (bytesRead == 0) break;

                    string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    Console.WriteLine($"Received on server: {message}");
                    //ProcessRequest(message);
                    string response = ProcessRequest(message);
                    byte[] responseBytes = Encoding.UTF8.GetBytes(response);
                    await stream.WriteAsync(responseBytes, 0, responseBytes.Length);
                    Console.WriteLine($"Sent: {response}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Client handling error: {ex.Message}");
                    break;
                }
            }
        }
        //private static async Task HandleClient(TcpClient client)
        //{
        //    using var stream = client.GetStream();
        //    byte[] buffer = new byte[2048];

        //    while (true)
        //    {
        //        try
        //        {
        //            int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
        //            if (bytesRead == 0) break;

        //            string message = Encoding.ASCII.GetString(buffer, 0, bytesRead);
        //            Console.WriteLine($"Received on server: {message}");
        //            //ProcessRequest(message);
        //            string response = ProcessRequest(message);
        //            byte[] responseBytes = Encoding.ASCII.GetBytes(response);
        //            await stream.WriteAsync(responseBytes, 0, responseBytes.Length);
        //            Console.WriteLine($"Sent: {response}");
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine($"Client handling error: {ex.Message}");
        //            break;
        //        }
        //    }
        //}

        //private static async Task HandleClient(TcpClient client)
        //{
        //    using var stream = client.GetStream();
        //    byte[] buffer = new byte[2048];
        //    var messageBuilder = new StringBuilder();

        //    while (true)
        //    {
        //        try
        //        {
        //            int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
        //            if (bytesRead == 0) break; // Client disconnected

        //            string messageChunk = Encoding.UTF8.GetString(buffer, 0, bytesRead);
        //            messageBuilder.Append(messageChunk);

        //            // Check if the message ends with a delimiter (e.g., newline or a custom character)
        //            if (messageChunk.EndsWith("\n"))
        //            {
        //                string completeMessage = messageBuilder.ToString().Trim();
        //                messageBuilder.Clear();

        //                Console.WriteLine($"Received on server: {completeMessage}");

        //                // Process the complete message
        //                string response = ProcessRequest(completeMessage);
        //                byte[] responseBytes = Encoding.UTF8.GetBytes(response + "\n"); // Add delimiter for client-side handling
        //                await stream.WriteAsync(responseBytes, 0, responseBytes.Length);

        //                Console.WriteLine($"Sent: {response}");
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine($"Client handling error: {ex.Message}");
        //            break;
        //        }
        //    }
        //}

        private static string ProcessRequest(string request)
        {
            try
            {
                Console.WriteLine($"Processing request: {request}");

                // Deserialize request to object (if needed)
                var requestObject = JsonSerializer.Deserialize<Classes.Request>(request);
                Console.WriteLine($"Parsed request: {requestObject}");

                Category categoryData = JsonSerializer.Deserialize<Category>(requestObject.Data.ToString());
                // Example action (for debugging)
                int? lastCategoryId = _cacheManager.GetAllCategories().Last().Id;
                _cacheManager.AddCategory(new Category { Id=lastCategoryId+1, Name = categoryData.Name, Description = categoryData.Description, Picture=categoryData.Picture });

                // Create a response object
                var responseObject = new Classes.Response { MessageAction = Enums.MessageAction.Add, MessageType = Enums.MessageType.Category, Data = requestObject.Data };

                // Serialize response to JSON
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

        //private static string ProcessRequest(string request)
        //{
        //    _cacheManager.AddCategory(new Category { Name = "Category 1", Description = "Category 1 Description" });
        //    string requestToJson = JsonSerializer.Serialize(request);
        //    Console.WriteLine(requestToJson);
        //    return "Test";
        //    //string result = JsonSerializer.Serialize(request);
        //    //return result;


        //    try
        //    {
        //        // Split concatenated JSON objects
        //        var messages = SplitJsonObjects(request);

        //        foreach (var messageJson in messages)
        //        {
        //            var message = JsonSerializer.Deserialize<Classes.Request>(messageJson);



        //            switch (message.MessageAction)
        //            {
        //                case Enums.MessageAction.HeartBeat:
        //                    if (message.Data == "PING")
        //                    {
        //                        return JsonSerializer.Serialize(new Classes.Response
        //                        {
        //                            MessageAction = Enums.MessageAction.HeartBeat,
        //                            MessageType = Enums.MessageType.HeartBeat,
        //                            Data = "PONG"
        //                        });
        //                    }
        //                    break;

        //                //case Enums.MessageAction.Login:
        //                //    var loginData = JsonSerializer.Deserialize<User>(message.Data);

        //                //    var user = _cacheManager.GetAllUsers()
        //                //        .FirstOrDefault(u => u.Email == loginData.Email && u.Password == loginData.Password);

        //                //    if (user != null)
        //                //    {
        //                //        return JsonSerializer.Serialize(new Response
        //                //        {
        //                //            MessageType = Enums.MessageType.LoginResponse,
        //                //            MessageAction = Enums.MessageAction.LoginResponse,
        //                //            Data = JsonSerializer.Serialize(user)
        //                //        });
        //                //    }

        //                //    return JsonSerializer.Serialize(new Response
        //                //    {
        //                //        MessageType = Enums.MessageType.Error,
        //                //        MessageAction = Enums.MessageAction.Error,
        //                //        Data = "Invalid credentials"
        //                //    });

        //                //default:
        //                //    return JsonSerializer.Serialize(new Classes.Response
        //                //    {
        //                //        MessageType = Enums.MessageType.Error,
        //                //        MessageAction = Enums.MessageAction.Error,
        //                //        Data = "Unknown request type"
        //                //    });
        //            }
        //        }

        //        return JsonSerializer.Serialize(new Classes.Response
        //        {
        //            MessageType = Enums.MessageType.Error,
        //            MessageAction = Enums.MessageAction.Error,
        //            Data = "No valid message found"
        //        });
        //    }
        //    catch (JsonException ex)
        //    {
        //        Console.WriteLine($"JSON Parsing Error: {ex.Message}");
        //        return JsonSerializer.Serialize(new Classes.Response
        //        {
        //            MessageType = Enums.MessageType.Error,
        //            MessageAction = Enums.MessageAction.Error,
        //            Data = "Malformed request"
        //        });
        //    }
        //}

        // Helper method to split concatenated JSON objects
        private static IEnumerable<string> SplitJsonObjects(string concatenatedJson)
        {
            var jsonObjects = new List<string>();
            var depth = 0;
            var startIndex = 0;

            for (int i = 0; i < concatenatedJson.Length; i++)
            {
                if (concatenatedJson[i] == '{') depth++;
                else if (concatenatedJson[i] == '}') depth--;

                if (depth == 0 && i > startIndex)
                {
                    jsonObjects.Add(concatenatedJson.Substring(startIndex, i - startIndex + 1));
                    startIndex = i + 1;
                }
            }

            return jsonObjects;
        }


        //private static string ProcessRequest(string request)
        //{

        //    try
        //    {
        //        //Request message;
        //        //if (request == "PING") return "PONG";
        //        //else { message = JsonSerializer.Deserialize<Request>(request); }

        //        var message = JsonSerializer.Deserialize<Request>(request);
        //        switch (message.Type)
        //        {
        //            case "LOGIN":
        //                var loginData = JsonSerializer.Deserialize<User>(message.Data);

        //                var user = _cacheManager.GetAllUsers()
        //                    .FirstOrDefault(u => u.Email == loginData.Email && u.Password == loginData.Password);

        //                if (user != null)
        //                {
        //                    return JsonSerializer.Serialize(new Response
        //                    {
        //                        Type = "LOGIN_RESPONSE",
        //                        Data = JsonSerializer.Serialize(user)
        //                    });
        //                }

        //                return JsonSerializer.Serialize(new Response
        //                {
        //                    Type = "ERROR",
        //                    Data = "Invalid credentials"
        //                });

        //            case "PING":
        //                return JsonSerializer.Serialize(new Classes.Response
        //                {
        //                    MessageAction = Enums.MessageAction.HeartBeat,
        //                    MessageType = Enums.MessageType.HeartBeat,
        //                    Data = "PONG"
        //                });

        //            default:
        //                return JsonSerializer.Serialize(new Response
        //                {
        //                    Type = "ERROR",
        //                    Data = "Unknown request type"
        //                });
        //        }
        //    }
        //    catch (JsonException ex)
        //    {
        //        Console.WriteLine($"JSON Parsing Error: {ex.Message}");
        //        return JsonSerializer.Serialize(new Response
        //        {
        //            Type = "ERROR",
        //            Data = "Malformed request"
        //        });
        //    }
        //}



    }

}