﻿using Evaluacion.Dominio.Core.Especificas.Personas;

namespace Evaluacion.Dominio.Core.Especificas.Proveedores
{
    public class ProveedorEntity : PersonaBase
    {
        public override TipoPersona TipoPersona => TipoPersona.Juridico;
    }
}
