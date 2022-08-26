using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

using System.Net.Mail;
using System.Net;
using System.IO;


namespace CapaLogica
{
    public class LogicaRecursos
    {
        //Metodo que devuelve la clave
        public static string GenerarClave()
        {
            string clave = Guid.NewGuid().ToString("N").Substring(0, 6);
            return clave;
        }


        //Encriptacion de texto en SHA256
        public static string ConvertirSha256(string texto)
        {
            StringBuilder sb = new StringBuilder();
            //USAR LA REFERENCIA DE "System.Security.Cryptography"
            using (SHA256 hast = SHA256Managed.Create())
            {
                Encoding enc = Encoding.UTF8;
                byte[] result = hast.ComputeHash(enc.GetBytes(texto));

                foreach (byte b in result)
                    sb.Append(b.ToString("x2"));
            }

            return sb.ToString();
        }


        //Metodo para enviar un correo al usuario
        public static bool EnviarCorreo(string correo, string asunto, string mensaje)
        {
            bool resultado = false;

            try
            {
                MailMessage mail = new MailMessage();
                mail.To.Add(correo);
                mail.From = new MailAddress("badloverjv@gmail.com");
                mail.Subject = asunto;
                mail.Body = mensaje;
                mail.IsBodyHtml = true;

                var smtp = new SmtpClient()
                {
                    Credentials = new NetworkCredential("badloverjv@gmail.com", "phfuflxdvltzntbs"),
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true

                };
                smtp.Send(mail);
                resultado = true;
            }
            catch (Exception) 
            {
                resultado = false;

            }

            return resultado;
        }


        //Metodo para convertir la imagen a base64
        public static string ConvertirBase64(string ruta, out bool conversion)
        {
            string textoBase64 = string.Empty;
            conversion = true;

            try
            {
                byte[] bytes = File.ReadAllBytes(ruta);
                textoBase64 = Convert.ToBase64String(bytes);
            }
            catch (Exception)
            {
                conversion = false;
                
            }

            return textoBase64;
        }


    }
}
