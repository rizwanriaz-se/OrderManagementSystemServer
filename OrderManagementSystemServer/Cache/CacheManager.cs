using System.Collections.ObjectModel;
using OrderManagementSystemServer.Utils;
using OrderManagementSystemServer.Cache.Models;

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


        private string m_stDataStorePath = $"{Directory.GetCurrentDirectory().Split(new string[] { "\\bin" }, StringSplitOptions.None)[0]}\\DataStore\\";

        private string m_stUserDataStorePath = "UserDataStore.xml";
        private string m_stCategoryDataStorePath = "CategoryDataStore.xml";
        private string m_stProductDataStorePath = "ProductDataStore.xml";
        private string m_stOrderDataStorePath = "OrderDataStore.xml";

        //public ObservableCo
        private CacheManager()
        {

            _AllUsers = CustomXMLSerializer.DeserializeFromXml<ObservableCollection<User>>($"{m_stDataStorePath}{m_stUserDataStorePath}");

            _AllCategories = CustomXMLSerializer.DeserializeFromXml<ObservableCollection<Category>>($"{m_stDataStorePath}{m_stCategoryDataStorePath}");

            _AllProducts = CustomXMLSerializer.DeserializeFromXml<ObservableCollection<Product>>($"{m_stDataStorePath}{m_stProductDataStorePath}");

            _AllOrders = CustomXMLSerializer.DeserializeFromXml<ObservableCollection<Order>>($"{m_stDataStorePath}{m_stOrderDataStorePath}");


        }

        public void SaveData(bool onlyUser)
        {
            if (onlyUser is false)
            {
                CustomXMLSerializer.SerializeToXml($"{m_stDataStorePath}{m_stUserDataStorePath}", _AllUsers);
                CustomXMLSerializer.SerializeToXml($"{m_stDataStorePath}{m_stCategoryDataStorePath}", _AllCategories);
                CustomXMLSerializer.SerializeToXml($"{m_stDataStorePath}{m_stProductDataStorePath}", _AllProducts);
                CustomXMLSerializer.SerializeToXml($"{m_stDataStorePath}{m_stOrderDataStorePath}", _AllOrders);
            }
            CustomXMLSerializer.SerializeToXml($"{m_stDataStorePath}{m_stUserDataStorePath}", _AllUsers);

        }

        public Category GetCategoryById(int id)
        {
            return _AllCategories.FirstOrDefault(c => c.Id == id);
        }
        public ObservableCollection<Category> GetAllCategories()
        {
            return _AllCategories;
        }
        public void AddCategory(Category category)
        {
            _AllCategories.Add(category);
        }
        public void DeleteCategory(Category category)
        {
            _AllCategories.Remove(category);
        }
        public void UpdateCategory(Category category)
        {
            var categoryToUpdate = _AllCategories.FirstOrDefault(c => c.Id == category.Id);
            categoryToUpdate.Name = category.Name;
            categoryToUpdate.Description = category.Description;
            categoryToUpdate.Picture = category.Picture;

            //MessageBox.Show("Category updated successfully");
        }
        public ObservableCollection<Order> GetAllOrders()
        {
            return _AllOrders;
        }
        public Order GetOrderById(int id)
        {
            return _AllOrders.FirstOrDefault(o => o.Id == id);
        }
        public void AddOrder(Order order)
        {
            //order.Id = _AllOrders.Max(o => o.Id) + 1;
            _AllOrders.Add(order);
            //return order;
        }
        public void UpdateOrder(Order updatedOrder)
        {
            // Retrieve the existing order with the same ID
            var existingOrder = _AllOrders.FirstOrDefault(o => o.Id == updatedOrder.Id);

            if (existingOrder != null)
            {
                // Update the existing order's properties
                existingOrder.User = updatedOrder.User;
                existingOrder.OrderDate = updatedOrder.OrderDate;
                existingOrder.Status = updatedOrder.Status;
                existingOrder.ShippedDate = updatedOrder.ShippedDate;
                existingOrder.ShippingAddress = updatedOrder.ShippingAddress;
                existingOrder.OrderDetails = updatedOrder.OrderDetails;

                //MessageBox.Show($"Order Updated: {existingOrder.Id}, {existingOrder.User.Name}, {existingOrder.OrderDate}, " +
                //$"{existingOrder.Products.Count} products, {existingOrder.ShippedDate}, {existingOrder.ShippingAddress}");
            }
            else
            {
                //MessageBox.Show("Order not found.");
            }
        }
        public void DeleteOrder(Order order)
        {
            _AllOrders.Remove(order);
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
        public void AddProduct(Product product)
        {
            _AllProducts.Add(product);
        }
        public void UpdateProduct(Product product)
        {
            var existingProduct = _AllProducts.FirstOrDefault(p => p.Id == product.Id);
            if (existingProduct != null)
            {
                existingProduct.Name = product.Name;
                existingProduct.Description = product.Description;
                existingProduct.Category = product.Category;
                existingProduct.Picture = product.Picture;
                existingProduct.UnitPrice = product.UnitPrice;
                existingProduct.UnitsInStock = product.UnitsInStock;

                //MessageBox.Show($"Product Updated: {existingProduct.Id}, {existingProduct.Name}, {existingProduct.Description}, {existingProduct.Category.Name}, {existingProduct.UnitPrice}, {existingProduct.UnitsInStock}");
            }
        }
        public void DeleteProduct(Product product)
        {
            _AllProducts.Remove(product);
            //int productId = product.Id;

            //var productToDelete = _AllProducts.FirstOrDefault(p => p.Id == productId);
            //if (productToDelete != null)
            //{
            //    _AllProducts.Remove(productToDelete);
            //}
        }
        public ObservableCollection<User> GetAllUsers()
        {
            return _AllUsers;
        }

        public void AddUser(User user)
        {
            _AllUsers.Add(user);
            SaveData(true);
        }

        //public void LoginUser(User user)
        //{

        //}

        public User GetUserByID(int id)
        {
            return _AllUsers.FirstOrDefault(u => u.Id == id);
        }
        public void UpdateUser(User user)
        {
            var existingUser = _AllUsers.FirstOrDefault(u => u.Id == user.Id);
            if (existingUser != null)
            {
                existingUser.Name = user.Name;
                existingUser.Email = user.Email;
                existingUser.Phone = user.Phone;
                existingUser.Password = user.Password;
                existingUser.IsAdmin = user.IsAdmin;

                //MessageBox.Show($"Product Updated: {existingProduct.Id}, {existingProduct.Name}, {existingProduct.Description}, {existingProduct.Category.Name}, {existingProduct.UnitPrice}, {existingProduct.UnitsInStock}");
            }
        }

        public void DeleteUser(User user)
        {
            _AllUsers.Remove(user);
        }

        //public User GetCurrentUser()
        //{
        //    return m_CurrentUser;
        //}

        //public void SetCurrentUser(User user)
        //{
        //    m_CurrentUser = user;
        //}
    }
}
