namespace SistemaEscuela.DTO.Calificacion
{
	public class ActualizarCalificacionDTO
	{
		public int Id { get; set; }

		public decimal Nota { get; set; }

		public string? Descripcion { get; set; }

		public int Trimestre { get; set; }
	}
}
