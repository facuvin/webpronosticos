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
    public partial class ABMUsuarios : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.LimpioFormulario();
                this.DesactivoBotones();
            }
        }

        private void DesactivoBotones()
        {
            btnAgregar.Enabled = false;
            btnModificar.Enabled = false;
            btnEliminar.Enabled = false;

            btnBuscar.Enabled = true;
        }

        private void LimpioFormulario()
        {
            txtUsuario.Text = "";
            txtPassword.Text = "";
            txtNombre.Text = "";
            txtNombre.Enabled = false;
            txtPassword.Enabled = false;
            txtUsuario.Enabled = true;
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            //controlo el dato de entrada
            if (txtUsuario.Text == "")
            {
                lblError.Text = "El usuario no puede estar vacio";
                return;
            }

            try
            {
                //buscar para saber si ya existe
                Usuario usuario = LogicaUsuario.Buscar(txtUsuario.Text.Trim());

                if (usuario != null) //si existe el usuario
                {
                    txtUsuario.Text = usuario.NombreUsuario;
                    txtPassword.Text = usuario.Password;
                    txtNombre.Text = usuario.Nombre;
                    Session["UnUsuario"] = usuario;
                    btnEliminar.Enabled = true;
                    btnModificar.Enabled = true;
                    txtNombre.Enabled = true;
                    txtPassword.Enabled = true;
                    txtUsuario.Enabled = false;
                }
                else//no existe ese usuario
                {
                    btnAgregar.Enabled = true;
                    Session["UnUsuario"] = null;
                    txtNombre.Enabled = true;
                    txtPassword.Enabled = true;
                    txtUsuario.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            string oMensaje = "", nombre = "", password = "", usuario = "";

            nombre = txtNombre.Text;
            password = txtPassword.Text.Trim();
            usuario = txtUsuario.Text.Trim();

            //controlo lo que entra
            if (nombre == "")
                oMensaje = oMensaje + "<br>Debe ingresar el nombre del usuario";

            if (password == "")
                oMensaje = oMensaje + "<br>Debe ingresar una contraseña";

            if (oMensaje != "")//si hay error 
            {
                lblError.Text = oMensaje;
            }

            else//sino contruyo el objeto usuario y lo agrego a la tabla correspondiente
            {
                try
                {
                    Usuario u = new Usuario(usuario, password, nombre);

                    LogicaUsuario.Agregar(u);
                    lblError.Text = "Alta con exito";
                    this.LimpioFormulario();
                    this.DesactivoBotones();
                }
                catch (Exception ex)
                {
                    lblError.Text = ex.Message;
                }
            }
        }

        protected void btnModificar_Click(object sender, EventArgs e)
        {
            string oMensaje = "", nombre = "", password = "", usuario = "";

            nombre = txtNombre.Text;
            password = txtPassword.Text.Trim();
            usuario = txtUsuario.Text.Trim();

            //controlo lo que entra
            if (nombre == "")
                oMensaje = oMensaje + "<br>Debe ingresar el nombre del usuario";

            if (password == "")
                oMensaje = oMensaje + "<br>Debe ingresar una contraseña";

            if (oMensaje != "")//si hay error 
            {
                lblError.Text = oMensaje;
            }

            else//los datos son correctos
            {
                try
                {
                    Usuario unUsuario = (Usuario)Session["UnUsuario"];
                    unUsuario.Nombre = nombre;

                    LogicaUsuario.Modificar(unUsuario);
                    lblError.Text = "Modificacion exitosa";
                    this.LimpioFormulario();
                    this.DesactivoBotones();
                }
                catch (Exception ex)
                {
                    lblError.Text = ex.Message;
                }
            }
        }

        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                Usuario u = (Usuario)Session["UnUsuario"];
                LogicaUsuario.Eliminar(u);
                lblError.Text = "Eliminacion exitosa";
                this.LimpioFormulario();
                this.DesactivoBotones();
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            this.LimpioFormulario();
            this.DesactivoBotones();
        }



    }
}