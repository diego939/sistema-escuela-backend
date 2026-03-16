namespace SistemaEscuela.DTO.Usuario
{
	public class CrearUsuarioDTO
	{
		public string Nombres { get; set; }

		public string Apellidos { get; set; }

		public string Email { get; set; }

		public string Password { get; set; }

		public string Telefono { get; set; }

		public string Dni { get; set; }

		public int IdRol { get; set; }
	}
}