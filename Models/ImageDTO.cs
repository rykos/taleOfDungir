using Microsoft.AspNetCore.Http;

namespace taleOfDungir.Models
{
    public class ImageDTO
    {
        public IFormFile File { get; set; }
        public string Category { get; set; }
    }
}