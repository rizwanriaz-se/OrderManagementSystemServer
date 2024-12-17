// See https://aka.ms/new-console-template for more information


using OrderManagementSystemServer.Cache;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Text.Json;
using OrderManagementSystemServer.Cache.Models;
using OrderManagementSystemServer;
using System.Diagnostics;
using static OrderManagementSystemServer.Enums;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static OrderManagementSystemServer.Cache.Models.Order;
using System.Numerics;
using System.Xml.Linq;

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
        //private static CacheManager _cacheManager;
        public static ServerManager serverManager = new ServerManager();

        public static async Task Main(string[] args)
        {
            await serverManager.Init();            
            //_cacheManager = CacheManager.Instance;

            //AppDomain.CurrentDomain.ProcessExit += OnProcessExit;
            //Console.CancelKeyPress += OnCancelKeyPress;

            //Console.WriteLine("Server starting...");
            //await StartServer();

        }

        //private static void OnProcessExit(object? sender, EventArgs e)
        //{
        //    try
        //    {
        //        _cacheManager.SaveData(false);
        //        Console.WriteLine("Cache data saved successfully.");
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Error saving cache data: {ex.Message}");
        //    }

        //}

        //private static void OnCancelKeyPress(object? sender, ConsoleCancelEventArgs e)
        //{
        //    try
        //    {
        //        _cacheManager.SaveData(false);
        //        Console.WriteLine("Cache data saved successfully.");
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Error saving cache data: {ex.Message}");
        //    }

        //}

        //private static async Task StartServer()
        //{
        //    TcpListener listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 4444);
        //    listener.Start();
        //    Console.WriteLine("Server listening on port 4444...");

        //    while (true)
        //    {
        //        TcpClient client = await listener.AcceptTcpClientAsync();
        //        Console.WriteLine("Client connected.");
        //        _ = Task.Run(() => HandleClient(client));
        //    }
        //}

        //private static async Task HandleClient(TcpClient client)
        //{
        //    using var stream = client.GetStream();
        //    byte[] buffer = new byte[25600];
        //    string messageBuffer = string.Empty;

        //    while (true)
        //    {
        //        try
        //        {
        //            int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
        //            if (bytesRead == 0) break;

        //            // Append newly read data to the buffer
        //            messageBuffer += Encoding.UTF8.GetString(buffer, 0, bytesRead);

        //            // Process the message buffer
        //            while (TryExtractJson(ref messageBuffer, out string jsonMessage))
        //            {
        //                Console.WriteLine($"Received JSON: {jsonMessage}");
        //                string response = ProcessRequest(jsonMessage);
        //                byte[] responseBytes = Encoding.UTF8.GetBytes(response);
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

        //private static bool TryExtractJson(ref string buffer, out string json)
        //{
        //    json = null;

        //    try
        //    {
        //        int openBraces = 0, closeBraces = 0;

        //        for (int i = 0; i < buffer.Length; i++)
        //        {
        //            if (buffer[i] == '{') openBraces++;
        //            if (buffer[i] == '}') closeBraces++;

        //            // When we find matching braces
        //            if (openBraces > 0 && openBraces == closeBraces)
        //            {
        //                json = buffer.Substring(0, i + 1);
        //                buffer = buffer.Substring(i + 1);
        //                return true;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Error extracting JSON: {ex.Message}");
        //    }

        //    return false; // No valid JSON found yet
        //}

        //private static async Task HandleClient(TcpClient client)
        //{
        //    using var stream = client.GetStream();
        //    byte[] buffer = new byte[25600];

        //    while (true)
        //    {
        //        try
        //        {
        //            int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
        //            if (bytesRead == 0) break;

        //            string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
        //            Console.WriteLine($"Received on server: {message}");
        //            //ProcessRequest(message);
        //            string response = ProcessRequest(message);
        //            byte[] responseBytes = Encoding.UTF8.GetBytes(response);
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

        //private static string ProcessRequest(string request)
        //{
        //    try
        //    {
        //        Console.WriteLine($"Processing request: {request}");

        //        // Deserialize request to object (if needed)
        //        var requestObject = JsonSerializer.Deserialize<Classes.Request>(request);
        //        if (requestObject == null)
        //        {
        //            throw new JsonException("Request object is null");
        //        }
        //        Console.WriteLine($"Parsed request: {requestObject}");

        //        Classes.Response responseObject;

        //        switch (requestObject.MessageType)
        //        {
        //            //case Enums.MessageType.All:
        //            //    switch (requestObject.MessageAction)
        //            //    {
        //            //        case Enums.MessageAction.Load:
        //            //            {
        //            //                responseObject = new Classes.Response
        //            //                { MessageAction = Enums.MessageAction.Load, MessageType = Enums.MessageType.All, Data = _cacheManager };
        //            //                break;
        //            //            }


        //            //        default:
        //            //            throw new InvalidOperationException("Invalid MessageAction");
        //            //    }
        //            //    break;

        //            case Enums.MessageType.Category:
        //                switch (requestObject.MessageAction)
        //                {
        //                    case Enums.MessageAction.Add:
        //                        {
        //                            var categoryData = JsonSerializer.Deserialize<Category>(requestObject.Data?.ToString() ?? string.Empty);
        //                            if (categoryData == null)
        //                            {
        //                                throw new JsonException("Category data is null");
        //                            }
        //                            var lastCategoryId = _cacheManager.GetAllCategories().LastOrDefault()?.Id ?? 0;

        //                            _cacheManager.AddCategory(new Category { Id = lastCategoryId + 1, Name = categoryData.Name, Description = categoryData.Description, Picture = categoryData.Picture });
        //                            responseObject = new Classes.Response
        //                            { MessageAction = Enums.MessageAction.Add, MessageType = Enums.MessageType.Category, Data = requestObject.Data };

        //                            break;
        //                        }
        //                    case Enums.MessageAction.Update:
        //                        {
        //                            var categoryData = JsonSerializer.Deserialize<Category>(requestObject.Data?.ToString() ?? string.Empty);
        //                            if (categoryData == null)
        //                            {
        //                                throw new JsonException("Category data is null");
        //                            }
        //                            Category categoryToUpdate = _cacheManager.GetAllCategories().FirstOrDefault(c => c.Id == categoryData.Id);
        //                            if (categoryToUpdate == null)
        //                            {
        //                                throw new JsonException("Category not found");
        //                            }

        //                            categoryToUpdate.Name = categoryData.Name;
        //                            categoryToUpdate.Description = categoryData.Description;
        //                            categoryToUpdate.Picture = categoryData.Picture;
        //                            _cacheManager.UpdateCategory(categoryToUpdate);
        //                            responseObject = new Classes.Response
        //                            { MessageAction = Enums.MessageAction.Update, MessageType = Enums.MessageType.Category, Data = requestObject.Data };
        //                            break;
        //                        }
        //                    case Enums.MessageAction.Delete:
        //                        {
        //                            Category categoryData = JsonSerializer.Deserialize<Category>(requestObject.Data?.ToString());

        //                            //var categoryToDelete = _cacheManager.GetAllCategories().FirstOrDefault(c => c.Id == categoryData.Id);
        //                            //if (categoryToDelete == null)
        //                            //{
        //                            //    throw new JsonException("Category not found");
        //                            //}
        //                            _cacheManager.DeleteCategory(categoryData);
        //                            //_cacheManager.DeleteCategory(categoryToDelete);

        //                            responseObject = new Classes.Response
        //                            { MessageAction = Enums.MessageAction.Delete, MessageType = Enums.MessageType.Category, Data = requestObject.Data };

        //                            break;
        //                        }
        //                    case Enums.MessageAction.Load:
        //                        {
        //                            responseObject = new Classes.Response
        //                            { MessageAction = Enums.MessageAction.Load, MessageType = Enums.MessageType.Category, Data = _cacheManager.GetAllCategories() };

        //                            break;
        //                        }
        //                    default:
        //                        throw new InvalidOperationException("Invalid MessageAction");
        //                }
        //                break;

        //            case Enums.MessageType.Product:
        //                switch (requestObject.MessageAction)
        //                {
        //                    case Enums.MessageAction.Add:
        //                        {
        //                            var productData = JsonSerializer.Deserialize<Product>(requestObject.Data?.ToString() ?? string.Empty);
        //                            if (productData == null)
        //                            {
        //                                throw new JsonException("Product data is null");
        //                            }
        //                            var lastProductId = _cacheManager.GetAllProducts().LastOrDefault()?.Id ?? 0;

        //                            _cacheManager.AddProduct(new Product
        //                            {
        //                                Id = lastProductId + 1,
        //                                Name = productData.Name,
        //                                UnitPrice = Convert.ToDecimal(productData.UnitPrice),
        //                                UnitsInStock = Convert.ToInt32(productData.UnitsInStock),
        //                                Description = productData.Description,
        //                                Category = productData.Category
        //                            });
        //                            responseObject = new Classes.Response
        //                            { MessageAction = Enums.MessageAction.Add, MessageType = Enums.MessageType.Product, Data = requestObject.Data };

        //                            break;
        //                        }
        //                    case Enums.MessageAction.Update:
        //                        {
        //                            var productData = JsonSerializer.Deserialize<Product>(requestObject.Data?.ToString() ?? string.Empty);
        //                            if (productData == null)
        //                            {
        //                                throw new JsonException("Product data is null");
        //                            }
        //                            Product productToUpdate = _cacheManager.GetAllProducts().FirstOrDefault(p => p.Id == productData.Id);
        //                            if (productToUpdate == null)
        //                            {
        //                                throw new JsonException("Product not found");
        //                            }

        //                            productToUpdate.Name = productData.Name;
        //                            productToUpdate.Description = productData.Description;
        //                            productToUpdate.Picture = productData.Picture;
        //                            productToUpdate.UnitPrice = productData.UnitPrice;
        //                            productToUpdate.UnitsInStock = productData.UnitsInStock;
        //                            productToUpdate.Category = productData.Category;

        //                            _cacheManager.UpdateProduct(productToUpdate);

        //                            responseObject = new Classes.Response
        //                            { MessageAction = Enums.MessageAction.Update, MessageType = Enums.MessageType.Product, Data = requestObject.Data };
        //                            break;
        //                        }
        //                    case Enums.MessageAction.Delete:
        //                        {
        //                            Product productData = JsonSerializer.Deserialize<Product>(requestObject.Data?.ToString());

        //                            //var categoryToDelete = _cacheManager.GetAllCategories().FirstOrDefault(c => c.Id == categoryData.Id);
        //                            //if (categoryToDelete == null)
        //                            //{
        //                            //    throw new JsonException("Category not found");
        //                            //}
        //                            _cacheManager.DeleteProduct(productData);
        //                            //_cacheManager.DeleteCategory(categoryToDelete);

        //                            responseObject = new Classes.Response
        //                            { MessageAction = Enums.MessageAction.Delete, MessageType = Enums.MessageType.Product, Data = requestObject.Data };

        //                            break;
        //                        }

        //                    case Enums.MessageAction.Load:
        //                        {
        //                            responseObject = new Classes.Response
        //                            { MessageAction = Enums.MessageAction.Load, MessageType = Enums.MessageType.Product, Data = _cacheManager.GetAllProducts() };

        //                            break;
        //                        }
        //                    default:
        //                        throw new InvalidOperationException("Invalid MessageAction");
        //                }
        //                break;

        //            case Enums.MessageType.Order:
        //                switch (requestObject.MessageAction)
        //                {
        //                    case Enums.MessageAction.Add:
        //                        {
        //                            var orderData = JsonSerializer.Deserialize<Order>(requestObject.Data?.ToString() ?? string.Empty);
        //                            if (orderData == null)
        //                            {
        //                                throw new JsonException("Order data is null");
        //                            }
        //                            var lastOrderId = _cacheManager.GetAllOrders().LastOrDefault()?.Id ?? 0;

        //                            _cacheManager.AddOrder(new Order
        //                            {
        //                                Id = lastOrderId + 1,
        //                                User = orderData.User,
        //                                OrderDate = DateTime.Now,
        //                                Status = OrderStatus.Pending,
        //                                OrderDetails = orderData.OrderDetails,
        //                                //OrderDetails = ProductRows.Select(row => new OrderDetail
        //                                //{
        //                                //    Product = GUIHandler.GetInstance().CacheManager.GetProductByName(row),
        //                                //    Quantity = row.Quantity
        //                                //}).ToList(),

        //                                ShippedDate = orderData.ShippedDate,
        //                                ShippingAddress = orderData.ShippingAddress
        //                            });
        //                            responseObject = new Classes.Response
        //                            { MessageAction = Enums.MessageAction.Add, MessageType = Enums.MessageType.Order, Data = requestObject.Data };

        //                            break;
        //                        }
        //                    case Enums.MessageAction.Update:
        //                        {
        //                            var orderData = JsonSerializer.Deserialize<Order>(requestObject.Data?.ToString() ?? string.Empty);
        //                            if (orderData == null)
        //                            {
        //                                throw new JsonException("Order data is null");
        //                            }
        //                            Order orderToUpdate = _cacheManager.GetAllOrders().FirstOrDefault(o => o.Id == orderData.Id);
        //                            if (orderToUpdate == null)
        //                            {
        //                                throw new JsonException("Product not found");
        //                            }

        //                            orderToUpdate.Status = orderData.Status;
        //                            orderToUpdate.User = orderData.User;
        //                            orderToUpdate.ShippedDate = orderData.ShippedDate;
        //                            orderToUpdate.OrderDetails = orderData.OrderDetails;
        //                            orderToUpdate.Id = orderData.Id;
        //                            orderToUpdate.ShippingAddress = orderData.ShippingAddress;

        //                            _cacheManager.UpdateOrder(orderToUpdate);

        //                            responseObject = new Classes.Response
        //                            { MessageAction = Enums.MessageAction.Update, MessageType = Enums.MessageType.Order, Data = requestObject.Data };
        //                            break;
        //                        }
        //                    case Enums.MessageAction.Delete:
        //                        {
        //                            Order orderData = JsonSerializer.Deserialize<Order>(requestObject.Data?.ToString());

        //                            //var categoryToDelete = _cacheManager.GetAllCategories().FirstOrDefault(c => c.Id == categoryData.Id);
        //                            //if (categoryToDelete == null)
        //                            //{
        //                            //    throw new JsonException("Category not found");
        //                            //}
        //                            _cacheManager.DeleteOrder(orderData);
        //                            //_cacheManager.DeleteCategory(categoryToDelete);

        //                            responseObject = new Classes.Response
        //                            { MessageAction = Enums.MessageAction.Delete, MessageType = Enums.MessageType.Order, Data = requestObject.Data };

        //                            break;
        //                        }

        //                    case Enums.MessageAction.Load:
        //                        {
        //                            responseObject = new Classes.Response
        //                            { MessageAction = Enums.MessageAction.Load, MessageType = Enums.MessageType.Order, Data = _cacheManager.GetAllOrders() };

        //                            break;
        //                        }
        //                    default:
        //                        throw new InvalidOperationException("Invalid MessageAction");
        //                }
        //                break;

        //            case Enums.MessageType.User:
        //                switch (requestObject.MessageAction)
        //                {
        //                    case Enums.MessageAction.Add:
        //                        {
        //                            var userData = JsonSerializer.Deserialize<User>(requestObject.Data?.ToString() ?? string.Empty);
        //                            if (userData == null)
        //                            {
        //                                throw new JsonException("User data is null");
        //                            }
        //                            var lastUserId = _cacheManager.GetAllUsers().LastOrDefault()?.Id ?? 0;

        //                            _cacheManager.AddUser(new User
        //                            {
        //                                Id = lastUserId + 1,
        //                                Name = userData.Name,
        //                                Email = userData.Email,
        //                                Phone = userData.Phone,
        //                                Password = userData.Password,
        //                                IsAdmin = userData.IsAdmin

        //                            });
        //                            responseObject = new Classes.Response
        //                            { MessageAction = Enums.MessageAction.Add, MessageType = Enums.MessageType.User, Data = requestObject.Data };

        //                            break;
        //                        }
        //                    case Enums.MessageAction.Update:
        //                        {
        //                            var userData = JsonSerializer.Deserialize<User>(requestObject.Data?.ToString() ?? string.Empty);
        //                            if (userData == null)
        //                            {
        //                                throw new JsonException("User data is null");
        //                            }
        //                            User userToUpdate = _cacheManager.GetAllUsers().FirstOrDefault(u => u.Id == userData.Id);
        //                            if (userToUpdate == null)
        //                            {
        //                                throw new JsonException("User not found");
        //                            }
        //                            userToUpdate.Name = userData.Name;
        //                            userToUpdate.Email = userData.Email;
        //                            userToUpdate.Phone = userData.Phone;
        //                            userToUpdate.Password = userData.Password;
        //                            userToUpdate.IsAdmin = userData.IsAdmin;
        //                            //userToUpdate.ShippingAddress = userData.ShippingAddress;

        //                            _cacheManager.UpdateUser(userToUpdate);

        //                            responseObject = new Classes.Response
        //                            { MessageAction = Enums.MessageAction.Update, MessageType = Enums.MessageType.User, Data = requestObject.Data };
        //                            break;
        //                        }
        //                    case Enums.MessageAction.Delete:
        //                        {
        //                            User userData = JsonSerializer.Deserialize<User>(requestObject.Data?.ToString());

        //                            //var categoryToDelete = _cacheManager.GetAllCategories().FirstOrDefault(c => c.Id == categoryData.Id);
        //                            //if (categoryToDelete == null)
        //                            //{
        //                            //    throw new JsonException("Category not found");
        //                            //}
        //                            _cacheManager.DeleteUser(userData);
        //                            //_cacheManager.DeleteCategory(categoryToDelete);

        //                            responseObject = new Classes.Response
        //                            { MessageAction = Enums.MessageAction.Delete, MessageType = Enums.MessageType.User, Data = requestObject.Data };

        //                            break;
        //                        }

        //                    case Enums.MessageAction.Load:
        //                        {
        //                            responseObject = new Classes.Response
        //                            { MessageAction = Enums.MessageAction.Load, MessageType = Enums.MessageType.User, Data = _cacheManager.GetAllUsers() };

        //                            break;
        //                        }
        //                    default:
        //                        throw new InvalidOperationException("Invalid MessageAction");
        //                }
        //                break;

        //            default:
        //                throw new InvalidOperationException("Invalid MessageType");
        //        }

        //        // Create a response object
        //        return JsonSerializer.Serialize(responseObject);
        //    }
        //    catch (JsonException ex)
        //    {
        //        Console.WriteLine($"JSON Error: {ex.Message}");
        //        return JsonSerializer.Serialize(new { Status = "Error", Message = "Invalid JSON" });
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Error: {ex.Message}");
        //        return JsonSerializer.Serialize(new { Status = "Error", Message = "An error occurred" });
        //    }
        //}

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
        //private static IEnumerable<string> SplitJsonObjects(string concatenatedJson)
        //{
        //    var jsonObjects = new List<string>();
        //    var depth = 0;
        //    var startIndex = 0;

        //    for (int i = 0; i < concatenatedJson.Length; i++)
        //    {
        //        if (concatenatedJson[i] == '{') depth++;
        //        else if (concatenatedJson[i] == '}') depth--;

        //        if (depth == 0 && i > startIndex)
        //        {
        //            jsonObjects.Add(concatenatedJson.Substring(startIndex, i - startIndex + 1));
        //            startIndex = i + 1;
        //        }
        //    }

        //    return jsonObjects;
        //}


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