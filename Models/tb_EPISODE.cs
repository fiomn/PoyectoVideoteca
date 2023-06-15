using System;
using System.Collections.Generic;

namespace ProyectoVideoteca.Models;

public partial class tb_EPISODE
{
    public int EPISODE_ID { get; set; }

    public string NAME_EPISODE { get; set; } = null!;

    public int NUMBER { get; set; }

    public string SYNOPSIS { get; set; } = null!;

    public int SEASON_ID { get; set; }

    public string? IMG { get; set; }

    public string? VIDEO { get; set; }

    public string? DURATION { get; set; }

    public virtual tb_SEASON SEASON { get; set; } = null!;
}
