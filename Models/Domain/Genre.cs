namespace ProyectoVideoteca.Models.Domain
{
    public class Genre
    {
        public int Id { get; set; }
        public string nameGenre { get; set; }

        public Genre()
        {

        }
        public Genre(int id, string nameGenre)
        {
            Id = id;
            this.nameGenre = nameGenre;

        }
    }
}
