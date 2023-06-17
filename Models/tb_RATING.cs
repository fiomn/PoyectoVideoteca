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

    public tb_RATING() { }

    public tb_RATING(string TITLE, string USERNAME, string COMMENT, double RATING)
    {
        this.TITLE = TITLE;
        this.USERNAME = USERNAME;
        this.COMMENT = COMMENT;
        this.RATING = RATING;
    }
}
