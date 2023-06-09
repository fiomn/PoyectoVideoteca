namespace ProyectoVideoteca.Models
{
    public class tb_MOVIESANDGENRES
    {

        public List<tb_MOVIE> movies { get; set; }

        public List<tb_GENRE> genres { get; set; }

        public tb_MOVIESANDGENRES(List<tb_MOVIE> movies, List<tb_GENRE> genres)
        {

            this.movies = movies;
            this.genres = genres;

        }
    }
}
