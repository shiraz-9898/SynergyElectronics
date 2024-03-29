using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SynergyElectronics.Areas.Identity.Data;

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

        ////add item to cart

        //public IActionResult AddCart(int prod_Id)
        //{
        //    TempData.Keep("user");
        //    int userId = Convert.ToInt32(TempData["user"]);
        //    if (userId > 0)
        //    {
        //        var exProduct = _context.Products.FirstOrDefault(x => x.Prod_Id == prod_Id);
        //        if (exProduct != null)
        //        {
        //            var prodToCart = new Cart()
        //            {
        //                Cart_Qty = 1,
        //                Cart_Price = exProduct.Prod_Price,
        //                Cust_Id = userId,
        //                Prod_Id = prod_Id
        //            };
        //            var existD = _context.Carts.FirstOrDefault(x => x.Cust_Id == userId && x.Prod_Id == prod_Id);
        //            if (existD != null)
        //            {
        //                //if product is available in user's cart then increase Qty

        //                existD.Cart_Qty += 1;
        //                existD.Cart_Price = existD.Cart_Qty * prodToCart.Cart_Price;

        //                _context.Carts.Update(existD);
        //                _context.SaveChanges();
        //            }
        //            else
        //            {
        //                //if product is not available in user's cart then add item in cart.
        //                _context.Carts.Add(prodToCart);
        //                _context.SaveChanges();
        //            }
        //        }
        //        return Json("ok");
        //    }

        //    //var records = _context.Carts.FirstOrDefault(x => x.Cust_Id == userId);
        //    //return Json(2);
        //    return Json("not");
        //}

        ////Cart page
        //public IActionResult Cart()
        //{
        //    TempData.Keep("user");
        //    return View();
        //}

        ////Get Cart Data
        //public IActionResult GetCartData(int? userId)
        //{
        //    if (userId != null)
        //    {
        //        var data = _context.Carts.Include(c => c.Customers).Include(c => c.Products).Where(x => x.Cust_Id == userId).ToList();
        //        return Json(data);
        //    }
        //    return Json("no data");
        //}

        ////registration page
        //public IActionResult Registration()
        //{
        //    return View();
        //}
        //[HttpPost]
        //public IActionResult Registration(Customer customers)
        //{
        //    if (customers.Cust_Email != null)
        //    {
        //        var data = _context.Customers.FirstOrDefault(x => x.Cust_Email == customers.Cust_Email);
        //        if (data == null)
        //        {
        //            _context.Customers.Add(customers);
        //            _context.SaveChanges();
        //            return RedirectToAction("Login");
        //        }
        //        TempData["response"] = $"User Already exist with \"{customers.Cust_Email}\" Email ID";
        //    }
        //    return View();
        //}

        ////login page
        //public IActionResult Login()
        //{
        //    return View();
        //}
        //[HttpPost]
        //public IActionResult Login(Customer customers)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var data = _context.Customers.FirstOrDefault(x => x.Cust_Email == customers.Cust_Email);
        //        if (data != null && customers.Cust_Password == data.Cust_Password)
        //        {
        //            TempData["user"] = JsonConvert.SerializeObject(data);
        //            return RedirectToAction("Index");
        //        }
        //        TempData["response"] = "Username or Password";
        //    }
        //    return View();
        //}

        ////logout page
        //public IActionResult Logout()
        //{
        //    TempData["user"] = null;
        //    return RedirectToAction("Index");
        //}
        //[HttpPost]
        //public IActionResult Edit(Cart carts)
        //{
        //    carts.Cart_Price = carts.Products.Prod_Price * carts.Cart_Qty;
        //    _context.Carts.Update(carts);
        //    _context.SaveChanges();
        //    var data = _context.Carts.Include(c => c.Customers).Include(c => c.Products).Where(x => x.Cust_Id == carts.Cust_Id).ToList();
        //    return Json(data);
        //}
        //[HttpPost]
        //public IActionResult Delete(int id)
        //{
        //    var data = _context.Carts.Find(id);
        //    if (data != null)
        //    {
        //        _context.Carts.Remove(data);
        //        _context.SaveChanges();
        //    }
        //    var response = _context.Carts.Include(c => c.Customers).Include(c => c.Products).Where(x => x.Cust_Id == data.Cust_Id).ToList();
        //    return Json(response);
        //}
    }
}
