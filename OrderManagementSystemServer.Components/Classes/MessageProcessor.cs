//using OrderManagementSystemServer.Cache;
using System.Text.Json;
using System.Collections.ObjectModel;
//using OrderManagementSystem.Repositories;
//using OrderManagementSystemServer.Repositories;
using OrderManagementSystemServer.Repository;
using OrderManagementSystemServer.Cache;
//using OrderManagementSystemServer.Repositories;

//using OrderManagementSystem.Repositories;

namespace OrderManagementSystemServer.Components.Classes
{

    public class Request
    {
        public Enums.MessageType MessageType { get; set; }
        public Enums.MessageAction MessageAction { get; set; }
        public object Data { get; set; }
    }

    public class Response
    {
        public Enums.MessageType MessageType { get; set; }
        public Enums.MessageAction MessageAction { get; set; }
        public object Data { get; set; }
    }

    public class MessageProcessor
    {
        private static CacheManager _cacheManager = CacheManager.Instance;

        public static Response ProcessCategoryMessage(Request request)
        {
            Response objResponse = null;

            switch (request.MessageAction)
            {
                case Enums.MessageAction.Add:
                    {
                        try
                        {
                            Category categoryData = JsonSerializer.Deserialize<Category>(request.Data?.ToString() ?? string.Empty);
                            Category objResponseCategory = _cacheManager.AddCategory(categoryData);

                            objResponse = new Response
                            {
                                MessageAction = Enums.MessageAction.Add,
                                MessageType = Enums.MessageType.Category,
                                Data = objResponseCategory
                            };

                        }
                        catch (Exception)
                        {
                            objResponse = new Response
                            {
                                MessageAction = Enums.MessageAction.Error,
                                MessageType = Enums.MessageType.Category,
                                Data = null
                            };
                        }

                        break;
                    }

                case Enums.MessageAction.Update:
                    {
                        try
                        {
                            Category categoryData = JsonSerializer.Deserialize<Category>(request.Data?.ToString() ?? string.Empty);
                            Category objResponseCategory = _cacheManager.UpdateCategory(categoryData);

                            objResponse = new Response
                            {
                                MessageAction = Enums.MessageAction.Update,
                                MessageType = Enums.MessageType.Category,
                                Data = objResponseCategory
                            };
                        }
                        catch (Exception)
                        {
                            objResponse = new Response
                            {
                                MessageAction = Enums.MessageAction.Error,
                                MessageType = Enums.MessageType.Category,
                                Data = null
                            };
                        }

                        break;
                    }

                case Enums.MessageAction.Delete:
                    {
                        try
                        {
                            Category categoryData = JsonSerializer.Deserialize<Category>(request.Data?.ToString() ?? string.Empty);
                            Category objResponseCategory = _cacheManager.DeleteCategory(categoryData);

                            objResponse = new Response
                            {
                                MessageAction = Enums.MessageAction.Delete,
                                MessageType = Enums.MessageType.Category,
                                Data = objResponseCategory
                            };
                        }
                        catch (Exception)
                        {
                            objResponse = new Response
                            {
                                MessageAction = Enums.MessageAction.Error,
                                MessageType = Enums.MessageType.Category,
                                Data = null
                            };
                        }

                        break;
                    }

                case Enums.MessageAction.Load:
                    {
                        try
                        {
                            ObservableCollection<Category> lstCategories = _cacheManager.GetAllCategories();

                            objResponse = new Response
                            {
                                MessageAction = Enums.MessageAction.Load,
                                MessageType = Enums.MessageType.Category,
                                Data = lstCategories
                            };
                        }
                        catch (Exception)
                        {
                            objResponse = new Response
                            {
                                MessageAction = Enums.MessageAction.Error,
                                MessageType = Enums.MessageType.Category,
                                Data = null
                            };
                        }

                        break;
                    }

                default:
                    objResponse = new Response
                    {
                        MessageAction = Enums.MessageAction.Load,
                        MessageType = Enums.MessageType.Category,
                        Data = null
                    };
                    break;
            }
            return objResponse;

        }

