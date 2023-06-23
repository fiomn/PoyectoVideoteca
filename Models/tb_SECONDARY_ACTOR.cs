using System;
using System.Collections.Generic;

namespace ProyectoVideoteca.Models;

public partial class tb_SECONDARY_ACTOR
{
    public string? MOVIE_TITLE { get; set; }

    public string? ACTOR_NAME { get; set; }

    public string? ACTOR_IMG { get; set; }

    public string? SERIE_TITLE { get; set; }

    public virtual tb_MOVIE? MOVIE_TITLENavigation { get; set; }

    public virtual tb_SERIE? SERIE_TITLENavigation { get; set; }
}
