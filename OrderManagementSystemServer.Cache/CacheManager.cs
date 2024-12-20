//using OrderManagementSystem.Repositories;
//using OrderManagementSystemServer.Repository;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection.Metadata;


//using OrderManagementSystemServer


//using OrderManagementSystemServer.Cache.Models;
//using static OrderManagementSystemServer.Cache.Models.Order;
using System.Runtime.CompilerServices;
using Microsoft.VisualBasic;
using OrderManagementSystemServer.Components.Utils;

//using OrderManagementSystemServer.Components.Utils;
using OrderManagementSystemServer.Repository;
using static OrderManagementSystemServer.Repository.Order;
//using OrderManagementSystem.Repositories;
//using static OrderManagementSystem.Repositories.Order;
//using OrderManagementSystemServer.Components.Classes.Constants;

namespace OrderManagementSystemServer.Cache
{
    public class CacheManager
    {
        private static CacheManager m_Instance;
        public ObservableCollection<Category> _AllCategories { get; private set; }
        public ObservableCollection<Order> _AllOrders { get; private set; }
        public ObservableCollection<Product> _AllProducts { get; private set; }
        public ObservableCollection<User> _AllUsers { get; private set; }

        private User m_CurrentUser;

        public User CurrentUser
        {
            get { return m_CurrentUser; }
            set { m_CurrentUser = value; }
        }

