using System;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using taleOfDungir.Data;
using taleOfDungir.Models;

namespace taleOfDungir.Controllers
{
    [ApiController]
    [Route("images")]
    public class ImagesController : ControllerBase
    {
        private readonly AppDbContext dbContext;
        public ImagesController(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult GetImage(long id)
        {
            ImageDBModel imageDBModel = this.dbContext.ImageDBModels.FirstOrDefault(img => img.Id == id);
            if (imageDBModel == default)
            {
                return null;
            }
            return File(imageDBModel.Image, "image/jpeg");
        }

        [HttpPost]
        public IActionResult NewImage([FromForm] ImageDTO imageDTO)
        {
            //No file
            if (imageDTO.File.Length < 1)
            {
                return BadRequest();
            }
            using (MemoryStream ms = new MemoryStream())
            {
                imageDTO.File.CopyTo(ms);
                ImageDBModel imageDBModel = new ImageDBModel() { Image = ms.ToArray(), Category = imageDTO.Category };
                this.dbContext.ImageDBModels.Add(imageDBModel);
                if (this.dbContext.SaveChanges() == 1)
                {
                    return Ok();
                }
            }
            return BadRequest();
        }
    }
}