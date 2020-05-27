using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiDemo.Models;

namespace WebApiDemo.Controllers
{
    /*
     * Get list with pagination
     *      IActionResult + ProducesResponseType(status code, TYPE) => ActionResult<T> + ProducesResponseType(status code)
     *      Get list with complex object in query by using [FromQuery]      
     * Get one
     * Post: add
     * Put: modify
     * Delete: remove
     * Patch: todo
     * 
     * physicalfile: download libai.jpg
     * 
     */
    [Route("api/[controller]")]
    //[ApiController]  //moved to custom base controller that no need to add to every controller
    //[Produces(System.Net.Mime.MediaTypeNames.Application.Json)] //or 
    //[Produces("application/json")]
    public class PoemController : ControllerBase
    {
        static List<PoemViewModel> Items = new List<PoemViewModel> {
                new PoemViewModel{  PoemId = 1, Title = "静夜思", Author = "李白", Content = "床前明月光。。。。", CreateDate = DateTime.Now, Decription = "" },
                new PoemViewModel{  PoemId = 2, Title = "凉州词", Author = "杜牧", Content = "葡萄美酒夜光杯。。。。", CreateDate = DateTime.Now, Decription = ""},
                new PoemViewModel{  PoemId = 3, Title = "池上", Author = "白居易", Content = "小娃撑小艇，偷采白莲回。。。。", CreateDate = DateTime.Now, Decription = ""}
            };

        /// <summary>
        /// Get pome list with pagination
        /// </summary>
        /// <remarks>
        /// 
        ///    GET /api/poem
        ///    [
        ///      { "poemid":1, "title":"jingyesi", "author":"libai"},
        ///      { "poemid":2, "title":"zengwanglun", "author":"libai"}
        ///    ]
        ///    
        /// </remarks>
        /// <param name="pagesize"></param>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        [HttpGet("")]
        //public IActionResult Get(int pagesize = 5, int pageNumber = 1)
        public ActionResult<IEnumerable<PoemViewModel>> Get(int pagesize = 5, int pageNumber = 1)
        {
            var items = Items.Skip(pagesize * (pageNumber - 1)).Take(pagesize);

            return Ok(items);
        }

        /// <summary>
        /// get list via complext model by GET, [FromQuery] must be specified.
        /// </summary>
        /// <remarks>
        /// url should be like <![CDATA[api/poem/list?paging.pageSize=2&paging.PageNumber=2]]> with prefix `paging` (name of the parameter of complex type)
        /// </remarks>
        /// <param name="paging"></param>
        /// <returns></returns>
        [HttpGet("list")]
        public IActionResult List([FromQuery]PagingModel paging)
        {
            var items = Items.Skip(paging.PageSize * (paging.PageNumber - 1)).Take(paging.PageSize);

            return Ok(items);
        }


        /// <summary>
        /// get poem by given id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <response code="200">Returns the newly created item</response>
        /// <response code="404">If the item is null</response> 
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var item = Items.FirstOrDefault(t => id == t.PoemId);

            if (item == null)
                return NotFound();

            return Ok(item);
        }

        [HttpGet("testxml/{id}")]
        public IActionResult GetTestXml(int id)
        {
            var item = Items.FirstOrDefault(t => id == t.PoemId);

            //if (item == null)
            //    return NotFound();

            var response = Ok(item);

            return response;
        }

        /// <summary>
        /// get the author photo by name
        /// </summary>
        /// <param name="name"></param>
        /// <param name="env"></param>
        /// <returns></returns>
        [HttpGet("author/{name:alpha}")]
        public IActionResult Get(string name, [FromServices] IWebHostEnvironment env)
        {
            var file = System.IO.Path.Combine(env.WebRootPath, $"imgs/{name}.jpg");
            Console.WriteLine(file);
            return PhysicalFile(file, "image/jpeg", $"{name}.jpg");
        }

