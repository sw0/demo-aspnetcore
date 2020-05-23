using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiDemo.Models;

namespace WebApiDemo.Controllers
{
    /*
     * Get list
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
    [ApiController]
    public class PoemController : ControllerBase
    {
        static List<PoemViewModel> Items = new List<PoemViewModel> {
                new PoemViewModel{  PoemId = 1, Title = "静夜思", Author = "李白", Content = "床前明月光。。。。", CreateDate = DateTime.Now, Decription = "" },
                new PoemViewModel{  PoemId = 2, Title = "凉州词", Author = "杜牧", Content = "葡萄美酒夜光杯。。。。", CreateDate = DateTime.Now, Decription = ""},
                new PoemViewModel{  PoemId = 3, Title = "池上", Author = "白居易", Content = "小娃撑小艇，偷采白莲回。。。。", CreateDate = DateTime.Now, Decription = ""}
            };

        [HttpGet("")]
        public IActionResult Get(int pagesize = 5, int pageNumber = 1)
        {
            var items = Items.Skip(pagesize * (pageNumber - 1)).Take(pagesize);

            return Ok(items);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var item = Items.FirstOrDefault(t => id == t.PoemId);

            if (item == null)
                return NotFound();

            return Ok(item);
        }

        [HttpGet("author/{name:alpha}")]
        public IActionResult Get(string name, [FromServices]IWebHostEnvironment env)
        {
            var file = System.IO.Path.Combine(env.WebRootPath, $"imgs/{name}.jpg");
            Console.WriteLine(file);
            return PhysicalFile(file, "image/jpeg", $"{name}.jpg");
        }

        [HttpPost]
        public IActionResult Post(PoemViewModel data)
        {
            if (data.PoemId > 0)
                return BadRequest();

            var newId = 1;
            if (Items.Count > 0)
                newId = Items.Max(r => r.PoemId) + 1;

            data.PoemId = newId;

            Items.Add(data);

            return CreatedAtAction(nameof(Post), new { id = newId }, data);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, PoemViewModel data)
        {
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

        [HttpDelete("{id}")]
        public IActionResult Put(int id)
        {
            var row = Items.FirstOrDefault(r => r.PoemId == id);

            if (row == null)
                return NotFound();

            Items.Remove(row);

            return NoContent();
        }
    }
}