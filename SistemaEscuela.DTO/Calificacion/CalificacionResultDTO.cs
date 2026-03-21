namespace SistemaEscuela.DTO.Calificacion
{
	public class CalificacionResultDTO
	{
		public int Id { get; set; }

		public int IdAlumno { get; set; }

		public string AlumnoNombre { get; set; }

		public int IdCursomateria { get; set; }

		public decimal Nota { get; set; }

		public string? Descripcion { get; set; }

		public int Trimestre { get; set; }

		public DateTime FechaRegistro { get; set; }
	}
}
