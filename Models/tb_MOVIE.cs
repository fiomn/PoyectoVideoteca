using System;
using System.Collections.Generic;

namespace ProyectoVideoteca.Models;

public partial class tb_MOVIE
{
    public string TITLE { get; set; } = null!;

    public int DURATION { get; set; }

    public virtual tb_AUDIOVISUAL_PRODUCTION TITLENavigation { get; set; } = null!;
}
