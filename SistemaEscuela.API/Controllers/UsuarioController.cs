using Microsoft.AspNetCore.Mvc;
using SistemaEscuela.BLL.Contratos;
using SistemaEscuela.DTO.Comun;
using SistemaEscuela.DTO.Usuario;

namespace SistemaEscuela.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UsuarioController : ControllerBase
	{
		private readonly IUsuarioService _usuarioService;

		public UsuarioController(IUsuarioService usuarioService)
		{
			_usuarioService = usuarioService;
		}

		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody] LoginDTO modelo)
		{
			try
			{
				var usuario = await _usuarioService.Login(modelo);
				return Ok(usuario);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpPost("crear")]
		public async Task<IActionResult> CrearUsuario([FromBody] CrearUsuarioDTO modelo)
		{
			try
			{
				var usuario = await _usuarioService.CrearUsuario(modelo);
				return Ok(usuario);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpGet("lista")]
		public async Task<IActionResult> ListaUsuarios()
		{
			try
			{
				var lista = await _usuarioService.ListaUsuarios();
				return Ok(lista);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpGet("lista-paginado")]
		public async Task<IActionResult> ListaUsuariosPaginado([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string search = "", [FromQuery] string sortBy = "nombres", [FromQuery] bool sortDescending = false)
		{
			try
			{
				var request = new PaginationRequest
				{
					PageNumber = pageNumber,
					PageSize = pageSize,
					Search = search,
					SortBy = sortBy,
					SortDescending = sortDescending
				};

				var resultado = await _usuarioService.ListaUsuariosPaginado(request);
				return Ok(resultado);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpPut("editar")]
		public async Task<IActionResult> EditarUsuario([FromBody] EditarUsuarioDTO modelo)
		{
			try
			{
				var usuario = await _usuarioService.EditarUsuario(modelo);
				return Ok(usuario);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpPut("activar")]
		public async Task<IActionResult> ActivarUsuario([FromBody] ActivarDesactivarUsuarioDTO modelo)
		{
			try
			{
				var resultado = await _usuarioService.ActivarUsuario(modelo);
				return Ok(new { mensaje = "Usuario activado correctamente", exitoso = resultado });
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpPut("desactivar")]
		public async Task<IActionResult> DesactivarUsuario([FromBody] ActivarDesactivarUsuarioDTO modelo)
		{
			try
			{
				var resultado = await _usuarioService.DesactivarUsuario(modelo);
				return Ok(new { mensaje = "Usuario desactivado correctamente", exitoso = resultado });
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpPut("cambiar-password")]
		public async Task<IActionResult> CambiarPassword([FromBody] CambiarPasswordDTO modelo)
		{
			try
			{
				var resultado = await _usuarioService.CambiarPassword(modelo);
				return Ok(new { mensaje = "Contraseña cambiada correctamente", exitoso = resultado });
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpGet("listar-roles")]
		public async Task<IActionResult> ListarRoles()
		{
			try
			{
				var lista = await _usuarioService.ListarRoles();
				return Ok(lista);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
	}
}