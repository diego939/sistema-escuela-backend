using System;
using System.Collections.Generic;

namespace SistemaEscuela.Model;

public partial class CursoMateria
{
    public int Id { get; set; }

    public int? IdCurso { get; set; }

    public int? IdMateria { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public DateTime? FechaEliminacion { get; set; }

    public virtual ICollection<Calificacion> Calificacions { get; set; } = new List<Calificacion>();

    public virtual Curso? IdCursoNavigation { get; set; }

    public virtual Materia? IdMateriaNavigation { get; set; }

    public virtual ICollection<Posteo> Posteos { get; set; } = new List<Posteo>();

    public virtual ICollection<ProfesorCursoMateria> ProfesorCursoMateria { get; set; } = new List<ProfesorCursoMateria>();
}
