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
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["UsuarioActual"] = null;
            }

            try
            {
                List<Pronostico> oPronosticos = LogicaPronostico.ListarPronosticosPorDia(DateTime.Now);
                for (int i = 0; i < oPronosticos.Count; i++)
                {
                    lstPronosticos.Items.Add(oPronosticos[i].ToString());
                }
            }

            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }
    }
}