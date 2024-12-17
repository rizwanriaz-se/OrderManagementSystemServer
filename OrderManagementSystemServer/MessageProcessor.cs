using OrderManagementSystemServer.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using System.Text.Json;
using OrderManagementSystemServer.Cache.Models;
using static OrderManagementSystemServer.Cache.Models.Order;
using System.Net.Sockets;

namespace OrderManagementSystemServer
{
    public class MessageProcessor
    {
        private static CacheManager _cacheManager = CacheManager.Instance;

        public static Classes.Response ReceiveMessage(Classes.Request request)
        {
            switch (request.MessageAction)
            {
                case Enums.MessageAction.Add:
                    return Add(request);
                    
                case Enums.MessageAction.Update:
                    return Update(request);
                    
                case Enums.MessageAction.Delete:
                    return Delete(request);
                    
                case Enums.MessageAction.Load:
                    return Load(request);
                    
                case Enums.MessageAction.Error:
                    return Load(request);
                    
                default:
                    return Load(request);
                    
            }
        }

        public static Classes.Response Add(Classes.Request request)
        {

            switch (request.MessageType)
            {
                case Enums.MessageType.Category:
                    var categoryData = JsonSerializer.Deserialize<Category>(request.Data?.ToString() ?? string.Empty);

                    if (categoryData == null)
                    {
                        throw new JsonException("Category data is null");
                    }

                    _cacheManager.AddCategory(categoryData);
                    
                    return new Classes.Response
                    { MessageAction = Enums.MessageAction.Add, MessageType = Enums.MessageType.Category, Data = request.Data };

                case Enums.MessageType.Order:
                    
                    var orderData = JsonSerializer.Deserialize<Order>(request.Data?.ToString() ?? string.Empty);
                    if (orderData == null)
                    {
                        throw new JsonException("Order data is null");
                    }

                    _cacheManager.AddOrder(orderData);
                    return new Classes.Response
                    { MessageAction = Enums.MessageAction.Add, MessageType = Enums.MessageType.Order, Data = request.Data };

                case Enums.MessageType.Product:
                    
                    var productData = JsonSerializer.Deserialize<Product>(request.Data?.ToString() ?? string.Empty);

                    if (productData == null)
                    {
                        throw new JsonException("Product data is null");
                    }

                    _cacheManager.AddProduct(productData);

                    return new Classes.Response
                    { MessageAction = Enums.MessageAction.Add, MessageType = Enums.MessageType.Product, Data = request.Data };

                case Enums.MessageType.User:
                    var userData = JsonSerializer.Deserialize<User>(request.Data?.ToString() ?? string.Empty);
                    if (userData == null)
                    {
                        throw new JsonException("User data is null");
                    }

                    _cacheManager.AddUser(userData);

                    return new Classes.Response
                    { MessageAction = Enums.MessageAction.Add, MessageType = Enums.MessageType.User, Data = request.Data };
                    
                case Enums.MessageType.Error:
                    return new Classes.Response
                    { MessageAction = Enums.MessageAction.Add, MessageType = Enums.MessageType.User, Data = null };

                default:
                    return new Classes.Response
                    { MessageAction = Enums.MessageAction.Add, MessageType = Enums.MessageType.User, Data = null };
            }
        }

        public static Classes.Response Update(Classes.Request request)
        {
            switch (request.MessageType)
            {
                case Enums.MessageType.Category:
                    var categoryData = JsonSerializer.Deserialize<Category>(request.Data?.ToString() ?? string.Empty);
                    
                    if (categoryData == null)
                    {
                        throw new JsonException("Category data is null");
                    }
                    
                    _cacheManager.UpdateCategory(categoryData);

                    return new Classes.Response
                    { MessageAction = Enums.MessageAction.Update, MessageType = Enums.MessageType.Category, Data = request.Data };

                case Enums.MessageType.Order:
                    var orderData = JsonSerializer.Deserialize<Order>(request.Data?.ToString() ?? string.Empty);
                    if (orderData == null)
                    {
                        throw new JsonException("Order data is null");
                    }
                    
                    _cacheManager.UpdateOrder(orderData);

                    return new Classes.Response
                    { MessageAction = Enums.MessageAction.Update, MessageType = Enums.MessageType.Order, Data = request.Data };

                case Enums.MessageType.Product:
                    var productData = JsonSerializer.Deserialize<Product>(request.Data?.ToString() ?? string.Empty);
                    if (productData == null)
                    {
                        throw new JsonException("Product data is null");
                    }

                    _cacheManager.UpdateProduct(productData);

                    return new Classes.Response
                    { MessageAction = Enums.MessageAction.Update, MessageType = Enums.MessageType.Product, Data = request.Data };
                    
                case Enums.MessageType.User:
                    var userData = JsonSerializer.Deserialize<User>(request.Data?.ToString() ?? string.Empty);
                    if (userData == null)
                    {
                        throw new JsonException("User data is null");
                    }
                    
                    _cacheManager.UpdateUser(userData);

                    return new Classes.Response
                    { MessageAction = Enums.MessageAction.Update, MessageType = Enums.MessageType.User, Data = request.Data };

                case Enums.MessageType.Error:
                    return new Classes.Response
                    { MessageAction = Enums.MessageAction.Update, MessageType = Enums.MessageType.User, Data = request.Data };
                    
                default:
                    return new Classes.Response
                    { MessageAction = Enums.MessageAction.Update, MessageType = Enums.MessageType.User, Data = request.Data };
            }
        }

