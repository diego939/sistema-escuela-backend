namespace SistemaEscuela.DTO.Calificacion
{
	public class RegistrarCalificacionDTO
	{
		public int IdAlumno { get; set; }

		public int IdCursomateria { get; set; }

		public decimal Nota { get; set; }

		public string? Descripcion { get; set; }

		public int Trimestre { get; set; }
	}
}
