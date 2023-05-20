using System;
using System.Collections.Generic;

namespace ProyectoVideoteca.Models;

public partial class tb_SERIE
{
    public string TITLE { get; set; } = null!;

    public virtual tb_AUDIOVISUAL_PRODUCTION TITLENavigation { get; set; } = null!;

    public virtual ICollection<tb_SEASON> tb_SEASONs { get; set; } = new List<tb_SEASON>();
}
