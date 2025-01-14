using OrderManagementSystemServer.Components.Utils;
using OrderManagementSystemServer.Repository;
using OrderManagementSystemServer.Utils;
using System.Collections.ObjectModel;
using static OrderManagementSystemServer.Repository.Order;

namespace OrderManagementSystemServer.Cache
{
    public class CacheManager
    {
        private static CacheManager m_objInstance;
        public ObservableCollection<Category> m_objCategories { get;  set; }
        public ObservableCollection<Order> m_objOrders { get; set; }
        public ObservableCollection<Product> m_objProducts { get; set; }
        public ObservableCollection<User> m_objUsers { get; set; }

        private User m_objCurrentUser;

        public User CurrentUser
        {
            get { return m_objCurrentUser; }
            set { m_objCurrentUser = value; }
        }

        public static CacheManager Instance
        {
            get
            {
                if (m_objInstance == null)
                    m_objInstance = new CacheManager();
                return m_objInstance;
            }
        }

        private CacheManager()
        {

            m_objUsers = CustomXMLSerializer.DeserializeFromXml<ObservableCollection<User>>($"{Constants.XMLDirectoryPath}{Constants.UserDataStoreName}");

            m_objCategories = CustomXMLSerializer.DeserializeFromXml<ObservableCollection<Category>>($"{Constants.XMLDirectoryPath}{Constants.CategoryDataStoreName}");

            m_objProducts = CustomXMLSerializer.DeserializeFromXml<ObservableCollection<Product>>($"{Constants.XMLDirectoryPath}{Constants.ProductDataStoreName}");

            m_objOrders = CustomXMLSerializer.DeserializeFromXml<ObservableCollection<Order>>($"{Constants.XMLDirectoryPath}{Constants.OrderDataStoreName}");


        }

        public void SaveData(bool onlyUser)
        {
            if (onlyUser is false)
            {
                CustomXMLSerializer.SerializeToXml($"{Constants.XMLDirectoryPath}{Constants.UserDataStoreName}", m_objUsers);
                CustomXMLSerializer.SerializeToXml($"{Constants.XMLDirectoryPath}{Constants.CategoryDataStoreName}", m_objCategories);
                CustomXMLSerializer.SerializeToXml($"{Constants.XMLDirectoryPath}{Constants.ProductDataStoreName}", m_objProducts);
                CustomXMLSerializer.SerializeToXml($"{Constants.XMLDirectoryPath}{Constants.OrderDataStoreName}", m_objOrders);
            }
            CustomXMLSerializer.SerializeToXml($"{Constants.XMLDirectoryPath}{Constants.UserDataStoreName}", m_objUsers);

        }

        public Category GetCategoryById(int id)
        {
            return m_objCategories.FirstOrDefault(c => c.Id == id);
        }
        public ObservableCollection<Category> GetAllCategories()
        {
            return m_objCategories;
        }
        
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

            if (m_objCategories.Any(c => c.Name.Equals(category.Name, StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidOperationException($"A category with the name '{category.Name}' already exists.");
            }

            int lastCategoryId = GetAllCategories().LastOrDefault()?.Id ?? 0;
            Category categoryToAdd = new Category { Id = lastCategoryId + 1, Name = category.Name, Description = category.Description};

            m_objCategories.Add(categoryToAdd);
            return categoryToAdd;
        }

        public int DeleteCategory(string category)
        {
            int categoryId = Int32.Parse(category);
            Category categoryToDelete = m_objCategories.FirstOrDefault(c => c.Id == categoryId);

            if (category == null || categoryToDelete == null)
            {
                throw new ArgumentNullException(nameof(category), "The category to be deleted is null.");
            }

            m_objCategories.Remove(categoryToDelete);

            return categoryId;

        }
        //public Category DeleteCategory(Category category)
        //{
        //    Category categoryToDelete = m_objCategories.FirstOrDefault(c => c.Id == category.Id);

        //    if (category == null || categoryToDelete == null)
        //    {
        //        throw new ArgumentNullException(nameof(category), "The category to be deleted is null.");
        //    }
            
        //    m_objCategories.Remove(categoryToDelete);

        //    return categoryToDelete;

        //}

        public Category UpdateCategory(Category updatedCategory)
        {
            Category categoryToUpdate = GetAllCategories().FirstOrDefault(c => c.Id == updatedCategory.Id);

            if (categoryToUpdate is null)
            {
                throw new Exception($"No category found with given ID {updatedCategory.Id}");
            }

            categoryToUpdate.Name = updatedCategory.Name;
            categoryToUpdate.Description = updatedCategory.Description;

            return categoryToUpdate;
        }
        public ObservableCollection<Order> GetAllOrders()
        {
            return m_objOrders;
        }
        
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

            m_objOrders.Add(orderToAdd);
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

            // Update the existing order's properties
            orderToUpdate.User = updatedOrder.User;
            orderToUpdate.OrderDate = updatedOrder.OrderDate;
            orderToUpdate.Status = updatedOrder.Status;
            orderToUpdate.ShippedDate = updatedOrder.ShippedDate;
            orderToUpdate.ShippingAddress = updatedOrder.ShippingAddress;
            orderToUpdate.OrderDetails = updatedOrder.OrderDetails;
          
            return orderToUpdate;
        }
        public int DeleteOrder(string order)
        {
            int orderId = Int32.Parse(order);
            Order orderToDelete = m_objOrders.FirstOrDefault(o => o.Id == orderId);

            if (order == null || orderToDelete == null)
            {
                throw new ArgumentNullException(nameof(order), "The order to be deleted is null.");
            }

            m_objOrders.Remove(orderToDelete);
           
            return orderId;
        }
        public ObservableCollection<Product> GetAllProducts()
        {
            return m_objProducts;
        }
       
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

