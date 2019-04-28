using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Library.Data;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace _4_10TEST.Controllers
{
    public class AdminController : Controller
    {
        private IHostingEnvironment _environment;
        private string _connectionString;

        public AdminController(IHostingEnvironment environment,
            IConfiguration configuration)
        {
            _environment = environment;
            _connectionString = configuration.GetConnectionString("ConStr");
        }
        public IActionResult LogIn()
        {
            return View();
        }
        public IActionResult Index()
        {
            Manager mgr = new Manager(_connectionString);
            return View(mgr.GetCategories());
        }
        public IActionResult addProduct(Products p, IFormFile ImageName)
        {
            string fileName = $"{Guid.NewGuid()}{Path.GetExtension(ImageName.FileName)}";
            string fullPath = Path.Combine(_environment.WebRootPath, "ProductImages", fileName);
            using (FileStream stream = new FileStream(fullPath, FileMode.CreateNew))
            {
                ImageName.CopyTo(stream);
            }
            p.ImageName = fileName; //fileName

            Manager mgr = new Manager(_connectionString);
           int id= mgr.AddProduct(p);
            return Redirect($"/home/products?id={id}");
        }
        public IActionResult addCategory(Categories c)
        {
            Manager mgr = new Manager(_connectionString);
            mgr.AddCategory(c);

            TempData["Message"] = $" Category Added";
            return Redirect("/admin/index");
        }
    }
}