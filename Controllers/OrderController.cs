using Microsoft.AspNet.Identity;
using MVCBookStore.Models;
using MVCBookStore.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace MVCBookStore.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Checkout
        public ActionResult Checkout()
        {
            var userId = User.Identity.GetUserId();
            var cart = db.Carts.Include("CartItems.Book").FirstOrDefault(c => c.UserId == userId);

            if (cart == null || !cart.CartItems.Any())
                return RedirectToAction("Index", "Home");

            ViewBag.CartTotal = cart.CartItems.Sum(i => i.Book.Price * i.Quantity);
            ViewBag.CartItems = cart.CartItems.ToList();

            var user = db.Users.Find(userId);
            var model = new CheckoutViewModel();

            if (user != null)
            {
                model.AddressLine1 = user.AddressLine1;
                model.AddressLine2 = user.AddressLine2;
                model.PostalCode = user.PostalCode;
                model.City = user.City;
                model.State = user.State;
            }

            return View(model);
        }

        // POST: Checkout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Checkout(CheckoutViewModel model)
        {
            var userId = User.Identity.GetUserId();
            var cart = db.Carts.Include("CartItems.Book").FirstOrDefault(c => c.UserId == userId);

            if (ModelState.IsValid && cart != null)
            {
                var user = db.Users.Find(userId);

                // Auto-Save Address
                if (user != null)
                {
                    user.AddressLine1 = model.AddressLine1;
                    user.AddressLine2 = model.AddressLine2;
                    user.PostalCode = model.PostalCode;
                    user.City = model.City;
                    user.State = model.State;
                }

                // Create Order
                var order = new Order
                {
                    UserId = userId,
                    OrderDate = DateTime.Now,
                    TotalAmount = cart.CartItems.Sum(i => i.Book.Price * i.Quantity),
                    Status = "Processing",
                    OrderItems = new List<OrderItem>()
                };

                foreach (var item in cart.CartItems)
                {
                    // Reduce Stock
                    if (item.Book.Quantity >= item.Quantity)
                    {
                        item.Book.Quantity -= item.Quantity;
                    }

                    order.OrderItems.Add(new OrderItem
                    {
                        BookId = item.BookId,
                        Quantity = item.Quantity,
                        PriceAtPurchase = item.Book.Price
                    });
                }

                // Create Payment
                var payment = new Payment
                {
                    UserId = userId,
                    PaymentMethod = "Cash on Delivery",
                    BillingAddress = $"{model.AddressLine1}|{model.AddressLine2}|{model.PostalCode}|{model.City}|{model.State}",
                    Status = "Pending",
                    PaymentDate = DateTime.Now,
                    Order = order
                };

                if (order.Payments == null) order.Payments = new List<Payment>();
                order.Payments.Add(payment);

                db.Orders.Add(order);
                db.CartItems.RemoveRange(cart.CartItems);
                db.SaveChanges();

                return RedirectToAction("OrderConfirmation", new { id = order.OrderId });
            }

            ViewBag.CartTotal = cart?.CartItems.Sum(i => i.Book.Price * i.Quantity) ?? 0;
            ViewBag.CartItems = cart?.CartItems.ToList() ?? new List<CartItem>();
            return View(model);
        }

        // GET: Order Confirmation
        public ActionResult OrderConfirmation(int id)
        {
            var userId = User.Identity.GetUserId();
            var order = db.Orders.Include("OrderItems.Book")
                                 .FirstOrDefault(o => o.OrderId == id && o.UserId == userId);
            if (order == null) return HttpNotFound();
            return View(order);
        }

        // GET: Customer Order Details (Read Only)
        public ActionResult Details(int id)
        {
            var userId = User.Identity.GetUserId();
            var order = db.Orders.Include("OrderItems.Book")
                                 .FirstOrDefault(o => o.OrderId == id && o.UserId == userId);

            if (order == null) return HttpNotFound();
            return View(order);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}