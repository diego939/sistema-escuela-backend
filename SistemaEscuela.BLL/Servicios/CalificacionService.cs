using Microsoft.EntityFrameworkCore;
using SistemaEscuela.BLL.Contratos;
using SistemaEscuela.DAL.Repositorios.Contrato;
using SistemaEscuela.DTO.Calificacion;
using SistemaEscuela.Model;

namespace SistemaEscuela.BLL.Servicios
{
	public class CalificacionService : ICalificacionService
	{
		private readonly IGenericRepository<Calificacion> _calificacionRepository;
		private readonly IGenericRepository<Usuario> _usuarioRepository;
		private readonly IGenericRepository<CursoMateria> _cursoMateriaRepository;
		private readonly IGenericRepository<Curso> _cursoRepository;
		private readonly IGenericRepository<Materia> _materiaRepository;

		public CalificacionService(
			IGenericRepository<Calificacion> calificacionRepository,
			IGenericRepository<Usuario> usuarioRepository,
			IGenericRepository<CursoMateria> cursoMateriaRepository,
			IGenericRepository<Curso> cursoRepository,
			IGenericRepository<Materia> materiaRepository)
		{
			_calificacionRepository = calificacionRepository;
			_usuarioRepository = usuarioRepository;
			_cursoMateriaRepository = cursoMateriaRepository;
			_cursoRepository = cursoRepository;
			_materiaRepository = materiaRepository;
		}

		public async Task<CalificacionResultDTO> RegistrarCalificacion(RegistrarCalificacionDTO modelo)
		{
			// Validar que el alumno existe
			var alumno = await _usuarioRepository.Consultar(u =>
				u.Id == modelo.IdAlumno &&
				u.FechaEliminacion == null)
				.FirstOrDefaultAsync();

			if (alumno == null)
				throw new Exception("El alumno no existe");

			// Validar que la asignación de materia al curso existe
			var cursoMateria = await _cursoMateriaRepository.Consultar(cm =>
				cm.Id == modelo.IdCursomateria &&
				cm.FechaEliminacion == null)
				.FirstOrDefaultAsync();

			if (cursoMateria == null)
				throw new Exception("La asignación de materia al curso no existe");

			// Validar la nota
			if (modelo.Nota < 0 || modelo.Nota > 10)
				throw new Exception("La nota debe estar entre 0 y 10");

			// Validar el trimestre
			if (modelo.Trimestre < 1 || modelo.Trimestre > 3)
				throw new Exception("El trimestre debe ser 1, 2 o 3");

			// Validar que no exista calificación previa para este trimestre
			var calificacionExistente = await _calificacionRepository.Consultar(c =>
				c.IdAlumno == modelo.IdAlumno &&
				c.IdCursomateria == modelo.IdCursomateria &&
				c.Trimestre == modelo.Trimestre &&
				c.FechaEliminacion == null)
				.FirstOrDefaultAsync();

			if (calificacionExistente != null)
				throw new Exception($"El alumno ya tiene una calificación registrada para el trimestre {modelo.Trimestre} en esta materia");

			// Crear la nueva calificación
			var calificacion = new Calificacion
			{
				IdAlumno = modelo.IdAlumno,
				IdCursomateria = modelo.IdCursomateria,
				Nota = modelo.Nota,
				Descripcion = modelo.Descripcion,
				Trimestre = modelo.Trimestre,
				FechaCreacion = DateTime.Now
			};

			var creada = await _calificacionRepository.Crear(calificacion);

			return new CalificacionResultDTO
			{
				Id = creada.Id,
				IdAlumno = creada.IdAlumno.Value,
				AlumnoNombre = $"{alumno.Nombres} {alumno.Apellidos}",
				IdCursomateria = creada.IdCursomateria.Value,
				Nota = creada.Nota.Value,
				Descripcion = creada.Descripcion,
				Trimestre = creada.Trimestre.Value,
				FechaRegistro = creada.FechaCreacion.Value
			};
		}

