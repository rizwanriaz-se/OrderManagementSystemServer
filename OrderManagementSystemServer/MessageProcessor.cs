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

        public static void ReceiveMessage(Classes.Request request)
        {
            switch (request.MessageAction)
            {
                case Enums.MessageAction.Add:
                    Add(request);
                    break;
                case Enums.MessageAction.Update:
                    Update(request);
                    break;
                case Enums.MessageAction.Delete:
                    Delete(request);
                    break;
                case Enums.MessageAction.Load:
                    Load(request);
                    break;
                case Enums.MessageAction.Error:
                    break;
                default:
                    break;
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
                    //var lastCategoryId = _cacheManager.GetAllCategories().LastOrDefault()?.Id ?? 0;

                    _cacheManager.AddCategory(categoryData);
                    //if (categoryData == null)
                    //{
                    //    throw new JsonException("Category data is null");
                    //}
                    //var lastCategoryId = _cacheManager.GetAllCategories().LastOrDefault()?.Id ?? 0;

                    //_cacheManager.AddCategory(new Category { Id = lastCategoryId + 1, Name = categoryData.Name, Description = categoryData.Description, Picture = categoryData.Picture });
                    return new Classes.Response
                    { MessageAction = Enums.MessageAction.Add, MessageType = Enums.MessageType.Category, Data = request.Data };

                case Enums.MessageType.Order:
                    //var orderData = JsonSerializer.Deserialize<Order>(request.Data?.ToString() ?? string.Empty);
                    //if (orderData == null)
                    //{
                    //    throw new JsonException("Order data is null");
                    //}
                    //var lastOrderId = _cacheManager.GetAllOrders().LastOrDefault()?.Id ?? 0;

                    //_cacheManager.AddOrder(new Order
                    //{
                    //    Id = lastOrderId + 1,
                    //    User = orderData.User,
                    //    OrderDate = DateTime.Now,
                    //    Status = OrderStatus.Pending,
                    //    OrderDetails = orderData.OrderDetails,
                    //    //OrderDetails = ProductRows.Select(row => new OrderDetail
                    //    //{
                    //    //    Product = GUIHandler.GetInstance().CacheManager.GetProductByName(row),
                    //    //    Quantity = row.Quantity
                    //    //}).ToList(),

                    //    ShippedDate = orderData.ShippedDate,
                    //    ShippingAddress = orderData.ShippingAddress
                    //});
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
                    var lastProductId = _cacheManager.GetAllProducts().LastOrDefault()?.Id ?? 0;

                    _cacheManager.AddProduct(new Product
                    {
                        Id = lastProductId + 1,
                        Name = productData.Name,
                        UnitPrice = Convert.ToDecimal(productData.UnitPrice),
                        UnitsInStock = Convert.ToInt32(productData.UnitsInStock),
                        Description = productData.Description,
                        Category = productData.Category
                    });
                    return new Classes.Response
                    { MessageAction = Enums.MessageAction.Add, MessageType = Enums.MessageType.Product, Data = request.Data };

                case Enums.MessageType.User:
                    var userData = JsonSerializer.Deserialize<User>(request.Data?.ToString() ?? string.Empty);
                    if (userData == null)
                    {
                        throw new JsonException("User data is null");
                    }
                    var lastUserId = _cacheManager.GetAllUsers().LastOrDefault()?.Id ?? 0;

                    _cacheManager.AddUser(new User
                    {
                        Id = lastUserId + 1,
                        Name = userData.Name,
                        Email = userData.Email,
                        Phone = userData.Phone,
                        Password = userData.Password,
                        IsAdmin = userData.IsAdmin

                    });
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
                    Category categoryToUpdate = _cacheManager.GetAllCategories().FirstOrDefault(c => c.Id == categoryData.Id);
                    if (categoryToUpdate == null)
                    {
                        throw new JsonException("Category not found");
                    }

                    categoryToUpdate.Name = categoryData.Name;
                    categoryToUpdate.Description = categoryData.Description;
                    categoryToUpdate.Picture = categoryData.Picture;
                    _cacheManager.UpdateCategory(categoryToUpdate);
                    
                    return new Classes.Response
                    { MessageAction = Enums.MessageAction.Update, MessageType = Enums.MessageType.Category, Data = request.Data };

                case Enums.MessageType.Order:
                    var orderData = JsonSerializer.Deserialize<Order>(request.Data?.ToString() ?? string.Empty);
                    if (orderData == null)
                    {
                        throw new JsonException("Order data is null");
                    }
                    Order orderToUpdate = _cacheManager.GetAllOrders().FirstOrDefault(o => o.Id == orderData.Id);
                    if (orderToUpdate == null)
                    {
                        throw new JsonException("Product not found");
                    }

                    orderToUpdate.Status = orderData.Status;
                    orderToUpdate.User = orderData.User;
                    orderToUpdate.ShippedDate = orderData.ShippedDate;
                    orderToUpdate.OrderDetails = orderData.OrderDetails;
                    orderToUpdate.Id = orderData.Id;
                    orderToUpdate.ShippingAddress = orderData.ShippingAddress;

                    _cacheManager.UpdateOrder(orderToUpdate);

                    return new Classes.Response
                    { MessageAction = Enums.MessageAction.Update, MessageType = Enums.MessageType.Order, Data = request.Data };

                case Enums.MessageType.Product:
                    var productData = JsonSerializer.Deserialize<Product>(request.Data?.ToString() ?? string.Empty);
                    if (productData == null)
                    {
                        throw new JsonException("Product data is null");
                    }
                    Product productToUpdate = _cacheManager.GetAllProducts().FirstOrDefault(p => p.Id == productData.Id);
                    if (productToUpdate == null)
                    {
                        throw new JsonException("Product not found");
                    }

                    productToUpdate.Name = productData.Name;
                    productToUpdate.Description = productData.Description;
                    productToUpdate.Picture = productData.Picture;
                    productToUpdate.UnitPrice = productData.UnitPrice;
                    productToUpdate.UnitsInStock = productData.UnitsInStock;
                    productToUpdate.Category = productData.Category;

                    _cacheManager.UpdateProduct(productToUpdate);

                    return new Classes.Response
                    { MessageAction = Enums.MessageAction.Update, MessageType = Enums.MessageType.Product, Data = request.Data };
                    
                case Enums.MessageType.User:
                    var userData = JsonSerializer.Deserialize<User>(request.Data?.ToString() ?? string.Empty);
                    if (userData == null)
                    {
                        throw new JsonException("User data is null");
                    }
                    User userToUpdate = _cacheManager.GetAllUsers().FirstOrDefault(u => u.Id == userData.Id);
                    if (userToUpdate == null)
                    {
                        throw new JsonException("User not found");
                    }
                    userToUpdate.Name = userData.Name;
                    userToUpdate.Email = userData.Email;
                    userToUpdate.Phone = userData.Phone;
                    userToUpdate.Password = userData.Password;
                    userToUpdate.IsAdmin = userData.IsAdmin;
                    //userToUpdate.ShippingAddress = userData.ShippingAddress;

                    _cacheManager.UpdateUser(userToUpdate);

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

                    //var categoryToDelete = _cacheManager.GetAllCategories().FirstOrDefault(c => c.Id == categoryData.Id);
                    //if (categoryToDelete == null)
                    //{
                    //    throw new JsonException("Category not found");
                    //}
                    _cacheManager.DeleteProduct(productData);
                    //_cacheManager.DeleteCategory(categoryToDelete);

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
