using System;
using System.Collections.Generic;

namespace ProyectoVideoteca.Models;

public partial class tb_RATING
{
    public int RATING_ID { get; set; }

    public string TITLE { get; set; } = null!;

    public string USERNAME { get; set; } = null!;

    public int RATING { get; set; }

    public string COMMENT { get; set; } = null!;

    public virtual tb_AUDIOVISUAL_PRODUCTION TITLENavigation { get; set; } = null!;

    public virtual tb_USER USERNAMENavigation { get; set; } = null!;
}
