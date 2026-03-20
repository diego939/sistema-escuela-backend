using Microsoft.AspNetCore.Mvc;
using SistemaEscuela.BLL.Contratos;
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
				return BadRequest(ex.Message);
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
				return BadRequest(ex.Message);
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
				return BadRequest(ex.Message);
			}
		}
	}
}
