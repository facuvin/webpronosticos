using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EnidadesCompartidas;
using Persistencia;

namespace Logica
{
    public class LogicaCiudad
    {
        public static void Agregar(Ciudad pCiudad)
        {
            PersistenciaCiudad.Agregar(pCiudad);
        }

        public static void Modificar(Ciudad pCiudad)
        {
            PersistenciaCiudad.Modificar(pCiudad);
        }

        public static void Eliminar(Ciudad pCiudad)
        {
            PersistenciaCiudad.Eliminar(pCiudad);
        }

        public static Ciudad Buscar(string pCodCiudad, string pCodPais)
        {
            Ciudad c = null;
            c = (Ciudad)PersistenciaCiudad.Buscar(pCodCiudad, pCodPais);
            return c;
        }

        public static List<Ciudad> ListarCiudadesPorPais(Pais pPais)
        {
            return PersistenciaCiudad.ListarCiudadesPorPais(pPais);
        }
    }
}
