//using OrderManagementSystem.Repositories;
//using OrderManagementSystemServer.Repository;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
            if (category == null)
            {
                throw new ArgumentNullException(nameof(category), "The category to be added is null.");
            }

            if (string.IsNullOrWhiteSpace(category.Name))
            {
                throw new ArgumentException("The category name cannot be null or empty.", nameof(category.Name));
            }

            if (_AllCategories.Any(c => c.Name.Equals(category.Name, StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidOperationException($"A category with the name '{category.Name}' already exists.");
            }

            int lastCategoryId = GetAllCategories().LastOrDefault()?.Id ?? 0;
            Category categoryToAdd = new Category { Id = lastCategoryId + 1, Name = category.Name, Description = category.Description, Picture = category.Picture };

            _AllCategories.Add(categoryToAdd);
            return categoryToAdd;
        }
        public Category DeleteCategory(Category category)
        {



            Category categoryToDelete = _AllCategories.FirstOrDefault(c => c.Id == category.Id);

            if (category == null || categoryToDelete == null)
            {
                throw new ArgumentNullException(nameof(category), "The category to be deleted is null.");
            }
            //if (categoryToDelete != null)
            //{
            _AllCategories.Remove(categoryToDelete);
            //}

            //_AllCategories.Remove(categoryToDelete);
            return categoryToDelete;

        }
        public Category UpdateCategory(Category updatedCategory)
        {
            Category categoryToUpdate = GetAllCategories().FirstOrDefault(c => c.Id == updatedCategory.Id);

            if (categoryToUpdate is null)
            {
                throw new Exception($"No category found with given ID {updatedCategory.Id}");
            }



            categoryToUpdate.Name = updatedCategory.Name;
            categoryToUpdate.Description = updatedCategory.Description;
            categoryToUpdate.Picture = updatedCategory.Picture;

            return categoryToUpdate;
        }
        public ObservableCollection<Order> GetAllOrders()
        {
            return _AllOrders;
        }
        //public Order GetOrderById(int id)
        //{
        //    return _AllOrders.FirstOrDefault(o => o.Id == id);
        //}
        public Order AddOrder(Order order)
        {

            if (order == null)
            {
                throw new ArgumentNullException(nameof(order), "The order to be added is null.");
            }

            if (string.IsNullOrEmpty(order.User.Name))
            {
                throw new ArgumentException("The user name submitting the order cannot be null or empty.", nameof(order.User.Name));
            }

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

            if (orderToUpdate == null)
            {
                throw new ArgumentNullException(nameof(updatedOrder), "The order to be updated is null.");
            }

            //if (orderToUpdate != null)
            //{
            // Update the existing order's properties
            orderToUpdate.User = updatedOrder.User;
            orderToUpdate.OrderDate = updatedOrder.OrderDate;
            orderToUpdate.Status = updatedOrder.Status;
            orderToUpdate.ShippedDate = updatedOrder.ShippedDate;
            orderToUpdate.ShippingAddress = updatedOrder.ShippingAddress;
            orderToUpdate.OrderDetails = updatedOrder.OrderDetails;
            //}
            //else
            //{
            //MessageBox.Show("Order not found.");
            //}
            return orderToUpdate;
        }
        public Order DeleteOrder(Order order)
        {
            Order orderToDelete = _AllOrders.FirstOrDefault(o => o.Id == order.Id);

            if (order == null || orderToDelete == null)
            {
                throw new ArgumentNullException(nameof(order), "The order to be deleted is null.");
            }


            //if (orderToDelete != null)
            //{
            _AllOrders.Remove(orderToDelete);
            //}
            //_AllOrders.Remove(orderToDelete);
            return orderToDelete;
        }
        public ObservableCollection<Product> GetAllProducts()
        {
            return _AllProducts;
        }
        //public Product GetProductByID(int id)
        //{

        //    return _AllProducts.FirstOrDefault(p => p.Id == id);
        //}
        //public Product GetProductByName(OrderDetail orderDetail)
        //{
        //    return _AllProducts.FirstOrDefault(p => p.Name == orderDetail.Product.Name);
        //}
        public Product AddProduct(Product product)
        {

            if (product == null)
            {
                throw new ArgumentNullException(nameof(product), "The product to be added is null.");
            }

            if (string.IsNullOrWhiteSpace(product.Name))
            {
                throw new ArgumentException("The product name cannot be null or empty.", nameof(product.Name));
            }

            if (_AllProducts.Any(p => p.Name.Equals(product.Name, StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidOperationException($"A product with the name '{product.Name}' already exists.");
            }

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

            if (productToUpdate == null)
            {
                throw new ArgumentNullException(nameof(updatedProduct), "The product to be updated is null.");
            }

            //if (productToUpdate != null)
            //{
            productToUpdate.Name = updatedProduct.Name;
            productToUpdate.Description = updatedProduct.Description;
            productToUpdate.Category = updatedProduct.Category;
            productToUpdate.Picture = updatedProduct.Picture;
            productToUpdate.UnitPrice = updatedProduct.UnitPrice;
            productToUpdate.UnitsInStock = updatedProduct.UnitsInStock;
            //}
            return productToUpdate;
        }
        public Product DeleteProduct(Product product)
        {
            //needs to be optimized, as currently it checks equality first in firstordefault and again in remove
            //Product productToDelete = _AllProducts.FirstOrDefault(p => p.Equals(product));
            //_AllProducts.Remove(product);
            Product productToDelete = _AllProducts.FirstOrDefault(p => p.Id == product.Id);

            if (product == null || productToDelete == null)
            {
                throw new ArgumentNullException(nameof(product), "The product to be deleted is null.");
            }

            //if (productToDelete != null)
            //{
            _AllProducts.Remove(productToDelete);
            //}
            //if (_AllProducts.Remove(product))
            //{
            //    Debug.WriteLine("Product successfully removed.");
            //}
            //else
            //{
            //    Debug.WriteLine("Failed to remove product. Check Equals and GetHashCode implementation.");
            //}

            return productToDelete;
        }
        public ObservableCollection<User> GetAllUsers()
        {
            return _AllUsers;
        }
        public User AddUser(User user)
        {

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "The user to be added is null.");
            }

            //if (string.IsNullOrWhiteSpace(user.Name))
            //{
            //    throw new ArgumentException("The user name cannot be null or empty.", nameof(user.Name));
            //}

            if (_AllUsers.Any(u => u.Email.Equals(user.Email, StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidOperationException($"A user with the email '{user.Email}' already exists.");
            }

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
        //public User GetUserByID(int id)
        //{
        //    return _AllUsers.FirstOrDefault(u => u.Id == id);
        //}
        public User UpdateUser(User updatedUser)
        {
            User userToUpdate = GetAllUsers().FirstOrDefault(u => u.Id == updatedUser.Id);

            if (userToUpdate == null)
            {
                throw new ArgumentNullException(nameof(updatedUser), "The user to be updated is null.");
            }

            //if (userToUpdate != null)
            //{
            userToUpdate.Name = updatedUser.Name;
            userToUpdate.Email = updatedUser.Email;
            userToUpdate.Phone = updatedUser.Phone;
            userToUpdate.Password = updatedUser.Password;
            userToUpdate.IsAdmin = updatedUser.IsAdmin;
            //}
            return userToUpdate;
        }
        //public User DeleteUser(User user)
        //{
        //    _AllUsers.Remove(user);
        //    return user;
        //}
        public User DeleteUser(User user)
        {



            User userToDelete = _AllUsers.FirstOrDefault(u => u.Id == user.Id);
            if (user == null || userToDelete == null)
            {
                throw new ArgumentNullException(nameof(user), "The user to be deleted is null.");
            }
            //if (userToDelete != null)
            //{
            _AllUsers.Remove(userToDelete);
            //}
            return userToDelete;
        }
    }
}
