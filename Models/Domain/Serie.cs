namespace ProyectoVideoteca.Models.Domain
{
    public class Serie
    {
        public int id { get; set; }
        public string serieTitle { get; set; }
        public DateOnly startYear { get; set; }
        public DateOnly? endYear { get; set; } //could be null
        public string creator { get; set; }
        public Genre genre { get; set; }
        public int duration { get; set; } //Duration = minutes
        public AgeRating ageRating { get; set; }
        public string synopsis { get; set; }
        public Language language { get; set; }
        public Country country { get; set; }
        public int score { get; set; }
        public Season season { get; set; }
        public Actor actor { get; set; }
        public Comment comment { get; set; }

        public Serie()
        {

        }
        public Serie(int id, string serieTitle, DateOnly startYear, DateOnly endYear, string creator,
            Genre genre,
            int duration, AgeRating ageRating, string synopsis, Language language, Country country,
            int score, Season season, Actor actor, Comment comment)
        {
            this.id = id;
            this.serieTitle = serieTitle;
            this.startYear = startYear;
            this.endYear = endYear;
            this.creator = creator;
            this.genre = genre;
            this.duration = duration;
            this.ageRating = ageRating;
            this.synopsis = synopsis;
            this.language = language;
            this.country = country;
            this.score = score;
            this.season = season;
            this.actor = actor;
            this.comment = comment;

        }

    }
}