            if (m_objProducts.Any(p => p.Name.Equals(product.Name, StringComparison.OrdinalIgnoreCase)))
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
            m_objProducts.Add(productToAdd);
            return productToAdd;
        }
        public Product UpdateProduct(Product updatedProduct)
        {
            Product productToUpdate = GetAllProducts().FirstOrDefault(p => p.Id == updatedProduct.Id);

            if (productToUpdate == null)
            {
                throw new ArgumentNullException(nameof(updatedProduct), "The product to be updated is null.");
            }

            productToUpdate.Name = updatedProduct.Name;
            productToUpdate.Description = updatedProduct.Description;
            productToUpdate.Category = updatedProduct.Category;
            productToUpdate.UnitPrice = updatedProduct.UnitPrice;
            productToUpdate.UnitsInStock = updatedProduct.UnitsInStock;

            return productToUpdate;
        }
        public int DeleteProduct(string product)
        {
            int productId = Int32.Parse(product);
            Product productToDelete = m_objProducts.FirstOrDefault(p => p.Id == productId);


            if (product == null || productToDelete == null)
            {
                throw new ArgumentNullException(nameof(product), "The product to be deleted is null.");
            }

            m_objProducts.Remove(productToDelete);

            return productId;
        }
        public ObservableCollection<User> GetAllUsers()
        {
            return m_objUsers;
        }

       
        public User AddUser(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "The user to be added is null.");
            }

            if (m_objUsers.Any(u => u.Email.Equals(user.Email, StringComparison.OrdinalIgnoreCase)))
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
                Password = PasswordHashUtil.HashPassword(user.Password),
                IsAdmin = user.IsAdmin
            };

            m_objUsers.Add(userToAdd);

            SaveData(true);
            return userToAdd;
        }
       
        public User UpdateUser(User updatedUser)
        {
            User userToUpdate = GetAllUsers().FirstOrDefault(u => u.Id == updatedUser.Id);

            if (userToUpdate == null)
            {
                throw new ArgumentNullException(nameof(updatedUser), "The user to be updated is null.");
            }

            userToUpdate.Name = updatedUser.Name;
            userToUpdate.Email = updatedUser.Email;
            userToUpdate.Phone = updatedUser.Phone;
            userToUpdate.Password = PasswordHashUtil.HashPassword(updatedUser.Password);
            userToUpdate.IsArchived = updatedUser.IsArchived;
            userToUpdate.IsAdmin = updatedUser.IsAdmin;
            userToUpdate.ApprovalStatus = updatedUser.ApprovalStatus;
            
            return userToUpdate;
        }
        
        public int DeleteUser(string user)
        {

            int userId = Int32.Parse(user);

            User userToDelete = m_objUsers.FirstOrDefault(u => u.Id == userId);
            if (user == null || userToDelete == null)
            {
                throw new ArgumentNullException(nameof(user), "The user to be deleted is null.");
            }
           
            //m_objUsers.Remove(userToDelete);
            userToDelete.IsArchived = true;
            return userId;
            //return userToDelete;
        }
    }
}
