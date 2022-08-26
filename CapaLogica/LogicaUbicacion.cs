using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CapaDatos;
using CapaEntidad;

namespace CapaLogica
{
    public class LogicaUbicacion
    {
        private DatosUbicacion objCapaDato = new DatosUbicacion();

        public List<Departamento> ObtenerDepartamento()
        {
            return objCapaDato.ObtenerDepartamento();
        }

        public List<Ciudad> ObtenerCiudad(string iddepartamento)
        {
            return objCapaDato.ObtenerCiudad(iddepartamento);
        }

    }
}
