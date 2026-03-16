using System;
using System.Collections.Generic;

namespace SistemaEscuela.Model;

public partial class Materia
{
    public int Id { get; set; }

    public string? Descripcion { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public DateTime? FechaEliminacion { get; set; }

    public virtual ICollection<CursoMateria> CursoMateria { get; set; } = new List<CursoMateria>();
}
