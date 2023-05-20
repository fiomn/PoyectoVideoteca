namespace ProyectoVideoteca.Models.Domain
{
    public class Comment
    {
        public int id { get; set; }
        public string comment { get; set; }

        public Comment()
        {

        }
        public Comment(int id, string comment)
        {
            this.id = id;
            this.comment = comment;

        }
    }
}
