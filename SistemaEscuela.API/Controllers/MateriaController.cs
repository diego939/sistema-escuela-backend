using Microsoft.AspNetCore.Mvc;
using SistemaEscuela.BLL.Contratos;
using SistemaEscuela.DTO.Comun;
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
				return BadRequest(new { message = ex.Message });
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
				return BadRequest(new { message = ex.Message });
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
				return BadRequest(new { message = ex.Message });
			}
		}

		/// <summary>
		/// Desasocia una materia de un curso (eliminación lógica de la relación CursoMateria).
		/// Mismo esquema que asociar: { "idCurso": 0, "idMateria": 0 }.
		/// </summary>
		[HttpPost("desasociar")]
		public async Task<IActionResult> DesasociarMateriaACurso([FromBody] AsociarMateriaDTO modelo)
		{
			try
			{
				var resultado = await _materiaService.DesasociarMateriaCurso(modelo.IdCurso, modelo.IdMateria);
				return Ok(resultado);
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}

		/// <summary>
		/// Alternativa REST al POST desasociar: DELETE .../curso/{idCurso}/materia/{idMateria}.
		/// </summary>
		[HttpDelete("curso/{idCurso}/materia/{idMateria}")]
		public async Task<IActionResult> DesasociarMateriaCursoPorRuta(int idCurso, int idMateria)
		{
			try
			{
				var resultado = await _materiaService.DesasociarMateriaCurso(idCurso, idMateria);
				return Ok(resultado);
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = ex.Message });
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
				return BadRequest(new { message = ex.Message });
			}
		}

		[HttpGet("lista-paginado")]
		public async Task<IActionResult> ObtenerMateriasPaginado([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string search = "", [FromQuery] string sortBy = "descripcion", [FromQuery] bool sortDescending = false)
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
				var materias = await _materiaService.ObtenerMateriasPaginado(request);
				return Ok(materias);
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}

		[HttpPut("editar")]
		public async Task<IActionResult> EditarMateria([FromBody] EditarMateriDTO modelo)
		{
			try
			{
				var materia = await _materiaService.EditarMateria(modelo);
				return Ok(materia);
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = ex.Message });
			}
		}
	}
}
