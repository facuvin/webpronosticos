using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EnidadesCompartidas;
using Persistencia;

namespace Logica
{
    public class LogicaPronostico
    {
        public static void Agregar(Pronostico pPronostico)
        {
            PersistenciaPronostico.Agregar(pPronostico);
        }

        public static List<Pronostico> ListarPronosticosPorCiudad(Ciudad pCiudad)
        {
            return PersistenciaPronostico.ListarPronosticosPorCiudad(pCiudad);
        }

        public static List<Pronostico> ListarPronosticosPorDia(DateTime pFecha)
        {
            return PersistenciaPronostico.ListarPronosticosPorDia(pFecha);
        }

        public static List<Pronostico> ListarPronosticos()
        {
            return PersistenciaPronostico.ListarPronosticos();
        }


    }
}
