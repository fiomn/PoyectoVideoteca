namespace ProyectoVideoteca.Models.Domain
{
    public class Language
    {
        public int id { get; set; }
        public string nameLanguage { get; set; }

        public Language()
        {

        }
        public Language(int id, string nameLanguage)
        {
            this.id = id;
            this.nameLanguage = nameLanguage;
        }
    }
}
