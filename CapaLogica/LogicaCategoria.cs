using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CapaDatos;
using CapaEntidad;

namespace CapaLogica
{
    public class LogicaCategoria
    {
        private DatosCategoria objCapaDato = new DatosCategoria();

        public List<Categoria> Listar()
        {
            return objCapaDato.Listar();
        }

        public int Registrar(Categoria obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            // Validamos los campos que no lleven vacios
            if (string.IsNullOrEmpty(obj.Descripcion) || string.IsNullOrWhiteSpace(obj.Descripcion))
            {
                Mensaje = "La descripcion de la categoria no puede ser vacia";
            }


            if (string.IsNullOrEmpty(Mensaje))
            {
                
                return objCapaDato.Registrar(obj, out Mensaje);
            }
            else
            {              
                return 0;
            }
            
        }

        public bool Editar(Categoria obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            // Validamos los campos que no lleven vacios
            if (string.IsNullOrEmpty(obj.Descripcion) || string.IsNullOrWhiteSpace(obj.Descripcion))
            {
                Mensaje = "La descripcion de la categoria no puede ser vacia";
            }


            if (string.IsNullOrEmpty(Mensaje))
            {

                return objCapaDato.Editar(obj, out Mensaje);
            }
            else
            {
                return false;
            }

        }

        public bool Eliminar(int id, out string Mensaje)
        {
            return objCapaDato.Eliminar(id, out Mensaje);
        }

    }
}
