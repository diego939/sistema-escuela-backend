using Microsoft.AspNetCore.Mvc;
using SistemaEscuela.BLL.Contratos;
using SistemaEscuela.DTO.Calificacion;

namespace SistemaEscuela.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CalificacionController : ControllerBase
	{
		private readonly ICalificacionService _calificacionService;

		public CalificacionController(ICalificacionService calificacionService)
		{
			_calificacionService = calificacionService;
		}

		[HttpPost("registrar")]
		public async Task<IActionResult> RegistrarCalificacion([FromBody] RegistrarCalificacionDTO modelo)
		{
			try
			{
				var resultado = await _calificacionService.RegistrarCalificacion(modelo);
				return Ok(resultado);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpPut("actualizar")]
		public async Task<IActionResult> ActualizarCalificacion([FromBody] ActualizarCalificacionDTO modelo)
		{
			try
			{
				var resultado = await _calificacionService.ActualizarCalificacion(modelo);
				return Ok(resultado);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpGet]
		public async Task<IActionResult> ObtenerTodasLasCalificaciones()
		{
			try
			{
				var calificaciones = await _calificacionService.ObtenerTodasLasCalificaciones();
				return Ok(calificaciones);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpGet("alumno/{idAlumno}")]
		public async Task<IActionResult> ObtenerCalificacionesPorAlumno(int idAlumno)
		{
			try
			{
				var calificaciones = await _calificacionService.ObtenerCalificacionesPorAlumno(idAlumno);
				return Ok(calificaciones);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpGet("alumno/{idAlumno}/materia/{idMateria}")]
		public async Task<IActionResult> ObtenerCalificacionesPorAlumnoYMateria(int idAlumno, int idMateria)
		{
			try
			{
				var calificaciones = await _calificacionService.ObtenerCalificacionesPorAlumnoYMateria(idAlumno, idMateria);
				return Ok(calificaciones);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpGet("curso/{idCurso}/materia/{idMateria}")]
		public async Task<IActionResult> ObtenerCalificacionesPorCursoYMateria(int idCurso, int idMateria)
		{
			try
			{
				var calificaciones = await _calificacionService.ObtenerCalificacionesPorCursoYMateria(idCurso, idMateria);
				return Ok(calificaciones);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpDelete("{idCalificacion}")]
		public async Task<IActionResult> EliminarCalificacion(int idCalificacion)
		{
			try
			{
				var resultado = await _calificacionService.EliminarCalificacion(idCalificacion);
				return Ok(resultado);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
	}
}
