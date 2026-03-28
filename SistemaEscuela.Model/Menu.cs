using System;
using System.Collections.Generic;

namespace SistemaEscuela.Model;

public partial class Menu
{
    public int Id { get; set; }

    public string? Nombre { get; set; }

    public string? Icono { get; set; }

    public string? UrlMenu { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public DateTime? FechaEliminacion { get; set; }

    public virtual ICollection<MenuRol> MenuRols { get; set; } = new List<MenuRol>();
}
