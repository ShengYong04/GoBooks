using MVCBookStore.Models;
using MVCBookStore.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace MVCBookStore.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index(int? genreId, string sortOrder, string search)
        {
            // 1. Fetch active books
            var booksQuery = db.Books.Include(b => b.Genre).Where(b => b.IsActive);

            // 2. Search Filter
            if (!string.IsNullOrEmpty(search))
            {
                string keyword = search.ToLower();
                booksQuery = booksQuery.Where(b =>
                    b.Title.ToLower().Contains(keyword) ||
                    b.Author.ToLower().Contains(search) ||
                    b.ISBN.ToLower().Contains(search));
            }

            // 3. Genre Filter
            if (genreId.HasValue)
            {
                booksQuery = booksQuery.Where(b => b.GenreId == genreId.Value);
            }

            // 4. Sorting Logic
            switch (sortOrder)
            {
                case "price_asc":
                    booksQuery = booksQuery.OrderBy(b => b.Price);
                    break;
                case "price_desc":
                    booksQuery = booksQuery.OrderByDescending(b => b.Price);
                    break;
                default:
                    booksQuery = booksQuery.OrderBy(b => b.Title);
                    break;
            }

            // 5. Prepare ViewModel
            var viewModel = new HomeViewModel
            {
                Books = booksQuery.ToList(),
                Genres = db.Genres.Where(g => g.IsActive).ToList(),
                GenreId = genreId,
                SortOrder = sortOrder,
                Search = search,
                CartItems = GetCartFromSession()
            };

            return View(viewModel);
        }

        // GET: Home/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var book = db.Books.Include(b => b.Genre)
                               .FirstOrDefault(b => b.BookId == id && b.IsActive);

            if (book == null)
                return HttpNotFound();

            // Recommended Books (Same Genre, exclude current book)
            var recommendedBooks = db.Books
                .Where(b => b.GenreId == book.GenreId && b.BookId != book.BookId && b.IsActive)
                .OrderByDescending(b => b.CreatedDate)
                .Take(8)
                .ToList();

            ViewBag.RecommendedBooks = recommendedBooks;

            return View(book);
        }

        // POST: Add to Cart (Session-based)
        [HttpPost]
        public ActionResult AddToCart(int bookId)
        {
            var book = db.Books.Find(bookId);
            if (book == null)
                return HttpNotFound();

            List<CartItem> cart = GetCartFromSession();

            var existingItem = cart.FirstOrDefault(c => c.BookId == bookId);
            if (existingItem != null)
            {
                existingItem.Quantity++;
            }
            else
            {
                cart.Add(new CartItem
                {
                    BookId = book.BookId,
                    Quantity = 1,
                    Book = book // for display purposes
                });
            }

            Session["Cart"] = cart;

            TempData["SuccessMessage"] = $"{book.Title} added to cart!";

            if (Request.UrlReferrer != null)
                return Redirect(Request.UrlReferrer.ToString());

            return RedirectToAction("Index");
        }

        // HELPER METHOD: Get Cart from Session
        private List<CartItem> GetCartFromSession()
        {
            if (Session["Cart"] == null)
            {
                Session["Cart"] = new List<CartItem>();
            }
            return (List<CartItem>)Session["Cart"];
        }

        // GET: FilterBooks (AJAX)
        public ActionResult FilterBooks(int? genreId, string sortOrder, string search)
        {
            var booksQuery = db.Books.Include(b => b.Genre).Where(b => b.IsActive);

            // Genre filter
            if (genreId.HasValue && genreId.Value > 0)
                booksQuery = booksQuery.Where(b => b.GenreId == genreId.Value);

            // Search filter
            if (!string.IsNullOrEmpty(search))
            {
                var keyword = search.ToLower();
                booksQuery = booksQuery.Where(b => b.Title.ToLower().Contains(keyword) ||
                                                   b.Author.ToLower().Contains(keyword));
            }

            // Sorting
            switch (sortOrder)
            {
                case "price_asc":
                    booksQuery = booksQuery.OrderBy(b => b.Price);
                    break;
                case "price_desc":
                    booksQuery = booksQuery.OrderByDescending(b => b.Price);
                    break;
                default:
                    booksQuery = booksQuery.OrderBy(b => b.Title);
                    break;
            }

            var viewModel = new HomeViewModel
            {
                Books = booksQuery.ToList(),
                Genres = db.Genres.Where(g => g.IsActive).ToList(),
                GenreId = genreId,
                SortOrder = sortOrder,
                Search = search
            };

            // Return the partial view for AJAX
            return PartialView("_BookGrid", viewModel);
        }
    }
}
