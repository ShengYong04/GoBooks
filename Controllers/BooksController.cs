using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVCBookStore.Models;
using System.IO; 

namespace MVCBookStore.Controllers
{
    [Authorize(Roles = "Admin")]
    public class BooksController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Books
        public ActionResult Index()
        {
            var books = db.Books.Include(b => b.Genre);
            return View(books.ToList());
        }

        // GET: Books/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Book book = db.Books.Find(id);
            if (book == null) return HttpNotFound();
            return View(book);
        }

        // GET: Books/Create
        public ActionResult Create()
        {
            ViewBag.GenreId = new SelectList(db.Genres, "GenreId", "GenreName");
            return View();
        }

        // POST: Books/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Book book, HttpPostedFileBase upload)
        {
            ModelState.Remove("CreatedDate");
            ModelState.Remove("ImageUrl");

            if (ModelState.IsValid)
            {
                book.CreatedDate = DateTime.Now;

                if (upload != null && upload.ContentLength > 0)
                {
                    string fileName = Guid.NewGuid() + Path.GetExtension(upload.FileName);
                    string path = Path.Combine(Server.MapPath("~/Content/Images/"), fileName);
                    upload.SaveAs(path);
                    book.ImageUrl = "/Content/Images/" + fileName;
                }
                else
                {
                    book.ImageUrl = "/Content/Images/default.png";
                }

                db.Books.Add(book);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.GenreId = new SelectList(db.Genres, "GenreId", "GenreName", book.GenreId);
            return View(book);
        }

        // GET: Books/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Book book = db.Books.Find(id);
            if (book == null) return HttpNotFound();

            ViewBag.GenreId = new SelectList(db.Genres, "GenreId", "GenreName", book.GenreId);
            return View(book);
        }

        // POST: Books/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Book book, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                var bookInDb = db.Books.AsNoTracking().FirstOrDefault(b => b.BookId == book.BookId);

                if (bookInDb != null)
                {
                    book.CreatedDate = bookInDb.CreatedDate; 

                    if (upload == null)
                    {
                        book.ImageUrl = bookInDb.ImageUrl;
                    }
                }

                // If a NEW image is uploaded, save it
                if (upload != null && upload.ContentLength > 0)
                {
                    string fileName = Guid.NewGuid() + Path.GetExtension(upload.FileName);
                    string path = Path.Combine(Server.MapPath("~/Content/Images/"), fileName);
                    upload.SaveAs(path);
                    book.ImageUrl = "/Content/Images/" + fileName;
                }

                db.Entry(book).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.GenreId = new SelectList(db.Genres, "GenreId", "GenreName", book.GenreId);
            return View(book);
        }

        // GET: Books/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Book book = db.Books.Find(id);
            if (book == null) return HttpNotFound();
            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Book book = db.Books.Find(id);
            db.Books.Remove(book);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}