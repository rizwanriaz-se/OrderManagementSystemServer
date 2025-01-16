using OrderManagementSystemServer.Cache;
using OrderManagementSystemServer.Repository;
using System.Text.Json;


namespace OrderManagementSystemServer.Components.Classes
{

    public class Request
    {
        public MessageType MessageType { get; set; }
        public MessageAction MessageAction { get; set; }
        public object Data { get; set; }
    }

    public class Response
    {
        public MessageType MessageType { get; set; }
        public MessageAction MessageAction { get; set; }
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
                    case MessageAction.Add:
                        {  
                            Category categoryData = ServerManager.DeserializeJson<Category>(request.Data);

                            return new Response
                            {
                                MessageAction = MessageAction.Add,
                                MessageType = MessageType.Category,
                                Data = CacheManager.Instance.AddCategory(categoryData),
                                Error = null
                            };
                        }

                    case MessageAction.Update:
                        {
                            Category categoryData = ServerManager.DeserializeJson<Category>(request.Data?.ToString() ?? null);

                            return new Response
                            {
                                MessageAction = MessageAction.Update,
                                MessageType = MessageType.Category,
                                Data = CacheManager.Instance.UpdateCategory(categoryData),
                                Error = null
                            };

                        }

                    case MessageAction.Delete:
                        {
                            string categoryId = request.Data.ToString();
                            //Category categoryData = JsonSerializer.Deserialize<Category>(request.Data?.ToString() ?? string.Empty);

                            return new Response
                            {
                                MessageAction = MessageAction.Delete,
                                MessageType = MessageType.Category,
                                Data = CacheManager.Instance.DeleteCategory(categoryId),
                                Error = null
                            };
                        }

                    case MessageAction.Load:
                        {
                            return new Response
                            {
                                MessageAction = MessageAction.Load,
                                MessageType = MessageType.Category,
                                Data = CacheManager.Instance.Categories,
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
                    MessageAction = MessageAction.Error,
                    MessageType = MessageType.Category,
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
                    case MessageAction.Add:
                        {
                            Order orderData = ServerManager.DeserializeJson<Order>(request.Data?.ToString() ?? string.Empty);

                            return new Response
                            {
                                MessageAction = MessageAction.Add,
                                MessageType = MessageType.Order,
                                Data = CacheManager.Instance.AddOrder(orderData),
                                Error = null
                            };
                        }
                        
                    case MessageAction.Update:
                        {
                            Order orderData = ServerManager.DeserializeJson<Order>(request.Data?.ToString() ?? string.Empty);

                            return new Response
                            {
                                MessageAction = MessageAction.Update,
                                MessageType = MessageType.Order,
                                Data = CacheManager.Instance.UpdateOrder(orderData),
                                Error = null
                            };

                        }

                    case MessageAction.Delete:
                        {
                            string orderId = request.Data.ToString();
                            //Order orderData = JsonSerializer.Deserialize<Order>(request.Data?.ToString() ?? string.Empty);

                            return new Response
                            {
                                MessageAction = MessageAction.Delete,
                                MessageType = MessageType.Order,
                                Data = CacheManager.Instance.DeleteOrder(orderId),
                                Error = null
                            };

                        }

                    case MessageAction.Load:
                        {
                            return new Response
                            {
                                MessageAction = MessageAction.Load,
                                MessageType = MessageType.Order,
                                Data = CacheManager.Instance.Orders,
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
                    MessageAction = MessageAction.Error,
                    MessageType = MessageType.Order,
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
                    case MessageAction.Add:
                        {
                            Product productData = ServerManager.DeserializeJson<Product>(request.Data?.ToString() ?? string.Empty);

                            return new Response
                            {
                                MessageAction = MessageAction.Add,
                                MessageType = MessageType.Product,
                                Data = CacheManager.Instance.AddProduct(productData),
                            };
                        }

                    case MessageAction.Update:
                        {
                            Product productData = ServerManager.DeserializeJson<Product>(request.Data);

                            return new Response
                            {
                                MessageAction = MessageAction.Update,
                                MessageType = MessageType.Product,
                                Data = CacheManager.Instance.UpdateProduct(productData),
                            };
                        }

                    case MessageAction.Delete:
                        {
                            string productId = request.Data.ToString();
                            //Product productData = JsonSerializer.Deserialize<Product>(request.Data?.ToString() ?? string.Empty);

                            return new Response
                            {
                                MessageAction = MessageAction.Delete,
                                MessageType = MessageType.Product,
                                Data = CacheManager.Instance.DeleteProduct(productId),
                            };
                        }

                    case MessageAction.Load:
                        {
                            return new Response
                            {
                                MessageAction = MessageAction.Load,
                                MessageType = MessageType.Product,
                                Data = CacheManager.Instance.Products,
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
                    MessageAction = MessageAction.Error,
                    MessageType = MessageType.Product,
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
                    case MessageAction.Add:
                        {
                            User userData = ServerManager.DeserializeJson<User>(request.Data);

                            return new Response
                            {
                                MessageAction = MessageAction.Add,
                                MessageType = MessageType.User,
                                Data = CacheManager.Instance.AddUser(userData),
                                Error = null
                            };


                        }

                    case MessageAction.Update:
                        {
                            User userData = ServerManager.DeserializeJson<User>(request.Data);

                            return new Response
                            {
                                MessageAction = MessageAction.Update,
                                MessageType = MessageType.User,
                                Data = CacheManager.Instance.UpdateUser(userData),
                                Error = null
                            };



                        }

                    case MessageAction.Delete:
                        {
                            string userId = request.Data.ToString();
                            //User userData = JsonSerializer.Deserialize<User>(request.Data?.ToString() ?? string.Empty);

                            return new Response
                            {
                                MessageAction = MessageAction.Delete,
                                MessageType = MessageType.User,
                                Data = CacheManager.Instance.DeleteUser(userId),
                                Error = null
                            };



                        }

                    case MessageAction.Load:
                        {
                            return new Response
                            {
                                MessageAction = MessageAction.Load,
                                MessageType = MessageType.User,
                                Data = CacheManager.Instance.Users,
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
                    MessageAction = MessageAction.Error,
                    MessageType = MessageType.User,
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
                    case MessageAction.Ping:
                        {
                            return new Response
                            {
                                MessageAction = MessageAction.Ping,
                                MessageType = MessageType.Heartbeat,
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
                    MessageAction = MessageAction.Ping,
                    MessageType = MessageType.Heartbeat,
                    Data = null,
                    Error = $"Error trying to send heartbeat response: {ex.Message}"
                };
            }

        }
    }
}
