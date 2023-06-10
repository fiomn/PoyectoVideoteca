namespace ProyectoVideoteca.Models
{
    public class tb_SERIESANDGENRES
    {

        public List<tb_SERIE> series { get; set; }

        public List<tb_GENRE> genres { get; set; }

        public tb_SERIESANDGENRES(List<tb_SERIE> series, List<tb_GENRE> genres)
        {

            this.series = series;
            this.genres = genres;

        }
    }
}
