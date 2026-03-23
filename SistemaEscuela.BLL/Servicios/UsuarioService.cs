using Microsoft.EntityFrameworkCore;
using SistemaEscuela.BLL.Contratos;
using SistemaEscuela.DAL.Repositorios.Contrato;
using SistemaEscuela.DTO.Usuario;
using SistemaEscuela.Model;
using SistemaEscuela.Utility;

namespace SistemaEscuela.BLL.Servicios
{
	public class UsuarioService : IUsuarioService
	{
		private readonly IGenericRepository<Usuario> _usuarioRepository;
		private readonly JwtHelper _jwtHelper;

		public UsuarioService(IGenericRepository<Usuario> usuarioRepository, JwtHelper jwtHelper)
		{
			_usuarioRepository = usuarioRepository;
			_jwtHelper = jwtHelper;
		}

		public async Task<UsuarioDTO> Login(LoginDTO modelo)
		{
			var usuario = await _usuarioRepository.Consultar(u =>
				u.Email == modelo.Email &&
				u.FechaEliminacion == null)
				.Include(u => u.IdRolNavigation)
				.FirstOrDefaultAsync();

			if (usuario == null)
				throw new Exception("Usuario no encontrado");

			// Verificar password con hash
			if (!PasswordHelper.VerifyPassword(modelo.Password, usuario.Password))
				throw new Exception("Credenciales incorrectas");

			// Generar token
			var token = _jwtHelper.GenerateToken(usuario);

			return new UsuarioDTO
			{
				Id = usuario.Id,
				Nombres = usuario.Nombres,
				Apellidos = usuario.Apellidos,
				Email = usuario.Email,
				Telefono = usuario.Telefono,
				IdRol = usuario.IdRol,
				Rol = usuario.IdRolNavigation.Descripcion,
				Dni = usuario.Dni,
				UrlImagen = usuario.UrlImagen,
				Token = token
			};
		}

		public async Task<UsuarioDTO> CrearUsuario(CrearUsuarioDTO modelo)
		{
			var usuario = new Usuario
			{
				Nombres = modelo.Nombres,
				Apellidos = modelo.Apellidos,
				Email = modelo.Email,
				Telefono = modelo.Telefono,
				IdRol = modelo.IdRol,
				Dni = modelo.Dni,
				Password = PasswordHelper.HashPassword(modelo.Password),
				FechaCreacion = DateTime.Now
			};

			var creado = await _usuarioRepository.Crear(usuario);

			return new UsuarioDTO
			{
				Id = creado.Id,
				Nombres = creado.Nombres,
				Apellidos = creado.Apellidos,
				Email = creado.Email
			};
		}

		public async Task<List<UsuarioDTO>> ListaUsuarios()
		{
			var query = _usuarioRepository.Consultar(u => u.FechaEliminacion == null);

			var lista = await query
				.Include(u => u.IdRolNavigation)
				.Select(u => new UsuarioDTO
				{
					Id = u.Id,
					Nombres = u.Nombres,
					Apellidos = u.Apellidos,
					Email = u.Email,
					Telefono = u.Telefono,
					Rol = u.IdRolNavigation.Descripcion,
					Dni = u.Dni,
					UrlImagen = u.UrlImagen
				})
				.ToListAsync();

				return lista;
		}

		public async Task<UsuarioDTO> EditarUsuario(EditarUsuarioDTO modelo)
		{
			var usuario = await _usuarioRepository.Consultar(u =>
				u.Id == modelo.Id &&
				u.FechaEliminacion == null)
				.Include(u => u.IdRolNavigation)
				.FirstOrDefaultAsync();

			if (usuario == null)
				throw new Exception("Usuario no encontrado");

			usuario.Nombres = modelo.Nombres;
			usuario.Apellidos = modelo.Apellidos;
			usuario.Email = modelo.Email;
			usuario.Telefono = modelo.Telefono;
			usuario.Dni = modelo.Dni;
			usuario.IdRol = modelo.IdRol;
			usuario.UrlImagen = modelo.UrlImagen;

			var editado = await _usuarioRepository.Editar(usuario);

			if (!editado)
				throw new Exception("Error al editar el usuario");

			return new UsuarioDTO
			{
				Id = usuario.Id,
				Nombres = usuario.Nombres,
				Apellidos = usuario.Apellidos,
				Email = usuario.Email,
				Telefono = usuario.Telefono,
				Rol = usuario.IdRolNavigation.Descripcion,
				Dni = usuario.Dni,
				UrlImagen = usuario.UrlImagen
			};
		}

		public async Task<bool> ActivarUsuario(ActivarDesactivarUsuarioDTO modelo)
		{
			var usuario = await _usuarioRepository.Consultar(u => u.Id == modelo.Id)
				.FirstOrDefaultAsync();

			if (usuario == null)
				throw new Exception("Usuario no encontrado");

			usuario.FechaEliminacion = null;
			return await _usuarioRepository.Editar(usuario);
		}

		public async Task<bool> DesactivarUsuario(ActivarDesactivarUsuarioDTO modelo)
		{
			var usuario = await _usuarioRepository.Consultar(u => u.Id == modelo.Id)
				.FirstOrDefaultAsync();

			if (usuario == null)
				throw new Exception("Usuario no encontrado");

			usuario.FechaEliminacion = DateTime.Now;
			return await _usuarioRepository.Editar(usuario);
		}

		public async Task<bool> CambiarPassword(CambiarPasswordDTO modelo)
		{
			var usuario = await _usuarioRepository.Consultar(u => u.Id == modelo.IdUsuario)
				.FirstOrDefaultAsync();

			if (usuario == null)
				throw new Exception("Usuario no encontrado");

			// Verificar que la contraseña actual sea correcta
			if (!PasswordHelper.VerifyPassword(modelo.PasswordActual, usuario.Password))
				throw new Exception("La contraseña actual es incorrecta");

			// Validar que la nueva contraseña no sea vacía
			if (string.IsNullOrWhiteSpace(modelo.PasswordNueva))
				throw new Exception("La contraseña nueva no puede estar vacía");

			// Validar que la contraseña nueva coincida con la confirmación
			if (modelo.PasswordNueva != modelo.RepetirPasswordNueva)
				throw new Exception("Las contraseñas nuevas no coinciden");

			// Cambiar la contraseña
			usuario.Password = PasswordHelper.HashPassword(modelo.PasswordNueva);
			return await _usuarioRepository.Editar(usuario);
		}
	}
}