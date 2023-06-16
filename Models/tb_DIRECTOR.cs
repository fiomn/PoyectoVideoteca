﻿using System;
using System.Collections.Generic;

namespace ProyectoVideoteca.Models;

public partial class tb_DIRECTOR
{
    public int DIRECTOR_ID { get; set; }

    public string NAME { get; set; } = null!;

    public string LAST_NAME { get; set; } = null!;

    public string? FULLNAME { get; set; }

    public virtual ICollection<tb_MOVIE> tb_MOVIE { get; set; } = new List<tb_MOVIE>();

    public virtual ICollection<tb_SERIE> tb_SERIE { get; set; } = new List<tb_SERIE>();
}
