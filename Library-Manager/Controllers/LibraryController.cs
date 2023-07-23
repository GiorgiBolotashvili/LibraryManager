using Library_Manager.Data;
using Library_Manager.DTO;
using Library_Manager.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Library_Manager.Controllers
{
    public class LibraryController : Controller
    {
        private LibraryDbContext _dbContext;

        public LibraryController(LibraryDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult GetAllBooks()
        {
            List<Book> books = _dbContext.Books.ToList();

            return View(books);
        }

        [HttpGet]
        public IActionResult GetBookDetails(int id)
        {
            var book = _dbContext.Books.FirstOrDefault(b => b.Id == id);
            if (book == null)
                return NotFound();

            return View(book);
        }

        public IActionResult CreateBook()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddBook(Book book)
        {
            ValidationBook(book);

            if (ModelState.IsValid)
            {
                if (book.ImageFile != null && book.ImageFile.Length > 0)
                {
                    var fileName = Path.GetFileName(book.ImageFile.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await book.ImageFile.CopyToAsync(stream);
                    }

                    book.ImageUrl = $"/images/{fileName}";
                }

                _dbContext.Books.Add(book);
                _dbContext.SaveChanges();

                return RedirectToAction("GetAllBooks");
            }

            return View("CreateBook", book);
        }

        [HttpGet]
        public IActionResult EditBook(int id)
        {
            var book = _dbContext.Books.Find(id);

            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        [HttpPost]
        public IActionResult EditBook(Book editedBook)
        {
            ValidationBook(editedBook);

            if (ModelState.IsValid)
            {
                var existingBook = _dbContext.Books.Find(editedBook.Id);

                if (existingBook == null)
                {
                    return NotFound();
                }
                existingBook.Title = editedBook.Title;
                existingBook.Description = editedBook.Description;
                existingBook.ImageUrl = editedBook.ImageUrl;
                existingBook.Rating = editedBook.Rating;
                existingBook.PublicationDate = editedBook.PublicationDate;
                existingBook.IsTaken = editedBook.IsTaken;

                _dbContext.SaveChanges();

                return RedirectToAction("GetAllBooks");
            }

            return View(editedBook);
        }

        [HttpGet]
        public IActionResult DeleteBook(int id)
        {
            var book = _dbContext.Books.Find(id);

            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        [HttpPost]
        public IActionResult DeleteBookConfirmed(int id)
        {
            var book = _dbContext.Books.Find(id);

            if (book == null)
            {
                return NotFound();
            }

            _dbContext.Books.Remove(book);
            _dbContext.SaveChanges();

            return RedirectToAction("GetAllBooks");
        }

        public IActionResult GetAllAuthors()
        {
            var authors = _dbContext.Authors.ToList();

            return View(authors);
        }

        public IActionResult GetAuthors(int bookId)
        {
            var bookAutors = _dbContext.BookAuthors.Where(x => x.BookId == bookId).ToList();

            List<Author> authors = new List<Author>();



            foreach (var item in bookAutors)
            {

                authors.Add(_dbContext.Authors.Where(x => x.Id == item.AuthorId).FirstOrDefault());
            }

            var viewModel = new BookAuthorsViewModel
            {
                Authors = authors,
                BookId = bookId,
                Title = _dbContext.Books.Where(x => x.Id == bookId).FirstOrDefault().Title
            };

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult CreateAuthor(int bookId)
        {
            ViewBag.BookId = bookId;
            return View();
        }

        [HttpPost]
        public IActionResult CreateAuthor(Author newAuthor, int bookId)
        {
            ValidationAuthors(newAuthor);

            if (ModelState.IsValid)
            {
                _dbContext.Authors.Add(newAuthor);
                _dbContext.SaveChanges();

                var bookAuthor = new BookAuthor
                {
                    BookId = bookId,
                    AuthorId = newAuthor.Id
                };

                _dbContext.BookAuthors.Add(bookAuthor);
                _dbContext.SaveChanges();

                return RedirectToAction("GetAuthors", new { bookId = bookId });
            }
            return View(newAuthor);
        }

        [HttpGet]
        public IActionResult EditAuthor(int id)
        {
            var author = _dbContext.Authors.Find(id);

            if (author == null)
            {
                return NotFound();
            }

            return View(author);
        }

        [HttpPost]
        public IActionResult EditAuthor(Author author)
        {
            ValidationAuthors(author);

            if (ModelState.IsValid)
            {
                var existingAuthors = _dbContext.Authors.Find(author.Id);

                if (existingAuthors == null)
                {
                    return NotFound();
                }
                existingAuthors.FirstName = author.FirstName;
                existingAuthors.LastName = author.LastName;
                existingAuthors.BirthYear = author.BirthYear;

                _dbContext.SaveChanges();

                return RedirectToAction("GetAllAuthors");
            }

            return View(author);
        }

        [HttpGet]
        public IActionResult DeleteAuthor(int id)
        {
            var author = _dbContext.Authors.Find(id);

            if (author == null)
            {
                return NotFound();
            }

            return View(author);
        }

        [HttpPost]
        public IActionResult DeleteAuthorConfirmed(int id)
        {
            var author = _dbContext.Authors.Find(id);

            if (author == null)
            {
                return NotFound();
            }

            _dbContext.Authors.Remove(author);
            _dbContext.SaveChanges();

            return RedirectToAction("GetAllAuthors");
        }

        public IActionResult SearchAuthors(string searchText)
        {
            var authors = _dbContext.Authors
                .Where(a => a.FirstName.Contains(searchText) || a.LastName.Contains(searchText))
                .Select(a => new { a.Id, a.FirstName, a.LastName })
                .ToList();

            return Json(authors);
        }
        public IActionResult CreateAuthorForm(int bookId)
        {
            ViewBag.BookId = bookId;
            return PartialView("_CreateAuthorForm");
        }

        private void ValidationBook(Book book)
        {
            ModelState.Remove(nameof(book.BookAuthors));
            ModelState.Remove(nameof(book.IsTaken));
            ModelState.Remove(nameof(book.Id));
            if (book.ImageFile is not null)
            {
                ModelState.Remove(nameof(book.ImageUrl));
            }
            else
            {
                ModelState.Remove(nameof(book.ImageFile));
            }
        }

        private void ValidationAuthors(Author author)
        {
            ModelState.Remove(nameof(author.BookAuthors));
            ModelState.Remove(nameof(author.Id));
        }
    }
}
