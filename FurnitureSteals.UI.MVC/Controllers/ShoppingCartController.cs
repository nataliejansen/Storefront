using Microsoft.AspNetCore.Mvc;
using FurnitureSteals.DATA.EF.Models;
using Microsoft.AspNetCore.Identity;
using FurnitureSteals.UI.MVC.Models;
using Newtonsoft.Json;

namespace FurnitureSteals.UI.MVC.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly storefrontContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ShoppingCartController(storefrontContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
            
        }

        public IActionResult Index()
        {
            var sessionCart = HttpContext.Session.GetString("cart");

            Dictionary<int, CartItemViewModel> shoppingCart = null;

            if (sessionCart == null || sessionCart.Count() == 0)
            {
                shoppingCart = new Dictionary<int, CartItemViewModel>();

                ViewBag.Message = "There are currently no items in your cart.";
            }
            else
            {
                ViewBag.Message = null;
                shoppingCart = JsonConvert.DeserializeObject<Dictionary<int, CartItemViewModel>>(sessionCart);
            }

            return View(shoppingCart);
        }

        public IActionResult AddToCart(int id)
        {
            Dictionary<int, CartItemViewModel> shoppingCart = null;

            var sessionCart = HttpContext.Session.GetString("cart");

            if (string.IsNullOrEmpty(sessionCart))
            {
                shoppingCart = new Dictionary<int, CartItemViewModel>();
            }
            else
            {
                shoppingCart = JsonConvert.DeserializeObject<Dictionary<int, CartItemViewModel>>(sessionCart);
            }

            Product product = _context.Products.Find(id);
            CartItemViewModel civm = new CartItemViewModel(1, product);

            if (shoppingCart.ContainsKey(product.ProductId))
            {
                shoppingCart[product.ProductId].Qty++;
            }
            else
            {
                shoppingCart.Add(product.ProductId, civm);
            }

            string jsonCart = JsonConvert.SerializeObject(shoppingCart);
            HttpContext.Session.SetString("cart", jsonCart);

            return RedirectToAction("Index");
        }

        public IActionResult RemoveFromCart(int id)
        {
            var sessionCart = HttpContext.Session.GetString("cart");

            Dictionary<int, CartItemViewModel> shoppingCart = JsonConvert.DeserializeObject<Dictionary<int,
                CartItemViewModel>>(sessionCart);

            shoppingCart.Remove(id);

            if (shoppingCart.Count == 0)
            {
                HttpContext.Session.Remove("cart");
            }
            else
            {
                string jsonCart = JsonConvert.SerializeObject(shoppingCart);
                HttpContext.Session.SetString("cart", jsonCart);
            }

            return RedirectToAction("Index");
        }

        public IActionResult UpdateCart(int productId, int qty)
        {
            var sessionCart = HttpContext.Session.GetString("cart");

            Dictionary<int, CartItemViewModel> shoppingCart = JsonConvert.DeserializeObject<Dictionary<int, CartItemViewModel>>(sessionCart);

            if (qty == 0)
            {
                shoppingCart.Remove(productId);
            }
            else
            {
                shoppingCart[productId].Qty = qty;
            }

            string jsonCart = JsonConvert.SerializeObject(shoppingCart);
            HttpContext.Session.SetString("cart", jsonCart);

            return RedirectToAction("Index");
        }

        //Need to add UserDetails and Orders to the DB so that the Submit Order button can be created and functional
    }

}
