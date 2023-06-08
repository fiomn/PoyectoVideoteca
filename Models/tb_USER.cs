using ProyectoVideoteca.Models.Domain;

namespace ProyectoVideoteca.Models;

public partial class tb_USER
{
    //public ApplicationUser applicationUser { get; set; }
    public string USERNAME { get; set; } = null!;

    public string PASSWORD { get; set; } = null!;

    public string NAME { get; set; } = null!;

    public string EMAIL { get; set; } = null!;

    public string? PASSWORD_CONFIRM { get; set; }

    public string ROLE { get; set; } = null!;

    public string? IMG { get; set; }

    public virtual ICollection<tb_RATING> tb_RATINGs { get; set; } = new List<tb_RATING>();
    
}
