using System;
using System.Collections.Generic;

namespace SistemaEscuela.Model;

public partial class Inscripcion
{
    public int Id { get; set; }

    public int? IdAlumno { get; set; }

    public int? IdCurso { get; set; }

    public int? IdMediodepago { get; set; }

    public string? Direccion { get; set; }

    public string? Comprobante { get; set; }

    public decimal? Monto { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public DateTime? FechaEliminacion { get; set; }

    public virtual ICollection<Asistencia> Asistencia { get; set; } = new List<Asistencia>();

    public virtual Usuario? IdAlumnoNavigation { get; set; }

    public virtual Curso? IdCursoNavigation { get; set; }

    public virtual MedioDePago? IdMediodepagoNavigation { get; set; }
}
