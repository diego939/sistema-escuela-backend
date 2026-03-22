using Microsoft.AspNetCore.Identity;

namespace SistemaEscuela.DTO.Usuario
{
	public class UsuarioDTO
	{
		public int Id { get; set; }

		public string Nombres { get; set; }

		public string Apellidos { get; set; }

		public string Email { get; set; }

		public string Telefono { get; set; }

		public string Rol { get; set; }

		public int? IdRol { get; set; }

		public string Dni { get; set; }

		public string UrlImagen { get; set; }

	}
}
