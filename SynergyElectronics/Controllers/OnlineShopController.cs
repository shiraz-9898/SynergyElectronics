using Amazon.SimpleNotificationService.Model;
using Amazon.SimpleNotificationService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SynergyElectronics.Areas.Identity.Data;
using System.Drawing;
using System.Security.Claims;
using Azure;
using Microsoft.AspNetCore.Routing;
using System.Net;

namespace SynergyElectronics.Controllers
{
    public class OnlineShopController : Controller
    {
        private readonly ApplicationDbContext _context;
        public OnlineShopController(ApplicationDbContext context)
        {
            _context = context;
        }

        //Homepage
        public IActionResult Index()
        {
            return View();
        }

        //AboutUs Page
        public IActionResult AboutUs()
        {
            return View();
        }

        //Contact us page
        [Authorize]
        public IActionResult Checkout(string? id)
        {
            var data = _context.Orders.Include(c => c.Users).Include(c => c.Products).Where(x => x.Invoice_Id == id).ToList();
            return View(data);
        }
        [Authorize]
        public IActionResult PaymentPage(int? id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var prodData = _context.Products.FirstOrDefault(x => x.Prod_Id == id);
            var userData = _context.Users.FirstOrDefault(x => x.Id == userId);
            if (id != null)
            {
                var orderData = new Order()
                { Qty = 1, User_Id = userId, Users = userData, Prod_Id = (int)id, Products = prodData };
                return View(orderData);
            }

            var orderData2 = new Order()
            { User_Id = userId, Users = userData };


            return View(orderData2);
        }

        [HttpPost]
        public IActionResult PaymentPage(Order order)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var date = DateTime.Now;
            order.Invoice_Id = "INV-0b" + date.Year + "-2c" + date.Month + "-4e" + date.Day + "-3a" + date.Hour + "-1d" + date.Minute;
            order.Id = 0;
            order.Created_Date = date.ToString("MMMM/dd/yyyy");
            order.User_Id = userId;
            order.Qty = 1;

            var userData = _context.Users.FirstOrDefault(c => c.Id == userId);
            
            userData.FullName = order.Users.FullName;
            userData.Address = order.Users.Address;
            userData.Country = order.Users.Country;
            userData.State = order.Users.State;
            userData.City = order.Users.City;
            userData.PinCode = order.Users.PinCode;

            _context.Users.Update(userData);
            _context.SaveChanges();

            order.Users = null;

            if (order.Prod_Id > 0)
            {
                _context.Orders.Update(order);
                _context.SaveChanges();
            }
            else
            {
                var data = _context.Carts.Include(c => c.Users).Include(c => c.Products).Where(x => x.User_Id == userId).ToList();
                foreach(var item in data)
                {
                    if (item.isSelected)
                    {
                        order.Id = 0;
                        order.Prod_Id = item.Prod_Id;
                        order.Qty = item.Cart_Qty;
                        _context.Orders.Add(order);
                        _context.SaveChanges();

                        _context.Remove(item);
                        _context.SaveChanges();
                    }
                }
                
            }

            return RedirectToAction("Checkout", new { id = order.Invoice_Id });
        }
        public IActionResult AllProducts()
        {
            var products = _context.Products.Include(p => p.SubCategories).Include(s => s.SubCategories.Categories).OrderBy(x => x.Prod_Id);

            return Json(products.ToList());
        }


        //ViewAll get data by category

        [HttpGet]
        public IActionResult SpecificList(int id)
        {
            TempData["catId"] = id;
            return View();
        }

        [HttpPost]
        public IActionResult SpecificList(SearchData? sData)
        {
            TempData["catId"] = sData.Cat_Id;
            TempData["dataArray"] = sData.Data;
            return View();
        }

        //Cart page
        [Authorize]
        public IActionResult Cart()
        {
            return View();
        }

        //Get Cart Data
        public IActionResult GetCartData()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId != null)
            {
                var data = _context.Carts.Include(c => c.Users).Include(c => c.Products).Where(x => x.User_Id == userId).OrderBy(x => x.Cart_Id).ToList();
                return Json(data);
            }
            return Json("no data");
        }

        //add item to cart
        [Authorize]
        public async Task<IActionResult> AddCart(int prod_Id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId != null)
            {
                var exProduct = _context.Products.FirstOrDefault(x => x.Prod_Id == prod_Id);
                if (exProduct != null)
                {
                    var prodToCart = new Cart()
                    {
                        Cart_Qty = 1,
                        Cart_Price = exProduct.Prod_Price,
                        User_Id = userId,
                        Prod_Id = prod_Id
                    };
                    var existD = _context.Carts.FirstOrDefault(x => x.User_Id == userId && x.Prod_Id == prod_Id);
                    if (existD != null)
                    {
                        //if product is available in user's cart then increase Qty

                        existD.Cart_Qty += 1;
                        existD.Cart_Price = existD.Cart_Qty * prodToCart.Cart_Price;

                        _context.Carts.Update(existD);
                        _context.SaveChanges();
                    }
                    else
                    {
                        //if product is not available in user's cart then add item in cart.
                        _context.Carts.Add(prodToCart);
                        _context.SaveChanges();
                    }
                }
                try
                {
                    var snsClient = new AmazonSimpleNotificationServiceClient(Amazon.RegionEndpoint.APSouth1);

                    var request = new PublishRequest
                    {
                        PhoneNumber = "+916261663791", // Replace with the recipient's phone number
                        //TopicArn = "arn:aws:sns:ap-south-1:6261663791:ExampleSNSTopic",
                        Message = "Hello from AWS SNS!"
                    };

                    var response = await snsClient.PublishAsync(request);
                    return Json(response.MessageId);
                }
                catch (Exception e)
                {
                    return Json(e.Message);
                }
            }
            return NoContent();
        }

        [HttpPost]
        public IActionResult Edit(Cart carts)
        {
            carts.Cart_Price = carts.Products.Prod_Price * carts.Cart_Qty;
            _context.Carts.Update(carts);
            _context.SaveChanges();
            var data = _context.Carts.Include(c => c.Users).Include(c => c.Products).Where(x => x.User_Id == carts.User_Id).OrderBy(x => x.Cart_Id).ToList();
            return Json(data);
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var data = _context.Carts.Find(id);
            if (data != null)
            {
                _context.Carts.Remove(data);
                _context.SaveChanges();
            }
            var response = _context.Carts.Include(c => c.Users).Include(c => c.Products).Where(x => x.User_Id == userId).OrderBy(x => x.Cart_Id).ToList();
            return Json(response);
        }
    }
}
