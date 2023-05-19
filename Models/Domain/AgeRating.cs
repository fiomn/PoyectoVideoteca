namespace ProyectoVideoteca.Models.Domain
{
    public class AgeRating
    {
        public int id { get; set; }
        public int ageRating { get; set; }

        public AgeRating()
        {

        }
        public AgeRating(int id, int ageRating)
        {
            this.id = id;
            this.ageRating = ageRating;

        }
    }
}
