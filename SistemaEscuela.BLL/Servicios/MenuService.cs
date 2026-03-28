using Microsoft.EntityFrameworkCore;
using SistemaEscuela.BLL.Contratos;
using SistemaEscuela.DAL.Repositorios.Contrato;
using SistemaEscuela.DTO.Menu;
using SistemaEscuela.Model;

namespace SistemaEscuela.BLL.Servicios
{
	public class MenuService : IMenuService
	{
		private readonly IGenericRepository<Menu> _menuRepository;
		private readonly IGenericRepository<MenuRol> _menuRolRepository;
		private readonly IGenericRepository<Rol> _rolRepository;
		private readonly IGenericRepository<Usuario> _usuarioRepository;

		public MenuService(
			IGenericRepository<Menu> menuRepository,
			IGenericRepository<MenuRol> menuRolRepository,
			IGenericRepository<Rol> rolRepository,
			IGenericRepository<Usuario> usuarioRepository)
		{
			_menuRepository = menuRepository;
			_menuRolRepository = menuRolRepository;
			_rolRepository = rolRepository;
			_usuarioRepository = usuarioRepository;
		}

		public async Task<MenuDTO> CrearMenu(CrearMenuDTO modelo)
		{
			// Validar que el nombre no esté vacío
			if (string.IsNullOrWhiteSpace(modelo.Nombre))
				throw new Exception("El nombre del menú es obligatorio");

			// Validar que la URL no esté vacía
			if (string.IsNullOrWhiteSpace(modelo.UrlMenu))
				throw new Exception("La URL del menú es obligatoria");

			// Crear el nuevo menú
			var menu = new Menu
			{
				Nombre = modelo.Nombre,
				Icono = modelo.Icono,
				UrlMenu = modelo.UrlMenu,
				FechaCreacion = DateTime.Now
			};

			var creado = await _menuRepository.Crear(menu);

			return new MenuDTO
			{
				Id = creado.Id,
				Nombre = creado.Nombre,
				Icono = creado.Icono,
				UrlMenu = creado.UrlMenu,
				FechaCreacion = creado.FechaCreacion.Value
			};
		}

		public async Task<MenuDTO> ActualizarMenu(ActualizarMenuDTO modelo)
		{
			// Buscar el menú
			var menu = await _menuRepository.Consultar(m =>
				m.Id == modelo.Id &&
				m.FechaEliminacion == null)
				.FirstOrDefaultAsync();

			if (menu == null)
				throw new Exception("El menú no existe");

			// Validar que el nombre no esté vacío
			if (string.IsNullOrWhiteSpace(modelo.Nombre))
				throw new Exception("El nombre del menú es obligatorio");

			// Validar que la URL no esté vacía
			if (string.IsNullOrWhiteSpace(modelo.UrlMenu))
				throw new Exception("La URL del menú es obligatoria");

			// Actualizar el menú
			menu.Nombre = modelo.Nombre;
			menu.Icono = modelo.Icono;
			menu.UrlMenu = modelo.UrlMenu;

			await _menuRepository.Editar(menu);

			return new MenuDTO
			{
				Id = menu.Id,
				Nombre = menu.Nombre,
				Icono = menu.Icono,
				UrlMenu = menu.UrlMenu,
				FechaCreacion = menu.FechaCreacion.Value
			};
		}

		public async Task<List<MenuDTO>> ObtenerTodosLosMenus()
		{
			// Obtener todos los menús activos
			var menus = await _menuRepository.Consultar(m =>
				m.FechaEliminacion == null)
				.OrderBy(m => m.Id)
				.ToListAsync();

			return menus
				.Select(m => new MenuDTO
				{
					Id = m.Id,
					Nombre = m.Nombre,
					Icono = m.Icono,
					UrlMenu = m.UrlMenu,
					FechaCreacion = m.FechaCreacion.Value
				})
				.ToList();
		}

