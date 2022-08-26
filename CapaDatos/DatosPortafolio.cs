using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CapaEntidad;
using System.Data.SqlClient;
using System.Data;
using System.Globalization;


namespace CapaDatos
{
    public class DatosPortafolio
    {
        //Listar Productos
        public List<Portafolio> Listar()
        {
            List<Portafolio> lista = new List<Portafolio>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    string query = "SELECT IdPortafolio, RutaImagen, NombreImagen, Activo, Descripcion FROM PORTAFOLIO";

                    SqlCommand cmd = new SqlCommand(query, oconexion);
                    
                    cmd.CommandType = CommandType.Text;
                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Portafolio()
                            {
                                IdPortafolio = Convert.ToInt32(dr["IdPortafolio"]),                               
                                RutaImagen = dr["RutaImagen"].ToString(),
                                NombreImagen = dr["NombreImagen"].ToString(),
                                Activo = Convert.ToBoolean(dr["Activo"]),
                                Descripcion = dr["Descripcion"].ToString(),
                            });
                        }
                    }
                }
            }
            catch (Exception)
            {
                lista = new List<Portafolio>();

            }
            return lista;
        }

        // CREAMOS LA FUNCION QUE REGISTRA Una imagen del portafolio
        public int Registrar(Portafolio obj, out string Mensaje)
        {
            int idautogenerado = 0;
            Mensaje = string.Empty;
            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {

                    SqlCommand cmd = new SqlCommand("sp_RegistrarPortafolio", oconexion);                    
                    cmd.Parameters.AddWithValue("Activo", obj.Activo);
                    cmd.Parameters.AddWithValue("Descripcion", obj.Descripcion);
                    cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();

                    cmd.ExecuteNonQuery();

                    idautogenerado = Convert.ToInt32(cmd.Parameters["Resultado"].Value);
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                }

            }
            catch (Exception ex)
            {
                idautogenerado = 0;
                Mensaje = ex.Message;

            }
            return idautogenerado;
        }

        // CREAMOS LA FUNCION QUE EDITA portafolio
        public bool Editar(Portafolio obj, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;
            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {

                    SqlCommand cmd = new SqlCommand("sp_EditarPortafolio", oconexion);
                    cmd.Parameters.AddWithValue("IdPortafolio", obj.IdPortafolio);                  
                    cmd.Parameters.AddWithValue("Activo", obj.Activo);
                    cmd.Parameters.AddWithValue("Descripcion", obj.Descripcion);
                    cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();

                    cmd.ExecuteNonQuery();

                    resultado = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                }

            }
            catch (Exception ex)
            {
                resultado = false;
                Mensaje = ex.Message;

            }
            return resultado;
        }

        //GUARDAR LOS DATOS DE LA IMAGEN
        public bool GuardarDatosImagen(Portafolio obj, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {

                    string query = "update PORTAFOLIO set RutaImagen = @rutaImagen, NombreImagen = @nombreImagen, Descripcion = @descripcion where IdPortafolio = @IdPortafolio";

                    SqlCommand cmd = new SqlCommand(query, oconexion);
                    cmd.Parameters.AddWithValue("@rutaImagen", obj.RutaImagen);
                    cmd.Parameters.AddWithValue("@nombreImagen", obj.NombreImagen);
                    cmd.Parameters.AddWithValue("@descripcion", obj.Descripcion);
                    cmd.Parameters.AddWithValue("@IdPortafolio", obj.IdPortafolio);
                    cmd.CommandType = CommandType.Text;

                    oconexion.Open();

                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        resultado = true;
                    }
                    else
                    {
                        Mensaje = " No se pudo actualizar la imagen";
                    }
                }
            }
            catch (Exception ex)
            {
                resultado = false;
                Mensaje = ex.Message;
            }
            return resultado;
        }


        // CREAMOS LA FUNCION QUE ELIMINA imagen portafolio
        public bool Eliminar(int id, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;
            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {

                    SqlCommand cmd = new SqlCommand("sp_EliminarPortafolio", oconexion);
                    cmd.Parameters.AddWithValue("IdPortafolio", id);
                    cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;
                    oconexion.Open();
                    cmd.ExecuteNonQuery();

                    resultado = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();

                }

            }
            catch (Exception ex)
            {
                resultado = false;
                Mensaje = ex.Message;

            }
            return resultado;
        }
    }

}

