using Microsoft.AspNet.Identity;
using MVCBookStore.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace MVCBookStore.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();

            var cart = db.Carts
                         .Include(c => c.CartItems.Select(i => i.Book))
                         .FirstOrDefault(c => c.UserId == userId);

            if (cart == null)
                cart = new Cart { UserId = userId };

            return View(cart);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddToCart(int bookId)
        {
            var userId = User.Identity.GetUserId();
            var book = db.Books.Find(bookId);

            if (book == null)
                return HttpNotFound();

            if (book.Quantity <= 0)
            {
                TempData["CartWarning"] = "This book is out of stock.";
                return Redirect(Request.UrlReferrer?.ToString() ?? "/");
            }

            var cart = db.Carts.FirstOrDefault(c => c.UserId == userId);
            if (cart == null)
            {
                cart = new Cart { UserId = userId, CreatedDate = DateTime.Now };
                db.Carts.Add(cart);
                db.SaveChanges();
            }

            var cartItem = db.CartItems
                             .FirstOrDefault(ci => ci.CartId == cart.CartId && ci.BookId == bookId);

            if (cartItem != null)
            {
                if (cartItem.Quantity >= book.Quantity)
                {
                    TempData["CartWarning"] = $"Only {book.Quantity} unit(s) left. You can't add more.";
                }
                else
                {
                    cartItem.Quantity++;
                    TempData["CartSuccess"] = "Item added to cart.";
                }
            }
            else
            {
                cartItem = new CartItem
                {
                    CartId = cart.CartId,
                    BookId = bookId,
                    Quantity = 1
                };
                db.CartItems.Add(cartItem);
                TempData["CartSuccess"] = "Item added to cart.";
            }

            db.SaveChanges();

            if (Request.IsAjaxRequest())
            {
                var updatedCart = db.Carts
                                    .Include(c => c.CartItems.Select(i => i.Book))
                                    .FirstOrDefault(c => c.UserId == userId);

                return PartialView("_CartSidebar", updatedCart);
            }

            return Redirect(Request.UrlReferrer?.ToString() ?? "/");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateQuantity(int cartItemId, int count)
        {
            var userId = User.Identity.GetUserId();
            var item = db.CartItems
                         .Include(i => i.Book)
                         .FirstOrDefault(i => i.CartItemId == cartItemId && i.Cart.UserId == userId);

            if (item != null)
            {
                var stock = item.Book.Quantity;

                if (count > 0 && item.Quantity >= stock)
                {
                    TempData["CartWarning"] = $"Maximum stock reached ({stock}).";
                }
                else
                {
                    item.Quantity += count;
                }

                if (item.Quantity <= 0)
                    db.CartItems.Remove(item);

                db.SaveChanges();
            }

            // AJAX
            if (Request.IsAjaxRequest())
            {
                var cart = db.Carts
                             .Include(c => c.CartItems.Select(i => i.Book))
                             .FirstOrDefault(c => c.UserId == userId);
                return PartialView("_CartSidebar", cart);
            }

            return Redirect(Request.UrlReferrer?.ToString() ?? "/Cart/Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveFromCart(int cartItemId)
        {
            var userId = User.Identity.GetUserId();
            var item = db.CartItems
                         .FirstOrDefault(i => i.CartItemId == cartItemId && i.Cart.UserId == userId);

            if (item != null)
            {
                db.CartItems.Remove(item);
                db.SaveChanges();
            }

            if (Request.IsAjaxRequest())
            {
                var cart = db.Carts
                             .Include(c => c.CartItems.Select(i => i.Book))
                             .FirstOrDefault(c => c.UserId == userId);
                return PartialView("_CartSidebar", cart);
            }

            return Redirect(Request.UrlReferrer?.ToString() ?? "/Cart/Index");
        }

        [ChildActionOnly]
        public ActionResult GetCartSidebar()
        {
            var userId = User.Identity.GetUserId();
            if (string.IsNullOrEmpty(userId))
                return Content("");

            var cart = db.Carts
                         .Include(c => c.CartItems.Select(i => i.Book))
                         .FirstOrDefault(c => c.UserId == userId);

            return PartialView("_CartSidebar", cart);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();
            base.Dispose(disposing);
        }
    }
}
