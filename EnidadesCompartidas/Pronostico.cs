using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EnidadesCompartidas
{
    public class Pronostico
    {
         //atributos
        private int _interno;
        private DateTime _fechayHora;
        private int _tempMax;
        private int _tempMin;
        private int _viento;
        private int _probLluvias;
        private string _tipoCielo;

        private Usuario _usu;
        private Ciudad _ciudad;


        //propiedades
        public int Interno 
        { 
            get { return _interno; } 
            set { _interno = value; }
        }

        public DateTime FechayHora
        {
            get { return _fechayHora; }
            set { _fechayHora = value; }
        }

        public int TempMax
        {
            get { return _tempMax; }
            set { _tempMax = value; }
        }

        public int TempMin
        {
            get { return _tempMin; }
            set { _tempMin = value; }
        }

        public int Viento
        {
            get { return _viento; }
            set 
            {
                if (value < 0)
                    throw new Exception("La velocidad del viento no puede ser un numero negativo");
                else
                    _viento = value; 
            }
        }

        public int ProbLluvias
        {
            get { return _probLluvias; }
            set 
            {
                if (value < 0 || value > 100)
                    throw new Exception("La probabilidad debe ser un numero entre 0 y 100");
                else
                    _probLluvias = value; 
            }
        }

        public string TipoCielo
        {
            get { return _tipoCielo; }
            set
            {
                if (value == null)
                    throw new Exception("Este campo no puede estar vacio");
                else if (value.Trim().ToUpper() != "DESPEJADO" && value.Trim().ToUpper() != "PARCIALMENTE NUBOSO" && value.Trim().ToUpper() != "NUBOSO")
                    throw new Exception("El tipo de cielo solo puede ser: DESPEJADO, PARCIALMENTE NUBOSO O NUBOSO");
                else
                    _tipoCielo = value;
            }
        }

        public Usuario Usuario
        {
            set
            {
                if (value == null)
                    throw new Exception("Debe ingresar un usuario");
                else
                    _usu = value;
            }
            get { return _usu; }
        }

        public Ciudad Ciudad
        {
            set
            {
                if (value == null)
                    throw new Exception("Debe ingresar una ciudad");
                else
                    _ciudad = value;
            }
            get { return _ciudad; }
        }


        //constructores
        public Pronostico(int pInterno, DateTime pFecha, int pTempMax, int pTempMin, int pViento, int pLluvias, string pTipoCielo, Usuario pUsuario, Ciudad pCiudad)
        {
            Interno = pInterno;
            FechayHora= pFecha;
            TempMax = pTempMax;
            TempMin = pTempMin;
            Viento = pViento;
            ProbLluvias = pLluvias;
            TipoCielo = pTipoCielo;
            Usuario = pUsuario;
            Ciudad = pCiudad;
        }


        //operaciones
        public override string ToString()
        {
            return "Fecha y Hora: " + _fechayHora + "\nTemperatura maxima: " + _tempMax + "ºC\nTemperatura minima: " + _tempMin + "ºC\nVel. del viento: " + _viento + "Km/H\nProbabilidad de lluvias y tormentas: " + _probLluvias + "%\nTipo de cielo: " + _tipoCielo + "\nRegistrado por: " + Usuario.NombreUsuario + "\nCiudad: " + Ciudad.Nombre + ", " + Ciudad.Pais.Nombre;
        }
    }
}
