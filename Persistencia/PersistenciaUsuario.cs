using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EnidadesCompartidas;
using System.Data.SqlClient;
using System.Data;

namespace Persistencia
{
    public class PersistenciaUsuario
    {
        public static Usuario Buscar(string pUsuario)
        {
            string oUsuario, oPassw, oNombre;


            Usuario u= null;
            SqlDataReader oReader;
            SqlConnection oConexion = new SqlConnection(Conexion.STR);
            SqlCommand oComando = new SqlCommand("BuscarUsuario", oConexion);
            oComando.CommandType = CommandType.StoredProcedure;
            oComando.Parameters.AddWithValue("@Usuario", pUsuario);

            try
            {
                oConexion.Open();
                oReader = oComando.ExecuteReader();

                if (oReader.Read())
                {
                    oUsuario = (string)oReader["Usuario"];
                    oPassw = (string)oReader["Passw"];
                    oNombre = (string)oReader["Nombre"];

                    u = new Usuario(oUsuario, oPassw, oNombre);
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
            return u;
        }

        public static int Agregar(Usuario pUsuario)
        {
            SqlConnection oConexion = new SqlConnection(Conexion.STR);
            SqlCommand oComando = new SqlCommand("AgregarUsuario", oConexion);
            oComando.CommandType = CommandType.StoredProcedure;

            oComando.Parameters.AddWithValue("@Usuario", pUsuario.NombreUsuario);
            oComando.Parameters.AddWithValue("@Password", pUsuario.Password);
            oComando.Parameters.AddWithValue("@Nombre", pUsuario.Nombre);

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
                    throw new Exception("Error al agregar Usuario");
                else if (oAfectados == -2)
                    throw new Exception("Ya existe un usuario con ese identificador");
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

        public static void Modificar(Usuario pUsuario)
        {
            SqlConnection oConexion = new SqlConnection(Conexion.STR);
            SqlCommand oComando = new SqlCommand("ModificarUsuario", oConexion);
            oComando.CommandType = CommandType.StoredProcedure;

            oComando.Parameters.AddWithValue("@usuario", pUsuario.NombreUsuario);
            oComando.Parameters.AddWithValue("@nombre", pUsuario.Nombre);
            oComando.Parameters.AddWithValue("@password", pUsuario.Password);

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
                throw new Exception("No existe un usuario con ese identificador");
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

        public static void Eliminar(Usuario pUsuario)
        {
            SqlConnection oConexion = new SqlConnection(Conexion.STR);
            SqlCommand oComando = new SqlCommand("BorrarUsuario", oConexion);
            oComando.CommandType = CommandType.StoredProcedure;

            SqlParameter oUsuario = new SqlParameter("@usuario", pUsuario.NombreUsuario);

            SqlParameter oRetorno = new SqlParameter("@Retorno", SqlDbType.Int);
            oRetorno.Direction = ParameterDirection.ReturnValue;

            oComando.Parameters.Add(oUsuario);
            oComando.Parameters.Add(oRetorno);

            try
            {
                oConexion.Open();
                oComando.ExecuteNonQuery();

                int oAfectados = (int)oComando.Parameters["@Retorno"].Value;
                if (oAfectados == -1)
                    throw new Exception("Error al intentar eliminar el usuario");
                else if (oAfectados == -2)
                    throw new Exception("El usuario iene pronosticos asociados - No se elimina");
                else if (oAfectados == -3)
                    throw new Exception("No existe un usuario con ese identificador");
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
    }
}
