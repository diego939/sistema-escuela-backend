namespace SistemaEscuela.DTO.Posteo
{
	public class CrearPosteoDTO
	{
		public int IdCursomateria { get; set; }

		public string Titulo { get; set; }

		public string Descripcion { get; set; }

		public string? UrlArchivo { get; set; }
	}
}
