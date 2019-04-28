using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using _4_10TEST.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Library.Data;
using Microsoft.AspNetCore.Http;

namespace _4_10TEST.Controllers
{
    public class HomeController : Controller
    {

        private IHostingEnvironment _environment;
        private string _connectionString;

        public HomeController(IHostingEnvironment environment,
            IConfiguration configuration)
        {
            _environment = environment;
            _connectionString = configuration.GetConnectionString("ConStr");
        }
        
        public IActionResult Index(int id)
        {
            var mgr = new Manager(_connectionString);
            IndexVeiwModels vm = new IndexVeiwModels();
            vm.AllCategories = mgr.GetCategories();
            vm.Products = mgr.GetProductsForCat(id);
            return View(vm);
        }
        public IActionResult JasonProducts(int id)
        {
            var mgr = new Manager(_connectionString);
            return Json(mgr.GetProductsForCat(id));
        }

        public IActionResult Products(int id)
        {
            Manager mgr = new Manager(_connectionString);
            return View(mgr.GetProductsById(id));
        }

        [HttpPost]
        public IActionResult addToCart(int amount, int productId)
        {
            Manager mgr = new Manager(_connectionString);

            string MyCart = HttpContext.Session.GetString("CartId");
            if (MyCart == null)
            {
                MyCart = mgr.NewCart().ToString();
                HttpContext.Session.SetString("CartId", $"{MyCart}");
            }
            mgr.AddItemsToCart(MyCart, productId, amount);
            return Json(amount);
        }

        public IActionResult MyCart()
        {
            Manager mgr = new Manager(_connectionString);
            string MyCartId = HttpContext.Session.GetString("CartId");
            CartVeiwModels cvm = new CartVeiwModels
            {
                CartTotal = mgr.GetCartTotal(int.Parse(MyCartId)),
                CartItems = mgr.GetCartItems(int.Parse(MyCartId))
            };
            return View(cvm);
        }
        public IActionResult DeleteFromCart(int ItemId)
        {
            Manager mgr = new Manager(_connectionString);
            string MyCartId = HttpContext.Session.GetString("CartId");
            mgr.DeleteFromCart(ItemId,int.Parse(MyCartId));
            return Redirect("/home/MyCart");
        }
        public IActionResult EditItem(int ProductId, int Quantity)
        {
            Manager mgr = new Manager(_connectionString);
            string MyCartId = HttpContext.Session.GetString("CartId");
            mgr.EditItem(ProductId, Quantity, int.Parse(MyCartId));
            return Redirect("/home/MyCart");
        }



        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
