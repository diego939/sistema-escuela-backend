using System;
using System.Collections.Generic;

namespace SistemaEscuela.Model;

public partial class Posteo
{
    public int Id { get; set; }

    public int? IdUsuario { get; set; }

    public int? IdCursomateria { get; set; }

    public string? Titulo { get; set; }

    public string? Descripcion { get; set; }

    public string? UrlArchivo { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public DateTime? FechaEliminacion { get; set; }

    public virtual CursoMateria? IdCursomateriaNavigation { get; set; }

    public virtual Usuario? IdUsuarioNavigation { get; set; }
}
