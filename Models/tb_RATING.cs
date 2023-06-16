using System;
using System.Collections.Generic;

namespace ProyectoVideoteca.Models;

public partial class tb_RATING
{
    public int RATING_ID { get; set; }

    public string TITLE { get; set; } = null!;

    public string USERNAME { get; set; } = null!;

    public string COMMENT { get; set; } = null!;

    public double? RATING { get; set; }

    public virtual tb_USER USERNAMENavigation { get; set; } = null!;
}
