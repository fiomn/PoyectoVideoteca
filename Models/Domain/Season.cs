namespace ProyectoVideoteca.Models.Domain
{
    public class Season
    {
        public int Id { get; set; }
        public int number { get; set; }
        public int numEpisodes { get; set; } //DB name = qtyEpisodes
        public Episode episode { get; set; } //Could be an array because a Serie contains many episodes

        public Season()
        {

        }
        public Season(int Id, int number, int numEpisodes, Episode episode)
        {
            this.Id = Id;
            this.number = number;
            this.numEpisodes = numEpisodes;
            this.episode = episode;
        }

    }

    public class Episode
    {
        public int id { get; set; }
        public string name { get; set; }
        public int duration { get; set; }
        public int score { get; set; }

        public Episode()
        {

        }
        public Episode(int id, string name, int duration, int score)
        {
            this.id = id;
            this.name = name;
            this.duration = duration;
            this.score = score;
        }
    }
}
