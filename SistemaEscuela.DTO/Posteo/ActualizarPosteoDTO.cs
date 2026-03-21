namespace SistemaEscuela.DTO.Posteo
{
	public class ActualizarPosteoDTO
	{
		public int Id { get; set; }

		public string Titulo { get; set; }

		public string Descripcion { get; set; }

		public string? UrlArchivo { get; set; }
	}
}
