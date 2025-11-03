using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MvcLibrary.Models;

namespace MvcLibrary.Controllers
{
    public class BorrowController : Controller
    {
        public static List<Book> Books = new List<Book>
        {
            new Book{ Id=1,Title="Clean Code",Author="Robert",AvailableInLibrary=2,TotalInLibrary=5},
            new Book{ Id=2,Title="Masnavi",Author="Molavi",AvailableInLibrary=4,TotalInLibrary=1}
        };
        private static List<User> Users = new List<User>
        {
            new User{Id=1,Name="Ali",BorrowedBook=false,Penalty=100},
            new User{Id=2,Name="Hasan",BorrowedBook=false,Penalty=0}
        };
        private static List<Borrow> Records = new List<Borrow>();
        public IActionResult Index()
        {
            return View(Records);
        }
        [HttpPost]
        public IActionResult BorrowBook(int userid,int bookid)
        {
            var user = Users.FirstOrDefault(u => u.Id == userid);
            var book = Books.FirstOrDefault(u => u.Id == bookid);
            if (user == null || book == null)
            {
                return Content("Not correct");
            }
            if (user.BorrowedBook)
            {
                return Content("User has borrowed a book");
            }
            if (user.Penalty != 0)
            {
                return Content("You should pay your debt first");
            }
            if (book.AvailableInLibrary == 0)
            {
                return Content("This book is not available");
            }
            book.AvailableInLibrary--;
            user.BorrowedBook = true;
            Records.Add(new Borrow
            {
                Id = Records.Count + 1,
                UserId = userid,
                BookId = bookid,
                BorrowDate = DateTime.Now,
                IsReturned=false
            }) ;
            return Content("Done");
        }
        public IActionResult ReturnBook(int recordid)
        {
            var record = Records.FirstOrDefault(r => r.Id == recordid);
            if (record == null || record.IsReturned)
            {
                return Content("Not correct");
            }
            record.IsReturned = true;
            record.ReturnDate = DateTime.Now;
            var user = Users.FirstOrDefault(u => u.Id == record.UserId);
            var book = Books.FirstOrDefault(u => u.Id ==record.BookId);
            if (user != null)
            {
                user.BorrowedBook = false;
            }
            if (book != null)
            {
                book.AvailableInLibrary++;
            }
            if (DateTime.Now.Day - record.BorrowDate.Day > 30)
            {
                var penaltydays = DateTime.Now.Day - record.BorrowDate.Day;
                user.Penalty += penaltydays * 10;
            }
            return Content("Done");
        }
        public IActionResult Pay(int userid)
        {
            var user = Users.FirstOrDefault(u => u.Id == userid);
            if (user == null)
            {
                return Content("user not found");
            }
            if (user.Penalty == 0)
            {
                return Content("You dont neeed to pay");
            }
            user.Penalty = 0;
            return Content("successfull");
        }
    }
}
