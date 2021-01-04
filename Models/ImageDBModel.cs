namespace taleOfDungir.Models
{
    public class ImageDBModel
    {
        public long Id { get; set; }
        /// <summary>
        /// Image category [item, avatar]
        /// </summary>
        public string Category { get; set; }
        /// <summary>
        /// ItemType enum
        /// </summary>
        public ItemType ItemType { get; set; } = 0;
        /// <summary>
        /// Serialized image
        /// </summary>
        public byte[] Image { get; set; }
    }
}