using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class DetalleVenta
    {
        public int IDDETALLEVENTA { get; set; }
        public int IDVENTA { get; set; }
        public Producto OPRODUCTO { get; set; }
        public int CANTIDAD { get; set; }
        public decimal TOTAL { get; set; }
        public string IDTRANSACCION { get; set; }
    }
}
