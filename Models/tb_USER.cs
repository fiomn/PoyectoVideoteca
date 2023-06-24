using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProyectoVideoteca.Models;

public partial class tb_USER
{
    public string USERNAME { get; set; } = null!;

    [Required]
    [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#$^+=!*()@%&]).{8,}$", ErrorMessage = "Minimun lenght 8 and must contain 1 UpperCase, 1 LowerCase, 1 Special Character and 1 Digit")]
    public string PASSWORD { get; set; } = null!;

    public string NAME { get; set; } = null!;

    public string EMAIL { get; set; } = null!;

    [Required]
    [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#$^+=!*()@%&]).{8,}$", ErrorMessage = "Minimun lenght 8 and must contain 1 UpperCase, 1 LowerCase, 1 Special Character and 1 Digit")]
    public string? PASSWORD_CONFIRM { get; set; }

    public string ROLE { get; set; } = null!;

    public string? IMG { get; set; }

    public virtual ICollection<tb_RATING> tb_RATINGs { get; set; } = new List<tb_RATING>();
}
