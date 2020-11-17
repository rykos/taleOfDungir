using System.ComponentModel.DataAnnotations;

namespace taleOfDungir.Models
{
    public class AdminRegisterModel : RegisterModel
    {
        [Required(ErrorMessage = "Key is required")]
        public string Key { get; set; }
    }
}