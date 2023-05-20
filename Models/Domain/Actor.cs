namespace ProyectoVideoteca.Models.Domain
{
    public class Actor
    {
        public int id { get; set; }
        public string name { get; set; }
        public string lastName { get; set; }

        public Actor()
        {

        }

        public Actor(int id, string name, string lastName)
        {
            this.id = id;
            this.name = name;
            this.lastName = lastName;

        }
    }

}
