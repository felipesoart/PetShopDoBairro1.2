using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PetShopDoBairro.adm
{
    public partial class empresas : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            BancoDeDadosPadraoDataContext contexto = new BancoDeDadosPadraoDataContext();

            var empresas = (from emp in contexto.EMPRESA
                            select emp).ToList();

            rptEmpresas.DataSource = empresas;
            rptEmpresas.DataBind();
        }
    }
}