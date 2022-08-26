using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class Venta
    {
        public int IDVENTA { get; set; }
        public int IDCLIENTE { get; set; }
        public int TOTALPRODUCTO { get; set; }
        public decimal MONTOTOTAL { get; set; }
        public string CONTACTO { get; set; }
        public string IDDISTRITO { get; set; }
        public string TELEFONO { get; set; }
        public string DIRECCION { get; set; }
        public string FECHATEXTO { get; set; }
        public string IDTRANSACCION { get; set; }

        public List<DetalleVenta> ODETALLEVENTA { get; set; }
    }
}
