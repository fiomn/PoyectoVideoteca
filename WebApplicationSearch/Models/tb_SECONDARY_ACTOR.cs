using System;
using System.Collections.Generic;

namespace WebApplicationSearch.Models;

public partial class tb_SECONDARY_ACTOR
{
    public string? MOVIE_TITLE { get; set; }

    public int? ACTOR_ID { get; set; }

    public string? ACTOR_NAME { get; set; }

    public virtual tb_ACTOR? ACTOR { get; set; }

    public virtual tb_MOVIE? MOVIE_TITLENavigation { get; set; }
}
