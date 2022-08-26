using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CapaDatos;
using CapaEntidad;

namespace CapaLogica
{
    public class LogicaUsuario
    {
        private DatosUsuario objCapaDato = new DatosUsuario();

        public List<Usuario> Listar()
        {
            return objCapaDato.Listar();
        }

        public int Registrar(Usuario obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            // Validamos los campos que no lleven vacios
            if (string.IsNullOrEmpty(obj.Nombres) || string.IsNullOrWhiteSpace(obj.Nombres))
            {
                Mensaje = "El nombre del usuario es requerido";
            }
            else if (string.IsNullOrEmpty(obj.Apellidos) || string.IsNullOrWhiteSpace(obj.Apellidos))
            {
                Mensaje = "El Apellido del usuario es requerido";
            }
            else if (string.IsNullOrEmpty(obj.Correo) || string.IsNullOrWhiteSpace(obj.Correo))
            {
                Mensaje = "El Correo del usuario es requerido";
            }


            if (string.IsNullOrEmpty(Mensaje))
            {

                string clave = LogicaRecursos.GenerarClave();
                string asunto = "Creacion de Cuenta";
                string mensaje_correo = "<h3>Su cuenta fue creada correctamente</h3><br><p> Su contraseña para acceder es : !clave!</p>";
                mensaje_correo = mensaje_correo.Replace("!clave!", clave);

                bool respuesta = LogicaRecursos.EnviarCorreo(obj.Correo, asunto, mensaje_correo);

                if (respuesta)
                {
                    obj.Clave = LogicaRecursos.ConvertirSha256(clave);
                    return objCapaDato.Registrar(obj, out Mensaje);
                }
                else
                {
                    Mensaje = "No se puede enviar el correo";
                    return 0;
                }                

                
            }else
            {
                return 0;
            }            
        }

        public bool Editar(Usuario obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            // Validamos los campos que no lleven vacios
            if (string.IsNullOrEmpty(obj.Nombres) || string.IsNullOrWhiteSpace(obj.Nombres))
            {
                Mensaje = "El nombre del usuario es requerido";
            }
            else if (string.IsNullOrEmpty(obj.Apellidos) || string.IsNullOrWhiteSpace(obj.Apellidos))
            {
                Mensaje = "El Apellido del usuario es requerido";
            }
            else if (string.IsNullOrEmpty(obj.Correo) || string.IsNullOrWhiteSpace(obj.Correo))
            {
                Mensaje = "El Correo del usuario es requerido";
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

        public bool CambiarClave(int idusuario, string nuevaclave, out string Mensaje)
        {
            return objCapaDato.CambiarClave(idusuario, nuevaclave, out Mensaje);
        }


        // Metodo para reestablecer la clave
        public bool ReestablecerClave(int idusuario, string correo, out string Mensaje)
        {
            Mensaje = string.Empty;
            string nuevaclave = LogicaRecursos.GenerarClave();
            bool resultado = objCapaDato.ReestablecerClave(idusuario, LogicaRecursos.ConvertirSha256(nuevaclave), out Mensaje);

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
