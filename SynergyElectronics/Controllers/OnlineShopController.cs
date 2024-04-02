using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SynergyElectronics.Areas.Identity.Data;
using System.Security.Claims;

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
        public IActionResult Checkout(int prodId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var data = _context.Carts.Include(c => c.Users).Include(c => c.Products).FirstOrDefault(x => x.Prod_Id == prodId && x.User_Id == userId);
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
                var cartData = new Cart()
                { Cart_Qty = 1, Cart_Price = prodData.Prod_Price, User_Id = userId, Users = userData, Prod_Id = (int)id, Products = prodData };
                return View(cartData);
            }

            var cartData2 = new Cart()
            { User_Id = userId, Users = userData };


            return View(cartData2);
        }
        public IActionResult AllProducts()
        {
            var products = _context.Products.Include(p => p.SubCategories).Include(s => s.SubCategories.Categories);

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
                var data = _context.Carts.Include(c => c.Users).Include(c => c.Products).Where(x => x.User_Id == userId).ToList();
                return Json(data);
            }
            return Json("no data");
        }

        //add item to cart
        [Authorize]
        public IActionResult AddCart(int prod_Id)
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
                return Json("ok");
            }           
            return NoContent();
        }

        [HttpPost]
        public IActionResult Edit(Cart carts)
        {
            carts.Cart_Price = carts.Products.Prod_Price * carts.Cart_Qty;
            _context.Carts.Update(carts);
            _context.SaveChanges();
            var data = _context.Carts.Include(c => c.Users).Include(c => c.Products).Where(x => x.User_Id == carts.User_Id).ToList();
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
            var response = _context.Carts.Include(c => c.Users).Include(c => c.Products).Where(x => x.User_Id == userId).ToList();
            return Json(response);
        }
    }
}
