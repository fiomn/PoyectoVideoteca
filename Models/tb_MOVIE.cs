using System;
using System.Collections.Generic;

namespace ProyectoVideoteca.Models;

public partial class tb_MOVIE
{
    public int ID { get; set; }

    public string TITLE { get; set; } = null!;

    public int DURATION { get; set; }

    public string? IMG { get; set; }

    public string? RELEASE_DATE { get; set; }

    public double? SCORE { get; set; }

    public int DIRECTOR_ID { get; set; }

    public int ACTOR_ID { get; set; }

    public string? GENRE { get; set; }

    public string? SYNOPSIS { get; set; }

    public string? CLASS { get; set; }

    public virtual tb_ACTOR ACTOR { get; set; } = null!;

    public virtual tb_DIRECTOR DIRECTOR { get; set; } = null!;
}
