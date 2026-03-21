namespace SistemaEscuela.DTO.Inscripcion
{
	public class InscripcionResultDTO
	{
		public int Id { get; set; }

		public int IdAlumno { get; set; }

		public string AlumnoNombre { get; set; }

		public int IdCurso { get; set; }

		public string CursoNombre { get; set; }

		public decimal? Monto { get; set; }

		public DateTime FechaInscripcion { get; set; }
	}
}
