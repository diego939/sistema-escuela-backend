namespace SistemaEscuela.DTO.Posteo
{
	public class PosteoDTO
	{
		public int Id { get; set; }

		public int IdUsuario { get; set; }

		public string ProfesorNombre { get; set; }

		public int IdCurso { get; set; }

		public string CursoNombre { get; set; }

		public int IdMateria { get; set; }

		public string MateriaNombre { get; set; }

		public string Titulo { get; set; }

		public string Descripcion { get; set; }

		public string? UrlArchivo { get; set; }

		public DateTime FechaPublicacion { get; set; }
	}
}
