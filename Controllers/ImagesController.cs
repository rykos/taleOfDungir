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
            //|| imageDTO.Files.Length > 5242880
            if (imageDTO.Files.Count() < 1)
                return BadRequest(Models.Response.ErrorResponse("No files"));

            using (MemoryStream ms = new MemoryStream())
            {
                foreach (IFormFile file in imageDTO.Files)
                {
                    file.CopyTo(ms);
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
                }
                int changesCount;
                if ((changesCount = this.dbContext.SaveChanges()) > 0)
                {
                    return Ok(changesCount);
                }
            }
            return BadRequest(Models.Response.ErrorResponse("Failed to add new image"));
        }

        [HttpGet]
        [Route("category/{category}")]
        public IActionResult GetImagesIdByCategory(string category)
        {
            long[] ids = this.dbContext.ImageDBModels.Where(img => img.Category == category).Select(img => img.Id).ToArray();
            return Ok(ids);
        }

        [Authorize(Roles = "admin")]
        [HttpDelete]
        [Route("{id}")]
        public IActionResult RemoveImageById(long id)
        {
            ImageDBModel imageDBModel = this.dbContext.ImageDBModels.FirstOrDefault(img => img.Id == id);
            if (imageDBModel == default)
            {
                return NotFound();
            }
            else
            {
                this.dbContext.ImageDBModels.Remove(imageDBModel);
                this.dbContext.SaveChanges();
                return Ok();
            }
        }

        /// <returns>255 on error</returns>
        private byte GetImageType(ImageDTO imageDTO)
        {
            object enumParsed = null;
            foreach (Type type in new Type[] { typeof(ItemType), typeof(EntityType) })
            {
                if (Enum.TryParse(type, imageDTO.Type, true, out enumParsed))
                    break;
            }
            return (enumParsed == null) ? byte.MaxValue : (byte)enumParsed;
        }
    }
}