﻿using Evaluacion.Aplicacion.Core.AdministracionPersonas.Genericas.Areas.Services;
using Evaluacion.Aplicacion.Core.AdministracionPersonas.Genericas.TipoDocumentos.Services;
using Evaluacion.Aplicacion.Core.Mapper.Configuration;
using Evaluacion.Infraestructura.Datos.Persistencia.Core.Base.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Evaluacion.Aplicacion.Core.AdministracionPersonas.Genericas.Configuration
{
    public static class GenericasConfigurator
    {
        public static void ConfigureGenericasService(this IServiceCollection services, DbSettings settings)
        {
            services.TryAddTransient<IAreaService, AreaService>();
            services.TryAddTransient<ITipoDocumentoService, TipoDocumentoService>();

            services.ConfigureMapper();
            services.ConfigureBaseRepository(settings);
        }
    }
}