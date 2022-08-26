using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaLogica
{
    public class LogicaCliente
    {
        private DatosCliente objCapaDato = new DatosCliente();


        public List<Cliente> Listar()
        {
            return objCapaDato.Listar();
        }

        public int Registrar(Cliente obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            // Validamos los campos que no lleven vacios
            if (string.IsNullOrEmpty(obj.Nombres) || string.IsNullOrWhiteSpace(obj.Nombres))
            {
                Mensaje = "El nombre del Cliente es requerido";
            }
            else if (string.IsNullOrEmpty(obj.Apellidos) || string.IsNullOrWhiteSpace(obj.Apellidos))
            {
                Mensaje = "El Apellido del Cliente es requerido";
            }
            else if (string.IsNullOrEmpty(obj.Correo) || string.IsNullOrWhiteSpace(obj.Correo))
            {
                Mensaje = "El Correo del Cliente es requerido";
            }


            if (string.IsNullOrEmpty(Mensaje))
            {

                obj.Clave = LogicaRecursos.ConvertirSha256(obj.Clave);
                return objCapaDato.Registrar(obj, out Mensaje);             

            }
            else
            {
                return 0;
            }
        }

        public bool CambiarClave(int idcliente, string nuevaclave, out string Mensaje)
        {
            return objCapaDato.CambiarClave(idcliente, nuevaclave, out Mensaje);
        }


        // Metodo para reestablecer la clave
        public bool ReestablecerClave(int idcliente, string correo, out string Mensaje)
        {
            Mensaje = string.Empty;
            string nuevaclave = LogicaRecursos.GenerarClave();
            bool resultado = objCapaDato.ReestablecerClave(idcliente, LogicaRecursos.ConvertirSha256(nuevaclave), out Mensaje);

            if (resultado)
            {
                string asunto = "Contraseña Reestablecida";
                string mensaje_correo = "<h3>Su cuenta fue reestablecida correctamente</h3><br><p> Su contraseña para acceder es : !clave!</p>";
                mensaje_correo = mensaje_correo.Replace("!clave!", nuevaclave);

                bool respuesta = LogicaRecursos.EnviarCorreo(correo, asunto, mensaje_correo);

                if (respuesta)
                {

                    return true;
                }
                else
                {
                    Mensaje = "No se pudo enviar el correo";
                    return false;
                }
            }
            else
            {
                Mensaje = "No se pudo reestablecer la contraseña";
                return false;
            }

        }
    }
}
