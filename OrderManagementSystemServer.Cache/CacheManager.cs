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
        public ObservableCollection<Category> objCategories { get; private set; }
        public ObservableCollection<Order> objOrders { get; private set; }
        public ObservableCollection<Product> objProducts { get; private set; }
        public ObservableCollection<User> objUsers { get; private set; }

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

            objUsers = CustomXMLSerializer.DeserializeFromXml<ObservableCollection<User>>($"{Constants.XMLDirectoryPath}{Constants.UserDataStoreName}");

            objCategories = CustomXMLSerializer.DeserializeFromXml<ObservableCollection<Category>>($"{Constants.XMLDirectoryPath}{Constants.CategoryDataStoreName}");

            objProducts = CustomXMLSerializer.DeserializeFromXml<ObservableCollection<Product>>($"{Constants.XMLDirectoryPath}{Constants.ProductDataStoreName}");

            objOrders = CustomXMLSerializer.DeserializeFromXml<ObservableCollection<Order>>($"{Constants.XMLDirectoryPath}{Constants.OrderDataStoreName}");


        }

        public void SaveData(bool onlyUser)
        {
            if (onlyUser is false)
            {
                CustomXMLSerializer.SerializeToXml($"{Constants.XMLDirectoryPath}{Constants.UserDataStoreName}", objUsers);
                CustomXMLSerializer.SerializeToXml($"{Constants.XMLDirectoryPath}{Constants.CategoryDataStoreName}", objCategories);
                CustomXMLSerializer.SerializeToXml($"{Constants.XMLDirectoryPath}{Constants.ProductDataStoreName}", objProducts);
                CustomXMLSerializer.SerializeToXml($"{Constants.XMLDirectoryPath}{Constants.OrderDataStoreName}", objOrders);
            }
            CustomXMLSerializer.SerializeToXml($"{Constants.XMLDirectoryPath}{Constants.UserDataStoreName}", objUsers);

        }

        public Category GetCategoryById(int id)
        {
            return objCategories.FirstOrDefault(c => c.Id == id);
        }
        public ObservableCollection<Category> GetAllCategories()
        {
            return objCategories;
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

            if (objCategories.Any(c => c.Name.Equals(category.Name, StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidOperationException($"A category with the name '{category.Name}' already exists.");
            }

            int lastCategoryId = GetAllCategories().LastOrDefault()?.Id ?? 0;
            Category categoryToAdd = new Category { Id = lastCategoryId + 1, Name = category.Name, Description = category.Description};

            objCategories.Add(categoryToAdd);
            return categoryToAdd;
        }

        public Category DeleteCategory(Category category)
        {
            Category categoryToDelete = objCategories.FirstOrDefault(c => c.Id == category.Id);

            if (category == null || categoryToDelete == null)
            {
                throw new ArgumentNullException(nameof(category), "The category to be deleted is null.");
            }
            
            objCategories.Remove(categoryToDelete);

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
            //categoryToUpdate.Picture = updatedCategory.Picture;

            return categoryToUpdate;
        }
        public ObservableCollection<Order> GetAllOrders()
        {
            return objOrders;
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

            objOrders.Add(orderToAdd);
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
        public Order DeleteOrder(Order order)
        {
            Order orderToDelete = objOrders.FirstOrDefault(o => o.Id == order.Id);

            if (order == null || orderToDelete == null)
            {
                throw new ArgumentNullException(nameof(order), "The order to be deleted is null.");
            }

            objOrders.Remove(orderToDelete);
           
            return orderToDelete;
        }
        public ObservableCollection<Product> GetAllProducts()
        {
            return objProducts;
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

            if (objProducts.Any(p => p.Name.Equals(product.Name, StringComparison.OrdinalIgnoreCase)))
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
            objProducts.Add(productToAdd);
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
            //productToUpdate.Picture = updatedProduct.Picture;
            productToUpdate.UnitPrice = updatedProduct.UnitPrice;
            productToUpdate.UnitsInStock = updatedProduct.UnitsInStock;

            return productToUpdate;
        }
        public Product DeleteProduct(Product product)
        {
            //needs to be optimized, as currently it checks equality first in firstordefault and again in remove
            //Product productToDelete = objProducts.FirstOrDefault(p => p.Equals(product));
            //objProducts.Remove(product);
            Product productToDelete = objProducts.FirstOrDefault(p => p.Id == product.Id);


            if (product == null || productToDelete == null)
            {
                throw new ArgumentNullException(nameof(product), "The product to be deleted is null.");
            }

            objProducts.Remove(productToDelete);

            return productToDelete;
        }
        public ObservableCollection<User> GetAllUsers()
        {
            return objUsers;
        }

       
        public User AddUser(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "The user to be added is null.");
            }

            if (objUsers.Any(u => u.Email.Equals(user.Email, StringComparison.OrdinalIgnoreCase)))
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

            objUsers.Add(userToAdd);

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
            userToUpdate.IsAdmin = updatedUser.IsAdmin;
            userToUpdate.ApprovalStatus = updatedUser.ApprovalStatus;
            
            return userToUpdate;
        }
        
        public User DeleteUser(User user)
        {
            User userToDelete = objUsers.FirstOrDefault(u => u.Id == user.Id);
            if (user == null || userToDelete == null)
            {
                throw new ArgumentNullException(nameof(user), "The user to be deleted is null.");
            }
           
            //objUsers.Remove(userToDelete);
            userToDelete.IsArchived = true;

            return userToDelete;
        }
    }
}
