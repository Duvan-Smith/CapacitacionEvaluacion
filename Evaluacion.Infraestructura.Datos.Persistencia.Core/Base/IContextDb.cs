using Evaluacion.Dominio.Core.Base;
using Evaluacion.Dominio.Core.Especificas.Empleados;
using Evaluacion.Dominio.Core.Especificas.Personas;
using Evaluacion.Dominio.Core.Especificas.Proveedores;
using Evaluacion.Dominio.Core.Genericas.Areas;
using Evaluacion.Dominio.Core.Genericas.TipoDocumentos;
using Microsoft.EntityFrameworkCore;
using System;

namespace Evaluacion.Infraestructura.Datos.Persistencia.Core.Base
{
    public interface IContextDb : IUnitOfWork, IDisposable
    {
        DbSet<AreaEntity> Areas { get; }
        DbSet<TipoDocumentoEntity> TipoDocumentos { get; }
        DbSet<PersonaEntity> Personas { get; }
        DbSet<EmpleadoEntity> Empleados { get; }
        DbSet<ProveedorEntity> Proveedores { get; }
    }
}
