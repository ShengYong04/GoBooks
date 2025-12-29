using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCBookStore.Models;
using System.Net;
using System.Data.Entity; 

namespace MVCBookStore.Controllers
{
    [Authorize(Roles = "Admin")] // Only Admin can access
    public class AdminOrderController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: AdminOrder
        public ActionResult Index(string status)
        {
            // 1. Get all orders first
            var orders = db.Orders.Include("User").OrderByDescending(o => o.OrderDate).AsQueryable();

            // 2. Apply Filter if status is provided
            if (!string.IsNullOrEmpty(status))
            {
                status = status.Trim();
                orders = orders.Where(o => o.Status == status);
            }

            ViewBag.CurrentStatus = status;

            return View(orders.ToList());
        }

        // GET: AdminOrder/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Order order = db.Orders.Include("OrderItems")
                                   .Include("OrderItems.Book")
                                   .Include("User")
                                   .Include("Payments")
                                   .FirstOrDefault(o => o.OrderId == id);

            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: AdminOrder/UpdateStatus
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateStatus(int orderId, string status)
        {
            Order order = db.Orders.Find(orderId);
            if (order != null)
            {
                order.Status = status;
                db.SaveChanges();
                TempData["SuccessMessage"] = "Order #" + orderId + " updated to " + status + "!";
            }
            return RedirectToAction("Details", new { id = orderId });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}