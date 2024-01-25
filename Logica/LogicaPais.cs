using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EnidadesCompartidas;
using Persistencia;

namespace Logica
{
    public class LogicaPais
    {
        public static void Agregar(Pais pPais)
        {
            PersistenciaPais.Agregar(pPais);
        }

        public static void Modificar(Pais pPais)
        {
            PersistenciaPais.Modificar(pPais);
        }

        public static void Eliminar(Pais pPais)
        {
            PersistenciaPais.Eliminar(pPais);
        }

        public static Pais Buscar(string codPais)
        {
            Pais p = null;
            p= (Pais)PersistenciaPais.Buscar(codPais);
            return p;
        }

        public static List<Pais> ListarPaises()
        {
            return PersistenciaPais.ListarPaises();
        }
    }
}
