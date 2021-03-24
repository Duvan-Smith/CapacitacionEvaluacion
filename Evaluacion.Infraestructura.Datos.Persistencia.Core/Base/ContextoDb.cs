using Evaluacion.Dominio.Core.Base;
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
        public virtual DbSet<AreaEntity> Area { get; set; }
        public virtual DbSet<TipoDocumentoEntity> TipoDocumento { get; set; }
        public virtual DbSet<PersonaEntity> Persona { get; set; }
        public virtual DbSet<EmpleadoEntity> Empleado { get; set; }
        public virtual DbSet<ProveedorEntity> Proveedor { get; set; }
        #endregion
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
