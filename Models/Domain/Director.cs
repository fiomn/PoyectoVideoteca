namespace ProyectoVideoteca.Models.Domain
{
    public class Director
    {
        public int id { get; set; }
        public string name { get; set; }
        public string lastName { get; set; }

        public Director()
        {

        }
        public Director(int id, string name, string lastName)
        {
            this.id = id;
            this.name = name;
            this.lastName = lastName;

        }
    }
}
