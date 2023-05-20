using System;
using System.Collections.Generic;

namespace ProyectoVideoteca.Models;

public partial class tb_AUDIOVISUAL_PRODUCTION
{
    public string TITLE { get; set; } = null!;

    public string SYNOPSIS { get; set; } = null!;

    public string CLASSIFICATION { get; set; } = null!;

    public DateTime RELEASE_DATE { get; set; }

    public string FRONT_PAGE { get; set; } = null!;

    public double SCORE { get; set; }

    public virtual tb_MOVIE? tb_MOVIE { get; set; }

    public virtual ICollection<tb_RATING> tb_RATINGs { get; set; } = new List<tb_RATING>();

    public virtual tb_SERIE? tb_SERIE { get; set; }
}
