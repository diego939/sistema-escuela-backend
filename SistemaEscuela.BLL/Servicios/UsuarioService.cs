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
			var usuario = await _usuarioRepository.Obtener(u =>
				u.Email == modelo.Email &&
				u.FechaEliminacion == null);

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
				Email = usuario.Email
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
	}
}