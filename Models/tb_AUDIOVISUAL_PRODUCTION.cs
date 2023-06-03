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

    public int DIRECTOR_ID { get; set; }

    public int ACTOR_ID { get; set; }

    public string GENRE_NAME { get; set; } = null!;

    public virtual tb_ACTOR ACTOR { get; set; } = null!;

    public virtual tb_DIRECTOR DIRECTOR { get; set; } = null!;

    public virtual tb_GENRE GENRE_NAMENavigation { get; set; } = null!;

    public virtual tb_MOVIE? tb_MOVIE { get; set; }

    public virtual ICollection<tb_RATING> tb_RATINGs { get; set; } = new List<tb_RATING>();

    public virtual tb_SERIE? tb_SERIE { get; set; }
}
