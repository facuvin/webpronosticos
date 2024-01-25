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
    public partial class ABMCiudades : System.Web.UI.Page
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
            txtCodPais.Text = "";
            txtCodCiudad.Text = "";
            txtNombre.Text = "";
            txtNombre.Enabled = false;
            txtCodPais.Enabled = true;
            txtCodCiudad.Enabled = true;
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            //controlo el dato de entrada
            if (txtCodPais.Text.Trim().Length != 3)
            {
                lblError.Text = "El codigo pais debe ser de 3 caracteres exactamente";
                return;
            }

            else if (txtCodCiudad.Text.Trim().Length != 3)
            {
                lblError.Text = "El codigo ciudad debe ser de 3 caracteres exactamente";
                return;
            }

            try
            {
                //buscar para saber si ya existe
                Ciudad ciudad = LogicaCiudad.Buscar(txtCodCiudad.Text.Trim().ToUpper(), txtCodPais.Text.Trim().ToUpper());

                if (ciudad != null) //si existe la ciudad
                {
                    txtCodCiudad.Text = ciudad.CodCiudad;
                    txtCodPais.Text = ciudad.Pais.CodPais;
                    txtNombre.Text = ciudad.Nombre;
                    Session["UnaCiudad"] = ciudad;
                    btnEliminar.Enabled = true;
                    btnModificar.Enabled = true;
                    txtNombre.Enabled = true;
                    txtCodPais.Enabled = false;
                    txtCodCiudad.Enabled = false;
                }
                else//no existe esa ciudad
                {
                    btnAgregar.Enabled = true;
                    Session["UnaCiudad"] = null;
                    txtNombre.Enabled = true;
                    txtCodPais.Enabled = false;
                    txtCodCiudad.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            string oMensaje = "", nombre = "", codPais = "", codCiudad = "";

            nombre = txtNombre.Text;
            codPais = txtCodPais.Text.Trim().ToUpper();
            codCiudad = txtCodCiudad.Text.Trim().ToUpper();

            //controlo lo que entra
            if (nombre == "")
                oMensaje = oMensaje + "<br>Debe ingresar el nombre de la ciudad";

            if (txtCodPais.Text.Trim().Length != 3)
                lblError.Text = "<br>El codigo pais debe ser de 3 caracteres exactamente";

            if (txtCodCiudad.Text.Trim().Length != 3)
                lblError.Text = "<br>El codigo ciudad debe ser de 3 caracteres exactamente";

            if (oMensaje != "")//si hay error 
            {
                lblError.Text = oMensaje;
            }

            else//sino contruyo el objeto ciudad y lo agrego a la tabla correspondiente
            {
                try
                {
                    Pais p = LogicaPais.Buscar(codPais);

                    Ciudad c = new Ciudad(p, codCiudad, nombre);

                    LogicaCiudad.Agregar(c);
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
            string oMensaje = "", nombre = "", codPais = "", codCiudad = "";

            nombre = txtNombre.Text;
            codPais = txtCodPais.Text.Trim().ToUpper();
            codCiudad = txtCodCiudad.Text.Trim().ToUpper();

            //controlo lo que entra
            if (nombre == "")
                oMensaje = oMensaje + "<br>Debe ingresar el nombre de la ciudad";

            if (txtCodPais.Text.Trim().Length != 3)
                oMensaje = oMensaje + "<br>El codigo pais debe ser de 3 caracteres exactamente";

            if (txtCodCiudad.Text.Trim().Length != 3)
                oMensaje = oMensaje + "<br>El codigo ciudad debe ser de 3 caracteres exactamente";

            if (oMensaje != "")//si hay error 
            {
                lblError.Text = oMensaje;
            }

            else//los datos son correctos
            {

                try
                {
                    Ciudad ciudad = (Ciudad)Session["UnaCiudad"];
                    ciudad.Nombre = nombre;


                    LogicaCiudad.Modificar(ciudad);
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
                Ciudad c = (Ciudad)Session["UnaCiudad"];
                LogicaCiudad.Eliminar(c);
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