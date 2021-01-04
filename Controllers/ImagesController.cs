using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize(Roles = "admin")]
        [HttpPost]
        public IActionResult NewImage([FromForm] ImageDTO imageDTO)
        {
            if (imageDTO.Category != "item" || imageDTO.Category != "avatar")
            {
                return BadRequest(new Response(Models.Response.Error, "Bad category"));
            }
            //No file | file size > 5MB
            if (imageDTO.File.Length < 1 || imageDTO.File.Length > 5242880)
            {
                return BadRequest(new Response(Models.Response.Error, "Invalid file"));
            }
            using (MemoryStream ms = new MemoryStream())
            {
                imageDTO.File.CopyTo(ms);
                ItemType itemType;
                //ItemType conversion failed
                if (!Enum.TryParse<ItemType>(imageDTO.ItemType, true, out itemType))
                {
                    return BadRequest(new Response(Models.Response.Error, "Invalid ItemType"));
                }
                ImageDBModel imageDBModel = new ImageDBModel()
                {
                    Image = ms.ToArray(),
                    Category = imageDTO.Category,
                    ItemType = itemType
                };
                this.dbContext.ImageDBModels.Add(imageDBModel);
                if (this.dbContext.SaveChanges() == 1)
                {
                    return Ok();
                }
            }
            return BadRequest();
        }

        [HttpGet]
        [Route("category/{category}")]
        public IActionResult GetImagesIdByCategory(string category)
        {
            long[] ids = this.dbContext.ImageDBModels.Where(img => img.Category == category).Select(img => img.Id).ToArray();
            return Ok(ids);
        }
    }
}