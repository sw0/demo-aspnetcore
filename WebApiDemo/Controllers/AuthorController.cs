using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiDemo.Filters;
using WebApiDemo.Models;
using WebApiDemo.Services;

namespace WebApiDemo.Controllers
{
    [Route("api/[controller]")]
    public class AuthorController : MyBaseController
    {
        IAuthorService _author;

        public AuthorController(IAuthorService author)
        {
            _author = author;
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var author = _author.GetAll().FirstOrDefault(r => r.AuthorId == id);

            return Ok(author);
        }

        [HttpGet("list")]
        public IEnumerable<AuthorViewModel> GetObjects()
        {
            return _author.GetAll();
        }

        /// <summary>
        /// Always failed in validation in custom filter: <see cref="AlwaysInvalidValidationAttribute"/>
        /// </summary>
        /// <returns></returns>
        [HttpGet("custom")]
        [AlwaysInvalidValidation]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        public IActionResult GetCustomValidation()
        {
            //ACTUALLY, WILL NOT HIT HERE. 
            //It short-circuits in custom ActionFilter.

            if (ModelState.IsValid)
            {
                return BadRequest(new ValidationProblemDetails(ModelState)
                {
                    Title = "Model validated in custom ActionFilter",
                });
            }
            return Content("Actually, this should not be reached. Because the filter always failed");
        }
    }
}
