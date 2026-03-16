using System;
using System.Collections.Generic;

namespace SistemaEscuela.Model;

public partial class MedioDePago
{
    public int Id { get; set; }

    public string? Descripcion { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public DateTime? FechaEliminacion { get; set; }

    public virtual ICollection<Inscripcion> Inscripcions { get; set; } = new List<Inscripcion>();
}
