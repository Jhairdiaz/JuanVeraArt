using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CapaEntidad;
using System.Data.SqlClient;
using System.Data;


namespace CapaDatos
{
    public class DatosUbicacion
    {

        public List<Departamento> ObtenerDepartamento()
        {
            List<Departamento> lista = new List<Departamento>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {

                    string query = "select * from DEPARTAMENTO";

                    SqlCommand cmd = new SqlCommand(query, oconexion);
                    cmd.CommandType = CommandType.Text;
                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(
                                new Departamento()
                                {
                                    IdDepartamento = dr["IdDepartamento"].ToString(),
                                    Descripcion = dr["Descripcion"].ToString(),
                                }
                            );
                        }
                    }
                }
            }
            catch (Exception)
            {
                lista = new List<Departamento>();

            }
            return lista;
        }


        public List<Ciudad> ObtenerCiudad(string iddepartamento)
        {
            List<Ciudad> lista = new List<Ciudad>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {

                    string query = "select * from CIUDAD where IdDepartamento = @iddepartamento";

                    SqlCommand cmd = new SqlCommand(query, oconexion);
                    cmd.Parameters.AddWithValue("@iddepartamento", iddepartamento);
                    cmd.CommandType = CommandType.Text;
                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(
                                new Ciudad()
                                {
                                    IdCiudad = dr["IdCiudad"].ToString(),
                                    Descripcion = dr["Descripcion"].ToString(),
                                }
                            );
                        }
                    }
                }
            }
            catch (Exception)
            {
                lista = new List<Ciudad>();


            }
            return lista;
        }

    }


}