		public async Task<bool> AsociarMenuARol(AsociarMenuARolDTO modelo)
		{
			// Validar que el menú existe
			var menu = await _menuRepository.Consultar(m =>
				m.Id == modelo.IdMenu &&
				m.FechaEliminacion == null)
				.FirstOrDefaultAsync();

			if (menu == null)
				throw new Exception("El menú no existe");

			// Validar que el rol existe
			var rol = await _rolRepository.Consultar(r =>
				r.Id == modelo.IdRol &&
				r.FechaEliminacion == null)
				.FirstOrDefaultAsync();

			if (rol == null)
				throw new Exception("El rol no existe");

			// Validar que la asociación no exista ya
			var asociacionExistente = await _menuRolRepository.Consultar(mr =>
				mr.IdMenu == modelo.IdMenu &&
				mr.IdRol == modelo.IdRol &&
				mr.FechaEliminacion == null)
				.FirstOrDefaultAsync();

			if (asociacionExistente != null)
				throw new Exception("El menú ya está asociado a este rol");

			// Crear la asociación
			var menuRol = new MenuRol
			{
				IdMenu = modelo.IdMenu,
				IdRol = modelo.IdRol,
				FechaCreacion = DateTime.Now
			};

			await _menuRolRepository.Crear(menuRol);

			return true;
		}

		public async Task<bool> DesasociarMenuDelRol(int idMenu, int idRol)
		{
			// Buscar la asociación
			var menuRol = await _menuRolRepository.Consultar(mr =>
				mr.IdMenu == idMenu &&
				mr.IdRol == idRol &&
				mr.FechaEliminacion == null)
				.FirstOrDefaultAsync();

			if (menuRol == null)
				throw new Exception("La asociación no existe");

			// Marcar como eliminada
			menuRol.FechaEliminacion = DateTime.Now;
			await _menuRolRepository.Editar(menuRol);

			return true;
		}

		public async Task<List<MenuRolDTO>> ObtenerMenusPorRol(int idRol)
		{
			// Validar que el rol existe
			var rol = await _rolRepository.Consultar(r =>
				r.Id == idRol &&
				r.FechaEliminacion == null)
				.FirstOrDefaultAsync();

			if (rol == null)
				throw new Exception("El rol no existe");

			// Obtener los menús asociados al rol
			var datos = await _menuRolRepository.Consultar(mr =>
				mr.IdRol == idRol &&
				mr.FechaEliminacion == null)
				.Include(mr => mr.IdMenuNavigation)
				.ToListAsync();

			// Mapear en memoria
			var menus = datos
				.Select(mr => new MenuRolDTO
				{
					IdMenu = mr.IdMenuNavigation.Id,
					MenuNombre = mr.IdMenuNavigation.Nombre,
					MenuIcono = mr.IdMenuNavigation.Icono,
					MenuUrl = mr.IdMenuNavigation.UrlMenu
				})
				.OrderBy(x => x.IdMenu)
				.ToList();

			return menus;
		}

		public async Task<List<MenuRolDTO>> ObtenerMenusDelUsuario(int idUsuario)
		{
			// Validar que el usuario existe
			var usuario = await _usuarioRepository.Consultar(u =>
				u.Id == idUsuario &&
				u.FechaEliminacion == null)
				.FirstOrDefaultAsync();

			if (usuario == null)
				throw new Exception("El usuario no existe");

			// Obtener el rol del usuario
			var usuarioConRol = await _usuarioRepository.Consultar(u =>
				u.Id == idUsuario &&
				u.FechaEliminacion == null)
				.FirstOrDefaultAsync();

			if (usuarioConRol.IdRol == null)
				throw new Exception("El usuario no tiene un rol asignado");

			// Obtener los menús del rol del usuario (RF31)
			var datos = await _menuRolRepository.Consultar(mr =>
				mr.IdRol == usuarioConRol.IdRol &&
				mr.FechaEliminacion == null)
				.Include(mr => mr.IdMenuNavigation)
				.ToListAsync();

			// Mapear en memoria
			var menus = datos
				.Select(mr => new MenuRolDTO
				{
					IdMenu = mr.IdMenuNavigation.Id,
					MenuNombre = mr.IdMenuNavigation.Nombre,
					MenuIcono = mr.IdMenuNavigation.Icono,
					MenuUrl = mr.IdMenuNavigation.UrlMenu
				})
				.OrderBy(x => x.MenuNombre)
				.ToList();

			return menus;
		}

		public async Task<bool> EliminarMenu(int idMenu)
		{
			// Buscar el menú
			var menu = await _menuRepository.Consultar(m =>
				m.Id == idMenu &&
				m.FechaEliminacion == null)
				.FirstOrDefaultAsync();

			if (menu == null)
				throw new Exception("El menú no existe");

			// Marcar como eliminado
			menu.FechaEliminacion = DateTime.Now;
			await _menuRepository.Editar(menu);

			return true;
		}
	}
}
