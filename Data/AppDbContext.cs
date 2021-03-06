using System.Collections.Generic;
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
                .HasForeignKey(i => i.CharacterId)
                .OnDelete(DeleteBehavior.Cascade);

            //Skills
            builder.Entity<Character>()
                .HasOne(c => c.Skills)
                .WithOne(s => s.Character)
                .HasForeignKey<Skills>(s => s.CharacterId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Character>()
                .HasMany(c => c.Missions)
                .WithOne(m => m.Character)
                .HasForeignKey(m => m.CharacterId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Event>()
                .HasMany(e => e.EventActions)
                .WithOne()
                .HasForeignKey(ea => ea.EventId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Character>()
                .HasOne(c => c.CharacterMetaData)
                .WithOne()
                .HasForeignKey<CharacterMetaData>(c => c.CharacterId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        public DbSet<Character> Characters { get; set; }//Player characters
        //
        public DbSet<Item> Items { get; set; }//Generic Items
        public DbSet<ItemName> ItemNames { get; set; }//Table for item names with specific ItemType
        //
        public DbSet<Mission> Missions { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<EventAction> EventActions { get; set; }
        //
        public DbSet<ImageDBModel> ImageDBModels { get; set; }

        public DbSet<CharacterMetaData> CharacterMetaDatas { get; set; }
    }
}