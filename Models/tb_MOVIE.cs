using System;
using System.Collections.Generic;
using System.Reflection.Metadata;

namespace ProyectoVideoteca.Models;

public partial class tb_MOVIE
{
    public int ID { get; set; }

    public string TITLE { get; set; } = null!;

    public string SYNOPSIS { get; set; } = null!;

    public string CLASS { get; set; } = null!;

    public string RELEASE_DATE { get; set; } = null!;

    public string IMG { get; set; } = null!;

    public double? SCORE { get; set; }

    public int DIRECTOR_ID { get; set; }

    public int ACTOR_ID { get; set; }

    public string GENRE { get; set; } = null!;

    public string VIDEO { get; set; } = null!;

    public string? DIRECTOR_NAME { get; set; }

    public string? ACTOR_NAME { get; set; }

    public string? DURATION { get; set; }

    public double? QSTREAM_SCORE { get; set; }

    public virtual tb_ACTOR ACTOR { get; set; } = null!;

    public virtual tb_DIRECTOR DIRECTOR { get; set; } = null!;

    private static string? currentMovieValue;
    public static string currentMovie
    {
        get { return currentMovieValue; }
        set { currentMovieValue = value; }
    }
}
