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
    public partial class ABMPaises : System.Web.UI.Page
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
            txtNombre.Text = "";
            txtNombre.Enabled = false;
            txtCodPais.Enabled = true;
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            //controlo el dato de entrada
            if (txtCodPais.Text.Trim().Length != 3) 
            {
                lblError.Text = "El codigo pais debe ser de 3 caracteres exactamente";
                return;
            }

            try
            {
                //buscar para saber si ya existe
                Pais pais = LogicaPais.Buscar(txtCodPais.Text.Trim().ToUpper());

                if (pais != null) //si existe el pais
                {
                    txtCodPais.Text = pais.CodPais;
                    txtNombre.Text = pais.Nombre;
                    Session["UnPais"] = pais;
                    btnEliminar.Enabled = true;
                    btnModificar.Enabled = true;
                    txtNombre.Enabled = true;
                    txtCodPais.Enabled = false;
                }
                else//no existe ese pais
                {
                    btnAgregar.Enabled = true;
                    Session["UnPais"] = null;
                    txtNombre.Enabled = true;
                    txtCodPais.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            string oMensaje = "", nombre = "", codPais = "";

            nombre = txtNombre.Text;
            codPais = txtCodPais.Text.Trim().ToUpper();

            //controlo lo que entra
            if (nombre == "")
                oMensaje = oMensaje + "<br>Debe ingresar el nombre del pais";

            if (txtCodPais.Text.Trim().Length != 3)
                oMensaje = oMensaje + "<br>El codigo pais debe ser de 3 caracteres exactamente";

            if (oMensaje != "")//si hay error 
            {
                lblError.Text = oMensaje;
            }

            else//sino contruyo el objeto pais y lo agrego a la tabla correspondiente
            {  
                try
                {
                    Pais p = new Pais(codPais, nombre);
                    LogicaPais.Agregar(p);
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
            string oMensaje = "", nombre = "", codPais = "";

            nombre = txtNombre.Text;
            codPais = txtCodPais.Text.Trim().ToUpper();

            //controlo lo que entra
            if (nombre == "")
                oMensaje = oMensaje + "<br>Debe ingresar el nombre del pais";

            if (txtCodPais.Text.Trim().Length != 3)
                oMensaje = oMensaje + "<br>El codigo pais debe ser de 3 caracteres exactamente";

            if (oMensaje != "")//si hay error 
            {
                lblError.Text = oMensaje;
            }

            else//los datos son correctos
            {

                try
                {
                    Pais pais = (Pais)Session["UnPais"];
                    pais.Nombre = nombre;

                    LogicaPais.Modificar(pais);
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
                Pais p = (Pais)Session["UnPais"];
                LogicaPais.Eliminar(p);
                lblError.Text = "Eliminacion exitosa";
                this.LimpioFormulario();
                this.DesactivoBotones();
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }


        protected void btnLimpiar_Click1(object sender, EventArgs e)
        {
            this.LimpioFormulario();
            this.DesactivoBotones();
        }


    }
}