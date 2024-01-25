using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EnidadesCompartidas;
using System.Data.SqlClient;
using System.Data;

namespace Persistencia
{
    public class PersistenciaCiudad
    {
        public static Ciudad Buscar(string pCodCiudad, string pCodPais)
        {
            string oCodCiudad, oNombre;
            Pais oPais;
            
          
            Ciudad c = null;
            SqlDataReader oReader;
            SqlConnection oConexion = new SqlConnection(Conexion.STR);
            //SqlCommand oComando = new SqlCommand("Exec BuscarCiudad " + pCodCiudad, oConexion);
            SqlCommand oComando = new SqlCommand("BuscarCiudad", oConexion);
            oComando.CommandType = CommandType.StoredProcedure;
            oComando.Parameters.AddWithValue("@CodPais", pCodPais);
            oComando.Parameters.AddWithValue("@CodCiudad", pCodCiudad);

            try
            {
                oConexion.Open();
                oReader = oComando.ExecuteReader();

                if (oReader.Read())
                {
                    oCodCiudad = (string)oReader["CodCiudad"];
                    oNombre = (string)oReader["Nombre"];

                    oPais = PersistenciaPais.Buscar(pCodPais);

                    c = new Ciudad(oPais, oCodCiudad, oNombre);
                }

                oReader.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oConexion.Close();
            }
            return c;
        }

        public static int Agregar(Ciudad pCiudad)
        {
            SqlConnection oConexion = new SqlConnection(Conexion.STR);
            SqlCommand oComando = new SqlCommand("AgregarCiudad", oConexion);
            oComando.CommandType = CommandType.StoredProcedure;

            oComando.Parameters.AddWithValue("@CodCiudad", pCiudad.CodCiudad);
            oComando.Parameters.AddWithValue("@CodPais", pCiudad.Pais.CodPais);
            oComando.Parameters.AddWithValue("@Nombre", pCiudad.Nombre);

            SqlParameter oRetorno = new SqlParameter("@Retorno", SqlDbType.Int);
            oRetorno.Direction = ParameterDirection.ReturnValue;
            oComando.Parameters.Add(oRetorno);

            int oAfectados = 0;

            try
            {
                oConexion.Open();
                oComando.ExecuteNonQuery();

                oAfectados = (int)oComando.Parameters["@Retorno"].Value;

                if (oAfectados == -1)
                    throw new Exception("El pais no existe");
                else if (oAfectados == -2)
                    throw new Exception("Ya existe una ciudad con esos identificadores");
                else if (oAfectados == -3)
                    throw new Exception("El cod pais debe ser exactamente de 3 caracteres");
                else if (oAfectados == -4)
                    throw new Exception("El cod ciudad debe ser exactamente de 3 caracteres");
                else if (oAfectados == 0)
                    throw new Exception("Error al agregar Ciudad");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oConexion.Close();
            }
            return oAfectados;
        }

        public static void Modificar(Ciudad pCiudad)
        {
            SqlConnection oConexion = new SqlConnection(Conexion.STR);
            SqlCommand oComando = new SqlCommand("ModificarCiudad", oConexion);
            oComando.CommandType = CommandType.StoredProcedure;

            oComando.Parameters.AddWithValue("@codCiudad", pCiudad.CodCiudad);
            oComando.Parameters.AddWithValue("@codPais", pCiudad.Pais.CodPais);
            oComando.Parameters.AddWithValue("@nombre", pCiudad.Nombre);

            SqlParameter oRetorno = new SqlParameter("@Retorno", SqlDbType.Int);
            oRetorno.Direction = ParameterDirection.ReturnValue;
            oComando.Parameters.Add(oRetorno);

            try
            {
                oConexion.Open();
                oComando.ExecuteNonQuery();
                int oAfectados = (int)oComando.Parameters["@Retorno"].Value;

                if (oAfectados == -1)
                    throw new Exception("Error - No se modifica");
                else if (oAfectados == -2)
                    throw new Exception("No existe una ciudad con esos identificadores");
            }

            catch (Exception ex)
            {
                throw ex;
            }

            finally
            {
                oConexion.Close();
            }
        }

        public static void Eliminar(Ciudad pCiudad)
        {
            SqlConnection oConexion = new SqlConnection(Conexion.STR);
            SqlCommand oComando = new SqlCommand("EliminarCiudad", oConexion);
            oComando.CommandType = CommandType.StoredProcedure;

            SqlParameter oCodPais = new SqlParameter("@codPais", pCiudad.Pais.CodPais);
            SqlParameter oCodCiudad = new SqlParameter("@codCiudad", pCiudad.CodCiudad);

            SqlParameter oRetorno = new SqlParameter("@Retorno", SqlDbType.Int);
            oRetorno.Direction = ParameterDirection.ReturnValue;

            oComando.Parameters.Add(oCodPais);
            oComando.Parameters.Add(oCodCiudad);
            oComando.Parameters.Add(oRetorno);

            try
            {
                oConexion.Open();
                oComando.ExecuteNonQuery();

                int oAfectados = (int)oComando.Parameters["@Retorno"].Value;

                if (oAfectados == -1)
                    throw new Exception("Error al intentar eliminar la ciudad");
                else if (oAfectados == -2)
                    throw new Exception("No existe una ciudad con esos identificadores");
            }

            catch (Exception ex)
            {
                throw ex;
            }

            finally
            {
                oConexion.Close();
            }
        }

        public static List<Ciudad> ListarCiudadesPorPais(Pais p)
        {
            string oCodPais, oCodCiudad, oNombre;

            List<Ciudad> oListaCiudades = new List<Ciudad>();

            SqlDataReader oReader;
            SqlConnection oConexion = new SqlConnection(Conexion.STR);
            SqlCommand oComando = new SqlCommand("ListarCiudadesPorPais", oConexion);
            oComando.CommandType = CommandType.StoredProcedure;
            oComando.Parameters.AddWithValue("@codPais", p.CodPais);

            try
            {
                oConexion.Open();
                oReader = oComando.ExecuteReader();

                while (oReader.Read())
                {
                    oCodPais = (string)oReader["CodPais"];
                    oCodCiudad = (string)oReader["CodCiudad"];
                    oNombre = (string)oReader["Nombre"];

                    Ciudad c = new Ciudad(p, oCodCiudad, oNombre);
                    oListaCiudades.Add(c);
                }
                oReader.Close();
            }

            catch (Exception ex)
            {
                throw ex;
            }

            finally
            {
                oConexion.Close();
            }

            return oListaCiudades;
        }
    }
}
