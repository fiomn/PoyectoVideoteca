using System;
using System.Collections.Generic;

namespace ProyectoVideoteca.Models;

public partial class tb_MOVIE
{
    public string TITLE { get; set; } = null!;

    public string SYNOPSIS { get; set; } = null!;

    public string CLASS { get; set; } = null!;

    public string RELEASE_DATE { get; set; } = null!;

    public string IMG { get; set; } = null!;

    public double? SCORE { get; set; }

    public string GENRE { get; set; } = null!;

    public string VIDEO { get; set; } = null!;

    public string? DIRECTOR_NAME { get; set; }

    public string? ACTOR_NAME { get; set; }

    public string? DURATION { get; set; }

    public double? QSTREAM_SCORE { get; set; }

    public string? ACTOR_IMG { get; set; }

    private static string? currentMovieValue;
    public static string currentMovie
    {
        get { return currentMovieValue; }
        set { currentMovieValue = value; }
    }
}
