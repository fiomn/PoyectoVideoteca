namespace ProyectoVideoteca.Models.Domain
{
    public class Movie
    {
        public int id { get; set; }
        public string movieTitle { get; set; }
        public DateOnly releaseYear { get; set; }
        public Director director { get; set; }
        public Genre genre { get; set; }
        public int duration { get; set; } //Duration = minutes
        public AgeRating ageRating { get; set; }
        public string synopsis { get; set; }
        public Language language { get; set; }
        public Country country { get; set; }
        public int score { get; set; }
        public Actor actor { get; set; }
        public Comment comment { get; set; }

        public Movie()
        {

        }
        public Movie(int id, string movieTitle, DateOnly releaseYear, Director director, Genre genre,
            int duration, AgeRating ageRating, string synopsis, Language language, Country country,
            int score, Actor actor, Comment comment)
        {
            this.id = id;
            this.movieTitle = movieTitle;
            this.releaseYear = releaseYear;
            this.director = director;
            this.genre = genre;
            this.duration = duration;
            this.ageRating = ageRating;
            this.synopsis = synopsis;
            this.language = language;
            this.country = country;
            this.score = score;
            this.actor = actor;
            this.comment = comment;

        }

    }
}
