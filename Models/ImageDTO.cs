using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace taleOfDungir.Models
{
    public class ImageDTO
    {
        [Required(ErrorMessage = "File required")]
        public IEnumerable<IFormFile> Files { get; set; }
        [Required(ErrorMessage = "Category required")]
        //item, avatar
        public string Category { get; set; }
        
        public string Type { get; set; }
    }
}