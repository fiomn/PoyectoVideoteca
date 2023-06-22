namespace ProyectoVideoteca.Models
{
    public class tb_ACTOR_DETAILS
    {

        public tb_MOVIE movie { get; set; }

        public tb_SERIE serie { get; set; }

        public List<tb_SECONDARY_ACTOR> actors { get; set; }

        public tb_ACTOR_DETAILS(tb_MOVIE movie, List<tb_SECONDARY_ACTOR> actors)
        {

            this.movie = movie;
            this.actors = actors;

        }

        public tb_ACTOR_DETAILS(tb_SERIE serie, List<tb_SECONDARY_ACTOR> actors)
        {

            this.serie = serie;
            this.actors = actors;

        }

    }
}
