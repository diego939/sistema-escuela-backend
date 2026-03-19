namespace SistemaEscuela.DTO.Curso
{
	public class CursoDTO
	{
		public int Id { get; set; }

		public int? Modulo { get; set; }

		public string? Division { get; set; }

		public string? Modalidad { get; set; }

		public string? Turno { get; set; }

		public int? Anio { get; set; }

		public int? CupoMaximo { get; set; }

		public DateTime? FechaCreacion { get; set; }
	}
}
