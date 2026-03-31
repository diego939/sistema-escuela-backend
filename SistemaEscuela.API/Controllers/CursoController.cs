using Microsoft.AspNetCore.Mvc;
using SistemaEscuela.BLL.Contratos;
using SistemaEscuela.DTO.Comun;
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
				return BadRequest(new { message = ex.Message });
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
				return BadRequest(new { message = ex.Message });
			}
		}

		[HttpGet("lista-paginado")]
		public async Task<IActionResult> ObtenerCursosPaginado([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string search = "", [FromQuery] string sortBy = "anio", [FromQuery] bool sortDescending = false)
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

				var resultado = await _cursoService.ObtenerCursosPaginado(request);
				return Ok(resultado);
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}

		[HttpPut("editar")]
		public async Task<IActionResult> EditarCurso([FromBody] EditarCursoDTO modelo)
		{
			try
			{
				var curso = await _cursoService.EditarCurso(modelo);
				return Ok(curso);
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}
	}
}
