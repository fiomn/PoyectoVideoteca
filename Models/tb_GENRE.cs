using System;
using System.Collections.Generic;

namespace ProyectoVideoteca.Models;

public partial class tb_GENRE
{
    public string GENRE_NAME { get; set; } = null!;

    public string DESCRIPTION { get; set; } = null!;

    public virtual ICollection<tb_AUDIOVISUAL_PRODUCTION> tb_AUDIOVISUAL_PRODUCTIONs { get; set; } = new List<tb_AUDIOVISUAL_PRODUCTION>();
}
