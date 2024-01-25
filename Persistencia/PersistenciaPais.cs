using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EnidadesCompartidas;
using System.Data.SqlClient;
using System.Data;

namespace Persistencia
{
    public class PersistenciaPais
    {
        public static Pais Buscar(string pCodPais)
        {
            string oCodPais, oNombre;


            Pais p = null;
            SqlDataReader oReader;
            SqlConnection oConexion = new SqlConnection(Conexion.STR);
            SqlCommand oComando = new SqlCommand("BuscarPais", oConexion);
            oComando.CommandType = CommandType.StoredProcedure;
            oComando.Parameters.AddWithValue("@CodPais", pCodPais);

            try
            {
                oConexion.Open();
                oReader = oComando.ExecuteReader();
                if (oReader.Read())
                {

                    oCodPais = (string)oReader["CodPais"];
                    oNombre = (string)oReader["Nombre"];
                    p = new Pais(oCodPais, oNombre);
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
            return p;
        }

        public static int Agregar(Pais pPais)
        {
            SqlConnection oConexion = new SqlConnection(Conexion.STR);
            SqlCommand oComando = new SqlCommand("AgregarPais", oConexion);
            oComando.CommandType = CommandType.StoredProcedure;

            oComando.Parameters.AddWithValue("@CodPais", pPais.CodPais);
            oComando.Parameters.AddWithValue("@Nombre", pPais.Nombre);

            SqlParameter oRetorno = new SqlParameter("@Retorno", SqlDbType.Int);
            oRetorno.Direction = ParameterDirection.ReturnValue;
            oComando.Parameters.Add(oRetorno);

            int oAfectados = 0;

            try
            {
                oConexion.Open();
                oComando.ExecuteNonQuery();
                oAfectados = (int)oComando.Parameters["@Retorno"].Value;

                if (oAfectados == 0)
                    throw new Exception("Error al agregar pais");
                else if (oAfectados == -2)
                    throw new Exception("Ya existe un pais con ese codigo");
                else if (oAfectados == -3)
                    throw new Exception("El codigo pais debe ser de 3 letras");
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

        public static void Modificar(Pais pPais)
        {
            SqlConnection oConexion = new SqlConnection(Conexion.STR);
            SqlCommand oComando = new SqlCommand("ModificarPais", oConexion);
            oComando.CommandType = CommandType.StoredProcedure;

            oComando.Parameters.AddWithValue("@CodPais", pPais.CodPais);
            oComando.Parameters.AddWithValue("@Nombre", pPais.Nombre);

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
                    throw new Exception("No existe un pais con ese codigo");
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

        public static void Eliminar(Pais pPais)
        {
            SqlConnection oConexion = new SqlConnection(Conexion.STR);
            SqlCommand oComando = new SqlCommand("EliminarPais", oConexion);
            oComando.CommandType = CommandType.StoredProcedure;

            SqlParameter oCodPais = new SqlParameter("@codPais", pPais.CodPais);

            SqlParameter oRetorno = new SqlParameter("@Retorno", SqlDbType.Int);
            oRetorno.Direction = ParameterDirection.ReturnValue;

            oComando.Parameters.Add(oCodPais);
            oComando.Parameters.Add(oRetorno);

            try
            {
                oConexion.Open();
                oComando.ExecuteNonQuery();

                int oAfectados = (int)oComando.Parameters["@Retorno"].Value;

                if (oAfectados == -1)
                    throw new Exception("Error al intentar eliminar el pais");
                else if (oAfectados == -2)
                    throw new Exception("No existe un pais con ese codigo");
                else if (oAfectados == -3)
                    throw new Exception("El pais tiene pronosticos asociados - No se elimina");
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

        public static List<Pais> ListarPaises()
        {
            string oCodPais, oNombre;

            List<Pais> oListaPaises = new List<Pais>();

            SqlDataReader oReader;
            SqlConnection oConexion = new SqlConnection(Conexion.STR);
            //SqlCommand oComando = new SqlCommand("Exec ListarPronosticosCiudad " + pCiudad.CodCiudad + pCiudad.Pais.CodPais, oConexion); //REVISAR
            SqlCommand oComando = new SqlCommand("ListarPaises", oConexion);
            oComando.CommandType = CommandType.StoredProcedure;

            try
            {
                oConexion.Open();
                oReader = oComando.ExecuteReader();

                while (oReader.Read())
                {
                    oCodPais = (string)oReader["CodPais"];
                    oNombre = (string)oReader["Nombre"];

                    Pais p = new Pais(oCodPais,oNombre);
                    oListaPaises.Add(p);
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

            return oListaPaises;
        }
    }
}
