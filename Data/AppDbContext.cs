using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using taleOfDungir.Models;

namespace taleOfDungir.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //Character
            builder.Entity<ApplicationUser>()
                .HasOne(au => au.Character)
                .WithOne(c => c.ApplicationUser)
                .HasForeignKey<Character>(c => c.ApplicationUserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            //Inventory
            builder.Entity<Character>()
                .HasMany(c => c.Inventory)
                .WithOne(i => i.Character)
                .HasForeignKey(e => e.CharacterId)
                .OnDelete(DeleteBehavior.Cascade);

            //Equipment
            builder.Entity<Character>()
                .HasMany(c => c.Equipment)
                .WithOne(e => e.Wearer)
                .HasForeignKey(e => e.WearerId)
                .OnDelete(DeleteBehavior.Cascade);

            //Skills
            builder.Entity<Character>()
                .HasOne(c => c.Skills)
                .WithOne(s => s.Character)
                .HasForeignKey<Skills>(s => s.CharacterId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        public DbSet<Character> Characters { get; set; }
        //
        public DbSet<Item> Items { get; set; }
        public DbSet<Weapon> Weapons { get; set; }
        public DbSet<Armor> Armors { get; set; }
    }
}