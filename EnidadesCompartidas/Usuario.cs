using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EnidadesCompartidas
{
    public class Usuario
    {
        //atributos
        private string _usuario;
        private string _password;
        private string _nombre;


        //propiedades
        public string NombreUsuario
        {
            get { return _usuario; }
            set 
            {
                if (value == null)
                    throw new Exception("Este campo no puede estar vacio");
                else if (value.Trim().Length > 30)
                    throw new Exception("El nombre de usuario no puede tener mas de 30 caracteres");
                else
                    _usuario = value; 
            }
        }

        public string Password
        {
            get { return _password; }
            set 
            {
                if (value == null)
                    throw new Exception("Este campo no puede estar vacio");
                else if (value.Trim().Length > 30)
                    throw new Exception("La contraseña no puede tener mas de 30 caracteres");
                else
                    _password = value; 
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
         public Usuario(string pUsuario, string pPassword, string pNombre)
        {
            NombreUsuario= pUsuario;
            Password = pPassword;
            Nombre = pNombre;
        }


        //operaciones
        public override string ToString()
        {
            return "Usuario: " + _usuario + " Password: " + _password + " Nombre: "+_nombre;
        }
    }
}
