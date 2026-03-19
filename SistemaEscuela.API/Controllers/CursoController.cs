using Microsoft.AspNetCore.Mvc;
using SistemaEscuela.BLL.Contratos;
using SistemaEscuela.DTO.Curso;

namespace SistemaEscuela.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CursoController : ControllerBase
	{
		private readonly ICursoService _cursoService;

		public CursoController(ICursoService cursoService)
		{
			_cursoService = cursoService;
		}

		[HttpPost("crear")]
		public async Task<IActionResult> CrearCurso([FromBody] CrearCursoDTO modelo)
		{
			try
			{
				var curso = await _cursoService.CrearCurso(modelo);
				return Ok(curso);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpGet("disponibles")]
		public async Task<IActionResult> ObtenerCursosDisponibles()
		{
			try
			{
				var cursos = await _cursoService.ObtenerCursosDisponibles();
				return Ok(cursos);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
	}
}
