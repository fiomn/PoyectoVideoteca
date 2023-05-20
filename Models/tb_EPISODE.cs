using System;
using System.Collections.Generic;

namespace ProyectoVideoteca.Models;

public partial class tb_EPISODE
{
    public int EPISODE_ID { get; set; }

    public string NAME { get; set; } = null!;

    public int NUMBER { get; set; }

    public string SYNOPSIS { get; set; } = null!;

    public int DURATION { get; set; }

    public int SEASON_ID { get; set; }

    public virtual tb_SEASON SEASON { get; set; } = null!;
}
