using Microsoft.AspNetCore.Mvc;
using SistemaEscuela.BLL.Contratos;
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
	}
}
