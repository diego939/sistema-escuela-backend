using System;
using System.Collections.Generic;

namespace SistemaEscuela.Model;

public partial class Calificacion
{
    public int Id { get; set; }

    public int? IdAlumno { get; set; }

    public int? IdCursomateria { get; set; }

    public decimal? Nota { get; set; }

    public string? Descripcion { get; set; }

    public int? Trimestre { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public DateTime? FechaEliminacion { get; set; }

    public virtual Usuario? IdAlumnoNavigation { get; set; }

    public virtual CursoMateria? IdCursomateriaNavigation { get; set; }
}