		public async Task<CalificacionResultDTO> ActualizarCalificacion(ActualizarCalificacionDTO modelo)
		{
			// Buscar la calificación
			var calificacion = await _calificacionRepository.Consultar(c =>
				c.Id == modelo.Id &&
				c.FechaEliminacion == null)
				.FirstOrDefaultAsync();

			if (calificacion == null)
				throw new Exception("La calificación no existe");

			// Validar la nota
			if (modelo.Nota < 0 || modelo.Nota > 10)
				throw new Exception("La nota debe estar entre 0 y 10");

			// Validar el trimestre
			if (modelo.Trimestre < 1 || modelo.Trimestre > 3)
				throw new Exception("El trimestre debe ser 1, 2 o 3");

			// Actualizar la calificación
			calificacion.Nota = modelo.Nota;
			calificacion.Descripcion = modelo.Descripcion;
			calificacion.Trimestre = modelo.Trimestre;

			await _calificacionRepository.Editar(calificacion);

			// Obtener el alumno para retornar el nombre
			var alumno = await _usuarioRepository.Consultar(u => u.Id == calificacion.IdAlumno)
				.FirstOrDefaultAsync();

			return new CalificacionResultDTO
			{
				Id = calificacion.Id,
				IdAlumno = calificacion.IdAlumno.Value,
				AlumnoNombre = $"{alumno.Nombres} {alumno.Apellidos}",
				IdCursomateria = calificacion.IdCursomateria.Value,
				Nota = calificacion.Nota.Value,
				Descripcion = calificacion.Descripcion,
				Trimestre = calificacion.Trimestre.Value,
				FechaRegistro = calificacion.FechaCreacion.Value
			};
		}

		public async Task<List<CalificacionDTO>> ObtenerTodasLasCalificaciones()
		{
			// Obtener todas las calificaciones activas
			var datos = await _calificacionRepository.Consultar(c =>
				c.FechaEliminacion == null)
				.Include(c => c.IdAlumnoNavigation)
				.Include(c => c.IdCursomateriaNavigation)
				.ThenInclude(cm => cm.IdCursoNavigation)
				.Include(c => c.IdCursomateriaNavigation)
				.ThenInclude(cm => cm.IdMateriaNavigation)
				.ToListAsync();

			// Mapear en memoria
			var calificaciones = datos
				.Select(c => new CalificacionDTO
				{
					Id = c.Id,
					IdAlumno = c.IdAlumno.Value,
					AlumnoNombre = $"{c.IdAlumnoNavigation.Nombres} {c.IdAlumnoNavigation.Apellidos}",
					IdCurso = c.IdCursomateriaNavigation.IdCurso.Value,
					CursoNombre = GenerarNombreCurso(c.IdCursomateriaNavigation.IdCursoNavigation),
					IdMateria = c.IdCursomateriaNavigation.IdMateria.Value,
					MateriaNombre = c.IdCursomateriaNavigation.IdMateriaNavigation.Descripcion,
					Nota = c.Nota.Value,
					Descripcion = c.Descripcion,
					Trimestre = c.Trimestre.Value,
					FechaRegistro = c.FechaCreacion.Value
				})
				.OrderByDescending(x => x.FechaRegistro)
				.ThenBy(x => x.AlumnoNombre)
				.ToList();

			return calificaciones;
		}

		public async Task<List<CalificacionDTO>> ObtenerCalificacionesPorAlumno(int idAlumno)
		{
			// Validar que el alumno existe
			var alumno = await _usuarioRepository.Consultar(u =>
				u.Id == idAlumno &&
				u.FechaEliminacion == null)
				.FirstOrDefaultAsync();

			if (alumno == null)
				throw new Exception("El alumno no existe");

			// Obtener las calificaciones del alumno
			var datos = await _calificacionRepository.Consultar(c =>
				c.IdAlumno == idAlumno &&
				c.FechaEliminacion == null)
				.Include(c => c.IdCursomateriaNavigation)
				.ThenInclude(cm => cm.IdCursoNavigation)
				.Include(c => c.IdCursomateriaNavigation)
				.ThenInclude(cm => cm.IdMateriaNavigation)
				.ToListAsync();

			// Mapear en memoria
			var calificaciones = datos
				.Select(c => new CalificacionDTO
				{
					Id = c.Id,
					IdAlumno = c.IdAlumno.Value,
					AlumnoNombre = $"{alumno.Nombres} {alumno.Apellidos}",
					IdCurso = c.IdCursomateriaNavigation.IdCurso.Value,
					CursoNombre = GenerarNombreCurso(c.IdCursomateriaNavigation.IdCursoNavigation),
					IdMateria = c.IdCursomateriaNavigation.IdMateria.Value,
					MateriaNombre = c.IdCursomateriaNavigation.IdMateriaNavigation.Descripcion,
					Nota = c.Nota.Value,
					Descripcion = c.Descripcion,
					Trimestre = c.Trimestre.Value,
					FechaRegistro = c.FechaCreacion.Value
				})
				.OrderBy(x => x.Trimestre)
				.ThenBy(x => x.MateriaNombre)
				.ToList();

			return calificaciones;
		}

