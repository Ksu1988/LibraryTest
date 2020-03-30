using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyLibrary.Data;
using MyLibrary.Models;

namespace MyLibrary.Controllers
{
    public class BooksController : Controller
    {
        private readonly LibraryContext _context;

        public BooksController(LibraryContext context)
        {
            _context = context;
        }

        // GET: Books
        public async Task<IActionResult> Index(SortState sortOrder = SortState.NameAsc)
        {
            IQueryable<Book> books = _context.Books.Include(b => b.Authors);

            ViewData["NameSort"] = sortOrder == SortState.NameAsc ? SortState.NameDesc : SortState.NameAsc;
            ViewData["PublishingSort"] = sortOrder == SortState.PublishingYearAsc ? SortState.PublishingYearDesc : SortState.PublishingYearAsc;

            switch (sortOrder)
            {
                case SortState.NameAsc:
                    books = books.OrderBy(s => s.Name);
                    break;
                case SortState.NameDesc:
                    books = books.OrderByDescending(s => s.Name);
                    break;
                case SortState.PublishingYearAsc:
                    books = books.OrderBy(s => s.PublishingYear);
                    break;
                case SortState.PublishingYearDesc:
                    books = books.OrderByDescending(s => s.PublishingYear);
                    break;
            }
            return View(await books.AsNoTracking().ToListAsync());
            //return View(await _context.Books.Include(b => b.Authors).ToListAsync());
        }

        // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books.Include(t => t.Authors)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // GET: Books/Create
        public IActionResult Create()
        {
            var book = new Book();
            return View(book);
        }

        // POST: Books/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,PadgesCount,PublishingYear,PublisherName,Image,Authors")] Book book, IFormFile formFile)
        {
            if (formFile != null)
            {
                byte[] imageData = null;
                // считываем переданный файл в массив байтов
                using (var binaryReader = new BinaryReader(formFile.OpenReadStream()))
                {
                    imageData = binaryReader.ReadBytes((int)formFile.Length);
                }
                // установка массива байтов
                book.Image = imageData;
            }
            if (book.Authors.Count == 0)
            {
                ModelState.AddModelError("Authors", "Должен быть хотя бы один автор");
            }

            foreach (var a in book.Authors)
            {
                if (string.IsNullOrEmpty(a.Name))
                    ModelState.AddModelError("Authors", "Введите имя автора");
                if (string.IsNullOrEmpty(a.Surname))
                    ModelState.AddModelError("Authors", "Введите фамилию автора");
            }

            if (ModelState.IsValid)
            {
                _context.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }

        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books.Include(t => t.Authors)
              .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,PadgesCount,PublishingYear,PublisherName,Image,Authors")] Book book, IFormFile formFile, int[] newAuthours)
        {
            if (id != book.Id)
            {
                return NotFound();
            }

            if (newAuthours != null)
            {
                foreach (var aId in newAuthours)
                {
                    var author = _context.Authors.Find(aId);
                    book.Authors.Add(author);
                }
            }

            if (formFile != null)
            {
                byte[] imageData = null;
                // считываем переданный файл в массив байтов
                using (var binaryReader = new BinaryReader(formFile.OpenReadStream()))
                {
                    imageData = binaryReader.ReadBytes((int)formFile.Length);
                }
                // установка массива байтов
                book.Image = imageData;
            }

            if (book.Authors.Count == 0)
            {
                ModelState.AddModelError("Authors", "Должен быть хотя бы один автор");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(book);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }

        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var book = await _context.Books.Include(t => t.Authors)
    .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _context.Books.Include(t => t.Authors)
.FirstOrDefaultAsync(m => m.Id == id);
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.Id == id);
        }

        // GET: Authors/Edit/5
        public async Task<IActionResult> EditAuthors(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var author = await _context.Authors.FindAsync(id);
            var author = await _context.Authors
                .Include(a => a.Book)
                .FirstOrDefaultAsync(m => m.AuthourId == id);
            if (author == null)
            {
                return NotFound();
            }
            ViewData["BookId"] = new SelectList(_context.Books, "Id", "Name", author.BookId);
            return View(author);

        }

        // POST: Authors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAuthors(int id, [Bind("AuthourId,Name,Surname,BookId")] Author author)
        {
            if (id != author.AuthourId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(author);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AuthorExists(author.AuthourId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Edit), new { id = author.BookId });
            }
            ViewData["BookId"] = author.BookId;
            return View(author);
        }

        // GET: Authors/Create
        public IActionResult CreateAuthor(int? id)
        {
            // Формируем список книг для передачи в представление
            SelectList books = new SelectList(_context.Books, "Id", "Name");
            ViewBag.Books = books;
            ViewBag.BookId = id;
            return View();
        }

        // POST: Authors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAuthor([Bind("AuthourId,Name,Surname,BookId")] Author author)
        {
            if (ModelState.IsValid)
            {
                _context.Add(author);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Edit), new { id = author.BookId });
            }
            ViewData["BookId"] = author.BookId;
            return View(author);
        }


        // GET: Authors/Delete/5
        public async Task<IActionResult> DeleteAuthour(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var author = await _context.Authors
                .Include(a => a.Book)
                .FirstOrDefaultAsync(m => m.AuthourId == id);
            if (author == null)
            {
                return NotFound();
            }

            return View(author);
        }

        // POST: Authors/Delete/5
        [HttpPost, ActionName("DeleteAuthour")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAuthourConfirmed(int id)
        {
            var author = await _context.Authors.FindAsync(id);
            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Edit), new { id = author.BookId });
        }

        private bool AuthorExists(int id)
        {
            return _context.Authors.Any(e => e.AuthourId == id);
        }
    }
}
