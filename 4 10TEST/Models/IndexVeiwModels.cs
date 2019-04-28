using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Library.Data;



public class IndexVeiwModels
{
    public IEnumerable<Categories> AllCategories {get;set;}
    public IEnumerable<Products> Products { get; set; }
}
public class CartVeiwModels
{
    public IEnumerable<ShoppingCartItems> CartItems{ get; set; }
    public decimal CartTotal { get; set; }
}


