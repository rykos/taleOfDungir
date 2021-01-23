using System;

namespace taleOfDungir.Models
{
    public class MerchantStock
    {
        public object[] Items { get; set; }
        /// <summary>
        /// Time at which items restock
        /// </summary>
        public DateTime RestockTime { get; set; }
    }
}