        public static Response ProcessOrderMessage(Request request)
        {
            Response objResponse = null;

            switch (request.MessageAction)
            {
                case Enums.MessageAction.Add:
                    {
                        try
                        {
                            Order orderData = JsonSerializer.Deserialize<Order>(request.Data?.ToString() ?? string.Empty);
                            Order objResponseOrder = _cacheManager.AddOrder(orderData);

                            objResponse = new Response
                            {
                                MessageAction = Enums.MessageAction.Add,
                                MessageType = Enums.MessageType.Order,
                                Data = objResponseOrder
                            };
                        }
                        catch (Exception)
                        {
                            objResponse = new Response
                            {
                                MessageAction = Enums.MessageAction.Error,
                                MessageType = Enums.MessageType.Order,
                                Data = null
                            };
                        }

                        break;
                    }

                case Enums.MessageAction.Update:
                    {
                        try
                        {
                            Order orderData = JsonSerializer.Deserialize<Order>(request.Data?.ToString() ?? string.Empty);
                            Order objResponseOrder = _cacheManager.UpdateOrder(orderData);

                            objResponse = new Response
                            {
                                MessageAction = Enums.MessageAction.Update,
                                MessageType = Enums.MessageType.Order,
                                Data = objResponseOrder
                            };
                        }
                        catch (Exception)
                        {
                            objResponse = new Response
                            {
                                MessageAction = Enums.MessageAction.Error,
                                MessageType = Enums.MessageType.Order,
                                Data = null
                            };
                        }

                        break;
                    }

                case Enums.MessageAction.Delete:
                    {
                        try
                        {
                            Order orderData = JsonSerializer.Deserialize<Order>(request.Data?.ToString() ?? string.Empty);
                            Order objResponseOrder = _cacheManager.DeleteOrder(orderData);

                            objResponse = new Response
                            {
                                MessageAction = Enums.MessageAction.Delete,
                                MessageType = Enums.MessageType.Order,
                                Data = objResponseOrder
                            };
                        }
                        catch (Exception)
                        {
                            objResponse = new Response
                            {
                                MessageAction = Enums.MessageAction.Error,
                                MessageType = Enums.MessageType.Order,
                                Data = null
                            };
                        }

                        break;
                    }

                case Enums.MessageAction.Load:
                    {
                        try
                        {
                            ObservableCollection<Order> lstOrders = _cacheManager.GetAllOrders();

                            objResponse = new Response
                            {
                                MessageAction = Enums.MessageAction.Load,
                                MessageType = Enums.MessageType.Order,
                                Data = lstOrders
                            };
                        }
                        catch (Exception)
                        {
                            objResponse = new Response
                            {
                                MessageAction = Enums.MessageAction.Error,
                                MessageType = Enums.MessageType.Order,
                                Data = null
                            };
                        }
                        break;
                    }
            }
            return objResponse;
        }

        public static Response ProcessProductMessage(Request request)
        {
            Response objResponse = null;

            switch (request.MessageAction)
            {
                case Enums.MessageAction.Add:
                    {
                        try
                        {
                            Product productData = JsonSerializer.Deserialize<Product>(request.Data?.ToString() ?? string.Empty);
                            Product objResponseProduct = _cacheManager.AddProduct(productData);

                            objResponse = new Response
                            {
                                MessageAction = Enums.MessageAction.Add,
                                MessageType = Enums.MessageType.Product,
                                Data = objResponseProduct
                            };
                        }
                        catch (Exception)
                        {
                            objResponse = new Response
                            {
                                MessageAction = Enums.MessageAction.Error,
                                MessageType = Enums.MessageType.Product,
                                Data = null
                            };
                        }

                        break;
                    }

                case Enums.MessageAction.Update:
                    {
                        try
                        {
                            Product productData = JsonSerializer.Deserialize<Product>(request.Data?.ToString() ?? string.Empty);
                            Product objResponseProduct = _cacheManager.UpdateProduct(productData);

                            objResponse = new Response
                            {
                                MessageAction = Enums.MessageAction.Update,
                                MessageType = Enums.MessageType.Product,
                                Data = objResponseProduct
                            };
                        }
                        catch (Exception)
                        {
                            objResponse = new Response
                            {
                                MessageAction = Enums.MessageAction.Error,
                                MessageType = Enums.MessageType.Product,
                                Data = null
                            };
                        }

                        break;
                    }

                case Enums.MessageAction.Delete:
                    {
                        try
                        {
                            Product productData = JsonSerializer.Deserialize<Product>(request.Data?.ToString() ?? string.Empty);
                            Product objResponseProduct = _cacheManager.DeleteProduct(productData);

                            objResponse = new Response
                            {
                                MessageAction = Enums.MessageAction.Delete,
                                MessageType = Enums.MessageType.Product,
                                Data = objResponseProduct
                            };
                        }
                        catch (Exception)
                        {
                            objResponse = new Response
                            {
                                MessageAction = Enums.MessageAction.Error,
                                MessageType = Enums.MessageType.Product,
                                Data = null
                            };
                        }

                        break;
                    }

                case Enums.MessageAction.Load:
                    {
                        try
                        {
                            ObservableCollection<Product> lstProducts = _cacheManager.GetAllProducts();

                            objResponse = new Response
                            {
                                MessageAction = Enums.MessageAction.Load,
                                MessageType = Enums.MessageType.Product,
                                Data = lstProducts
                            };
                        }
                        catch (Exception)
                        {
                            objResponse = new Response
                            {
                                MessageAction = Enums.MessageAction.Error,
                                MessageType = Enums.MessageType.Product,
                                Data = null
                            };
                        }
                        break;
                    }
            }
            return objResponse;
        }

