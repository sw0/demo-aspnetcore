using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiDemo.Models
{
    public class AuthorViewModel
    {
        public int AuthorId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Dynasity { get; set; }

        public string Name => LastName + FirstName;
    }
}
