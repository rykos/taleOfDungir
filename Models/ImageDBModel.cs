namespace taleOfDungir.Models
{
    public class ImageDBModel
    {
        public long Id { get; set; }
        /// <summary>
        /// Image category [weapon, armor, avatar]
        /// </summary>
        public string Category { get; set; }
        /// <summary>
        /// Serialized image
        /// </summary>
        public byte[] Image { get; set; }
    }
}