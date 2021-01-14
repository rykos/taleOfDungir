using System;
using System.Linq;
using taleOfDungir.Data;
using taleOfDungir.Models;

namespace taleOfDungir.Helpers
{
    public class NameHelper : NameHelperProvider
    {
        private readonly AppDbContext dbContext;
        public NameHelper(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public string NameFor(ItemType itemType)
        {
            int count = this.dbContext.ItemNames.Where(i => i.ItemType == itemType)?.Count() ?? 0;
            if (count == 0)
            {
                return "Missing names";
            }
            Random rnd = new Random();
            return this.dbContext.ItemNames.Where(i => i.ItemType == itemType)?.Skip(rnd.Next(0, count))?.FirstOrDefault()?.Name ?? "Missing names";
        }

        public long RandomAvatarId()
        {
            int count = this.dbContext.ImageDBModels.Where(img => img.Category == "avatar").Count();
            Random rnd = new Random();
            return this.dbContext.ImageDBModels.Where(img => img.Category == "avatar").Select(img => img.Id).Skip(rnd.Next(0, count)).First();
        }

        public long RandomAvatarIdFor(EntityType entityType)
        {
            int count = this.dbContext.ImageDBModels.Where(img => img.Category == "avatar" && img.Type == (byte)entityType).Count();
            Random rnd = new Random();
            return this.dbContext.ImageDBModels.Where(img => img.Category == "avatar" && img.Type == (byte)entityType)
                .Select(img => img.Id).Skip(rnd.Next(0, count)).First();
        }

        /// <summary>
        /// Returns random image id with specific itemType. (-1 if non existant)
        /// </summary>
        public long RandomItemImageIDFor(ItemType itemType)
        {
            Random rnd = new Random();
            int amount = this.dbContext.ImageDBModels.Where(img => img.Category == "item" && img.Type == Convert.ToByte(itemType)).Count();
            if (amount == 0)
            {
                DebugHelper.WriteError($"ERROR: NO IMAGE FOR ItemType:({itemType})");
                return -1;
            }
            long imgId = this.dbContext.ImageDBModels.Where(img => img.Category == "item" && img.Type == Convert.ToByte(itemType)).Select(img => img.Id).Skip(rnd.Next(0, amount)).FirstOrDefault();
            return imgId;
        }
    }

    public interface NameHelperProvider
    {
        string NameFor(ItemType itemType);
        long RandomAvatarId();
        long RandomAvatarIdFor(EntityType entityType);
        long RandomItemImageIDFor(ItemType itemType);
    }
}