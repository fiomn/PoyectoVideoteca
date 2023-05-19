namespace ProyectoVideoteca.Models.Domain
{
    public class Country
    {
        public int id { get; set; }
        public string name { get; set; }

        public Country()
        {

        }
        public Country(int id, string name)
        {
            this.id = id;
            this.name = name;

        }
    }
}
