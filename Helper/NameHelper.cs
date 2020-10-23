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

        public string GetNameFor(ItemType itemType)
        {
            int count = this.dbContext.ItemNames.Count();
            Random rnd = new Random();
            return this.dbContext.ItemNames.Where(i => i.ItemType == itemType).Skip(rnd.Next(0, count)).First().Name;
        }
    }

    public interface NameHelperProvider
    {
        string GetNameFor(ItemType itemType);
    }
}