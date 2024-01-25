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
    public partial class PronosticoPorCiudad : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
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

        protected void btnListar_Click(object sender, EventArgs e)
        {
            gvPronosticos.DataSource = null;
            gvPronosticos.DataBind();

            try
            {
                if (gvCiudades.SelectedRow == null)
                {
                    lblError.Text = "Debe seleccionar una ciudad";
                }

                else
                {
                    string CodCiud = gvCiudades.SelectedRow.Cells[1].Text;

                    Pais p = (Pais)Session["Pais"];
                    Ciudad c= LogicaCiudad.Buscar(CodCiud, p.CodPais);


                    List<Pronostico> _miLista = LogicaPronostico.ListarPronosticosPorCiudad(c);

                    if (_miLista.Count == 0)
                        lblError.Text = "No hay pronosticos para la ciudad seleccionada";

                    else
                    {
                        gvPronosticos.DataSource = _miLista;
                        gvPronosticos.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }



        }

        protected void btnListarCiudades_Click(object sender, EventArgs e)
        {
            try
            {
                gvCiudades.DataSource = null;
                gvCiudades.DataBind();
                gvPronosticos.DataSource = null;
                gvPronosticos.DataBind();

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