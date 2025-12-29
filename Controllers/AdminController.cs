using MVCBookStore.Models;
using MVCBookStore.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace MVCBookStore.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        // GET: Admin/Index (Dashboard)
        public ActionResult Index()
        {
            // 1. Basic Stats
            var totalSales = db.Orders.Where(o => o.Status != "Cancelled").Sum(o => (decimal?)o.TotalAmount) ?? 0;
            var totalOrders = db.Orders.Count();
            var pendingOrders = db.Orders.Count(o => o.Status == "Processing" || o.Status == "Pending");
            var totalBooks = db.Books.Count();

            // 2. Recent Orders (Last 5)
            var recentOrders = db.Orders.Include("User")
                                        .OrderByDescending(o => o.OrderDate)
                                        .Take(5)
                                        .ToList();

            // 3. Low Stock Books (Quantity < 5)
            var lowStockBooks = db.Books.Where(b => b.Quantity < 5)
                                        .OrderBy(b => b.Quantity)
                                        .Take(5)
                                        .ToList();

            // 4. Sales Chart Data (Last 7 Days)
            var today = DateTime.Today; 
            var weekAgo = today.AddDays(-6);

            // Group by Date
            var lastWeekOrders = db.Orders
                .Where(o => o.OrderDate >= weekAgo && o.Status != "Cancelled")
                .Select(o => new { o.OrderDate, o.TotalAmount })
                .ToList();

            var salesByDate = new Dictionary<string, decimal>();
            for (int i = 0; i <= 6; i++)
            {
                var d = weekAgo.AddDays(i).ToString("MMM dd");
                salesByDate[d] = 0;
            }

            foreach (var o in lastWeekOrders)
            {
                var d = o.OrderDate.ToString("MMM dd");
                if (salesByDate.ContainsKey(d))
                {
                    salesByDate[d] += o.TotalAmount;
                }
            }

            var model = new AdminDashboardViewModel
            {
                TotalSales = totalSales,
                TotalOrders = totalOrders,
                PendingOrdersCount = pendingOrders,
                TotalBooks = totalBooks,
                RecentOrders = recentOrders,
                LowStockBooks = lowStockBooks,
                ChartLabels = salesByDate.Keys.ToList(),
                ChartValues = salesByDate.Values.ToList()
            };

            return View(model);
        }
    }
}