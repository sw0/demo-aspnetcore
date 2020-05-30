using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiDemo.Models;

namespace WebApiDemo.Services
{
    public interface IAuthorService
    {
        List<AuthorViewModel> GetAll();
        bool IsValidAuthor(int authorId);
        bool IsValidAuthor(string author);
    }
}
