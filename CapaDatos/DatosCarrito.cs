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
    public class DatosCarrito
    {
        // CREAMOS LA FUNCION QUE REGISTRA UN NUEVO PRODUCTO EN EL CARRITO
        public bool ExisteCarrito(int idcliente, int idproducto)
        {
            bool resultado = true;
            
            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {

                    SqlCommand cmd = new SqlCommand("sp_ExisteCarrito", oconexion);
                    cmd.Parameters.AddWithValue("IdCliente", idcliente);
                    cmd.Parameters.AddWithValue("IdProducto", idproducto);                  
                    cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;                    
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();

                    cmd.ExecuteNonQuery();

                    resultado = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
                    
                }

            }
            catch (Exception )
            {
                resultado = false;
                

            }
            return resultado;
        }

        // METODO PARA LA OPERACION DEL CARRITO Y TRANSACION
        public bool OperacionCarrito(int idcliente, int idproducto, bool sumar, out string Mensaje)
        {
            bool resultado = true;

            Mensaje = string.Empty;
            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {

                    SqlCommand cmd = new SqlCommand("sp_OperacionCarrito", oconexion);
                    cmd.Parameters.AddWithValue("IdCliente", idcliente);
                    cmd.Parameters.AddWithValue("IdProducto", idproducto);
                    cmd.Parameters.AddWithValue("Sumar", sumar);                    
                    cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
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

        //METODO PARA VALIDAR LA CANTIDAD DE PRODUCTOS EN EL CARRITO
        public int CantidadEnCarrito(int idcliente)
        {
            int resultado = 0;
            
            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {

                    SqlCommand cmd = new SqlCommand("select count(*) from CARRITO where IdCliente = @idcliente", oconexion);
                    cmd.Parameters.AddWithValue("@idcliente", idcliente);
                    cmd.CommandType = CommandType.Text;
                    oconexion.Open();
                    resultado = Convert.ToInt32(cmd.ExecuteScalar());
                }

            }
            catch (Exception )
            {
                resultado = 0;                
            }
            return resultado;
        }

        public List<Carrito> ListarProducto(int idcliente)
        {
            List<Carrito> lista = new List<Carrito>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {

                    string query = "select * from fn_obtenerCarritoCliente(@idcliente)";

                    SqlCommand cmd = new SqlCommand(query, oconexion);
                    cmd.Parameters.AddWithValue("@idcliente", idcliente);
                    cmd.CommandType = CommandType.Text;

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Carrito()
                            {
                                oProducto = new Producto() { 

                                    IdProducto = Convert.ToInt32(dr["IdProducto"]),                                    
                                    Descripcion = dr["Descripcion"].ToString(),
                                    oCategoria = new Categoria() { Descripcion = dr["Categoria"].ToString() },
                                    Precio = Convert.ToDecimal(dr["Precio"], new CultureInfo("es-CO")),                                    
                                    RutaImagen = dr["RutaImagen"].ToString(),
                                    NombreImagen = dr["NombreImagen"].ToString()
                                    
                                },
                                Cantidad = Convert.ToInt32(dr["Cantidad"])
                            });
                        }
                    }
                }
            }
            catch (Exception)
            {
                lista = new List<Carrito>();

            }
            return lista;
        }


        public bool EliminarCarrito(int idcliente, int idproducto)
        {
            bool resultado = true;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {

                    SqlCommand cmd = new SqlCommand("sp_EliminarCarrito", oconexion);
                    cmd.Parameters.AddWithValue("IdCliente", idcliente);
                    cmd.Parameters.AddWithValue("IdProducto", idproducto);
                    cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();

                    cmd.ExecuteNonQuery();

                    resultado = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
                }
            }
            catch (Exception)
            {
                resultado = false;
            }
            return resultado;
        }
    }
}
