using Microsoft.AspNetCore.Mvc;
using SistemaEscuela.BLL.Contratos;
using SistemaEscuela.DTO.Posteo;

namespace SistemaEscuela.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class PosteoController : ControllerBase
	{
		private readonly IPosteoService _posteoService;

		public PosteoController(IPosteoService posteoService)
		{
			_posteoService = posteoService;
		}

		[HttpPost("publicar")]
		public async Task<IActionResult> PublicarPosteo([FromBody] CrearPosteoDTO modelo, [FromQuery] int idProfesor)
		{
			try
			{
				var resultado = await _posteoService.PublicarPosteo(modelo, idProfesor);
				return Ok(resultado);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpPut("actualizar")]
		public async Task<IActionResult> ActualizarPosteo([FromBody] ActualizarPosteoDTO modelo)
		{
			try
			{
				var resultado = await _posteoService.ActualizarPosteo(modelo);
				return Ok(resultado);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpGet]
		public async Task<IActionResult> ObtenerTodosLosPosteos()
		{
			try
			{
				var posteos = await _posteoService.ObtenerTodosLosPosteos();
				return Ok(posteos);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpGet("cursomateria/{idCursomateria}")]
		public async Task<IActionResult> ObtenerPosteosPorCursoMateria(int idCursomateria)
		{
			try
			{
				var posteos = await _posteoService.ObtenerPosteosPorCursoMateria(idCursomateria);
				return Ok(posteos);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpGet("curso/{idCurso}")]
		public async Task<IActionResult> ObtenerPosteosPorCurso(int idCurso)
		{
			try
			{
				var posteos = await _posteoService.ObtenerPosteosPorCurso(idCurso);
				return Ok(posteos);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpGet("profesor/{idProfesor}")]
		public async Task<IActionResult> ObtenerPosteosPorProfesor(int idProfesor)
		{
			try
			{
				var posteos = await _posteoService.ObtenerPosteosPorProfesor(idProfesor);
				return Ok(posteos);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpDelete("{idPosteo}")]
		public async Task<IActionResult> EliminarPosteo(int idPosteo)
		{
			try
			{
				var resultado = await _posteoService.EliminarPosteo(idPosteo);
				return Ok(resultado);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
	}
}
