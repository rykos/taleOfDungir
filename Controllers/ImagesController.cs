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
                return Ok();
            }
            return File(imageDBModel.Image, "image/jpeg");
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public IActionResult NewImage([FromForm] ImageDTO imageDTO)
        {
            if (new string[] { "item", "avatar" }.Contains(imageDTO.Category) == false)
                return BadRequest(Models.Response.ErrorResponse("Bad category"));
            //No file | file size > 5MB
            if (imageDTO.File.Length < 1 || imageDTO.File.Length > 5242880)
                return BadRequest(Models.Response.ErrorResponse("Invalid file"));

            using (MemoryStream ms = new MemoryStream())
            {
                imageDTO.File.CopyTo(ms);
                byte imageType = this.GetImageType(imageDTO);
                if (imageType == byte.MaxValue)
                    return BadRequest(Models.Response.ErrorResponse("Bad type"));

                ImageDBModel imageDBModel = new ImageDBModel()
                {
                    Image = ms.ToArray(),
                    Category = imageDTO.Category,
                    Type = imageType
                };
                this.dbContext.ImageDBModels.Add(imageDBModel);
                if (this.dbContext.SaveChanges() == 1)
                {
                    return Ok();
                }
            }
            return BadRequest(Models.Response.ErrorResponse("Failed to add new image"));
        }

        /// <returns>255 on error</returns>
        private byte GetImageType(ImageDTO imageDTO)
        {
            object enumParsed = null;
            foreach (Type type in new Type[] { typeof(ItemType), typeof(EntityType) })
            {
                if (Enum.TryParse(typeof(EntityType), imageDTO.Type, true, out enumParsed))
                    break;
            }
            return (enumParsed == null) ? byte.MaxValue : (byte)enumParsed;
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