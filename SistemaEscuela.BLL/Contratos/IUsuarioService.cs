using SistemaEscuela.DTO.Usuario;

namespace SistemaEscuela.BLL.Contratos
{
	public interface IUsuarioService
	{
		Task<UsuarioDTO> Login(LoginDTO modelo);

		Task<UsuarioDTO> CrearUsuario(CrearUsuarioDTO modelo);

		Task<List<UsuarioDTO>> ListaUsuarios();

		Task<UsuarioDTO> EditarUsuario(EditarUsuarioDTO modelo);

		Task<bool> ActivarUsuario(ActivarDesactivarUsuarioDTO modelo);

		Task<bool> DesactivarUsuario(ActivarDesactivarUsuarioDTO modelo);

		Task<bool> CambiarPassword(CambiarPasswordDTO modelo);
	}
}