		public async Task<List<CalificacionDTO>> ObtenerCalificacionesPorAlumnoYMateria(int idAlumno, int idMateria)
		{
			// Validar que el alumno existe
			var alumno = await _usuarioRepository.Consultar(u =>
				u.Id == idAlumno &&
				u.FechaEliminacion == null)
				.FirstOrDefaultAsync();

			if (alumno == null)
				throw new Exception("El alumno no existe");

			// Validar que la materia existe
			var materia = await _materiaRepository.Consultar(m =>
				m.Id == idMateria &&
				m.FechaEliminacion == null)
				.FirstOrDefaultAsync();

			if (materia == null)
				throw new Exception("La materia no existe");

			// Obtener las calificaciones del alumno en la materia
			var datos = await _calificacionRepository.Consultar(c =>
				c.IdAlumno == idAlumno &&
				c.IdCursomateriaNavigation.IdMateria == idMateria &&
				c.FechaEliminacion == null)
				.Include(c => c.IdCursomateriaNavigation)
				.ThenInclude(cm => cm.IdCursoNavigation)
				.Include(c => c.IdCursomateriaNavigation)
				.ThenInclude(cm => cm.IdMateriaNavigation)
				.ToListAsync();

			// Mapear en memoria
			var calificaciones = datos
				.Select(c => new CalificacionDTO
				{
					Id = c.Id,
					IdAlumno = c.IdAlumno.Value,
					AlumnoNombre = $"{alumno.Nombres} {alumno.Apellidos}",
					IdCurso = c.IdCursomateriaNavigation.IdCurso.Value,
					CursoNombre = GenerarNombreCurso(c.IdCursomateriaNavigation.IdCursoNavigation),
					IdMateria = c.IdCursomateriaNavigation.IdMateria.Value,
					MateriaNombre = c.IdCursomateriaNavigation.IdMateriaNavigation.Descripcion,
					Nota = c.Nota.Value,
					Descripcion = c.Descripcion,
					Trimestre = c.Trimestre.Value,
					FechaRegistro = c.FechaCreacion.Value
				})
				.OrderBy(x => x.Trimestre)
				.ToList();

			return calificaciones;
		}

		public async Task<List<CalificacionDTO>> ObtenerCalificacionesPorCursoYMateria(int idCurso, int idMateria)
		{
			// Validar que el curso existe
			var curso = await _cursoRepository.Consultar(c =>
				c.Id == idCurso &&
				c.FechaEliminacion == null)
				.FirstOrDefaultAsync();

			if (curso == null)
				throw new Exception("El curso no existe");

			// Validar que la materia existe
			var materia = await _materiaRepository.Consultar(m =>
				m.Id == idMateria &&
				m.FechaEliminacion == null)
				.FirstOrDefaultAsync();

			if (materia == null)
				throw new Exception("La materia no existe");

			// Obtener las calificaciones del curso en la materia
			var datos = await _calificacionRepository.Consultar(c =>
				c.IdCursomateriaNavigation.IdCurso == idCurso &&
				c.IdCursomateriaNavigation.IdMateria == idMateria &&
				c.FechaEliminacion == null)
				.Include(c => c.IdAlumnoNavigation)
				.Include(c => c.IdCursomateriaNavigation)
				.ThenInclude(cm => cm.IdCursoNavigation)
				.Include(c => c.IdCursomateriaNavigation)
				.ThenInclude(cm => cm.IdMateriaNavigation)
				.ToListAsync();

			// Mapear en memoria
			var calificaciones = datos
				.Select(c => new CalificacionDTO
				{
					Id = c.Id,
					IdAlumno = c.IdAlumno.Value,
					AlumnoNombre = $"{c.IdAlumnoNavigation.Nombres} {c.IdAlumnoNavigation.Apellidos}",
					IdCurso = c.IdCursomateriaNavigation.IdCurso.Value,
					CursoNombre = GenerarNombreCurso(c.IdCursomateriaNavigation.IdCursoNavigation),
					IdMateria = c.IdCursomateriaNavigation.IdMateria.Value,
					MateriaNombre = c.IdCursomateriaNavigation.IdMateriaNavigation.Descripcion,
					Nota = c.Nota.Value,
					Descripcion = c.Descripcion,
					Trimestre = c.Trimestre.Value,
					FechaRegistro = c.FechaCreacion.Value
				})
				.OrderBy(x => x.AlumnoNombre)
				.ThenBy(x => x.Trimestre)
				.ToList();

			return calificaciones;
		}

		public async Task<bool> EliminarCalificacion(int idCalificacion)
		{
			// Buscar la calificación
			var calificacion = await _calificacionRepository.Consultar(c =>
				c.Id == idCalificacion &&
				c.FechaEliminacion == null)
				.FirstOrDefaultAsync();

			if (calificacion == null)
				throw new Exception("La calificación no existe");

			// Marcar como eliminada
			calificacion.FechaEliminacion = DateTime.Now;
			await _calificacionRepository.Editar(calificacion);

			return true;
		}

		private string GenerarNombreCurso(Curso curso)
		{
			return $"Año: {curso.Anio} - Grado {curso.Modulo}° - División ({curso.Division}) - Turno {curso.Turno}";
		}
	}
}
