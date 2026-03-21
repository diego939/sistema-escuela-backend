using Microsoft.EntityFrameworkCore;
using SistemaEscuela.BLL.Contratos;
using SistemaEscuela.DAL.Repositorios.Contrato;
using SistemaEscuela.DTO.Inscripcion;
using SistemaEscuela.Model;

namespace SistemaEscuela.BLL.Servicios
{
	public class InscripcionService : IInscripcionService
	{
		private readonly IGenericRepository<Inscripcion> _inscripcionRepository;
		private readonly IGenericRepository<Usuario> _usuarioRepository;
		private readonly IGenericRepository<Curso> _cursoRepository;

		public InscripcionService(
			IGenericRepository<Inscripcion> inscripcionRepository,
			IGenericRepository<Usuario> usuarioRepository,
			IGenericRepository<Curso> cursoRepository)
		{
			_inscripcionRepository = inscripcionRepository;
			_usuarioRepository = usuarioRepository;
			_cursoRepository = cursoRepository;
		}

		public async Task<InscripcionResultDTO> InscribirAlumnoEnCurso(InscribirAlumnoDTO modelo)
		{
			// Validar que el alumno existe
			var alumno = await _usuarioRepository.Consultar(u =>
				u.Id == modelo.IdAlumno &&
				u.FechaEliminacion == null)
				.FirstOrDefaultAsync();

			if (alumno == null)
				throw new Exception("El alumno no existe");

			// Validar que el curso existe
			var curso = await _cursoRepository.Consultar(c =>
				c.Id == modelo.IdCurso &&
				c.FechaEliminacion == null)
				.FirstOrDefaultAsync();

			if (curso == null)
				throw new Exception("El curso no existe");

			// Validar que el alumno no esté ya inscrito en este curso
			var inscripcionExistente = await _inscripcionRepository.Consultar(i =>
				i.IdAlumno == modelo.IdAlumno &&
				i.IdCurso == modelo.IdCurso &&
				i.FechaEliminacion == null)
				.FirstOrDefaultAsync();

			if (inscripcionExistente != null)
				throw new Exception("El alumno ya está inscrito en este curso");

			// Validar el cupo del curso
			var countInscritos = await _inscripcionRepository.Consultar(i =>
				i.IdCurso == modelo.IdCurso &&
				i.FechaEliminacion == null)
				.CountAsync();

			if (curso.CupoMaximo.HasValue && countInscritos >= curso.CupoMaximo.Value)
				throw new Exception("El curso ha alcanzado su cupo máximo");

			// Crear la nueva inscripción
			var inscripcion = new Inscripcion
			{
				IdAlumno = modelo.IdAlumno,
				IdCurso = modelo.IdCurso,
				IdMediodepago = modelo.IdMediodepago,
				Direccion = modelo.Direccion,
				Comprobante = modelo.Comprobante,
				Monto = modelo.Monto,
				FechaCreacion = DateTime.Now
			};

			var creada = await _inscripcionRepository.Crear(inscripcion);

			return new InscripcionResultDTO
			{
				Id = creada.Id,
				IdAlumno = creada.IdAlumno.Value,
				AlumnoNombre = $"{alumno.Nombres} {alumno.Apellidos}",
				IdCurso = creada.IdCurso.Value,
				CursoNombre = GenerarNombreCurso(curso),
				Monto = creada.Monto,
				FechaInscripcion = creada.FechaCreacion.Value
			};
		}

		public async Task<List<InscripcionDTO>> ObtenerInscripcionesDelAlumno(int idAlumno)
		{
			// Validar que el alumno existe
			var alumno = await _usuarioRepository.Consultar(u =>
				u.Id == idAlumno &&
				u.FechaEliminacion == null)
				.FirstOrDefaultAsync();

			if (alumno == null)
				throw new Exception("El alumno no existe");

			// Obtener las inscripciones del alumno
			var datos = await _inscripcionRepository.Consultar(i =>
				i.IdAlumno == idAlumno &&
				i.FechaEliminacion == null)
				.Include(i => i.IdCursoNavigation)
				.Include(i => i.IdAlumnoNavigation)
				.ToListAsync();

			// Mapear en memoria
			var inscripciones = datos
				.Select(i => new InscripcionDTO
				{
					Id = i.Id,
					IdAlumno = i.IdAlumno.Value,
					AlumnoNombre = $"{i.IdAlumnoNavigation.Nombres} {i.IdAlumnoNavigation.Apellidos}",
					IdCurso = i.IdCurso.Value,
					Modulo = i.IdCursoNavigation.Modulo.ToString(),
					Division = i.IdCursoNavigation.Division,
					Modalidad = i.IdCursoNavigation.Modalidad,
					Turno = i.IdCursoNavigation.Turno,
					Anio = i.IdCursoNavigation.Anio.Value,
					Monto = i.Monto,
					FechaInscripcion = i.FechaCreacion.Value
				})
				.OrderByDescending(x => x.FechaInscripcion)
				.ToList();

			return inscripciones;
		}

		public async Task<List<InscripcionDTO>> ObtenerAlumnosInscritosEnCurso(int idCurso)
		{
			// Validar que el curso existe
			var curso = await _cursoRepository.Consultar(c =>
				c.Id == idCurso &&
				c.FechaEliminacion == null)
				.FirstOrDefaultAsync();

			if (curso == null)
				throw new Exception("El curso no existe");

			// Obtener los alumnos inscritos en el curso
			var datos = await _inscripcionRepository.Consultar(i =>
				i.IdCurso == idCurso &&
				i.FechaEliminacion == null)
				.Include(i => i.IdAlumnoNavigation)
				.Include(i => i.IdCursoNavigation)
				.ToListAsync();

			// Mapear en memoria
			var inscripciones = datos
				.Select(i => new InscripcionDTO
				{
					Id = i.Id,
					IdAlumno = i.IdAlumno.Value,
					AlumnoNombre = $"{i.IdAlumnoNavigation.Nombres} {i.IdAlumnoNavigation.Apellidos}",
					IdCurso = i.IdCurso.Value,
					Modulo = i.IdCursoNavigation.Modulo.ToString(),
					Division = i.IdCursoNavigation.Division,
					Modalidad = i.IdCursoNavigation.Modalidad,
					Turno = i.IdCursoNavigation.Turno,
					Anio = i.IdCursoNavigation.Anio.Value,
					Monto = i.Monto,
					FechaInscripcion = i.FechaCreacion.Value
				})
				.OrderBy(x => x.AlumnoNombre)
				.ToList();

			return inscripciones;
		}

		public async Task<bool> DesinscribirAlumno(int idAlumno, int idCurso)
		{
			// Buscar la inscripción activa
			var inscripcion = await _inscripcionRepository.Consultar(i =>
				i.IdAlumno == idAlumno &&
				i.IdCurso == idCurso &&
				i.FechaEliminacion == null)
				.FirstOrDefaultAsync();

			if (inscripcion == null)
				throw new Exception("La inscripción no existe");

			// Marcar como eliminada
			inscripcion.FechaEliminacion = DateTime.Now;
			await _inscripcionRepository.Editar(inscripcion);

			return true;
		}

		private string GenerarNombreCurso(Curso curso)
		{
			return $"Año: {curso.Anio} - Grado {curso.Modulo}° - División ({curso.Division}) - Turno {curso.Turno}";
		}
	}
}
