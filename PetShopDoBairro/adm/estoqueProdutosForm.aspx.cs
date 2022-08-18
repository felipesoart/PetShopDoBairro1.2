using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PetShopDoBairro.adm
{
    public partial class estoqueProdutosForm : System.Web.UI.Page
    {
        private string caminhoLog = ConfigurationManager.AppSettings["LogRegistro"];
        DateTime dataExecucao = DateTime.Now;

        private int _codigo = 0;

        private int _idOrdemServico = -1;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                carregarProdutos();
                carregarLocalEstoque();

                if (Request["id"] != null)
                {
                    int.TryParse(Request["id"].ToString(), out _idOrdemServico);

                    carregarProdutos();
                    carregarLocalEstoque();
                    carregarDados();

                }

            }
            else
            {
                if (Request["id"] != null)
                {
                    int.TryParse(Request["id"].ToString(), out _idOrdemServico);
                }
            }



        }

        private void carregarDados()
        {
            BancoDeDadosPadraoDataContext contexto = new BancoDeDadosPadraoDataContext();
            var estoqueProduto = (from ple in contexto.PRODUTOLOCALESTOQUE
                                  where ple.ID == _idOrdemServico
                                  select ple).FirstOrDefault();
            if (estoqueProduto != null)
            {
                ddlProduto.SelectedValue = estoqueProduto.IDPRODUTO.ToString();
                ddlLocalEstoque.SelectedValue = estoqueProduto.IDLOCALESTOQUE.ToString();
                txtValorCusto.Text = estoqueProduto.VALORCUSTO.ToString(CultureInfo.CreateSpecificCulture("pt-BR"));
                txtValorVenda.Text = estoqueProduto.VALORVENDA.ToString(CultureInfo.CreateSpecificCulture("pt-BR"));
                txtQuantidade.Text = estoqueProduto.QUANTIDADE.ToString(CultureInfo.CreateSpecificCulture("pt-BR"));
            }
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                BancoDeDadosPadraoDataContext contexto = new BancoDeDadosPadraoDataContext();
                PRODUTOLOCALESTOQUE estoqueProduto = null;
                if (_idOrdemServico == -1)
                {
                    estoqueProduto = new PRODUTOLOCALESTOQUE();
                    contexto.PRODUTOLOCALESTOQUE.InsertOnSubmit(estoqueProduto);

                    estoqueProduto.CRIADOPOR = Session["nomeUsuario"].ToString();
                    estoqueProduto.CRIADOEM = DateTime.Now;
                }
                else
                {
                    estoqueProduto = (from ple in contexto.PRODUTOLOCALESTOQUE
                                      where ple.ID == _idOrdemServico
                                      select ple).FirstOrDefault();

                    estoqueProduto.MODIFICADOPOR = Session["nomeUsuario"].ToString();
                    estoqueProduto.MODIFICADOEM = DateTime.Now;
                }

                estoqueProduto.VALORCUSTO = Convert.ToDecimal(txtValorCusto.Text);
                estoqueProduto.VALORVENDA = Convert.ToDecimal(txtValorVenda.Text);
                estoqueProduto.QUANTIDADE = Convert.ToDecimal(txtQuantidade.Text);
                estoqueProduto.IDPRODUTO = Convert.ToInt32(ddlProduto.SelectedValue);
                estoqueProduto.IDLOCALESTOQUE = Convert.ToInt32(ddlLocalEstoque.SelectedValue);

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

        protected void carregarProdutos()
        {
            BancoDeDadosPadraoDataContext contexto = new BancoDeDadosPadraoDataContext();
            var produtos = (from p in contexto.PRODUTO
                            select new
                            {
                                p,
                                Id = p.ID,
                                NomeProdutoCodigo = p.CODIGO + " - " + p.NOME
                            })
                            .OrderBy(x => x.p.CODIGO)
                            .ToList();

            if (produtos != null)
            {
                ddlProduto.DataSource = produtos;
                ddlProduto.DataBind();

                ddlProduto.Items.Insert(0, new ListItem("Selecione...", "0"));
            }
        }

        protected void carregarLocalEstoque()
        {
            BancoDeDadosPadraoDataContext contexto = new BancoDeDadosPadraoDataContext();
            var localEstoque = (from p in contexto.LOCALESTOQUE
                                select p)
                            .OrderBy(x => x.ID)
                            .ToList();

            if (localEstoque != null)
            {
                ddlLocalEstoque.DataSource = localEstoque;
                ddlLocalEstoque.DataBind();

                ddlLocalEstoque.Items.Insert(0, new ListItem("Selecione...", "0"));
            }
        }
    }
}