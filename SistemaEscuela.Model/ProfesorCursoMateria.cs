using System;
using System.Collections.Generic;

namespace SistemaEscuela.Model;

public partial class ProfesorCursoMateria
{
    public int Id { get; set; }

    public int? IdProfesor { get; set; }

    public int? IdCursomateria { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public DateTime? FechaEliminacion { get; set; }

    public virtual CursoMateria? IdCursomateriaNavigation { get; set; }

    public virtual Usuario? IdProfesorNavigation { get; set; }
}
