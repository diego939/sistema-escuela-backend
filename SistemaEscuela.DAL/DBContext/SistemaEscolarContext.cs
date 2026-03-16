using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using SistemaEscuela.Model;

namespace SistemaEscuela.DAL.DBContext
{
	public partial class SistemaEscolarContext : DbContext
	{
	    public SistemaEscolarContext()
	    {
	    }

	    public SistemaEscolarContext(DbContextOptions<SistemaEscolarContext> options)
	        : base(options)
	    {
	    }

	    public virtual DbSet<Asistencia> Asistencia { get; set; }

	    public virtual DbSet<Calificacion> Calificacions { get; set; }

	    public virtual DbSet<Curso> Cursos { get; set; }

	    public virtual DbSet<CursoMateria> CursoMateria { get; set; }

	    public virtual DbSet<Inscripcion> Inscripcions { get; set; }

	    public virtual DbSet<Materia> Materia { get; set; }

	    public virtual DbSet<MedioDePago> MedioDePagos { get; set; }

	    public virtual DbSet<Menu> Menus { get; set; }

	    public virtual DbSet<MenuRol> MenuRols { get; set; }

	    public virtual DbSet<Posteo> Posteos { get; set; }

	    public virtual DbSet<PreceptorCurso> PreceptorCursos { get; set; }

	    public virtual DbSet<ProfesorCursoMateria> ProfesorCursoMateria { get; set; }

	    public virtual DbSet<Rol> Rols { get; set; }

	    public virtual DbSet<Usuario> Usuarios { get; set; }

	    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }

