using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using MVCBookStore.Models;
using MVCBookStore.ViewModels;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MVCBookStore.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public ManageController() { }

        public ManageController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get => _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            private set => _signInManager = value;
        }

        public ApplicationUserManager UserManager
        {
            get => _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            private set => _userManager = value;
        }

        // ===============================
        // GET: /Manage/Index (Profile & Orders)
        // ===============================
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var user = db.Users.Find(userId);

            var orders = db.Orders.Include("OrderItems.Book")
                                  .Where(o => o.UserId == userId)
                                  .OrderByDescending(o => o.OrderDate)
                                  .ToList();

            var model = new UserProfileViewModel
            {
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                AddressLine1 = user.AddressLine1,
                AddressLine2 = user.AddressLine2,
                PostalCode = user.PostalCode,
                City = user.City,
                State = user.State,
                OrderHistory = orders
            };

            return View(model);
        }

        // ===============================
        // POST: /Manage/Index (Save Profile)
        // ===============================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(UserProfileViewModel model)
        {
            var userId = User.Identity.GetUserId();
            var user = db.Users.Find(userId);

            // Reload orders if returning view
            model.OrderHistory = db.Orders.Include("OrderItems.Book")
                                          .Where(o => o.UserId == userId)
                                          .OrderByDescending(o => o.OrderDate)
                                          .ToList();

            if (ModelState.IsValid)
            {
                user.FullName = model.FullName;
                user.PhoneNumber = model.PhoneNumber;
                user.AddressLine1 = model.AddressLine1;
                user.AddressLine2 = model.AddressLine2;
                user.PostalCode = model.PostalCode;
                user.City = model.City;
                user.State = model.State;

                db.SaveChanges();
                ViewBag.SuccessMessage = "Profile updated successfully!";
            }

            return View(model);
        }

        // ===============================
        // GET: /Manage/ChangePassword
        // ===============================
        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                return RedirectToAction("Index", new { Message = ManageMessageId.ChangePasswordSuccess });
            }

            AddErrors(result);
            return View(model);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }
            base.Dispose(disposing);
        }

        #region Helpers
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError("", error);
        }

        public enum ManageMessageId { ChangePasswordSuccess, Error }
        #endregion

        // ===============================
        // GET: /Manage/OrderDetails/{orderId}
        // ===============================
        public ActionResult OrderDetails(int orderId)
        {
            var userId = User.Identity.GetUserId();

            var order = db.Orders
              .Include("OrderItems.Book")
              .Include("User") // <-- Include the user
              .FirstOrDefault(o => o.OrderId == orderId && o.UserId == userId);

            if (order == null) return HttpNotFound("Order not found.");

            var model = new OrderDetailsViewModel
            {
                OrderId = order.OrderId,
                OrderDate = order.OrderDate,
                Status = order.Status,
                TotalAmount = order.TotalAmount,
                FullName = order.User.FullName,
                Email = order.User.Email,
                PhoneNumber = order.User.PhoneNumber,
                AddressLine1 = order.User.AddressLine1,
                AddressLine2 = order.User.AddressLine2,
                City = order.User.City,
                State = order.User.State,
                PostalCode = order.User.PostalCode,
                OrderItems = order.OrderItems.Select(oi => new OrderItemViewModel
                {
                    BookTitle = oi.Book.Title,
                    Quantity = oi.Quantity,
                    Price = oi.PriceAtPurchase
                }).ToList()
            };


            return View(model);
        }

    }
}
