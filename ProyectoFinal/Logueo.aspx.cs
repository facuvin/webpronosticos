using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EnidadesCompartidas;
using Logica;

namespace ProyectoFinal
{
    public partial class Logueo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnIngresar_Click(object sender, EventArgs e)
        {
            try
            {
                Usuario unUsu;

                unUsu = LogicaUsuario.Buscar(txtUsuario.Text);

                if (unUsu == null)
                    lblError.Text = "El usuario ingresado no existe";

                else
                {
                    if (unUsu.Password != txtPassword.Text)
                        lblError.Text = "La contraseña es incorrecta";

                    else
                    {
                        Session["UsuarioActual"] = unUsu;
                        Response.Redirect("~/Bienvenido.aspx");//CORROBORAR
                    }
                }
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
         }
    }
}