using System.Collections.Generic;

namespace MVCBookStore.ViewModels
{
    public class AdminDashboardViewModel
    {
        public decimal TotalSales { get; set; }
        public int TotalOrders { get; set; }
        public int PendingOrdersCount { get; set; }
        public int TotalBooks { get; set; }

        public IEnumerable<MVCBookStore.Models.Order> RecentOrders { get; set; }
        public IEnumerable<MVCBookStore.Models.Book> LowStockBooks { get; set; }

        public List<string> ChartLabels { get; set; }
        public List<decimal> ChartValues { get; set; }
    }
}