namespace SistemaEscuela.DTO.Inscripcion
{
	public class InscripcionDTO
	{
		public int Id { get; set; }

		public int IdAlumno { get; set; }

		public string AlumnoNombre { get; set; }

		public int IdCurso { get; set; }

		public string Modulo { get; set; }

		public string Division { get; set; }

		public string Modalidad { get; set; }

		public string Turno { get; set; }

		public int Anio { get; set; }

		public decimal? Monto { get; set; }

		public DateTime FechaInscripcion { get; set; }
	}
}
