namespace SistemaEscuela.DTO.Calificacion
{
	public class CalificacionDTO
	{
		public int Id { get; set; }

		public int IdAlumno { get; set; }

		public string AlumnoNombre { get; set; }

		public int IdCurso { get; set; }

		public string CursoNombre { get; set; }

		public int IdMateria { get; set; }

		public string MateriaNombre { get; set; }

		public decimal Nota { get; set; }

		public string? Descripcion { get; set; }

		public int Trimestre { get; set; }

		public DateTime FechaRegistro { get; set; }
	}
}
