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
        private static CacheManager _cacheManager = CacheManager.Instance;

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
                                Data = _cacheManager.AddCategory(categoryData),
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
                                Data = _cacheManager.UpdateCategory(categoryData),
                                Error = null
                            };

                        }

                    case Enums.MessageAction.Delete:
                        {
                            Category categoryData = JsonSerializer.Deserialize<Category>(request.Data?.ToString() ?? string.Empty);

                            return new Response
                            {
                                MessageAction = Enums.MessageAction.Delete,
                                MessageType = Enums.MessageType.Category,
                                Data = _cacheManager.DeleteCategory(categoryData),
                                Error = null
                            };

                        }

                    case Enums.MessageAction.Load:
                        {
                            return new Response
                            {
                                MessageAction = Enums.MessageAction.Load,
                                MessageType = Enums.MessageType.Category,
                                Data = _cacheManager.GetAllCategories(),
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
                                Data = _cacheManager.AddOrder(orderData),
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
                                Data = _cacheManager.UpdateOrder(orderData),
                                Error = null
                            };

                        }

                    case Enums.MessageAction.Delete:
                        {
                            Order orderData = JsonSerializer.Deserialize<Order>(request.Data?.ToString() ?? string.Empty);

                            return new Response
                            {
                                MessageAction = Enums.MessageAction.Delete,
                                MessageType = Enums.MessageType.Order,
                                Data = _cacheManager.DeleteOrder(orderData),
                                Error = null
                            };

                        }

                    case Enums.MessageAction.Load:
                        {
                            return new Response
                            {
                                MessageAction = Enums.MessageAction.Load,
                                MessageType = Enums.MessageType.Order,
                                Data = _cacheManager.GetAllOrders(),
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
                                Data = _cacheManager.AddProduct(productData),
                            };
                        }

                    case Enums.MessageAction.Update:
                        {
                            Product productData = JsonSerializer.Deserialize<Product>(request.Data?.ToString() ?? string.Empty);

                            return new Response
                            {
                                MessageAction = Enums.MessageAction.Update,
                                MessageType = Enums.MessageType.Product,
                                Data = _cacheManager.UpdateProduct(productData),
                            };
                        }

                    case Enums.MessageAction.Delete:
                        {
                            Product productData = JsonSerializer.Deserialize<Product>(request.Data?.ToString() ?? string.Empty);

                            return new Response
                            {
                                MessageAction = Enums.MessageAction.Delete,
                                MessageType = Enums.MessageType.Product,
                                Data = _cacheManager.DeleteProduct(productData),
                            };
                        }

                    case Enums.MessageAction.Load:
                        {
                            return new Response
                            {
                                MessageAction = Enums.MessageAction.Load,
                                MessageType = Enums.MessageType.Product,
                                Data = _cacheManager.GetAllProducts(),
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
                                Data = _cacheManager.AddUser(userData),
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
                                Data = _cacheManager.UpdateUser(userData),
                                Error = null
                            };



                        }

                    case Enums.MessageAction.Delete:
                        {
                            User userData = JsonSerializer.Deserialize<User>(request.Data?.ToString() ?? string.Empty);

                            return new Response
                            {
                                MessageAction = Enums.MessageAction.Delete,
                                MessageType = Enums.MessageType.User,
                                Data = _cacheManager.DeleteUser(userData),
                                Error = null
                            };



                        }

                    case Enums.MessageAction.Load:
                        {
                            return new Response
                            {
                                MessageAction = Enums.MessageAction.Load,
                                MessageType = Enums.MessageType.User,
                                Data = _cacheManager.GetAllUsers(),
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
