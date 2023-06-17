namespace ProyectoVideoteca.Models
{
    public class tb_SERIEANDCOMMENTS
    {
        public tb_SERIE serie { get; set; }

        public List<tb_RATING> comments { get; set; }

        public int totalPages { get; set; }

        public int currentPage { get; set; }

        public tb_SERIEANDCOMMENTS(tb_SERIE serie, List<tb_RATING> comments, int totalPages, int currentPage)
        {
            this.serie = serie;
            this.comments = comments;
            this.totalPages = totalPages;
            this.currentPage = currentPage;
        }


    }
}
