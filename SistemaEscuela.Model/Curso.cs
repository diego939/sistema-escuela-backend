using System;
using System.Collections.Generic;

namespace SistemaEscuela.Model;

public partial class Curso
{
    public int Id { get; set; }

    public int? Modulo { get; set; }

    public string? Division { get; set; }

    public string? Modalidad { get; set; }

    public string? Turno { get; set; }

    public int? Anio { get; set; }

    public int? CupoMaximo { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public DateTime? FechaEliminacion { get; set; }

    public virtual ICollection<CursoMateria> CursoMateria { get; set; } = new List<CursoMateria>();

    public virtual ICollection<Inscripcion> Inscripcions { get; set; } = new List<Inscripcion>();

    public virtual ICollection<PreceptorCurso> PreceptorCursos { get; set; } = new List<PreceptorCurso>();
}
