using Microsoft.AspNetCore.Mvc;
using SistemaEscuela.BLL.Contratos;
using SistemaEscuela.BLL.Servicios;
using SistemaEscuela.DTO.Comun;
using SistemaEscuela.DTO.Profesor;

namespace SistemaEscuela.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProfesorController : ControllerBase
	{
		private readonly IProfesorService _profesorService;

		public ProfesorController(IProfesorService profesorService)
		{
			_profesorService = profesorService;
		}

		[HttpPost("asignar")]
		public async Task<IActionResult> AsignarProfesorAMateria([FromBody] AsignarProfesorDTO modelo)
		{
			try
			{
				var resultado = await _profesorService.AsignarProfesorAMateria(modelo);
				return Ok(resultado);
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}

		[HttpGet("{idProfesor}/materias")]
		public async Task<IActionResult> ObtenerMateriasDelProfesor(int idProfesor)
		{
			try
			{
				var materias = await _profesorService.ObtenerMateriasDelProfesor(idProfesor);
				return Ok(materias);
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}

		[HttpGet("lista-paginado")]
		public async Task<IActionResult> ListaProfesoresPaginado([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string search = "", [FromQuery] string sortBy = "nombres", [FromQuery] bool sortDescending = false)
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

				var resultado = await _profesorService.ListaProfesoresPaginado(request);
				return Ok(resultado);
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}

		[HttpGet("{idProfesor}/cursos")]
		public async Task<IActionResult> ObtenerCursosDelProfesor(int idProfesor)
		{
			try
			{
				var cursos = await _profesorService.ObtenerCursosDelProfesor(idProfesor);
				return Ok(cursos);
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}
	}
}
