using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Library.Data
{
    public class Manager
    {
        public string _connectionString { get; set; }
        public Manager(string connectionString)
        {
            _connectionString = connectionString;
        }

        public int AddProduct(Products p)
        {
            SqlConnection conn = new SqlConnection(_connectionString);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = " INSERT into Products(Name,ImageName,Description,CategoryId,Price) " +
            "VALUES(@name,@imageName,@description,@catId,@price)" +
            "Select SCOPE_Identity()";
            cmd.Parameters.AddWithValue("@imageName", p.ImageName);
            cmd.Parameters.AddWithValue("@name", p.Name);
            cmd.Parameters.AddWithValue("@description", p.Description);
            cmd.Parameters.AddWithValue("@catId", p.CategoryId);
            cmd.Parameters.AddWithValue("@price", p.Price);
            conn.Open();
           var id = cmd.ExecuteScalar();
            //cmd.ExecuteNonQuery();
            conn.Close();
            conn.Dispose();
            return int.Parse(id.ToString());

        }
        public void AddCategory(Categories c)
        {
            SqlConnection conn = new SqlConnection(_connectionString);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = " INSERT into Categories(Name) VALUES(@name)";
            cmd.Parameters.AddWithValue("@name", c.Name);
            conn.Open();
            // var id = cmd.ExecuteScalar();
            cmd.ExecuteNonQuery();
            conn.Close();
            conn.Dispose();

        }
        public IEnumerable<Categories> GetCategories()
        {
            SqlConnection conn = new SqlConnection(_connectionString);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM Categories";
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            List<Categories> categories = new List<Categories>();
            while (reader.Read())
            {
                categories.Add(new Categories
                {
                    Name = (string)reader["Name"],
                    Id = (int)reader["Id"]
                });
            }
            return categories;

        }
        public IEnumerable<Products> GetProductsForCat(int id)
        {
            SqlConnection conn = new SqlConnection(_connectionString);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM Products where CategoryId=@catId ";
            cmd.Parameters.AddWithValue("@catId", id);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            List<Products> products = new List<Products>();
            while (reader.Read())
            {
                products.Add(new Products
                {
                    Name = (string)reader["Name"],
                    ProductId = (int)reader["ProductId"],
                    CategoryId = (int)reader["CategoryId"],
                    Description = (string)reader["Description"],
                    ImageName = (string)reader["ImageName"],
                    Price = (decimal)reader["Price"]
                });

            }
            return products;
        }
        public Products GetProductsById(int id)
        {
            SqlConnection conn = new SqlConnection(_connectionString);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT TOP 1 * FROM Products where ProductId=@id ";
            cmd.Parameters.AddWithValue("@id", id);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            // List<Products> products = new List<Products>();
            if (reader.Read())
            {
                var p = new Products
                {
                    Name = (string)reader["Name"],
                    ProductId = (int)reader["ProductId"],
                    CategoryId = (int)reader["CategoryId"],
                    Description = (string)reader["Description"],
                    ImageName = (string)reader["ImageName"],
                    Price = (decimal)reader["Price"]
                };
                return p;
            }
            return null;
        }

        public int NewCart()
        {
            SqlConnection conn = new SqlConnection(_connectionString);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = " INSERT into ShoppingCart(Date) " +
            "VALUES(@date) Select Scope_Identity()";
            cmd.Parameters.AddWithValue("@date", DateTime.Now);
            conn.Open();
            var id = cmd.ExecuteScalar();
            conn.Close();
            conn.Dispose();
            return int.Parse(id.ToString());
        }
        public void AddItemsToCart(string cartId, int productId, int amount)
        {
            SqlConnection conn = new SqlConnection(_connectionString);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = " INSERT into ShoppingCartItems(ProductId,Quantity,CartId) " +
            "VALUES(@productId,@amount,@cartId)";
            cmd.Parameters.AddWithValue("@productId", productId);
            cmd.Parameters.AddWithValue("@amount", amount);
            cmd.Parameters.AddWithValue("@cartId", cartId);
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
            conn.Dispose();
        }
        public IEnumerable<ShoppingCartItems> GetCartItems(int id)
        {
            SqlConnection conn = new SqlConnection(_connectionString);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM ShoppingCartItems sc " +
                 "LEFT JOIN Products p on p.ProductId = sc.ProductId " +
                 "where sc.CartId=@cartId";
            cmd.Parameters.AddWithValue("@cartId", id);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            List<ShoppingCartItems> items = new List<ShoppingCartItems>();
            while (reader.Read())
            {
                items.Add(new ShoppingCartItems
                {
                    ProductId = (int)reader["ProductId"],
                    Quantity = (int)reader["Quantity"],
                    ProductName = (string)reader["Name"],
                    Price = (decimal)reader["Price"]
                });

            }
            return items;
        }

        public decimal GetCartTotal(int id)
        {
            SqlConnection conn = new SqlConnection(_connectionString);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"SELECT p.Price*sc.Quantity as 'Total' from ShoppingCartItems sc 
             LEFT JOIN Products p on p.ProductId = sc.ProductId
              where sc.CartId = @cartID";
            cmd.Parameters.AddWithValue("@cartId", id);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            decimal total = 0;
            while (reader.Read())
            {
                total = (decimal)reader["total"];
            }
            return total;

        }
        public void DeleteFromCart(int itemId, int cartId)
        {
            SqlConnection conn = new SqlConnection(_connectionString);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"DELETE FROM shoppingCartItems WHERE ProductId = @itemId and cartId= @cartId";
            cmd.Parameters.AddWithValue("@itemId", itemId);
            cmd.Parameters.AddWithValue("@cartId", cartId);
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
            cmd.Dispose();           

        }
        public void EditItem(int itemId,int quantity, int cartId)
        {
            SqlConnection conn = new SqlConnection(_connectionString);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"UPDATE shoppingCartItems SET Quantity=@quantity WHERE ProductId = @itemId and cartId= @cartId";
            cmd.Parameters.AddWithValue("@itemId", itemId);
            cmd.Parameters.AddWithValue("@cartId", cartId);
            cmd.Parameters.AddWithValue("@quantity", quantity);
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
            cmd.Dispose();

        }
    }
}