	    protected override void OnModelCreating(ModelBuilder modelBuilder)
	    {
	        modelBuilder.Entity<Asistencia>(entity =>
	        {
	            entity.HasKey(e => e.Id).HasName("PK__Asistenc__3213E83F3FD841E3");

	            entity.Property(e => e.Id).HasColumnName("id");
	            entity.Property(e => e.Estado)
	                .HasMaxLength(1)
	                .IsUnicode(false)
	                .IsFixedLength()
	                .HasColumnName("estado");
	            entity.Property(e => e.Fecha).HasColumnName("fecha");
	            entity.Property(e => e.FechaEliminacion)
	                .HasColumnType("datetime")
	                .HasColumnName("fecha_eliminacion");
	            entity.Property(e => e.IdAsistente).HasColumnName("id_asistente");
	            entity.Property(e => e.IdInscripcion).HasColumnName("id_inscripcion");

	            entity.HasOne(d => d.IdAsistenteNavigation).WithMany(p => p.Asistencia)
	                .HasForeignKey(d => d.IdAsistente)
	                .HasConstraintName("FK__Asistenci__id_as__02084FDA");

	            entity.HasOne(d => d.IdInscripcionNavigation).WithMany(p => p.Asistencia)
	                .HasForeignKey(d => d.IdInscripcion)
	                .HasConstraintName("FK__Asistenci__id_in__01142BA1");
	        });

	        modelBuilder.Entity<Calificacion>(entity =>
	        {
	            entity.HasKey(e => e.Id).HasName("PK__Califica__3213E83F66C14F22");

	            entity.ToTable("Calificacion");

	            entity.Property(e => e.Id).HasColumnName("id");
	            entity.Property(e => e.Descripcion)
	                .HasMaxLength(200)
	                .IsUnicode(false)
	                .HasColumnName("descripcion");
	            entity.Property(e => e.FechaCreacion)
	                .HasDefaultValueSql("(getdate())")
	                .HasColumnType("datetime")
	                .HasColumnName("fecha_creacion");
	            entity.Property(e => e.FechaEliminacion)
	                .HasColumnType("datetime")
	                .HasColumnName("fecha_eliminacion");
	            entity.Property(e => e.IdAlumno).HasColumnName("id_alumno");
	            entity.Property(e => e.IdCursomateria).HasColumnName("id_cursomateria");
	            entity.Property(e => e.Nota)
	                .HasColumnType("decimal(4, 2)")
	                .HasColumnName("nota");
	            entity.Property(e => e.Trimestre).HasColumnName("trimestre");

	            entity.HasOne(d => d.IdAlumnoNavigation).WithMany(p => p.Calificacions)
	                .HasForeignKey(d => d.IdAlumno)
	                .HasConstraintName("FK__Calificac__id_al__05D8E0BE");

	            entity.HasOne(d => d.IdCursomateriaNavigation).WithMany(p => p.Calificacions)
	                .HasForeignKey(d => d.IdCursomateria)
	                .HasConstraintName("FK__Calificac__id_cu__06CD04F7");
	        });

	        modelBuilder.Entity<Curso>(entity =>
	        {
	            entity.HasKey(e => e.Id).HasName("PK__Curso__3213E83F8DC7DF47");

	            entity.ToTable("Curso");

	            entity.Property(e => e.Id).HasColumnName("id");
	            entity.Property(e => e.Anio).HasColumnName("anio");
	            entity.Property(e => e.CupoMaximo)
	                .HasDefaultValue(20)
	                .HasColumnName("cupo_maximo");
	            entity.Property(e => e.Division)
	                .HasMaxLength(1)
	                .IsUnicode(false)
	                .IsFixedLength()
	                .HasColumnName("division");
	            entity.Property(e => e.FechaCreacion)
	                .HasDefaultValueSql("(getdate())")
	                .HasColumnType("datetime")
	                .HasColumnName("fecha_creacion");
	            entity.Property(e => e.FechaEliminacion)
	                .HasColumnType("datetime")
	                .HasColumnName("fecha_eliminacion");
	            entity.Property(e => e.Modalidad)
	                .HasMaxLength(50)
	                .IsUnicode(false)
	                .HasColumnName("modalidad");
	            entity.Property(e => e.Modulo).HasColumnName("modulo");
	            entity.Property(e => e.Turno)
	                .HasMaxLength(20)
	                .IsUnicode(false)
	                .HasColumnName("turno");
	        });

	        modelBuilder.Entity<CursoMateria>(entity =>
	        {
	            entity.HasKey(e => e.Id).HasName("PK__CursoMat__3213E83F6F89C969");

	            entity.Property(e => e.Id).HasColumnName("id");
	            entity.Property(e => e.FechaCreacion)
	                .HasDefaultValueSql("(getdate())")
	                .HasColumnType("datetime")
	                .HasColumnName("fecha_creacion");
	            entity.Property(e => e.FechaEliminacion)
	                .HasColumnType("datetime")
	                .HasColumnName("fecha_eliminacion");
	            entity.Property(e => e.IdCurso).HasColumnName("id_curso");
	            entity.Property(e => e.IdMateria).HasColumnName("id_materia");

	            entity.HasOne(d => d.IdCursoNavigation).WithMany(p => p.CursoMateria)
	                .HasForeignKey(d => d.IdCurso)
	                .HasConstraintName("FK__CursoMate__id_cu__6B24EA82");

	            entity.HasOne(d => d.IdMateriaNavigation).WithMany(p => p.CursoMateria)
	                .HasForeignKey(d => d.IdMateria)
	                .HasConstraintName("FK__CursoMate__id_ma__6C190EBB");
	        });

	        modelBuilder.Entity<Inscripcion>(entity =>
	        {
	            entity.HasKey(e => e.Id).HasName("PK__Inscripc__3213E83F180326E3");

	            entity.ToTable("Inscripcion");

	            entity.Property(e => e.Id).HasColumnName("id");
	            entity.Property(e => e.Comprobante)
	                .HasMaxLength(255)
	                .IsUnicode(false)
	                .HasColumnName("comprobante");
	            entity.Property(e => e.Direccion)
	                .HasMaxLength(200)
	                .IsUnicode(false)
	                .HasColumnName("direccion");
	            entity.Property(e => e.FechaCreacion)
	                .HasDefaultValueSql("(getdate())")
	                .HasColumnType("datetime")
	                .HasColumnName("fecha_creacion");
	            entity.Property(e => e.FechaEliminacion)
	                .HasColumnType("datetime")
	                .HasColumnName("fecha_eliminacion");
	            entity.Property(e => e.IdAlumno).HasColumnName("id_alumno");
	            entity.Property(e => e.IdCurso).HasColumnName("id_curso");
	            entity.Property(e => e.IdMediodepago).HasColumnName("id_mediodepago");
	            entity.Property(e => e.Monto)
	                .HasColumnType("decimal(10, 2)")
	                .HasColumnName("monto");

	            entity.HasOne(d => d.IdAlumnoNavigation).WithMany(p => p.Inscripcions)
	                .HasForeignKey(d => d.IdAlumno)
	                .HasConstraintName("FK__Inscripci__id_al__7C4F7684");

	            entity.HasOne(d => d.IdCursoNavigation).WithMany(p => p.Inscripcions)
	                .HasForeignKey(d => d.IdCurso)
	                .HasConstraintName("FK__Inscripci__id_cu__7D439ABD");

	            entity.HasOne(d => d.IdMediodepagoNavigation).WithMany(p => p.Inscripcions)
	                .HasForeignKey(d => d.IdMediodepago)
	                .HasConstraintName("FK__Inscripci__id_me__7E37BEF6");
	        });

	        modelBuilder.Entity<Materia>(entity =>
	        {
	            entity.HasKey(e => e.Id).HasName("PK__Materia__3213E83FA584A006");

	            entity.Property(e => e.Id).HasColumnName("id");
	            entity.Property(e => e.Descripcion)
	                .HasMaxLength(100)
	                .IsUnicode(false)
	                .HasColumnName("descripcion");
	            entity.Property(e => e.FechaCreacion)
	                .HasDefaultValueSql("(getdate())")
	                .HasColumnType("datetime")
	                .HasColumnName("fecha_creacion");
	            entity.Property(e => e.FechaEliminacion)
	                .HasColumnType("datetime")
	                .HasColumnName("fecha_eliminacion");
	        });

	        modelBuilder.Entity<MedioDePago>(entity =>
	        {
	            entity.HasKey(e => e.Id).HasName("PK__MedioDeP__3213E83FD344D888");

	            entity.ToTable("MedioDePago");

	            entity.Property(e => e.Id).HasColumnName("id");
	            entity.Property(e => e.Descripcion)
	                .HasMaxLength(50)
	                .IsUnicode(false)
	                .HasColumnName("descripcion");
	            entity.Property(e => e.FechaCreacion)
	                .HasDefaultValueSql("(getdate())")
	                .HasColumnType("datetime")
	                .HasColumnName("fecha_creacion");
	            entity.Property(e => e.FechaEliminacion)
	                .HasColumnType("datetime")
	                .HasColumnName("fecha_eliminacion");
	        });

	        modelBuilder.Entity<Menu>(entity =>
	        {
	            entity.HasKey(e => e.Id).HasName("PK__Menu__3213E83F9B41BFC8");

	            entity.ToTable("Menu");

	            entity.Property(e => e.Id).HasColumnName("id");
	            entity.Property(e => e.FechaCreacion)
	                .HasDefaultValueSql("(getdate())")
	                .HasColumnType("datetime")
	                .HasColumnName("fecha_creacion");
	            entity.Property(e => e.FechaEliminacion)
	                .HasColumnType("datetime")
	                .HasColumnName("fecha_eliminacion");
	            entity.Property(e => e.Nombre)
	                .HasMaxLength(100)
	                .IsUnicode(false)
	                .HasColumnName("nombre");
	            entity.Property(e => e.UrlMenu)
	                .HasMaxLength(200)
	                .IsUnicode(false)
	                .HasColumnName("url_menu");
	        });

	        modelBuilder.Entity<MenuRol>(entity =>
	        {
	            entity.HasKey(e => e.Id).HasName("PK__MenuRol__3213E83FBCEBEBC4");

	            entity.ToTable("MenuRol");

	            entity.Property(e => e.Id).HasColumnName("id");
	            entity.Property(e => e.FechaCreacion)
	                .HasDefaultValueSql("(getdate())")
	                .HasColumnType("datetime")
	                .HasColumnName("fecha_creacion");
	            entity.Property(e => e.FechaEliminacion)
	                .HasColumnType("datetime")
	                .HasColumnName("fecha_eliminacion");
	            entity.Property(e => e.IdMenu).HasColumnName("id_menu");
	            entity.Property(e => e.IdRol).HasColumnName("id_rol");

	            entity.HasOne(d => d.IdMenuNavigation).WithMany(p => p.MenuRols)
	                .HasForeignKey(d => d.IdMenu)
	                .HasConstraintName("FK__MenuRol__id_menu__123EB7A3");

	            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.MenuRols)
	                .HasForeignKey(d => d.IdRol)
	                .HasConstraintName("FK__MenuRol__id_rol__1332DBDC");
	        });