        public static Classes.Response Delete(Classes.Request request)
        {
            switch (request.MessageType)
            {
                case Enums.MessageType.Category:
                    Category categoryData = JsonSerializer.Deserialize<Category>(request.Data?.ToString());

                    _cacheManager.DeleteCategory(categoryData);
                    
                    return new Classes.Response
                    { MessageAction = Enums.MessageAction.Delete, MessageType = Enums.MessageType.Category, Data = request.Data };

                case Enums.MessageType.Order:
                    Order orderData = JsonSerializer.Deserialize<Order>(request.Data?.ToString());

                    _cacheManager.DeleteOrder(orderData);

                    return new Classes.Response
                    { MessageAction = Enums.MessageAction.Delete, MessageType = Enums.MessageType.Order, Data = request.Data };
                    break;

                case Enums.MessageType.Product:
                    Product productData = JsonSerializer.Deserialize<Product>(request.Data?.ToString());

                    _cacheManager.DeleteProduct(productData);

                    return new Classes.Response
                    { MessageAction = Enums.MessageAction.Delete, MessageType = Enums.MessageType.Product, Data = request.Data };
     
                case Enums.MessageType.User:
                    User userData = JsonSerializer.Deserialize<User>(request.Data?.ToString());

                    _cacheManager.DeleteUser(userData);

                    return new Classes.Response
                    { MessageAction = Enums.MessageAction.Delete, MessageType = Enums.MessageType.User, Data = request.Data };
                    
                case Enums.MessageType.Error:
                    return new Classes.Response
                    { MessageAction = Enums.MessageAction.Delete, MessageType = Enums.MessageType.User, Data = request.Data };
               
                default:
                    return new Classes.Response
                    { MessageAction = Enums.MessageAction.Delete, MessageType = Enums.MessageType.User, Data = request.Data };
            }
        }

        public static Classes.Response Load(Classes.Request request)
        {
            switch (request.MessageType)
            {
                case Enums.MessageType.Category:
                    return new Classes.Response
                                    { MessageAction = Enums.MessageAction.Load, MessageType = Enums.MessageType.Category, Data = _cacheManager.GetAllCategories() };
                    
                case Enums.MessageType.Order:
                    return new Classes.Response
                                    { MessageAction = Enums.MessageAction.Load, MessageType = Enums.MessageType.Order, Data = _cacheManager.GetAllOrders() };
                    
                case Enums.MessageType.Product:
                    return new Classes.Response
                                    { MessageAction = Enums.MessageAction.Load, MessageType = Enums.MessageType.Product, Data = _cacheManager.GetAllProducts() };
                    
                case Enums.MessageType.User:
                    return new Classes.Response
                                    { MessageAction = Enums.MessageAction.Load, MessageType = Enums.MessageType.User, Data = _cacheManager.GetAllUsers() };
                 
                case Enums.MessageType.Error:
                    return new Classes.Response
                    { MessageAction = Enums.MessageAction.Load, MessageType = Enums.MessageType.User, Data = _cacheManager.GetAllUsers() };
                    
                default:
                    return new Classes.Response
                    { MessageAction = Enums.MessageAction.Load, MessageType = Enums.MessageType.User, Data = _cacheManager.GetAllUsers() };
            }
        }
    }
}
