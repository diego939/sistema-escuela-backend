using SistemaEscuela.DTO.Menu;

namespace SistemaEscuela.BLL.Contratos
{
	public interface IMenuService
	{
		Task<MenuDTO> CrearMenu(CrearMenuDTO modelo);

		Task<MenuDTO> ActualizarMenu(ActualizarMenuDTO modelo);

		Task<List<MenuDTO>> ObtenerTodosLosMenus();

		Task<bool> AsociarMenuARol(AsociarMenuARolDTO modelo);

		Task<bool> DesasociarMenuDelRol(int idMenu, int idRol);

		Task<List<MenuRolDTO>> ObtenerMenusPorRol(int idRol);

		Task<List<MenuRolDTO>> ObtenerMenusDelUsuario(int idUsuario);

		Task<bool> EliminarMenu(int idMenu);
	}
}
