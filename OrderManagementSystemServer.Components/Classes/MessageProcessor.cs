using OrderManagementSystemServer.Cache;
using OrderManagementSystemServer.Repository;
using System.Text.Json;


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
        public object? Data { get; set; }
        public string? Error { get; set; }
    }

    public class MessageProcessor
    {

        public static Response ProcessCategoryMessage(Request request)
        {
            try
            {
                switch (request.MessageAction)
                {
                    case Enums.MessageAction.Add:
                        {
                            Category categoryData = JsonSerializer.Deserialize<Category>(request.Data?.ToString() ?? string.Empty);

                            return new Response
                            {
                                MessageAction = Enums.MessageAction.Add,
                                MessageType = Enums.MessageType.Category,
                                Data = CacheManager.Instance.AddCategory(categoryData),
                                Error = null
                            };
                        }

                    case Enums.MessageAction.Update:
                        {
                            Category categoryData = JsonSerializer.Deserialize<Category>(request.Data?.ToString() ?? string.Empty);

                            return new Response
                            {
                                MessageAction = Enums.MessageAction.Update,
                                MessageType = Enums.MessageType.Category,
                                Data = CacheManager.Instance.UpdateCategory(categoryData),
                                Error = null
                            };

                        }

                    case Enums.MessageAction.Delete:
                        {
                            string categoryId = request.Data.ToString();
                            //Category categoryData = JsonSerializer.Deserialize<Category>(request.Data?.ToString() ?? string.Empty);

                            return new Response
                            {
                                MessageAction = Enums.MessageAction.Delete,
                                MessageType = Enums.MessageType.Category,
                                Data = CacheManager.Instance.DeleteCategory(categoryId),
                                Error = null
                            };

                        }

                    case Enums.MessageAction.Load:
                        {
                            return new Response
                            {
                                MessageAction = Enums.MessageAction.Load,
                                MessageType = Enums.MessageType.Category,
                                Data = CacheManager.Instance.GetAllCategories(),
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
                    MessageAction = Enums.MessageAction.Error,
                    MessageType = Enums.MessageType.Category,
                    Data = null,
                    Error = $"Error trying to {request.MessageAction} {request.MessageType}: {ex.Message}"
                };
            }
        }

        public static Response ProcessOrderMessage(Request request)
        {
            try
            {
                
                switch (request.MessageAction)
                {
                    case Enums.MessageAction.Add:
                        {
                            Order orderData = JsonSerializer.Deserialize<Order>(request.Data?.ToString() ?? string.Empty);

                            return new Response
                            {
                                MessageAction = Enums.MessageAction.Add,
                                MessageType = Enums.MessageType.Order,
                                Data = CacheManager.Instance.AddOrder(orderData),
                                Error = null
                            };
                        }

                    case Enums.MessageAction.Update:
                        {
                            Order orderData = JsonSerializer.Deserialize<Order>(request.Data?.ToString() ?? string.Empty);

                            return new Response
                            {
                                MessageAction = Enums.MessageAction.Update,
                                MessageType = Enums.MessageType.Order,
                                Data = CacheManager.Instance.UpdateOrder(orderData),
                                Error = null
                            };

                        }

                    case Enums.MessageAction.Delete:
                        {
                            string orderId = request.Data.ToString();
                            //Order orderData = JsonSerializer.Deserialize<Order>(request.Data?.ToString() ?? string.Empty);

                            return new Response
                            {
                                MessageAction = Enums.MessageAction.Delete,
                                MessageType = Enums.MessageType.Order,
                                Data = CacheManager.Instance.DeleteOrder(orderId),
                                Error = null
                            };

                        }

                    case Enums.MessageAction.Load:
                        {
                            return new Response
                            {
                                MessageAction = Enums.MessageAction.Load,
                                MessageType = Enums.MessageType.Order,
                                Data = CacheManager.Instance.GetAllOrders(),
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
                    MessageAction = Enums.MessageAction.Error,
                    MessageType = Enums.MessageType.Order,
                    Data = null,
                    Error = $"Error trying to {request.MessageAction} {request.MessageType}: {ex.Message}"
                };
            }
        }

        public static Response ProcessProductMessage(Request request)
        {
            try
            {
                
                switch (request.MessageAction)
                {
                    case Enums.MessageAction.Add:
                        {
                            Product productData = JsonSerializer.Deserialize<Product>(request.Data?.ToString() ?? string.Empty);

                            return new Response
                            {
                                MessageAction = Enums.MessageAction.Add,
                                MessageType = Enums.MessageType.Product,
                                Data = CacheManager.Instance.AddProduct(productData),
                            };
                        }

                    case Enums.MessageAction.Update:
                        {
                            Product productData = JsonSerializer.Deserialize<Product>(request.Data?.ToString() ?? string.Empty);

                            return new Response
                            {
                                MessageAction = Enums.MessageAction.Update,
                                MessageType = Enums.MessageType.Product,
                                Data = CacheManager.Instance.UpdateProduct(productData),
                            };
                        }

                    case Enums.MessageAction.Delete:
                        {
                            string productId = request.Data.ToString();
                            //Product productData = JsonSerializer.Deserialize<Product>(request.Data?.ToString() ?? string.Empty);

                            return new Response
                            {
                                MessageAction = Enums.MessageAction.Delete,
                                MessageType = Enums.MessageType.Product,
                                Data = CacheManager.Instance.DeleteProduct(productId),
                            };
                        }

                    case Enums.MessageAction.Load:
                        {
                            return new Response
                            {
                                MessageAction = Enums.MessageAction.Load,
                                MessageType = Enums.MessageType.Product,
                                Data = CacheManager.Instance.GetAllProducts(),
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
                    MessageAction = Enums.MessageAction.Error,
                    MessageType = Enums.MessageType.Product,
                    Data = null,
                    Error = $"Error trying to {request.MessageAction} {request.MessageType}: {ex.Message}"
                };
            }
        }

        public static Response ProcessUserMessage(Request request)
        {
            try
            {
                
                switch (request.MessageAction)
                {
                    case Enums.MessageAction.Add:
                        {
                            User userData = JsonSerializer.Deserialize<User>(request.Data?.ToString() ?? string.Empty);

                            return new Response
                            {
                                MessageAction = Enums.MessageAction.Add,
                                MessageType = Enums.MessageType.User,
                                Data = CacheManager.Instance.AddUser(userData),
                                Error = null
                            };


                        }

                    case Enums.MessageAction.Update:
                        {
                            User userData = JsonSerializer.Deserialize<User>(request.Data?.ToString() ?? string.Empty);

                            return new Response
                            {
                                MessageAction = Enums.MessageAction.Update,
                                MessageType = Enums.MessageType.User,
                                Data = CacheManager.Instance.UpdateUser(userData),
                                Error = null
                            };



                        }

                    case Enums.MessageAction.Delete:
                        {
                            string userId = request.Data.ToString();
                            //User userData = JsonSerializer.Deserialize<User>(request.Data?.ToString() ?? string.Empty);

                            return new Response
                            {
                                MessageAction = Enums.MessageAction.Delete,
                                MessageType = Enums.MessageType.User,
                                Data = CacheManager.Instance.DeleteUser(userId),
                                Error = null
                            };



                        }

                    case Enums.MessageAction.Load:
                        {
                            return new Response
                            {
                                MessageAction = Enums.MessageAction.Load,
                                MessageType = Enums.MessageType.User,
                                Data = CacheManager.Instance.GetAllUsers(),
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
                    MessageAction = Enums.MessageAction.Error,
                    MessageType = Enums.MessageType.User,
                    Data = null,
                    Error = $"Error trying to {request.MessageAction} {request.MessageType}: {ex.Message}"
                };
            }

        }

        public static Response ProcessHeartbeatMessage(Request request)
        {
            try
            {
                switch (request.MessageAction)
                {
                    case Enums.MessageAction.Ping:
                        {
                            return new Response
                            {
                                MessageAction = Enums.MessageAction.Ping,
                                MessageType = Enums.MessageType.Heartbeat,
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
                    MessageAction = Enums.MessageAction.Ping,
                    MessageType = Enums.MessageType.Heartbeat,
                    Data = null,
                    Error = $"Error trying to send heartbeat response: {ex.Message}"
                };
            }

        }
    }
}
