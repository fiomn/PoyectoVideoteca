namespace ProyectoVideoteca.Models
{
    public class tb_SERIEANDCOMMENTS
    {
        public tb_SERIE serie { get; set; }

        public tb_SEASON? season { get; set; }

        public List<tb_EPISODE>? episodes { get; set; }

        public List<tb_RATING> comments { get; set; }

        public int totalPages { get; set; }

        public int currentPage { get; set; }


        public tb_SERIEANDCOMMENTS(tb_SERIE serie, tb_SEASON season, List<tb_EPISODE> episodes, List<tb_RATING> comments, int totalPages, int currentPage)
        {
            this.serie = serie;
            this.season = season;
            this.episodes = episodes;
            this.comments = comments;
            this.totalPages = totalPages;
            this.currentPage = currentPage;
        }

        public tb_SERIEANDCOMMENTS(tb_SERIE serie, List<tb_RATING> comments, int totalPages, int currentPage)
        {
            this.serie = serie;
            this.comments = comments;
            this.totalPages = totalPages;
            this.currentPage = currentPage;
        }


    }
}
