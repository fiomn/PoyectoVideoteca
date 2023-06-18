namespace ProyectoVideoteca.Models
{
    public class Message
    {
        public string text { set; get; }
        public string type { set; get; }

    }

    public enum Types { danger, success, warning };

}
