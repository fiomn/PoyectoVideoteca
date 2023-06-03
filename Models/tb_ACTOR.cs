using System;
using System.Collections.Generic;

namespace ProyectoVideoteca.Models;

public partial class tb_ACTOR
{
    public int ACTOR_ID { get; set; }

    public string NAME { get; set; } = null!;

    public string LAST_NAME { get; set; } = null!;

    public virtual ICollection<tb_AUDIOVISUAL_PRODUCTION> tb_AUDIOVISUAL_PRODUCTIONs { get; set; } = new List<tb_AUDIOVISUAL_PRODUCTION>();
}
