using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CapaDatos;
using CapaEntidad;


namespace CapaLogica
{
    public class LogicaPortafolio
    {
        private DatosPortafolio objCapaDato = new DatosPortafolio();

        public List<Portafolio> Listar()
        {
            return objCapaDato.Listar();
        }

        public int Registrar(Portafolio obj, out string Mensaje)
        {
            
                return objCapaDato.Registrar(obj, out Mensaje);            

        }

        public bool Editar(Portafolio obj, out string Mensaje)
        {
            Mensaje = string.Empty;


            if (string.IsNullOrEmpty(Mensaje))
            {

                return objCapaDato.Editar(obj, out Mensaje);
            }
            else
            {
                return false;
            }

        }

        public bool GuardarDatosImagen(Portafolio obj, out string Mensaje)
        {
            return objCapaDato.GuardarDatosImagen(obj, out Mensaje);
        }

        public bool Eliminar(int id, out string Mensaje)
        {
            return objCapaDato.Eliminar(id, out Mensaje);
        }

    }
}
