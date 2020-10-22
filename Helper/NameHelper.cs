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
            return this.dbContext.ItemNames.Where(i => i.ItemType == itemType).Select(i => i.Name).OrderBy(i => Guid.NewGuid()).First();
        }
    }

    public interface NameHelperProvider
    {
        string GetNameFor(ItemType itemType);
    }
}