namespace taleOfDungir.Models
{
    public class ImageDBModel
    {
        public long Id { get; set; }
        /// <summary>
        /// Image category [item, avatar]
        /// </summary>
        public string Category { get; set; }
        public byte Type { get; set; } = 0;
        /// <summary>
        /// Serialized image
        /// </summary>
        public byte[] Image { get; set; }
    }
}