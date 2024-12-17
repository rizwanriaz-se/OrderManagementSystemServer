using OrderManagementSystemServer.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using OrderManagementSystemServer.Cache.Models;
using static OrderManagementSystemServer.Cache.Models.Order;
using System.Text.Json;

namespace OrderManagementSystemServer
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
            byte[] buffer = new byte[25600];
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

            return false; // No valid JSON found yet
        }

        private static string ProcessRequest(string request)
        {
            try
            {
                Console.WriteLine($"Processing request: {request}");

                // Deserialize request to object (if needed)
                var requestObject = JsonSerializer.Deserialize<Classes.Request>(request);
                if (requestObject == null)
                {
                    throw new JsonException("Request object is null");
                }
                Console.WriteLine($"Parsed request: {requestObject}");

                Classes.Response responseObject;
                MessageProcessor.ReceiveMessage(requestObject);

                //switch (requestObject.MessageAction)
                //{

                //    case Enums.MessageAction.Add:
                //        responseObject = MessageProcessor.Add(requestObject);
                //        break;
                //    case Enums.MessageAction.Update:
                //        responseObject = MessageProcessor.Update(requestObject);
                //        break;
                //    case Enums.MessageAction.Delete:
                //        responseObject = MessageProcessor.Delete(requestObject);
                //        break;
                //    case Enums.MessageAction.Load:
                //        responseObject = MessageProcessor.Load(requestObject);
                //        break;
                //    default:
                //        throw new InvalidOperationException("Invalid MessageAction");

                //}
                responseObject = new Classes.Response();
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

        //        switch (requestObject.MessageAction)
        //        {
        //            case Enums.MessageAction.Add:
        //                {
        //                    var categoryData = JsonSerializer.Deserialize<Category>(requestObject.Data?.ToString() ?? string.Empty);
        //                    if (categoryData == null)
        //                    {
        //                        throw new JsonException("Category data is null");
        //                    }
        //                    var lastCategoryId = _cacheManager.GetAllCategories().LastOrDefault()?.Id ?? 0;

        //                    _cacheManager.AddCategory(new Category { Id = lastCategoryId + 1, Name = categoryData.Name, Description = categoryData.Description, Picture = categoryData.Picture });
        //                    responseObject = new Classes.Response
        //                    { MessageAction = Enums.MessageAction.Add, MessageType = Enums.MessageType.Category, Data = requestObject.Data };

        //                    break;
        //                }
        //            case Enums.MessageAction.Update:
        //                {
        //                    var categoryData = JsonSerializer.Deserialize<Category>(requestObject.Data?.ToString() ?? string.Empty);
        //                    if (categoryData == null)
        //                    {
        //                        throw new JsonException("Category data is null");
        //                    }
        //                    Category categoryToUpdate = _cacheManager.GetAllCategories().FirstOrDefault(c => c.Id == categoryData.Id);
        //                    if (categoryToUpdate == null)
        //                    {
        //                        throw new JsonException("Category not found");
        //                    }

        //                    categoryToUpdate.Name = categoryData.Name;
        //                    categoryToUpdate.Description = categoryData.Description;
        //                    categoryToUpdate.Picture = categoryData.Picture;
        //                    _cacheManager.UpdateCategory(categoryToUpdate);
        //                    responseObject = new Classes.Response
        //                    { MessageAction = Enums.MessageAction.Update, MessageType = Enums.MessageType.Category, Data = requestObject.Data };
        //                    break;
        //                }
        //            case Enums.MessageAction.Delete:
        //                {
        //                    Category categoryData = JsonSerializer.Deserialize<Category>(requestObject.Data?.ToString());

        //                    //var categoryToDelete = _cacheManager.GetAllCategories().FirstOrDefault(c => c.Id == categoryData.Id);
        //                    //if (categoryToDelete == null)
        //                    //{
        //                    //    throw new JsonException("Category not found");
        //                    //}
        //                    _cacheManager.DeleteCategory(categoryData);
        //                    //_cacheManager.DeleteCategory(categoryToDelete);

        //                    responseObject = new Classes.Response
        //                    { MessageAction = Enums.MessageAction.Delete, MessageType = Enums.MessageType.Category, Data = requestObject.Data };

        //                    break;
        //                }
        //            case Enums.MessageAction.Load:
        //                {
        //                    responseObject = new Classes.Response
        //                    { MessageAction = Enums.MessageAction.Load, MessageType = Enums.MessageType.Category, Data = _cacheManager.GetAllCategories() };

        //                    break;
        //                }
        //            default:
        //                throw new InvalidOperationException("Invalid MessageAction");
        //        }
        //        break;

        //    case Enums.MessageType.Product:
        //        switch (requestObject.MessageAction)
        //        {
        //            case Enums.MessageAction.Add:
        //                {
        //                    var productData = JsonSerializer.Deserialize<Product>(requestObject.Data?.ToString() ?? string.Empty);
        //                    if (productData == null)
        //                    {
        //                        throw new JsonException("Product data is null");
        //                    }
        //                    var lastProductId = _cacheManager.GetAllProducts().LastOrDefault()?.Id ?? 0;

        //                    _cacheManager.AddProduct(new Product
        //                    {
        //                        Id = lastProductId + 1,
        //                        Name = productData.Name,
        //                        UnitPrice = Convert.ToDecimal(productData.UnitPrice),
        //                        UnitsInStock = Convert.ToInt32(productData.UnitsInStock),
        //                        Description = productData.Description,
        //                        Category = productData.Category
        //                    });
        //                    responseObject = new Classes.Response
        //                    { MessageAction = Enums.MessageAction.Add, MessageType = Enums.MessageType.Product, Data = requestObject.Data };

        //                    break;
        //                }
        //            case Enums.MessageAction.Update:
        //                {
        //                    var productData = JsonSerializer.Deserialize<Product>(requestObject.Data?.ToString() ?? string.Empty);
        //                    if (productData == null)
        //                    {
        //                        throw new JsonException("Product data is null");
        //                    }
        //                    Product productToUpdate = _cacheManager.GetAllProducts().FirstOrDefault(p => p.Id == productData.Id);
        //                    if (productToUpdate == null)
        //                    {
        //                        throw new JsonException("Product not found");
        //                    }

        //                    productToUpdate.Name = productData.Name;
        //                    productToUpdate.Description = productData.Description;
        //                    productToUpdate.Picture = productData.Picture;
        //                    productToUpdate.UnitPrice = productData.UnitPrice;
        //                    productToUpdate.UnitsInStock = productData.UnitsInStock;
        //                    productToUpdate.Category = productData.Category;

        //                    _cacheManager.UpdateProduct(productToUpdate);

        //                    responseObject = new Classes.Response
        //                    { MessageAction = Enums.MessageAction.Update, MessageType = Enums.MessageType.Product, Data = requestObject.Data };
        //                    break;
        //                }
        //            case Enums.MessageAction.Delete:
        //                {
        //                    Product productData = JsonSerializer.Deserialize<Product>(requestObject.Data?.ToString());

        //                    //var categoryToDelete = _cacheManager.GetAllCategories().FirstOrDefault(c => c.Id == categoryData.Id);
        //                    //if (categoryToDelete == null)
        //                    //{
        //                    //    throw new JsonException("Category not found");
        //                    //}
        //                    _cacheManager.DeleteProduct(productData);
        //                    //_cacheManager.DeleteCategory(categoryToDelete);

        //                    responseObject = new Classes.Response
        //                    { MessageAction = Enums.MessageAction.Delete, MessageType = Enums.MessageType.Product, Data = requestObject.Data };

        //                    break;
        //                }

        //            case Enums.MessageAction.Load:
        //                {
        //                    responseObject = new Classes.Response
        //                    { MessageAction = Enums.MessageAction.Load, MessageType = Enums.MessageType.Product, Data = _cacheManager.GetAllProducts() };

        //                    break;
        //                }
        //            default:
        //                throw new InvalidOperationException("Invalid MessageAction");
        //        }
        //        break;

        //    case Enums.MessageType.Order:
        //        switch (requestObject.MessageAction)
        //        {
        //            case Enums.MessageAction.Add:
        //                {
        //                    var orderData = JsonSerializer.Deserialize<Order>(requestObject.Data?.ToString() ?? string.Empty);
        //                    if (orderData == null)
        //                    {
        //                        throw new JsonException("Order data is null");
        //                    }
        //                    var lastOrderId = _cacheManager.GetAllOrders().LastOrDefault()?.Id ?? 0;

        //                    _cacheManager.AddOrder(new Order
        //                    {
        //                        Id = lastOrderId + 1,
        //                        User = orderData.User,
        //                        OrderDate = DateTime.Now,
        //                        Status = OrderStatus.Pending,
        //                        OrderDetails = orderData.OrderDetails,
        //                        //OrderDetails = ProductRows.Select(row => new OrderDetail
        //                        //{
        //                        //    Product = GUIHandler.GetInstance().CacheManager.GetProductByName(row),
        //                        //    Quantity = row.Quantity
        //                        //}).ToList(),

        //                        ShippedDate = orderData.ShippedDate,
        //                        ShippingAddress = orderData.ShippingAddress
        //                    });
        //                    responseObject = new Classes.Response
        //                    { MessageAction = Enums.MessageAction.Add, MessageType = Enums.MessageType.Order, Data = requestObject.Data };

        //                    break;
        //                }
        //            case Enums.MessageAction.Update:
        //                {
        //                    var orderData = JsonSerializer.Deserialize<Order>(requestObject.Data?.ToString() ?? string.Empty);
        //                    if (orderData == null)
        //                    {
        //                        throw new JsonException("Order data is null");
        //                    }
        //                    Order orderToUpdate = _cacheManager.GetAllOrders().FirstOrDefault(o => o.Id == orderData.Id);
        //                    if (orderToUpdate == null)
        //                    {
        //                        throw new JsonException("Product not found");
        //                    }

        //                    orderToUpdate.Status = orderData.Status;
        //                    orderToUpdate.User = orderData.User;
        //                    orderToUpdate.ShippedDate = orderData.ShippedDate;
        //                    orderToUpdate.OrderDetails = orderData.OrderDetails;
        //                    orderToUpdate.Id = orderData.Id;
        //                    orderToUpdate.ShippingAddress = orderData.ShippingAddress;

        //                    _cacheManager.UpdateOrder(orderToUpdate);

        //                    responseObject = new Classes.Response
        //                    { MessageAction = Enums.MessageAction.Update, MessageType = Enums.MessageType.Order, Data = requestObject.Data };
        //                    break;
        //                }
        //            case Enums.MessageAction.Delete:
        //                {
        //                    Order orderData = JsonSerializer.Deserialize<Order>(requestObject.Data?.ToString());

        //                    //var categoryToDelete = _cacheManager.GetAllCategories().FirstOrDefault(c => c.Id == categoryData.Id);
        //                    //if (categoryToDelete == null)
        //                    //{
        //                    //    throw new JsonException("Category not found");
        //                    //}
        //                    _cacheManager.DeleteOrder(orderData);
        //                    //_cacheManager.DeleteCategory(categoryToDelete);

        //                    responseObject = new Classes.Response
        //                    { MessageAction = Enums.MessageAction.Delete, MessageType = Enums.MessageType.Order, Data = requestObject.Data };

        //                    break;
        //                }

        //            case Enums.MessageAction.Load:
        //                {
        //                    responseObject = new Classes.Response
        //                    { MessageAction = Enums.MessageAction.Load, MessageType = Enums.MessageType.Order, Data = _cacheManager.GetAllOrders() };

        //                    break;
        //                }
        //            default:
        //                throw new InvalidOperationException("Invalid MessageAction");
        //        }
        //        break;

        //    case Enums.MessageType.User:
        //        switch (requestObject.MessageAction)
        //        {
        //            case Enums.MessageAction.Add:
        //                {
        //                    var userData = JsonSerializer.Deserialize<User>(requestObject.Data?.ToString() ?? string.Empty);
        //                    if (userData == null)
        //                    {
        //                        throw new JsonException("User data is null");
        //                    }
        //                    var lastUserId = _cacheManager.GetAllUsers().LastOrDefault()?.Id ?? 0;

        //                    _cacheManager.AddUser(new User
        //                    {
        //                        Id = lastUserId + 1,
        //                        Name = userData.Name,
        //                        Email = userData.Email,
        //                        Phone = userData.Phone,
        //                        Password = userData.Password,
        //                        IsAdmin = userData.IsAdmin

        //                    });
        //                    responseObject = new Classes.Response
        //                    { MessageAction = Enums.MessageAction.Add, MessageType = Enums.MessageType.User, Data = requestObject.Data };

        //                    break;
        //                }
        //            case Enums.MessageAction.Update:
        //                {
        //                    var userData = JsonSerializer.Deserialize<User>(requestObject.Data?.ToString() ?? string.Empty);
        //                    if (userData == null)
        //                    {
        //                        throw new JsonException("User data is null");
        //                    }
        //                    User userToUpdate = _cacheManager.GetAllUsers().FirstOrDefault(u => u.Id == userData.Id);
        //                    if (userToUpdate == null)
        //                    {
        //                        throw new JsonException("User not found");
        //                    }
        //                    userToUpdate.Name = userData.Name;
        //                    userToUpdate.Email = userData.Email;
        //                    userToUpdate.Phone = userData.Phone;
        //                    userToUpdate.Password = userData.Password;
        //                    userToUpdate.IsAdmin = userData.IsAdmin;
        //                    //userToUpdate.ShippingAddress = userData.ShippingAddress;

        //                    _cacheManager.UpdateUser(userToUpdate);

        //                    responseObject = new Classes.Response
        //                    { MessageAction = Enums.MessageAction.Update, MessageType = Enums.MessageType.User, Data = requestObject.Data };
        //                    break;
        //                }
        //            case Enums.MessageAction.Delete:
        //                {
        //                    User userData = JsonSerializer.Deserialize<User>(requestObject.Data?.ToString());

        //                    //var categoryToDelete = _cacheManager.GetAllCategories().FirstOrDefault(c => c.Id == categoryData.Id);
        //                    //if (categoryToDelete == null)
        //                    //{
        //                    //    throw new JsonException("Category not found");
        //                    //}
        //                    _cacheManager.DeleteUser(userData);
        //                    //_cacheManager.DeleteCategory(categoryToDelete);

        //                    responseObject = new Classes.Response
        //                    { MessageAction = Enums.MessageAction.Delete, MessageType = Enums.MessageType.User, Data = requestObject.Data };

        //                    break;
        //                }

        //            case Enums.MessageAction.Load:
        //                {
        //                    responseObject = new Classes.Response
        //                    { MessageAction = Enums.MessageAction.Load, MessageType = Enums.MessageType.User, Data = _cacheManager.GetAllUsers() };

        //                    break;
        //                }
        //            default:
        //                throw new InvalidOperationException("Invalid MessageAction");
        //        }
        //        break;

        //    default:
        //        throw new InvalidOperationException("Invalid MessageType");
        //}

        //// Create a response object
        //return JsonSerializer.Serialize(responseObject);
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

    }
}
