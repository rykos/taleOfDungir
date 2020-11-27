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

        public static string Success { get { return "Success"; } }
        public static string Error { get { return "Error"; } }
    }
}