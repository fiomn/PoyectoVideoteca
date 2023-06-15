namespace ProyectoVideoteca.Models
{
    public class tb_MOVIEANDCOMMENTS
    {

        public tb_MOVIE movie { get; set; }

        public List<tb_RATING> comments { get; set; }

        public tb_MOVIEANDCOMMENTS(tb_MOVIE movie, List<tb_RATING> comments)
        {

            this.movie = movie;
            this.comments = comments;

        }
    }
}
