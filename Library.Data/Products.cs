using System;

namespace Library.Data
{
    public class Products
    {
        public string Description { get; set; }
        public string Name { get; set; }
        public string ImageName { get; set; }
        public int ProductId { get; set; }
        public int CategoryId { get; set; }
        public decimal Price { get; set; }
    }
    public class Categories
    {
        public string Name { get; set; }       
        public int Id { get; set; }
    }
    public class ShoppingCart
    {
        public DateTime Date { get; set; }
        public int Id { get; set; }
    }
    public class ShoppingCartItems
    {
         public int CartId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
    }
}
