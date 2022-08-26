using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Configuration;
using System.Data;

namespace CapaDatos
{
    public class Conexion
    {
        public static string cn = ConfigurationManager.ConnectionStrings["conexion"].ToString();
    }
}
