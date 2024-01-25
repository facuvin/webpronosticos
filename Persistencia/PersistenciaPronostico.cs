using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EnidadesCompartidas;
using System.Data.SqlClient;
using System.Data;

namespace Persistencia
{
    public class PersistenciaPronostico
    {
        public static void Agregar(Pronostico pPronostico)
        {
            SqlConnection oConexion = new SqlConnection(Conexion.STR);
            SqlCommand oComando = new SqlCommand("AgregarPronostico", oConexion);
            oComando.CommandType = CommandType.StoredProcedure;

            oComando.Parameters.AddWithValue("@fechaYhora", pPronostico.FechayHora);
            oComando.Parameters.AddWithValue("@TempMax", pPronostico.TempMax);
            oComando.Parameters.AddWithValue("@tempMin", pPronostico.TempMin);
            oComando.Parameters.AddWithValue("@viento", pPronostico.Viento);
            oComando.Parameters.AddWithValue("@probLluvia", pPronostico.ProbLluvias);
            oComando.Parameters.AddWithValue("@tipoCielo", pPronostico.TipoCielo);
            oComando.Parameters.AddWithValue("@usuario", pPronostico.Usuario.NombreUsuario);
            oComando.Parameters.AddWithValue("@CodCiudad", pPronostico.Ciudad.CodCiudad);
            oComando.Parameters.AddWithValue("@CodPais", pPronostico.Ciudad.Pais.CodPais);

            SqlParameter oRetorno = new SqlParameter("@Retorno", SqlDbType.Int);
            oRetorno.Direction = ParameterDirection.ReturnValue;
            oComando.Parameters.Add(oRetorno);

            try
            {
                oConexion.Open();
                oComando.ExecuteNonQuery();
                int oAfectados = (int)oComando.Parameters["@Retorno"].Value;

                if (oAfectados == -2)
                    throw new Exception("La ciudad no existe");
                else if (oAfectados == -1)
                    throw new Exception("Error");
                else if (oAfectados == -3)
                    throw new Exception("El usuario no existe");
                else if (oAfectados == -4)
                    throw new Exception("Ya hay un pronostico para esa fecha y a esa hora, en esa ciudad");
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

        public static List<Pronostico> ListarPronosticosPorDia(DateTime pFecha)
        {
            int oInterno, oTempMax, oTempMin, oViento, oProbLluvia;
            string oNombUsuario, oCodCiudad, oCodPais, oTipoCielo;
            DateTime oFechaYHora;
            Usuario oUsuario;
            Ciudad oCiudad;

            List<Pronostico> oListaPronosticos = new List<Pronostico>();

            SqlDataReader oReader;
            SqlConnection oConexion = new SqlConnection(Conexion.STR);
            //SqlCommand oComando = new SqlCommand("Exec ListarPronosticosPorDia "+ pFecha, oConexion);
            SqlCommand oComando = new SqlCommand("ListarPronosticosPorDia", oConexion);
            oComando.CommandType = CommandType.StoredProcedure;
            oComando.Parameters.AddWithValue("@fecha", pFecha);

            try
            {
                oConexion.Open();
                oReader = oComando.ExecuteReader();

                while (oReader.Read())
                {
                    oInterno = (int)oReader["Interno"];
                    oTempMax = (int)oReader["TempMax"];
                    oTempMin = (int)oReader["TempMin"];
                    oViento = (int)oReader["Viento"];
                    oProbLluvia = (int)oReader["ProbLluvia"];
                    oTipoCielo = (string)oReader["TipoCielo"];
                    oNombUsuario = (string)oReader["Usuario"];
                    oCodCiudad = (string)oReader["CodCiudad"];
                    oCodPais = (string)oReader["CodPais"];
                    oFechaYHora = Convert.ToDateTime(oReader["FechaYHora"]);

                    oUsuario = PersistenciaUsuario.Buscar(oNombUsuario);

                    oCiudad = PersistenciaCiudad.Buscar(oCodCiudad, oCodPais);


                    Pronostico p = new Pronostico(oInterno, oFechaYHora, oTempMax, oTempMin, oViento, oProbLluvia, oTipoCielo, oUsuario, oCiudad);
                    oListaPronosticos.Add(p);
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

            return oListaPronosticos;
        }

        public static List<Pronostico> ListarPronosticosPorCiudad(Ciudad pCiudad)
        {
            int oInterno, oTempMax, oTempMin, oViento, oProbLluvia;
            string oNombUsuario, oCodCiudad, oCodPais, oTipoCielo;
            DateTime oFechaYHora;
            Usuario oUsuario;
            Ciudad oCiudad;

            List<Pronostico> oListaPronosticos = new List<Pronostico>();

            SqlDataReader oReader;
            SqlConnection oConexion = new SqlConnection(Conexion.STR);
            //SqlCommand oComando = new SqlCommand("Exec ListarPronosticosCiudad "+ pCiudad.CodCiudad + pCiudad.Pais.CodPais, oConexion); //REVISAR
            SqlCommand oComando = new SqlCommand("ListarPronosticosCiudad", oConexion);
            oComando.CommandType = CommandType.StoredProcedure;
            oComando.Parameters.AddWithValue("@codPais", pCiudad.Pais.CodPais);
            oComando.Parameters.AddWithValue("@codCiudad", pCiudad.CodCiudad);


            try
            {
                oConexion.Open();
                oReader = oComando.ExecuteReader();

                while (oReader.Read())
                {
                    oInterno = (int)oReader["Interno"];
                    oTempMax = (int)oReader["TempMax"];
                    oTempMin = (int)oReader["TempMin"];
                    oViento = (int)oReader["Viento"];
                    oTipoCielo = (string)oReader["TipoCielo"];
                    oProbLluvia = (int)oReader["ProbLluvia"];
                    oNombUsuario = (string)oReader["Usuario"];
                    oCodCiudad = (string)oReader["CodCiudad"];
                    oCodPais = (string)oReader["CodPais"];
                    oFechaYHora = Convert.ToDateTime(oReader["FechaYHora"]);

                    oUsuario = PersistenciaUsuario.Buscar(oNombUsuario);

                    oCiudad = PersistenciaCiudad.Buscar(oCodCiudad, oCodPais);


                    Pronostico p = new Pronostico(oInterno, oFechaYHora, oTempMax, oTempMin, oViento, oProbLluvia, oTipoCielo.Trim().ToUpper(), oUsuario, oCiudad); 
                    oListaPronosticos.Add(p);
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

            return oListaPronosticos;
        }

        public static List<Pronostico> ListarPronosticos()
        {
            int oInterno, oTempMax, oTempMin, oViento, oProbLluvia;
            string oNombUsuario, oCodCiudad, oCodPais, oTipoCielo;
            DateTime oFechaYHora;
            Usuario oUsuario;
            Ciudad oCiudad;

            List<Pronostico> oListaPronosticos = new List<Pronostico>();

            SqlDataReader oReader;
            SqlConnection oConexion = new SqlConnection(Conexion.STR);
            SqlCommand oComando = new SqlCommand("Exec ListarPronosticos", oConexion); //REVISAR

            try
            {
                oConexion.Open();
                oReader = oComando.ExecuteReader();

                while (oReader.Read())
                {
                    oInterno = (int)oReader["Interno"];
                    oTempMax = (int)oReader["TempMax"];
                    oTempMin = (int)oReader["TempMin"];
                    oViento = (int)oReader["Viento"];
                    oProbLluvia = (int)oReader["ProbLluvia"];
                    oTipoCielo = (string)oReader["TipoCielo"];
                    oNombUsuario = (string)oReader["Usuario"];
                    oCodCiudad = (string)oReader["CodCiudad"];
                    oCodPais = (string)oReader["CodPais"];
                    oFechaYHora = Convert.ToDateTime(oReader["FechaYHora"]);

                    oUsuario = PersistenciaUsuario.Buscar(oNombUsuario);

                    oCiudad = PersistenciaCiudad.Buscar(oCodCiudad, oCodPais);


                    Pronostico p = new Pronostico(oInterno, oFechaYHora, oTempMax, oTempMin, oViento, oProbLluvia, oTipoCielo, oUsuario, oCiudad);
                    oListaPronosticos.Add(p);
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

            return oListaPronosticos;
        }
    }
}
