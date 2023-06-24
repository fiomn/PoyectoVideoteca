using System;
using System.Collections.Generic;

namespace ProyectoVideoteca.Models;

public partial class tb_SEASON
{
    public int SEASON_ID { get; set; }

    public int NUMBER { get; set; }

    public string TITLE { get; set; } = null!;

    public int? EPISODES_NUMBER { get; set; }

    public string? VIDEO { get; set; }

    public virtual tb_SERIE TITLENavigation { get; set; } = null!;

    private static string? currentSeasonValue;
    public static string currentSeason
    {
        get { return currentSeasonValue; }
        set { currentSeasonValue = value; }
    }

    public virtual ICollection<tb_EPISODE> tb_EPISODEs { get; set; } = new List<tb_EPISODE>();
}