        /// <summary>
        /// add a new poem
        /// </summary>
        /// <remarks>
        /// 
        ///    POST /api/poem
        ///    {"title":"new title", "author":"author name","content":"this is content"}
        ///    
        /// </remarks>
        /// <param name="data"></param>
        /// <returns></returns>
        /// <response code="201">Returns the newly created item</response>
        /// <response code="400">If the item is null</response> 
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(PoemViewModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost]
        //[Consumes("application/xml","application/json")]
        public IActionResult Post(PoemViewModel data)
        {
            if (!ModelState.IsValid)
            {
                //[ApiController] 's default model state invalid got suppresssed: SuppressModelStateInvalidFilter
                //see Startup.cs and https://docs.microsoft.com/en-us/aspnet/core/web-api/?view=aspnetcore-3.1#binding-source-parameter-inference
                //return BadRequest(ModelState); //old version response: SerializableError class

                //after 2.2 (include 2.2)
                return BadRequest(new ValidationProblemDetails(ModelState)
                {
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                    //Instance = 
                    //todo learn more about ValidationProblemDetails
                });
            }

            if (data.PoemId > 0)
                return BadRequest();

            var newId = 1;
            if (Items.Count > 0)
                newId = Items.Max(r => r.PoemId) + 1;

            data.PoemId = newId;

            Items.Add(data);

            return CreatedAtAction(nameof(Post), new { id = newId }, data);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(PoemViewModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("create")]
        public IActionResult Post2([FromForm]PoemViewModel data)
        {
            if (!ModelState.IsValid)
            {
                //[ApiController] 's default model state invalid got suppresssed: SuppressModelStateInvalidFilter
                //see Startup.cs and https://docs.microsoft.com/en-us/aspnet/core/web-api/?view=aspnetcore-3.1#binding-source-parameter-inference
                //return BadRequest(ModelState); //old version response: SerializableError class

                //after 2.2 (include 2.2)
                return BadRequest(new ValidationProblemDetails(ModelState)
                {
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                    //Instance = 
                    //todo learn more about ValidationProblemDetails
                });
            }

            if (data.PoemId > 0)
                return BadRequest();

            var newId = 1;
            if (Items.Count > 0)
                newId = Items.Max(r => r.PoemId) + 1;

            data.PoemId = newId;

            Items.Add(data);

            return CreatedAtAction(nameof(Post), new { id = newId }, data);
        }

        /// <summary>
        /// update given poem data
        /// </summary>
        /// <param name="id"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        /// <response code="200">update successfully and return latest poem</response>
        /// <response code="400">If the item is null</response> 
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PoemViewModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Put(int id, PoemViewModel data)
        {
            if (!ModelState.IsValid)
            {
                //[ApiController] 's default model state invalid got suppresssed: SuppressModelStateInvalidFilter
                //see Startup.cs and https://docs.microsoft.com/en-us/aspnet/core/web-api/?view=aspnetcore-3.1#binding-source-parameter-inference
                //return BadRequest(ModelState); //old version response: SerializableError class

                //after 2.2 (include 2.2)
                return BadRequest(new ValidationProblemDetails(ModelState)
                {
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                    //Instance = 
                    //todo learn more about ValidationProblemDetails
                });
            }

            if (data.PoemId != id)
                return BadRequest();

            var row = Items.FirstOrDefault(r => r.PoemId == id);

            if (row == null)
                return NotFound();

            row.Title = data.Title;
            row.Author = data.Author;
            row.Content = data.Content;
            row.Decription = data.Decription;
            row.CreateDate = data.CreateDate;

            return Ok(data);
        }
        /// <summary>
        /// delete the poem by given id
        /// </summary>
        /// <response code="201">Returns the newly created item</response>
        /// <response code="404">cannot find the item</response> 
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var row = Items.FirstOrDefault(r => r.PoemId == id);

            if (row == null)
                return NotFound();

            Items.Remove(row);

            return NoContent();
        }
    }
}