using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PetShopDoBairro.adm
{
    public partial class equipamentos : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            BancoDeDadosPadraoDataContext contexto = new BancoDeDadosPadraoDataContext();

            var equipamentos = (from eq in contexto.EQUIPAMENTO
                            select new { eq, nomeUsuario = eq.USUARIO.CPFCNPJ + " - " + eq.USUARIO.NOME } ).ToList();

            rptEquipamentos.DataSource = equipamentos;
            rptEquipamentos.DataBind();
        }
    }
}