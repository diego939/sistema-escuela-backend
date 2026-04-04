using Microsoft.AspNetCore.Mvc;
using SistemaEscuela.BLL.Contratos;
using SistemaEscuela.BLL.Servicios;
using SistemaEscuela.DTO.Comun;
using SistemaEscuela.DTO.Preceptor;

namespace SistemaEscuela.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class PreceptorController : ControllerBase
	{
		private readonly IPreceptorService _preceptorService;

		public PreceptorController(IPreceptorService preceptorService)
		{
			_preceptorService = preceptorService;
		}

		[HttpPost("asignar")]
		public async Task<IActionResult> AsignarPreceptorACurso([FromBody] AsignarPreceptorDTO modelo)
		{
			try
			{
				var resultado = await _preceptorService.AsignarPreceptorACurso(modelo);
				return Ok(resultado);
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}

		[HttpGet("{idPreceptor}/cursos")]
		public async Task<IActionResult> ObtenerCursosDelPreceptor(int idPreceptor)
		{
			try
			{
				var cursos = await _preceptorService.ObtenerCursosDelPreceptor(idPreceptor);
				return Ok(cursos);
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}

		[HttpDelete("{idPreceptor}/cursos/{idCurso}")]
		public async Task<IActionResult> DesasignarPreceptorDelCurso(int idPreceptor, int idCurso)
		{
			try
			{
				var resultado = await _preceptorService.DesasignarPreceptorDelCurso(idPreceptor, idCurso);
				return Ok(resultado);
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}

		[HttpGet("lista-paginado")]
		public async Task<IActionResult> ListaPreceptoresPaginado([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string search = "", [FromQuery] string sortBy = "nombres", [FromQuery] bool sortDescending = false)
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

				var resultado = await _preceptorService.ListaPreceptoresPaginado(request);
				return Ok(resultado);
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}
	}
}
