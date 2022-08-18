using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PetShopDoBairro.adm
{
    public partial class BemVindo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            titulo.InnerText = "Bem vindo, " + Session["nomeUsuario"].ToString();
        }
    }
}