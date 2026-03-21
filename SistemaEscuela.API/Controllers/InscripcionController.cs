using Microsoft.AspNetCore.Mvc;
using SistemaEscuela.BLL.Contratos;
using SistemaEscuela.DTO.Inscripcion;

namespace SistemaEscuela.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class InscripcionController : ControllerBase
	{
		private readonly IInscripcionService _inscripcionService;

		public InscripcionController(IInscripcionService inscripcionService)
		{
			_inscripcionService = inscripcionService;
		}

		[HttpPost("inscribir")]
		public async Task<IActionResult> InscribirAlumnoEnCurso([FromBody] InscribirAlumnoDTO modelo)
		{
			try
			{
				var resultado = await _inscripcionService.InscribirAlumnoEnCurso(modelo);
				return Ok(resultado);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpGet("alumno/{idAlumno}")]
		public async Task<IActionResult> ObtenerInscripcionesDelAlumno(int idAlumno)
		{
			try
			{
				var inscripciones = await _inscripcionService.ObtenerInscripcionesDelAlumno(idAlumno);
				return Ok(inscripciones);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpGet("curso/{idCurso}")]
		public async Task<IActionResult> ObtenerAlumnosInscritosEnCurso(int idCurso)
		{
			try
			{
				var alumnos = await _inscripcionService.ObtenerAlumnosInscritosEnCurso(idCurso);
				return Ok(alumnos);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpDelete("alumno/{idAlumno}/curso/{idCurso}")]
		public async Task<IActionResult> DesinscribirAlumno(int idAlumno, int idCurso)
		{
			try
			{
				var resultado = await _inscripcionService.DesinscribirAlumno(idAlumno, idCurso);
				return Ok(resultado);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
	}
}
