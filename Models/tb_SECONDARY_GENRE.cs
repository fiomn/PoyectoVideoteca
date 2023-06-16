using System;
using System.Collections.Generic;

namespace ProyectoVideoteca.Models;

public partial class tb_SECONDARY_GENRE
{
    public string? MOVIE_TITLE { get; set; }

    public string? GENRE_NAME { get; set; }

    public virtual tb_MOVIE? MOVIE_TITLENavigation { get; set; }
}
