using Microsoft.AspNetCore.Mvc;
using SistemaEscuela.BLL.Contratos;
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
				return BadRequest(ex.Message);
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
				return BadRequest(ex.Message);
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
				return BadRequest(ex.Message);
			}
		}
	}
}
