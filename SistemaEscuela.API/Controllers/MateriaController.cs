using Microsoft.AspNetCore.Mvc;
using SistemaEscuela.BLL.Contratos;
using SistemaEscuela.DTO.Materia;

namespace SistemaEscuela.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class MateriaController : ControllerBase
	{
		private readonly IMateriaService _materiaService;

		public MateriaController(IMateriaService materiaService)
		{
			_materiaService = materiaService;
		}

		[HttpPost("crear")]
		public async Task<IActionResult> CrearMateria([FromBody] CrearMateriaDTO modelo)
		{
			try
			{
				var materia = await _materiaService.CrearMateria(modelo);
				return Ok(materia);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpGet("lista")]
		public async Task<IActionResult> ObtenerMaterias()
		{
			try
			{
				var materias = await _materiaService.ObtenerMaterias();
				return Ok(materias);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpPost("asociar")]
		public async Task<IActionResult> AsociarMateriaACurso([FromBody] AsociarMateriaDTO modelo)
		{
			try
			{
				var resultado = await _materiaService.AsociarMateriaACurso(modelo);
				return Ok(resultado);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpGet("curso/{idCurso}")]
		public async Task<IActionResult> ObtenerMateriasDelCurso(int idCurso)
		{
			try
			{
				var materias = await _materiaService.ObtenerMateriasDelCurso(idCurso);
				return Ok(materias);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
	}
}
