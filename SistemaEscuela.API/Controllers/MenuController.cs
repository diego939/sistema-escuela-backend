using Microsoft.AspNetCore.Mvc;
using SistemaEscuela.BLL.Contratos;
using SistemaEscuela.DTO.Menu;

namespace SistemaEscuela.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class MenuController : ControllerBase
	{
		private readonly IMenuService _menuService;

		public MenuController(IMenuService menuService)
		{
			_menuService = menuService;
		}

		[HttpPost("crear")]
		public async Task<IActionResult> CrearMenu([FromBody] CrearMenuDTO modelo)
		{
			try
			{
				var resultado = await _menuService.CrearMenu(modelo);
				return Ok(resultado);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpPut("actualizar")]
		public async Task<IActionResult> ActualizarMenu([FromBody] ActualizarMenuDTO modelo)
		{
			try
			{
				var resultado = await _menuService.ActualizarMenu(modelo);
				return Ok(resultado);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpGet]
		public async Task<IActionResult> ObtenerTodosLosMenus()
		{
			try
			{
				var menus = await _menuService.ObtenerTodosLosMenus();
				return Ok(menus);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpPost("asociar")]
		public async Task<IActionResult> AsociarMenuARol([FromBody] AsociarMenuARolDTO modelo)
		{
			try
			{
				var resultado = await _menuService.AsociarMenuARol(modelo);
				return Ok(resultado);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpDelete("desasociar/{idMenu}/{idRol}")]
		public async Task<IActionResult> DesasociarMenuDelRol(int idMenu, int idRol)
		{
			try
			{
				var resultado = await _menuService.DesasociarMenuDelRol(idMenu, idRol);
				return Ok(resultado);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpGet("rol/{idRol}")]
		public async Task<IActionResult> ObtenerMenusPorRol(int idRol)
		{
			try
			{
				var menus = await _menuService.ObtenerMenusPorRol(idRol);
				return Ok(menus);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpGet("usuario/{idUsuario}")]
		public async Task<IActionResult> ObtenerMenusDelUsuario(int idUsuario)
		{
			try
			{
				var menus = await _menuService.ObtenerMenusDelUsuario(idUsuario);
				return Ok(menus);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpDelete("{idMenu}")]
		public async Task<IActionResult> EliminarMenu(int idMenu)
		{
			try
			{
				var resultado = await _menuService.EliminarMenu(idMenu);
				return Ok(resultado);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
	}
}
