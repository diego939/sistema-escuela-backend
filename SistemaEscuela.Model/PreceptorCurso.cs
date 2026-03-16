using System;
using System.Collections.Generic;

namespace SistemaEscuela.Model;

public partial class PreceptorCurso
{
    public int Id { get; set; }

    public int? IdPreceptor { get; set; }

    public int? IdCurso { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public DateTime? FechaEliminacion { get; set; }

    public virtual Curso? IdCursoNavigation { get; set; }

    public virtual Usuario? IdPreceptorNavigation { get; set; }
}
