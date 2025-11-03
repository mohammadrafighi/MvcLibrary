using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcLibrary.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool BorrowedBook { get; set; }
        public double Penalty { get; set; } 
    }
}
