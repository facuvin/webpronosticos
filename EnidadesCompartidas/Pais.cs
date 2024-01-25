using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EnidadesCompartidas
{
    public class Pais
    {
        //atributos
        private string _codPais;
        private string _nombre;


        //propiedades
        public string CodPais
        {
            get { return _codPais; }
            set 
            {
                if (value == null)
                    throw new Exception("Este campo no puede estar vacio");
                else if (value.Trim().Length != 3)
                    throw new Exception("El codigo debe ser de 3 caracteres");
                else
                    _codPais = value; 
            }
        }

        public string Nombre
        {
            get { return _nombre; }
            set 
            {
                if (value == null)
                    throw new Exception("Este campo no puede estar vacio");
                else if (value.Trim().Length > 30)
                    throw new Exception("El nombre no puede tener mas de 30 caracteres");
                else
                    _nombre = value; 
            }
        }



        //constructores
         public Pais(string pCodigo, string pNombre)
        {
            CodPais = pCodigo;
            Nombre = pNombre;
        }


        //operaciones
        public override string ToString()
        {
            return "Codigo Pais: " + _codPais + " Nombre: "+_nombre;
        }

        public string MostrarCodigo()
        {
            return _codPais;
        }
    }
}
