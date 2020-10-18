namespace taleOfDungir.Models
{
    public class Response
    {
        public string Type { get; set; }
        public string Message { get; set; }

        public Response(string Type, string Message)
        {
            this.Type = Type;
            this.Message = Message;
        }
    }
}