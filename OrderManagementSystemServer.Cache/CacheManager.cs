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
        private ObservableCollection<Category> m_lstCategories;
        private ObservableCollection<Order> m_lstOrders;
        private ObservableCollection<Product> m_lstProducts;
        private ObservableCollection<User> m_lstUsers;

        public ObservableCollection<Category> Categories
        {
            get { return m_lstCategories; }
            set { m_lstCategories = value; }
        }
        public ObservableCollection<Order> Orders
        {
            get { return m_lstOrders; }
            set { m_lstOrders = value; }
        }
        public ObservableCollection<Product> Products
        {
            get { return m_lstProducts; }
            set { m_lstProducts = value; }
        }
        public ObservableCollection<User> Users
        {
            get { return m_lstUsers; }
            set { m_lstUsers = value; }
        }

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

            Users = CustomXMLSerializer.DeserializeFromXml<ObservableCollection<User>>($"{Constants.XMLDirectoryPath}{Constants.UserDataStoreName}");

            Categories = CustomXMLSerializer.DeserializeFromXml<ObservableCollection<Category>>($"{Constants.XMLDirectoryPath}{Constants.CategoryDataStoreName}");

            Products = CustomXMLSerializer.DeserializeFromXml<ObservableCollection<Product>>($"{Constants.XMLDirectoryPath}{Constants.ProductDataStoreName}");

            Orders = CustomXMLSerializer.DeserializeFromXml<ObservableCollection<Order>>($"{Constants.XMLDirectoryPath}{Constants.OrderDataStoreName}");
        }

        public void SaveData(bool onlyUser)
        {
            if (onlyUser is false)
            {
                CustomXMLSerializer.SerializeToXml($"{Constants.XMLDirectoryPath}{Constants.UserDataStoreName}", Users);
                CustomXMLSerializer.SerializeToXml($"{Constants.XMLDirectoryPath}{Constants.CategoryDataStoreName}", Categories);
                CustomXMLSerializer.SerializeToXml($"{Constants.XMLDirectoryPath}{Constants.ProductDataStoreName}", Products);
                CustomXMLSerializer.SerializeToXml($"{Constants.XMLDirectoryPath}{Constants.OrderDataStoreName}", Orders);
            }
            CustomXMLSerializer.SerializeToXml($"{Constants.XMLDirectoryPath}{Constants.UserDataStoreName}", Users);

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

            if (Categories.Any(c => c.Name.Equals(category.Name, StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidOperationException($"A category with the name '{category.Name}' already exists.");
            }

            int lastCategoryId = Categories.LastOrDefault()?.Id ?? 0;
            Category categoryToAdd = new Category { Id = lastCategoryId + 1, Name = category.Name, Description = category.Description};

            Categories.Add(categoryToAdd);
            return categoryToAdd;
        }

        public int DeleteCategory(string category)
        {
            int categoryId = Int32.Parse(category);
            Category categoryToDelete = Categories.FirstOrDefault(c => c.Id == categoryId);

            if (category == null || categoryToDelete == null)
            {
                throw new ArgumentNullException(nameof(category), "The category to be deleted is null.");
            }

            Categories.Remove(categoryToDelete);

            return categoryId;

        }
   

        public Category UpdateCategory(Category updatedCategory)
        {
            Category categoryToUpdate = Categories.FirstOrDefault(c => c.Id == updatedCategory.Id);

            if (categoryToUpdate is null)
            {
                throw new Exception($"No category found with given ID {updatedCategory.Id}");
            }

            categoryToUpdate.Name = updatedCategory.Name;
            categoryToUpdate.Description = updatedCategory.Description;

            return categoryToUpdate;
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

            int lastOrderId = Orders.LastOrDefault()?.Id ?? 0;

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

            Orders.Add(orderToAdd);
            return orderToAdd;
        }
        public Order UpdateOrder(Order updatedOrder)
        {
            // Retrieve the existing order with the same ID
            Order orderToUpdate = Orders.FirstOrDefault(o => o.Id == updatedOrder.Id);

            if (orderToUpdate == null)
            {
                throw new ArgumentNullException(nameof(updatedOrder), "The order to be updated is null.");
            }

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
            Order orderToDelete = Orders.FirstOrDefault(o => o.Id == orderId);

            if (order == null || orderToDelete == null)
            {
                throw new ArgumentNullException(nameof(order), "The order to be deleted is null.");
            }

            Orders.Remove(orderToDelete);
           
            return orderId;
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

            if (Products.Any(p => p.Name.Equals(product.Name, StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidOperationException($"A product with the name '{product.Name}' already exists.");
            }

            int lastProductId = Products.LastOrDefault()?.Id ?? 0;

            Product productToAdd = new Product
            {
                Id = lastProductId + 1,
                Name = product.Name,
                UnitPrice = Convert.ToDecimal(product.UnitPrice),
                UnitsInStock = Convert.ToInt32(product.UnitsInStock),
                Description = product.Description,
                Category = product.Category
            };
            Products.Add(productToAdd);
            return productToAdd;
        }
        public Product UpdateProduct(Product updatedProduct)
        {
            Product productToUpdate = Products.FirstOrDefault(p => p.Id == updatedProduct.Id);

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
            Product productToDelete = Products.FirstOrDefault(p => p.Id == productId);


            if (product == null || productToDelete == null)
            {
                throw new ArgumentNullException(nameof(product), "The product to be deleted is null.");
            }

            Products.Remove(productToDelete);

            return productId;
        }
       
        public User AddUser(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "The user to be added is null.");
            }

            if (Users.Any(u => u.Email.Equals(user.Email, StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidOperationException($"A user with the email '{user.Email}' already exists.");
            }

            int lastUserId = Users.LastOrDefault()?.Id ?? 0;
           
            User userToAdd = new User
            {
                Id = lastUserId + 1,
                Name = user.Name,
                Email = user.Email,
                Phone = user.Phone,
                Password = PasswordHashUtil.HashPassword(user.Password),
                IsAdmin = user.IsAdmin
            };

            Users.Add(userToAdd);

            SaveData(true);
            return userToAdd;
        }
       
        public User UpdateUser(User updatedUser)
        {
            User userToUpdate = Users.FirstOrDefault(u => u.Id == updatedUser.Id);

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
            userToUpdate.UserApprovalStatus = updatedUser.UserApprovalStatus;
            
            return userToUpdate;
        }
        
        public int DeleteUser(string user)
        {

            int userId = Int32.Parse(user);

            User userToDelete = Users.FirstOrDefault(u => u.Id == userId);
            if (user == null || userToDelete == null)
            {
                throw new ArgumentNullException(nameof(user), "The user to be deleted is null.");
            }
           
            userToDelete.IsArchived = true;
            return userId;
        }
    }
}
