﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class Portafolio
    {
        public int IdPortafolio { get; set; }      
        public string NombreImagen { get; set; }
        public string RutaImagen { get; set; }
        public bool Activo { get; set; }
        public string Descripcion { get; set; }

        public string Base64 { get; set; }
        public string Extension { get; set; }
    }
}
