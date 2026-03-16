using System;
using System.Collections.Generic;

namespace SistemaEscuela.Model;

public partial class Asistencia
{
    public int Id { get; set; }

    public int? IdInscripcion { get; set; }

    public int? IdAsistente { get; set; }

    public string? Estado { get; set; }

    public DateOnly? Fecha { get; set; }

    public DateTime? FechaEliminacion { get; set; }

    public virtual Usuario? IdAsistenteNavigation { get; set; }

    public virtual Inscripcion? IdInscripcionNavigation { get; set; }
}
