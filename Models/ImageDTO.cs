using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace taleOfDungir.Models
{
    public class ImageDTO
    {
        [Required(ErrorMessage = "File required")]
        public IFormFile File { get; set; }
        [Required(ErrorMessage = "Category required")]
        public string Category { get; set; }
        /// <summary>
        /// Represented as string for smaller margin of error in transport
        /// </summary>
        [Required(ErrorMessage = "Item type required")]
        public string ItemType { get; set; }
    }
}