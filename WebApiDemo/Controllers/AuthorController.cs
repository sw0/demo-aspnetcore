using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using System;
using System.Collections.Generic;
using System.IO;
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
        IWebHostEnvironment _env;
        IAuthorService _author;

        public AuthorController(IWebHostEnvironment env, IAuthorService author)
        {
            _env = env;
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


        [HttpPost("upload")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Post(AuthorPhotoCreateViewModel authorPhoto)
        {
            if(authorPhoto?.Data == null)
            {
                return BadRequest("no photo data provided");
            }

            var filename = Path.GetRandomFileName().Replace(".", "");

            var file = Path.Combine(_env.WebRootPath, $"imgs/{authorPhoto.AuthorId}-{filename}{authorPhoto.Extension}");

            if (System.IO.File.Exists(file))
            {
                return Conflict("file already existed");
            }

            await System.IO.File.WriteAllBytesAsync(file, authorPhoto.Data);

            return Ok(new
            {
                FileName = file,
            });
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
