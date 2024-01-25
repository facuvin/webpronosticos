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
    public partial class PronosticoPorDia : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                gvPronosticos.DataSource = null;
                gvPronosticos.DataBind();

                DateTime fecha;
                fecha = cdrCalendario.SelectedDate;

                //Cargo los pronosticos en una lista
                List<Pronostico> _listaP = LogicaPronostico.ListarPronosticosPorDia(fecha);

                if (_listaP.Count == 0)
                    lblError.Text = "No hay pronosticos para la fecha indicada";

                else
                {
                    //cargo los datos en la grilla
                    gvPronosticos.DataSource = _listaP;
                    gvPronosticos.DataBind();
                }
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }
    }
}