        public static CacheManager Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    m_Instance = new CacheManager();
                }
                return m_Instance;
            }
        }

        //private static string currentDirectory = Directory.GetCurrentDirectory();
        //private List<string> currentDirectoryList = currentDirectory.Split(new string[] { "\\bin" }, StringSplitOptions.None).ToList();
        //private string m_stDataStorePath = $"{Directory.GetCurrentDirectory().Split(new string[] { "\\bin" }, StringSplitOptions.None)[0]}\\..\\OrderManagementSystemServer.Components\\DataStore\\";
        //private string m_stDataStorePath = "C:\\Users\\rriaz\\source\\repos\\OrderManagementSystemDataStore\\";
        //Console.WriteLine(m_stDataStorePath);
        //private strin


        //private string m_stUserDataStorePath = "UserDataStore.xml";
        //private string m_stCategoryDataStorePath = "CategoryDataStore.xml";
        //private string m_stProductDataStorePath = "ProductDataStore.xml";
        //private string m_stOrderDataStorePath = "OrderDataStore.xml";

        //public ObservableCo
        private CacheManager()
        {

            _AllUsers = CustomXMLSerializer.DeserializeFromXml<ObservableCollection<User>>($"{Constants.XMLDirectoryPath}{Constants.UserDataStoreName}");

            _AllCategories = CustomXMLSerializer.DeserializeFromXml<ObservableCollection<Category>>($"{Constants.XMLDirectoryPath}{Constants.CategoryDataStoreName}");

            _AllProducts = CustomXMLSerializer.DeserializeFromXml<ObservableCollection<Product>>($"{Constants.XMLDirectoryPath}{Constants.ProductDataStoreName}");

            _AllOrders = CustomXMLSerializer.DeserializeFromXml<ObservableCollection<Order>>($"{Constants.XMLDirectoryPath}{Constants.OrderDataStoreName}");


        }

        public void SaveData(bool onlyUser)
        {
            if (onlyUser is false)
            {
                CustomXMLSerializer.SerializeToXml($"{Constants.XMLDirectoryPath}{Constants.UserDataStoreName}", _AllUsers);
                CustomXMLSerializer.SerializeToXml($"{Constants.XMLDirectoryPath}{Constants.CategoryDataStoreName}", _AllCategories);
                CustomXMLSerializer.SerializeToXml($"{Constants.XMLDirectoryPath}{Constants.ProductDataStoreName}", _AllProducts);
                CustomXMLSerializer.SerializeToXml($"{Constants.XMLDirectoryPath}{Constants.OrderDataStoreName}", _AllOrders);
            }
            CustomXMLSerializer.SerializeToXml($"{Constants.XMLDirectoryPath}{Constants.UserDataStoreName}", _AllUsers);

        }

        public Category GetCategoryById(int id)
        {
            return _AllCategories.FirstOrDefault(c => c.Id == id);
        }
        public ObservableCollection<Category> GetAllCategories()
        {
            return _AllCategories;
        }
        //public Category AddCategory(Category category, ref string stErrorMsg)
        //{
        //    if (category is null)
        //    {
        //        stErrorMsg = "Couldn't add the Category";
        //        return null;
        //    }

        //    int lastCategoryId = GetAllCategories().LastOrDefault()?.Id ?? 0;
        //    Category categoryToAdd = new Category { Id = lastCategoryId + 1, Name = category.Name, Description = category.Description, Picture = category.Picture };

        //    _AllCategories.Add(categoryToAdd);
        //    return categoryToAdd;
        //}

        public Category AddCategory(Category category)
        {
            if (category is null)
            {
                //stErrorMsg = "Couldn't add the Category";
                return null;
            }

            int lastCategoryId = GetAllCategories().LastOrDefault()?.Id ?? 0;
            Category categoryToAdd = new Category { Id = lastCategoryId + 1, Name = category.Name, Description = category.Description, Picture = category.Picture };

            _AllCategories.Add(categoryToAdd);
            return categoryToAdd;
        }


        public Category DeleteCategory(Category category)
        {

            _AllCategories.Remove(category);
            return category;

        }
       
        public Category UpdateCategory(Category updatedCategory)
        {
            Category categoryToUpdate = GetAllCategories().FirstOrDefault(c => c.Id == updatedCategory.Id);

            categoryToUpdate.Name = updatedCategory.Name;
            categoryToUpdate.Description = updatedCategory.Description;
            categoryToUpdate.Picture = updatedCategory.Picture;

            return categoryToUpdate;
        }
        public ObservableCollection<Order> GetAllOrders()
        {
            return _AllOrders;
        }
        public Order GetOrderById(int id)
        {
            return _AllOrders.FirstOrDefault(o => o.Id == id);
        }
        public Order AddOrder(Order order)
        {
            var lastOrderId = GetAllOrders().LastOrDefault()?.Id ?? 0;

            Order orderToAdd = new Order
            {
                Id = lastOrderId + 1,
                User = order.User,
                OrderDate = DateTime.Now,
                Status = OrderStatus.Pending,
                OrderDetails = order.OrderDetails,
                ShippedDate = order.ShippedDate,
                ShippingAddress = order.ShippingAddress
            };

            _AllOrders.Add(orderToAdd);
            return orderToAdd;
        }
        public Order UpdateOrder(Order updatedOrder)
        {
            // Retrieve the existing order with the same ID
            Order orderToUpdate = GetAllOrders().FirstOrDefault(o => o.Id == updatedOrder.Id);

            if (orderToUpdate != null)
            {
                // Update the existing order's properties
                orderToUpdate.User = updatedOrder.User;
                orderToUpdate.OrderDate = updatedOrder.OrderDate;
                orderToUpdate.Status = updatedOrder.Status;
                orderToUpdate.ShippedDate = updatedOrder.ShippedDate;
                orderToUpdate.ShippingAddress = updatedOrder.ShippingAddress;
                orderToUpdate.OrderDetails = updatedOrder.OrderDetails;
            }
            else
            {
                //MessageBox.Show("Order not found.");
            }
            return orderToUpdate;
        }
        public Order DeleteOrder(Order order)
        {
            _AllOrders.Remove(order);
            return order;
        }
        public ObservableCollection<Product> GetAllProducts()
        {
            return _AllProducts;
        }
        public Product GetProductByID(int id)
        {

            return _AllProducts.FirstOrDefault(p => p.Id == id);
        }
        public Product GetProductByName(OrderDetail orderDetail)
        {
            return _AllProducts.FirstOrDefault(p => p.Name == orderDetail.Product.Name);
        }
        public Product AddProduct(Product product)
        {
            var lastProductId = GetAllProducts().LastOrDefault()?.Id ?? 0;

            Product productToAdd = new Product
            {
                Id = lastProductId + 1,
                Name = product.Name,
                UnitPrice = Convert.ToDecimal(product.UnitPrice),
                UnitsInStock = Convert.ToInt32(product.UnitsInStock),
                Description = product.Description,
                Category = product.Category
            };
            _AllProducts.Add(productToAdd);
            return productToAdd;
        }
        public Product UpdateProduct(Product updatedProduct)
        {
            Product productToUpdate = GetAllProducts().FirstOrDefault(p => p.Id == updatedProduct.Id);
            if (productToUpdate != null)
            {
                productToUpdate.Name = updatedProduct.Name;
                productToUpdate.Description = updatedProduct.Description;
                productToUpdate.Category = updatedProduct.Category;
                productToUpdate.Picture = updatedProduct.Picture;
                productToUpdate.UnitPrice = updatedProduct.UnitPrice;
                productToUpdate.UnitsInStock = updatedProduct.UnitsInStock;
            }
            return productToUpdate;
        }
        public Product DeleteProduct(Product product)
        {
            _AllProducts.Remove(product);
            return product;
        }
        public ObservableCollection<User> GetAllUsers()
        {
            return _AllUsers;
        }

        public User AddUser(User user)
        {
            var lastUserId = GetAllUsers().LastOrDefault()?.Id ?? 0;

            User userToAdd = new User
            {
                Id = lastUserId + 1,
                Name = user.Name,
                Email = user.Email,
                Phone = user.Phone,
                Password = user.Password,
                IsAdmin = user.IsAdmin

            };

            _AllUsers.Add(userToAdd);
            
            SaveData(true);
            return userToAdd;
        }

        public User GetUserByID(int id)
        {
            return _AllUsers.FirstOrDefault(u => u.Id == id);
        }
        public User UpdateUser(User updatedUser)
        {
            User userToUpdate = GetAllUsers().FirstOrDefault(u => u.Id == updatedUser.Id);
            if (userToUpdate != null)
            {
                userToUpdate.Name = updatedUser.Name;
                userToUpdate.Email = updatedUser.Email;
                userToUpdate.Phone = updatedUser.Phone;
                userToUpdate.Password = updatedUser.Password;
                userToUpdate.IsAdmin = updatedUser.IsAdmin;
            }
            return userToUpdate;
        }

        public User DeleteUser(User user)
        {
            _AllUsers.Remove(user);
            return user;
        }
    }
}
