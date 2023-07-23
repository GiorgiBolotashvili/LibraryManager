using Library_Manager.Data;
using Library_Manager.DTO;
using Library_Manager.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Library_Manager.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private LibraryDbContext _dbContext;

        public HomeController(ILogger<HomeController> logger, LibraryDbContext dbContext)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public IActionResult Index()
        {
            List<Book> books = _dbContext.Books.ToList();

            return View(books);
        }

        [HttpPost]
        public IActionResult UpdateTakenStatus(int bookId, bool isTaken)
        {
            var book = _dbContext.Books.Find(bookId);
            if (book != null)
            {
                book.IsTaken = !isTaken;
                _dbContext.SaveChanges();
                return Ok();
            }
            return NotFound();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}