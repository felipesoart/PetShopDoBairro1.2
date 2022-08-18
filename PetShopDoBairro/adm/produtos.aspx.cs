using iTextSharp.text;
using iTextSharp.text.pdf;
using PetShopDoBairro.adm.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PetShopDoBairro.adm
{
    public partial class produtos : System.Web.UI.Page
    {
        private string caminhoLog = ConfigurationManager.AppSettings["LogRegistro"];
        DateTime dataExecucao = DateTime.Now;
        

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                carregarDados();                
            }
            
        }

        private void carregarDados()
        {
            BancoDeDadosPadraoDataContext contexto = new BancoDeDadosPadraoDataContext();

            var produtos = (from p in contexto.PRODUTO
                                 select p).ToList();

            rptProdutos.DataSource = produtos;
            rptProdutos.DataBind();
        }

        protected void rptProdutos_ItemCommand1(object source, RepeaterCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Excluir")
                {
                    BancoDeDadosPadraoDataContext contexto = new BancoDeDadosPadraoDataContext();
                    var valueCommandArgument = Convert.ToInt32(e.CommandArgument);
                    var produto = (from p in contexto.PRODUTO
                                        where p.ID == valueCommandArgument
                                        select p)
                                            .FirstOrDefault();

                    if (produto != null)
                    {
                        contexto.PRODUTO.DeleteOnSubmit(produto);

                        contexto.SubmitChanges();
                    }

                    carregarDados();
                }

            }
            catch (Exception ex)
            {
                LogRegistro.registrarLog(ex.Message + Environment.NewLine + ex.StackTrace + Environment.NewLine, "PetShopBairro.Web", caminhoLog, dataExecucao);
                divMensagem.Visible = true;
                divMensagem.Attributes["class"] += " alert-danger ";
                divMensagem.InnerHtml += ex.Message;
              
            }
        }
      
    }
}