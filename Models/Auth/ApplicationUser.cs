using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace taleOfDungir.Models
{
    public class ApplicationUser : IdentityUser
    {
        public Character Character { get; set; }
        public Int64 CharacterId { get; set; }
    }
}