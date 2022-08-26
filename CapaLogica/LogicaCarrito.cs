using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CapaDatos;
using CapaEntidad;


namespace CapaLogica
{
    public class LogicaCarrito
    {
        private DatosCarrito objCapaDato = new DatosCarrito();

        public bool ExisteCarrito(int idcliente, int idproducto)
        {
            return objCapaDato.ExisteCarrito(idcliente, idproducto);
        }

        public bool OperacionCarrito(int idcliente, int idproducto, bool sumar, out string Mensaje)
        {
            return objCapaDato.OperacionCarrito(idcliente, idproducto, sumar, out Mensaje);
        }

        public int CantidadEnCarrito(int idcliente)
        {
            return objCapaDato.CantidadEnCarrito(idcliente); 
        }

        public List<Carrito> ListarProducto(int idcliente)
        {
            return objCapaDato.ListarProducto(idcliente);
        }

        public bool EliminarCarrito(int idcliente, int idproducto)
        {
            return objCapaDato.EliminarCarrito(idcliente, idproducto);
        }
    }
}