	        modelBuilder.Entity<Posteo>(entity =>
	        {
	            entity.HasKey(e => e.Id).HasName("PK__Posteo__3213E83F178865D7");

	            entity.ToTable("Posteo");

	            entity.Property(e => e.Id).HasColumnName("id");
	            entity.Property(e => e.Descripcion)
	                .HasMaxLength(500)
	                .IsUnicode(false)
	                .HasColumnName("descripcion");
	            entity.Property(e => e.FechaCreacion)
	                .HasDefaultValueSql("(getdate())")
	                .HasColumnType("datetime")
	                .HasColumnName("fecha_creacion");
	            entity.Property(e => e.FechaEliminacion)
	                .HasColumnType("datetime")
	                .HasColumnName("fecha_eliminacion");
	            entity.Property(e => e.IdCursomateria).HasColumnName("id_cursomateria");
	            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");
	            entity.Property(e => e.Titulo)
	                .HasMaxLength(200)
	                .IsUnicode(false)
	                .HasColumnName("titulo");
	            entity.Property(e => e.UrlArchivo)
	                .HasMaxLength(255)
	                .IsUnicode(false)
	                .HasColumnName("url_archivo");

	            entity.HasOne(d => d.IdCursomateriaNavigation).WithMany(p => p.Posteos)
	                .HasForeignKey(d => d.IdCursomateria)
	                .HasConstraintName("FK__Posteo__id_curso__0B91BA14");

	            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Posteos)
	                .HasForeignKey(d => d.IdUsuario)
	                .HasConstraintName("FK__Posteo__id_usuar__0A9D95DB");
	        });

	        modelBuilder.Entity<PreceptorCurso>(entity =>
	        {
	            entity.HasKey(e => e.Id).HasName("PK__Precepto__3213E83F5F05A113");

	            entity.ToTable("PreceptorCurso");

	            entity.Property(e => e.Id).HasColumnName("id");
	            entity.Property(e => e.FechaCreacion)
	                .HasDefaultValueSql("(getdate())")
	                .HasColumnType("datetime")
	                .HasColumnName("fecha_creacion");
	            entity.Property(e => e.FechaEliminacion)
	                .HasColumnType("datetime")
	                .HasColumnName("fecha_eliminacion");
	            entity.Property(e => e.IdCurso).HasColumnName("id_curso");
	            entity.Property(e => e.IdPreceptor).HasColumnName("id_preceptor");

	            entity.HasOne(d => d.IdCursoNavigation).WithMany(p => p.PreceptorCursos)
	                .HasForeignKey(d => d.IdCurso)
	                .HasConstraintName("FK__Preceptor__id_cu__75A278F5");

	            entity.HasOne(d => d.IdPreceptorNavigation).WithMany(p => p.PreceptorCursos)
	                .HasForeignKey(d => d.IdPreceptor)
	                .HasConstraintName("FK__Preceptor__id_pr__74AE54BC");
	        });

	        modelBuilder.Entity<ProfesorCursoMateria>(entity =>
	        {
	            entity.HasKey(e => e.Id).HasName("PK__Profesor__3213E83F9B634F5A");

	            entity.Property(e => e.Id).HasColumnName("id");
	            entity.Property(e => e.FechaCreacion)
	                .HasDefaultValueSql("(getdate())")
	                .HasColumnType("datetime")
	                .HasColumnName("fecha_creacion");
	            entity.Property(e => e.FechaEliminacion)
	                .HasColumnType("datetime")
	                .HasColumnName("fecha_eliminacion");
	            entity.Property(e => e.IdCursomateria).HasColumnName("id_cursomateria");
	            entity.Property(e => e.IdProfesor).HasColumnName("id_profesor");

	            entity.HasOne(d => d.IdCursomateriaNavigation).WithMany(p => p.ProfesorCursoMateria)
	                .HasForeignKey(d => d.IdCursomateria)
	                .HasConstraintName("FK__ProfesorC__id_cu__70DDC3D8");

	            entity.HasOne(d => d.IdProfesorNavigation).WithMany(p => p.ProfesorCursoMateria)
	                .HasForeignKey(d => d.IdProfesor)
	                .HasConstraintName("FK__ProfesorC__id_pr__6FE99F9F");
	        });

	        modelBuilder.Entity<Rol>(entity =>
	        {
	            entity.HasKey(e => e.Id).HasName("PK__Rol__3213E83FD489D6A1");

	            entity.ToTable("Rol");

	            entity.Property(e => e.Id).HasColumnName("id");
	            entity.Property(e => e.Descripcion)
	                .HasMaxLength(50)
	                .IsUnicode(false)
	                .HasColumnName("descripcion");
	            entity.Property(e => e.FechaCreacion)
	                .HasDefaultValueSql("(getdate())")
	                .HasColumnType("datetime")
	                .HasColumnName("fecha_creacion");
	            entity.Property(e => e.FechaEliminacion)
	                .HasColumnType("datetime")
	                .HasColumnName("fecha_eliminacion");
	        });

	        modelBuilder.Entity<Usuario>(entity =>
	        {
	            entity.HasKey(e => e.Id).HasName("PK__Usuario__3213E83FC6C4C01D");

	            entity.ToTable("Usuario");

	            entity.Property(e => e.Id).HasColumnName("id");
	            entity.Property(e => e.Apellidos)
	                .HasMaxLength(100)
	                .IsUnicode(false)
	                .HasColumnName("apellidos");
	            entity.Property(e => e.Dni)
	                .HasMaxLength(20)
	                .IsUnicode(false)
	                .HasColumnName("dni");
	            entity.Property(e => e.Email)
	                .HasMaxLength(100)
	                .IsUnicode(false)
	                .HasColumnName("email");
	            entity.Property(e => e.FechaCreacion)
	                .HasDefaultValueSql("(getdate())")
	                .HasColumnType("datetime")
	                .HasColumnName("fecha_creacion");
	            entity.Property(e => e.FechaEliminacion)
	                .HasColumnType("datetime")
	                .HasColumnName("fecha_eliminacion");
	            entity.Property(e => e.IdRol).HasColumnName("id_rol");
	            entity.Property(e => e.Nombres)
	                .HasMaxLength(100)
	                .IsUnicode(false)
	                .HasColumnName("nombres");
	            entity.Property(e => e.Password)
	                .HasMaxLength(255)
	                .IsUnicode(false)
	                .HasColumnName("password");
	            entity.Property(e => e.Telefono)
	                .HasMaxLength(20)
	                .IsUnicode(false)
	                .HasColumnName("telefono");
	            entity.Property(e => e.UrlImagen)
	                .HasMaxLength(255)
	                .IsUnicode(false)
	                .HasColumnName("url_imagen");

	            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.Usuarios)
	                .HasForeignKey(d => d.IdRol)
	                .HasConstraintName("FK__Usuario__id_rol__60A75C0F");
	        });

	        OnModelCreatingPartial(modelBuilder);
	    }

	    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
	}
}
