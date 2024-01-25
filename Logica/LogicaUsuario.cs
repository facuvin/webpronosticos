using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EnidadesCompartidas;
using Persistencia;

namespace Logica
{
    public class LogicaUsuario
    {
        public static void Agregar(Usuario pUsuario)
        {
            PersistenciaUsuario.Agregar(pUsuario);
        }

        public static void Modificar(Usuario pUsuario)
        {
            PersistenciaUsuario.Modificar(pUsuario);
        }

        public static void Eliminar(Usuario pUsuario)
        {
            PersistenciaUsuario.Eliminar(pUsuario);
        }

        public static Usuario Buscar(string pUsuario)
        {
            Usuario u = null;
            u = (Usuario)PersistenciaUsuario.Buscar(pUsuario);
            return u;
        }
    }
}
