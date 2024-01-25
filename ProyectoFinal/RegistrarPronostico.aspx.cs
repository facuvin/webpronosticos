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
    public partial class RegistrarPronostico : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    this.LimpioFormulario();
                    txtFechayHora.Attributes.Add("Type", "datetime-local");

                    List<Pais> ListaPaises = LogicaPais.ListarPaises();

                    for (int i = 0; i < ListaPaises.Count; i++)
                    {
                        ddlPaises.Items.Add(ListaPaises[i].MostrarCodigo());
                    }
                }
                catch (Exception ex)
                {
                    lblError.Text = ex.Message;
                }
            }
        }


        private void LimpioFormulario()
        {

            txtFechayHora.Text = "";
            txtTempMaxima.Text = "";
            txtTempMinima.Text = "";
            txtViento.Text = "";
            txtLluvias.Text = "";
            txtTipoCielo.Text = "";
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            string oMensaje = "", tipoCielo = "", codCiudad = "", codPais = "";
            DateTime fechayhora;
            int tempMax = 0, tempMin = 0, viento = 0, lluvias = 0;

            try
            {
                fechayhora = Convert.ToDateTime(txtFechayHora.Text);
                tempMax = Convert.ToInt32(txtTempMaxima.Text);
                tempMin = Convert.ToInt32(txtTempMinima.Text);
                viento = Convert.ToInt32(txtViento.Text);
                lluvias = Convert.ToInt32(txtLluvias.Text);
                tipoCielo = txtTipoCielo.Text.Trim().ToUpper();
                codPais = ddlPaises.SelectedValue;
                codCiudad = gvCiudades.SelectedRow.Cells[1].Text;

                //controlo lo que entra
                if (gvCiudades.SelectedRow == null)
                    oMensaje = oMensaje + "Debe seleccionar una ciudad";
                
                if (viento < 0)
                    oMensaje = oMensaje + "<br>La velocidad del viento no puede ser un numero negativo";

                if (lluvias < 0)
                    oMensaje = oMensaje + "<br>La probabilidad de lluvia no puede ser un numero negativo";

                if (codPais.Length != 3)
                    lblError.Text = "<br>El codigo pais debe ser de 3 caracteres exactamente";

                if (codCiudad.Length != 3)
                    lblError.Text = "<br>El codigo ciudad debe ser de 3 caracteres exactamente";

                if (oMensaje != "")//si hay error 
                {
                    lblError.Text = oMensaje;
                }

                else//sino contruyo el objeto pronostico y lo agrego a la tabla correspondiente
                {
                    Ciudad c = LogicaCiudad.Buscar(codCiudad, codPais);

                    Usuario u = (Usuario)Session["UsuarioActual"];

                    Pronostico p = new Pronostico(0, fechayhora, tempMax, tempMin, viento, lluvias, tipoCielo, u, c);

                    try
                    {
                        LogicaPronostico.Agregar(p);
                        lblError.Text = "Pronostico agregado con exito";
                        this.LimpioFormulario();
                    }
                    catch (Exception ex)
                    {
                        lblError.Text = ex.Message;
                    }
                }
            }

            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }

        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            this.LimpioFormulario();
        }

        protected void btnListar_Click(object sender, EventArgs e)
        {
            try
            {
                gvCiudades.DataSource = null;
                gvCiudades.DataBind();

                string codPais;

                codPais = ddlPaises.SelectedValue;

                Pais p = LogicaPais.Buscar(codPais);
                Session["Pais"] = p;

                //Cargo las ciudades del pais en una lista
                List<Ciudad> _listaC = LogicaCiudad.ListarCiudadesPorPais(p);

                if (_listaC.Count == 0)
                    lblError.Text = "No hay ciudades registradas para el pais";

                else
                {
                    //cargo los datos en la grilla
                    gvCiudades.DataSource = _listaC;
                    gvCiudades.DataBind();
                }
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }


    }
}