        public static Response ProcessUserMessage(Request request)
        {
            Response objResponse = null;

            switch (request.MessageAction)
            {
                case Enums.MessageAction.Add:
                    {
                        try
                        {
                            User userData = JsonSerializer.Deserialize<User>(request.Data?.ToString() ?? string.Empty);
                            User objResponseuser = _cacheManager.AddUser(userData);

                            objResponse = new Response
                            {
                                MessageAction = Enums.MessageAction.Add,
                                MessageType = Enums.MessageType.User,
                                Data = objResponseuser
                            };
                        }
                        catch (Exception)
                        {
                            objResponse = new Response
                            {
                                MessageAction = Enums.MessageAction.Error,
                                MessageType = Enums.MessageType.User,
                                Data = null
                            };
                        }

                        break;
                    }

                case Enums.MessageAction.Update:
                    {
                        try
                        {
                            User userData = JsonSerializer.Deserialize<User>(request.Data?.ToString() ?? string.Empty);
                            User objResponseuser = _cacheManager.UpdateUser(userData);

                            objResponse = new Response
                            {
                                MessageAction = Enums.MessageAction.Update,
                                MessageType = Enums.MessageType.User,
                                Data = objResponseuser
                            };
                        }
                        catch (Exception)
                        {
                            objResponse = new Response
                            {
                                MessageAction = Enums.MessageAction.Error,
                                MessageType = Enums.MessageType.User,
                                Data = null
                            };
                        }

                        break;
                    }

                case Enums.MessageAction.Delete:
                    {
                        try
                        {
                            User userData = JsonSerializer.Deserialize<User>(request.Data?.ToString() ?? string.Empty);
                            User objResponseuser = _cacheManager.DeleteUser(userData);

                            objResponse = new Response
                            {
                                MessageAction = Enums.MessageAction.Delete,
                                MessageType = Enums.MessageType.User,
                                Data = objResponseuser
                            };
                        }
                        catch (Exception)
                        {
                            objResponse = new Response
                            {
                                MessageAction = Enums.MessageAction.Error,
                                MessageType = Enums.MessageType.User,
                                Data = null
                            };
                        }

                        break;
                    }

                case Enums.MessageAction.Load:
                    {
                        try
                        {
                            ObservableCollection<User> lstUsers = _cacheManager.GetAllUsers();

                            objResponse = new Response
                            {
                                MessageAction = Enums.MessageAction.Load,
                                MessageType = Enums.MessageType.User,
                                Data = lstUsers
                            };
                        }
                        catch (Exception)
                        {
                            objResponse = new Response
                            {
                                MessageAction = Enums.MessageAction.Error,
                                MessageType = Enums.MessageType.User,
                                Data = null
                            };
                        }
                        break;
                    }
            }
            return objResponse;
        }

        public static Response ProcessHeartbeatMessage(Request request)
        {
            Response objResponse = null;

            switch (request.MessageAction)
            {
                case Enums.MessageAction.Ping:
                    {
                        objResponse = new Response
                        {
                            MessageAction = Enums.MessageAction.Ping,
                            MessageType = Enums.MessageType.Heartbeat,
                            Data = "PONG"
                        };
                        break;
                    }
            }
            return objResponse;
        }
    }
}
