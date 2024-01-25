using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EnidadesCompartidas
{
    public class Ciudad
    {
        //atributos
        private string _codCiudad;
        private string _nombre;

        private Pais _pais;

        //propiedades

        public string CodCiudad
        {
            get { return _codCiudad; }
            set 
            {
                if (value == null)
                    throw new Exception("Este campo no puede estar vacio");
                else if (value.Trim().Length!=3)
                    throw new Exception("El codigo debe ser de 3 caracteres");
                else
                    _codCiudad = value; 
            }
        }

        public string Nombre
        {
            get { return _nombre; }
            set 
            {
                if (value == null)
                    throw new Exception("Este campo no puede estar vacio");
                else if (value.Trim().Length>30)
                    throw new Exception("El nombre no puede tener mas de 30 caracteres");
                else
                    _nombre = value; 
            }
        }

        public Pais Pais
        {
            set
            {
                if (value == null)
                    throw new Exception("Debe ingresar un pais");
                else
                    _pais = value;
            }
            get { return _pais; }
        }


        //constructores
         public Ciudad(Pais pPais, string pCodCiudad, string pNombre)
        {
            Pais = pPais;
            CodCiudad = pCodCiudad;
            Nombre = pNombre;
        }


        //operaciones
        public override string ToString()
        {
            return "Codigo Pais: " + _pais.CodPais + " Codigo Ciudad: " + _codCiudad + " Nombre: "+_nombre;
        }
    }
}
