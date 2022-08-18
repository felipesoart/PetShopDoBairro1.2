using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PetShopDoBairro.adm
{
    public partial class usuarios : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            BancoDeDadosPadraoDataContext contexto = new BancoDeDadosPadraoDataContext();

            var usuarios = (from u in contexto.USUARIO
                            select u).ToList();

            rptUsuairos.DataSource = usuarios;
            rptUsuairos.DataBind();
        }
    }
}