using Evaluacion.Dominio.Core.Base;
using Evaluacion.Dominio.Core.Especificas.Clientes;
using Evaluacion.Dominio.Core.Especificas.Empleados;
using Evaluacion.Dominio.Core.Especificas.Personas;
using Evaluacion.Dominio.Core.Especificas.Proveedores;
using Evaluacion.Dominio.Core.Genericas.Areas;
using Evaluacion.Dominio.Core.Genericas.TipoDocumentos;
using Evaluacion.Infraestructura.Datos.Persistencia.Core.Base.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Linq;

namespace Evaluacion.Infraestructura.Datos.Persistencia.Core.Base
{
    public class ContextoDb : DbContext, IContextDb
    {
        private readonly DbSettings _settings;
        #region Tablas BD
        public virtual DbSet<AreaEntity> Areas { get; set; }
        public virtual DbSet<TipoDocumentoEntity> TipoDocumentos { get; set; }
        public virtual DbSet<PersonaEntity> Personas { get; set; }
        public virtual DbSet<EmpleadoEntity> Empleados { get; set; }
        public virtual DbSet<ProveedorEntity> Proveedores { get; set; }
        public virtual DbSet<ClienteEntity> Clientes { get; set; }
        #endregion
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EmpleadoEntity>()
                .HasOne(p => p.AreaEntity)
                .WithMany(b => b.Empleado);

            modelBuilder.Entity<AreaEntity>()
                .HasOne(p => p.PersonaEntity)
                .WithMany(b => b.Area);

            modelBuilder.Entity<PersonaEntity>()
                .HasOne(p => p.EmpleadoEntity)
                .WithMany(b => b.PersonaEmpleado);

            modelBuilder.Entity<PersonaEntity>()
                .HasOne(p => p.ProveedorEntity)
                .WithMany(b => b.PersonaProveedor);

            modelBuilder.Entity<PersonaEntity>()
                .HasOne(p => p.TipoDocumentoEntity)
                .WithMany(b => b.PersonaTipoDocumento);

            modelBuilder.Entity<PersonaEntity>()
                .HasOne(p => p.ClienteEntity)
                .WithMany(b => b.PersonaCliente);

        }
        public ContextoDb(IOptions<DbSettings> settings) =>
           _settings = settings.Value;
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
            optionsBuilder.UseSqlServer(_settings.ConnectionString);
        public int Commit() => base.SaveChanges();
        public void Rollback() =>
            base.ChangeTracker
            .Entries()
            .Where(e => e.Entity != null)
            .ToList()
            .ForEach(e => e.State = EntityState.Detached);
        public new DbSet<T> Set<T>() where T : EntidadBase => base.Set<T>();
        public void SetAttach<T>(T item) where T : EntidadBase
        {
            if (Entry(item).State == EntityState.Detached)
                base.Set<T>().Attach(item);
        }
        public void SetDeatached<T>(T item) where T : EntidadBase => Entry(item).State = EntityState.Detached;
        public void SetModified<T>(T item) where T : EntidadBase => Entry(item).State = EntityState.Modified;
    }
}
