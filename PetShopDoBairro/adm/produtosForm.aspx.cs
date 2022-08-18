using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PetShopDoBairro.adm
{
    public partial class produtosForm : System.Web.UI.Page
    {
        private string caminhoLog = ConfigurationManager.AppSettings["LogRegistro"];
        DateTime dataExecucao = DateTime.Now;

        private int _codigo = 0;

        private int _idOrdemServico = -1;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request["id"] != null)
            {
                int.TryParse(Request["id"].ToString(), out _idOrdemServico);

                if (!Page.IsPostBack)
                {
                    carregarDados();
                    txtCodigo.Enabled = false;
                }
            }            
            else
            {
                carregarCodigo();
                txtCodigo.Enabled = false;
            }
        }

        private void carregarCodigo()
        {
            BancoDeDadosPadraoDataContext contexto = new BancoDeDadosPadraoDataContext();
            var produto = (from p in contexto.PRODUTO                          
                           select p).OrderByDescending(pu => pu.ID).FirstOrDefault();
                       
                _codigo = Convert.ToInt32(produto == null ? 0 : produto.CODIGO) + 1;
                txtCodigo.Text = _codigo.ToString();
          
        }

        private void carregarDados()
        {
            BancoDeDadosPadraoDataContext contexto = new BancoDeDadosPadraoDataContext();
            var produto = (from o in contexto.PRODUTO
                           where o.ID == _idOrdemServico
                           select o).FirstOrDefault();
            if (produto != null)
            {
                txtNome.Text = produto.NOME;
                txtCodigo.Text = produto.CODIGO.ToString();

            }
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                BancoDeDadosPadraoDataContext contexto = new BancoDeDadosPadraoDataContext();
                PRODUTO produto = null;
                if (_idOrdemServico == -1)
                {
                    produto = new PRODUTO();
                    contexto.PRODUTO.InsertOnSubmit(produto);

                    produto.CRIADOPOR = Session["nomeUsuario"].ToString();
                    produto.CRIADOEM = DateTime.Now;
                }
                else
                {
                    produto = (from p in contexto.PRODUTO
                               where p.ID == _idOrdemServico
                                    select p).FirstOrDefault();

                    produto.MODIFICADOPOR = Session["nomeUsuario"].ToString();
                    produto.MODIFICADOEM = DateTime.Now;
                }

                produto.NOME = txtNome.Text;
                produto.CODIGO = Convert.ToInt32(txtCodigo.Text);
                
                contexto.SubmitChanges();

                divMensagem.Visible = true;
                divMensagem.Attributes["class"] += " alert-success ";
                divMensagem.InnerHtml += "As alterações foram salvas com sucesso!";

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