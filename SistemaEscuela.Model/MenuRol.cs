using System;
using System.Collections.Generic;

namespace SistemaEscuela.Model;

public partial class MenuRol
{
    public int Id { get; set; }

    public int? IdMenu { get; set; }

    public int? IdRol { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public DateTime? FechaEliminacion { get; set; }

    public virtual Menu? IdMenuNavigation { get; set; }

    public virtual Rol? IdRolNavigation { get; set; }
}
