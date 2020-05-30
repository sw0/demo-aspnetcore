using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiDemo.Models;

namespace WebApiDemo.Services
{
    public class AuthorService : IAuthorService
    {
        //TODO we should use domain models here instead
        private static List<AuthorViewModel> Authors = new List<AuthorViewModel>();
        ILogger<AuthorService> _logger;
        public AuthorService(ILogger<AuthorService> logger)
        {
            _logger = logger;
        }

        static AuthorService()
        {
            Authors.Add(new AuthorViewModel { AuthorId = 1, Dynasity = "唐", FirstName = "白", LastName = "李" });
            Authors.Add(new AuthorViewModel { AuthorId = 2, Dynasity = "唐", FirstName = "居易", LastName = "白" });
            Authors.Add(new AuthorViewModel { AuthorId = 3, Dynasity = "唐", FirstName = "甫", LastName = "杜" });
            Authors.Add(new AuthorViewModel { AuthorId = 3, Dynasity = "唐", FirstName = "牧", LastName = "杜" });
            Authors.Add(new AuthorViewModel { AuthorId = 3, Dynasity = "宋", FirstName = "清照", LastName = "李" });
        }


        public List<AuthorViewModel> GetAll()
        {
            return Authors;
        }

        public bool IsValidAuthor(int authorId)
        {
            return Authors.Any(a => a.AuthorId == authorId);
        }

        public bool IsValidAuthor(string author)
        {
            return Authors.Any(a => a.Name == author);
        }
    }
}
