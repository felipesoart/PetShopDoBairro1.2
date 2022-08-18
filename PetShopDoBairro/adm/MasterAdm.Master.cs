using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PetShopDoBairro.adm
{
    public partial class MasterAdm : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            tituloUsuario.InnerText = Session["nomeUsuario"].ToString();

            if (Session["tipoPessoa"].ToString() == "0")//Gestor
            {
                liempresas.Visible = true;
            }

            

        }
    }
}