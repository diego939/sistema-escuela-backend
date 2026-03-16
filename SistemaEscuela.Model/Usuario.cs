using System;
using System.Collections.Generic;

namespace SistemaEscuela.Model;

public partial class Usuario
{
    public int Id { get; set; }

    public int? IdRol { get; set; }

    public string? Nombres { get; set; }

    public string? Apellidos { get; set; }

    public string? Dni { get; set; }

    public string? Telefono { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }

    public string? UrlImagen { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public DateTime? FechaEliminacion { get; set; }

    public virtual ICollection<Asistencia> Asistencia { get; set; } = new List<Asistencia>();

    public virtual ICollection<Calificacion> Calificacions { get; set; } = new List<Calificacion>();

    public virtual Rol? IdRolNavigation { get; set; }

    public virtual ICollection<Inscripcion> Inscripcions { get; set; } = new List<Inscripcion>();

    public virtual ICollection<Posteo> Posteos { get; set; } = new List<Posteo>();

    public virtual ICollection<PreceptorCurso> PreceptorCursos { get; set; } = new List<PreceptorCurso>();

    public virtual ICollection<ProfesorCursoMateria> ProfesorCursoMateria { get; set; } = new List<ProfesorCursoMateria>();
}
