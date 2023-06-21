using System;
using System.Collections.Generic;

namespace ProyectoVideoteca.Models;

public partial class tb_GLOBALSETTING
{
    public int id { get; set; }

    public string mode { get; set; } = null!;

    public string modeBtn { get; set; } = null!;
}
