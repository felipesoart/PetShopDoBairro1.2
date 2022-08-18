using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PetShopDoBairro.adm
{
    public partial class vendasForm : System.Web.UI.Page
    {
        private string caminhoLog = ConfigurationManager.AppSettings["LogRegistro"];
        DateTime dataExecucao = DateTime.Now;
                
        private int codigo = -1;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                carregarLocalEstoque();
                if (ddlTipovenda.SelectedValue == "0")
                {
                    divProdutoLocalEstoque.Visible = true;
                    divQuantidade.Visible = true;
                    divDecricaoDespesa.Visible = false;
                    divValorGasto.Visible = false;
                }
                else
                {
                    divProdutoLocalEstoque.Visible = false;
                    divQuantidade.Visible = false;
                    divDecricaoDespesa.Visible = true;
                    divValorGasto.Visible = true;
                }


                if (Request["cod"] != null)
                {
                    int.TryParse(Request["cod"].ToString(), out codigo);
                    hdfCodigo.Value = codigo.ToString();

                    divExcluirItenVenda.Visible = false;
                    divExcluirFormaPagamento.Visible = false;
                    divItenVenda.Visible = false;
                    divFormaPagamento.Visible = false;
                    divGerarVenda.Visible = false;

                    carregarDados();
                }
            }            
            else
            {
                if (ddlTipovenda.SelectedValue == "0")
                {
                    divProdutoLocalEstoque.Visible = true;
                    divQuantidade.Visible = true;
                    divDecricaoDespesa.Visible = false;
                    divValorGasto.Visible = false;
                }
                else
                {
                    divProdutoLocalEstoque.Visible = false;
                    divQuantidade.Visible = false;
                    divDecricaoDespesa.Visible = true;
                    divValorGasto.Visible = true;
                }

                if (hdfCodigo.Value == "")
                { 
                    carregarCodigo();
                }
                if (Request["cod"] != null)
                {
                    int.TryParse(Request["cod"].ToString(), out codigo);
                    hdfCodigo.Value = codigo.ToString();
                }
                    carregarDados();
              
            }
        }

        private void carregarCodigo()
        {
            BancoDeDadosPadraoDataContext contexto = new BancoDeDadosPadraoDataContext();
            var venda = (from v in contexto.VENDA
                           select v).OrderByDescending(vu => vu.ID).FirstOrDefault();

            var codigo   = Convert.ToInt32(venda == null ? 0 : venda.CODIGO) + 1;
            hdfCodigo.Value = codigo.ToString();

        }

        private void carregarDados()
        {
            BancoDeDadosPadraoDataContext contexto = new BancoDeDadosPadraoDataContext();

            if (Request["cod"] != null)
            {
                var itensVenda = (from i in contexto.ITENSVENDA
                                  where i.CODIGO == codigo
                                  select i).ToList();
                if (itensVenda.Count > 0)
                {
                    lbItens.InnerText = "Total:" + itensVenda.Sum(x => x.VALOR).ToString("C2");
                    hdfValorTotalItens.Value = itensVenda.Sum(x => x.VALOR).ToString();
                    divTotalItemVenda.Visible = true;
                    rptItensVenda.DataSource = itensVenda;
                    rptItensVenda.DataBind();
                }

                var itensFormaPagamento = (from f in contexto.FORMAPAGAMENTO
                                  where f.CODIGO == codigo
                                  select f).ToList();
                if (itensFormaPagamento.Count > 0)
                {
                    var VFormaPagamento = itensFormaPagamento.Sum(x => x.VALORFORMAPAGAMENTO);
                    var VAcrescimo = itensFormaPagamento.Sum(x => x.VALORACRESCIMO);
                    var VDesconto = itensFormaPagamento.Sum(x => x.VALORDESCONTO);

                    var vTotal = Convert.ToDecimal((VFormaPagamento + VAcrescimo) - VDesconto);
                                        
                    lbFormaPagamento.InnerText = "Total:" + vTotal.ToString("C2");
                    hdfValorTotalItens.Value = vTotal.ToString();
                    divTotalFormaPagamento.Visible = true;
                    rptItensFormaPagamento.DataSource = itensFormaPagamento;
                    rptItensFormaPagamento.DataBind();
                }

                var itenVenda = (from v in contexto.VENDA
                                           where v.CODIGO == codigo
                                           select v).FirstOrDefault();

                if (itenVenda != null)
                {
                    ddlLocalEstoqueVenda.SelectedValue = itenVenda.IDLOCALESTOQUE.ToString();
                }

            }
            else
            {
                var hdf = Convert.ToInt32(hdfCodigo.Value);
                var itens = (from i in contexto.ITENSVENDA
                                  where i.CODIGO == hdf
                                  select i).ToList();

                if (itens.Count > 0)
                {

                    lbItens.InnerText = "Total:" + itens.Sum(x => x.VALOR).ToString("C2");
                    hdfValorTotalItens.Value = itens.Sum(x => x.VALOR).ToString();


                    divExcluirItenVenda.Visible = true;
                    divTotalItemVenda.Visible = true;
                    rptItensVenda.DataSource = itens;
                    rptItensVenda.DataBind();
                }
                else
                {
                    divExcluirItenVenda.Visible = false;
                    divTotalItemVenda.Visible = false;
                    rptItensVenda.DataSource = null;
                    rptItensVenda.DataBind();
                }

               
                var itensForma = (from f in contexto.FORMAPAGAMENTO
                             where f.CODIGO == hdf
                                  select f
                              ).ToList();

                if (itensForma.Count > 0)
                {
                    var VFormaPagamento = itensForma.Sum(x => x.VALORFORMAPAGAMENTO);
                    var VAcrescimo = itensForma.Sum(x => x.VALORACRESCIMO);
                    var VDesconto = itensForma.Sum(x => x.VALORDESCONTO);

                    var vTotal = Convert.ToDecimal((VFormaPagamento + VAcrescimo) - VDesconto);

                    lbFormaPagamento.InnerText = "Total:" + vTotal.ToString("C2");
                    hdfValorTotalItens.Value = vTotal.ToString();

                    divTotalFormaPagamento.Visible = true;
                    divExcluirFormaPagamento.Visible = true;
                    rptItensFormaPagamento.DataSource = itensForma;
                    rptItensFormaPagamento.DataBind();
                }
                else
                {
                    divTotalFormaPagamento.Visible = false;
                    divExcluirFormaPagamento.Visible = false;
                    rptItensFormaPagamento.DataSource = itensForma;
                    rptItensFormaPagamento.DataBind();
                }


                var itenVenda = (from v in contexto.VENDA
                                 where v.CODIGO == hdf
                                 select v).FirstOrDefault();

                if (itenVenda != null)
                {
                    ddlLocalEstoqueVenda.SelectedValue = itenVenda.IDLOCALESTOQUE.ToString();
                }
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

                ddlLocalEstoqueVenda.DataSource = localEstoque;
                ddlLocalEstoqueVenda.DataBind();
            }
        }

        protected void btnAdicionarItem_Click(object sender, EventArgs e)
        {

            try
            {
                BancoDeDadosPadraoDataContext contexto = new BancoDeDadosPadraoDataContext();
                ITENSVENDA itensVenda = null;
                //if (codigo == -1)
                //{
                    itensVenda = new ITENSVENDA();
                    contexto.ITENSVENDA.InsertOnSubmit(itensVenda);

                    itensVenda.CRIADOPOR = Session["nomeUsuario"].ToString();
                    itensVenda.CRIADOEM = DateTime.Now;
                //}
                //else
                //{
                //    itensVenda = (from ple in contexto.PRODUTOLOCALESTOQUE
                //                  where ple.ID == itensVenda
                //                  select ple).FirstOrDefault();

                //    itensVenda.MODIFICADOPOR = Session["nomeUsuario"].ToString();
                //    itensVenda.MODIFICADOEM = DateTime.Now;
                //}

                if (ddlTipovenda.SelectedValue == "0")
                {
                    itensVenda.IDPRODUTOLOCALESTOQUE = Convert.ToInt32(ddlProdutosLocalEstoque.SelectedValue);

                    if (verificaQuantidadeEstoque())
                    {
                        itensVenda.QUANTIDADE = Convert.ToDecimal(txtQuantidade.Text);
                    }
                    else
                    {
                        throw new Exception("A quantidade especificada para este item excede a quantidade em estoque!");
                    }   

                    abaterEstoque();

                    var valor = Convert.ToDecimal(txtQuantidade.Text) * Convert.ToDecimal(hdfProdutosLocalEstoqueValorVenda.Value);
                    itensVenda.DESCRICAO = string.Empty;
                    itensVenda.VALOR = valor;
                }
                else
                {
                    int? id = null;

                    itensVenda.IDPRODUTOLOCALESTOQUE = id;                    
                    itensVenda.QUANTIDADE = 0;                    
                    itensVenda.VALOR = Convert.ToDecimal(txtValorGasto.Text);
                    itensVenda.DESCRICAO = txtdecricaoDespesa.Text;
                }
                              
                itensVenda.CODIGO = Convert.ToInt32(hdfCodigo.Value);

                itensVenda.IDLOCALESTOQUE = Convert.ToInt32(ddlLocalEstoque.SelectedValue);

                contexto.SubmitChanges();

                divMensagem.Visible = true;
                divMensagem.Attributes["class"] += " alert-success ";
                divMensagem.InnerHtml += "As alterações foram salvas com sucesso!";

                carregarDados();

            }
            catch (Exception ex)
            {
                LogRegistro.registrarLog(ex.Message + Environment.NewLine + ex.StackTrace + Environment.NewLine, "PetShopBairro.Web", caminhoLog, dataExecucao);
                divMensagem.Visible = true;
                divMensagem.Attributes["class"] += " alert-danger ";
                divMensagem.InnerHtml += ex.Message;
            }
        }
        
        private bool verificaQuantidadeEstoque()
        {
            BancoDeDadosPadraoDataContext contexto = new BancoDeDadosPadraoDataContext();

            var produtoLocalEstoque = (from ple in contexto.PRODUTOLOCALESTOQUE
                                       where ple.ID == Convert.ToInt32(ddlProdutosLocalEstoque.SelectedValue)
                                       select ple).FirstOrDefault();

            if (produtoLocalEstoque.QUANTIDADE > Convert.ToDecimal(txtQuantidade.Text))
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        private void abaterEstoque()
        {
            BancoDeDadosPadraoDataContext contexto = new BancoDeDadosPadraoDataContext();

            var produtoLocalEstoque = (from ple in contexto.PRODUTOLOCALESTOQUE
                                  where ple.ID == Convert.ToInt32(ddlProdutosLocalEstoque.SelectedValue)
                                    select ple).FirstOrDefault();

            var abaterQuantidade = produtoLocalEstoque.QUANTIDADE - Convert.ToDecimal(txtQuantidade.Text);

            produtoLocalEstoque.QUANTIDADE = abaterQuantidade;

            contexto.SubmitChanges();

        }

        protected void ddlLocalEstoque_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlLocalEstoque.SelectedValue != "0")
            {
                carregarProdultosLocalEstoque();
            }
        }

        protected void carregarProdultosLocalEstoque()
        {
            BancoDeDadosPadraoDataContext contexto = new BancoDeDadosPadraoDataContext();
            var valueLocalEstoque = Convert.ToInt32(ddlLocalEstoque.SelectedValue);
            var produtosLocalEstoque = (from p in contexto.PRODUTOLOCALESTOQUE
                                        where p.IDLOCALESTOQUE == valueLocalEstoque
                                        select new { p, NOME = p.PRODUTO.CODIGO + " - " + p.PRODUTO.NOME, ID = p.ID })
                            .OrderBy(x => x.ID)
                            .ToList();

            if (produtosLocalEstoque != null)
            {
                ddlProdutosLocalEstoque.DataSource = produtosLocalEstoque;
                ddlProdutosLocalEstoque.DataBind();

                ddlProdutosLocalEstoque.Items.Insert(0, new ListItem("Selecione...", "0"));
            }
        }

        protected void ddlProdutosLocalEstoque_SelectedIndexChanged(object sender, EventArgs e)
        {
            BancoDeDadosPadraoDataContext contexto = new BancoDeDadosPadraoDataContext();
            var valueProdutosLocalEstoque = Convert.ToInt32(ddlProdutosLocalEstoque.SelectedValue);
            var produtosLocalEstoque = (from p in contexto.PRODUTOLOCALESTOQUE
                                        where p.ID == valueProdutosLocalEstoque
                                        select p).FirstOrDefault();
            if (produtosLocalEstoque != null)
            {
                txtQuantidade.Attributes.Add("max:", produtosLocalEstoque.QUANTIDADE.ToString());
                hdfProdutosLocalEstoqueValorVenda.Value = produtosLocalEstoque.VALORVENDA.ToString();
            }
        }
              

        private void incluirQuatidadeItensAoEstoque(decimal? quantidade, int? IdProdutoLocalEstoque)
        {
            BancoDeDadosPadraoDataContext contexto = new BancoDeDadosPadraoDataContext();

            var produtoLocalEstoque = (from ple in contexto.PRODUTOLOCALESTOQUE
                                       where ple.ID == IdProdutoLocalEstoque
                                       select ple).FirstOrDefault();

            var incluirQuantidade = produtoLocalEstoque.QUANTIDADE + Convert.ToDecimal(quantidade);

            produtoLocalEstoque.QUANTIDADE = incluirQuantidade;

            contexto.SubmitChanges();
        }

        protected void btnAdicionarItemFormaPagamento_Click(object sender, EventArgs e)
        {
            try
            {
                BancoDeDadosPadraoDataContext contexto = new BancoDeDadosPadraoDataContext();
                FORMAPAGAMENTO itensFormaPagamento = null;
                
                itensFormaPagamento = new FORMAPAGAMENTO();
                contexto.FORMAPAGAMENTO.InsertOnSubmit(itensFormaPagamento);

                itensFormaPagamento.CRIADOPOR = Session["nomeUsuario"].ToString();
                itensFormaPagamento.CRIADOEM = DateTime.Now;
                //}
                //else
                //{
                //    itensVenda = (from ple in contexto.PRODUTOLOCALESTOQUE
                //                  where ple.ID == itensVenda
                //                  select ple).FirstOrDefault();

                //    itensVenda.MODIFICADOPOR = Session["nomeUsuario"].ToString();
                //    itensVenda.MODIFICADOEM = DateTime.Now;
                //}
                itensFormaPagamento.CODIGO = Convert.ToInt32(hdfCodigo.Value);
                itensFormaPagamento.VALORFORMAPAGAMENTO = Convert.ToDecimal(string.IsNullOrEmpty(txtValorFormaPagamento.Text) ? "0,00" : txtValorFormaPagamento.Text);
                itensFormaPagamento.VALORDESCONTO = Convert.ToDecimal(string.IsNullOrEmpty(txtValorDesconto.Text) ? "0,00" : txtValorDesconto.Text); 
                itensFormaPagamento.VALORACRESCIMO = Convert.ToDecimal(string.IsNullOrEmpty(txtValorAcrescimo.Text) ? "0,00" : txtValorAcrescimo.Text);
                itensFormaPagamento.NSU = txtNsu.Text;
                itensFormaPagamento.CODAUTORIZACAO = txtCodAutorizacao.Text;
                itensFormaPagamento.MEIOFORMAPAGAMENTO = Convert.ToInt32(ddlMeioFormaPagamento.SelectedValue);
                itensFormaPagamento.DESCRICAO = ddlMeioFormaPagamento.SelectedItem.Text + " - "+ ddlQuantidadeParcelas.SelectedItem.Text + " - " + txtDescricao.Text;
                itensFormaPagamento.QUANTIDADEPARCELAS = Convert.ToInt32(ddlQuantidadeParcelas.SelectedValue);
                
                contexto.SubmitChanges();

                divMensagem.Visible = true;
                divMensagem.Attributes["class"] += " alert-success ";
                divMensagem.InnerHtml += "As alterações foram salvas com sucesso!";

                carregarDados();

            }
            catch (Exception ex)
            {
                LogRegistro.registrarLog(ex.Message + Environment.NewLine + ex.StackTrace + Environment.NewLine, "PetShopBairro.Web", caminhoLog, dataExecucao);
                divMensagem.Visible = true;
                divMensagem.Attributes["class"] += " alert-danger ";
                divMensagem.InnerHtml += ex.Message;
            }
        }
       

        protected void btnGerarVenda_Click(object sender, EventArgs e)
        {
            try
            {
                BancoDeDadosPadraoDataContext contexto = new BancoDeDadosPadraoDataContext();
                VENDA venda = null;

                venda = new VENDA();
                contexto.VENDA.InsertOnSubmit(venda);

                venda.CRIADOPOR = Session["nomeUsuario"].ToString();
                venda.CRIADOEM = DateTime.Now;
               
                venda.IDLOCALESTOQUE = Convert.ToInt32(ddlLocalEstoqueVenda.SelectedValue);

                formaPagamentoLocalEstoque();

                venda.CODIGO = Convert.ToInt32(hdfCodigo.Value);
                venda.TIPOVENDA = Convert.ToInt32(ddlTipovenda.SelectedValue);

                var valoresItensVendas = (from i in contexto.ITENSVENDA
                                             where i.CODIGO == venda.CODIGO
                                             select i)
                                            .ToList();


                decimal VItensdaVendas = 0.00m;
                if (valoresItensVendas.Count > 0)
                {
                    VItensdaVendas = valoresItensVendas.Sum(x => x.VALOR);                  

                    venda.VALORTOTALITENS = Convert.ToDecimal(VItensdaVendas);
                }
                else
                {
                    throw new Exception("Não foi definida os itens para essa venda!");
                }

                var valoresFormaPagamento = (from f in contexto.FORMAPAGAMENTO
                                         where f.CODIGO == venda.CODIGO
                                             select f)
                                            .ToList();

                decimal valorTotalFP = 0.00m;
                if (valoresFormaPagamento.Count > 0)
                {
                    var VFormaPagamento = valoresFormaPagamento.Sum(x => x.VALORFORMAPAGAMENTO);
                    var VAcrescimo = valoresFormaPagamento.Sum(x => x.VALORACRESCIMO);
                    var VDesconto = valoresFormaPagamento.Sum(x => x.VALORDESCONTO);
                    valorTotalFP = Convert.ToDecimal((VFormaPagamento + VAcrescimo) - VDesconto);
                    venda.VALORTOTAL = valorTotalFP;
                }
                else
                {
                    throw new Exception("Não foi definida a forma de pagamento para essa venda!");
                }

                if (VItensdaVendas > valorTotalFP)
                {
                    throw new Exception("O valor total das formas de pagamento é menor que o valor total dos itens da venda");
                }

                contexto.SubmitChanges();                              

                Response.Redirect("vendas.aspx");

            }
            catch (Exception ex)
            {
                LogRegistro.registrarLog(ex.Message + Environment.NewLine + ex.StackTrace + Environment.NewLine, "PetShopBairro.Web", caminhoLog, dataExecucao);
                divMensagem.Visible = true;
                divMensagem.Attributes["class"] += " alert-danger ";
                divMensagem.InnerHtml += ex.Message;
            }
        }

        private void formaPagamentoLocalEstoque()
        {
            BancoDeDadosPadraoDataContext contextof = new BancoDeDadosPadraoDataContext();
            var formaPagamento = (from fp in contextof.FORMAPAGAMENTO
                              where fp.CODIGO == Convert.ToInt32(hdfCodigo.Value)
                              select fp).ToList();

            if(formaPagamento.Count > 0)
            {
                foreach (var item in formaPagamento)
                {
                    item.IDLOCALESTOQUE = Convert.ToInt32(ddlLocalEstoqueVenda.SelectedValue);
                }               

            }
            contextof.SubmitChanges();
        }


        protected void btnExcluirFormaPagamento_Click(object sender, EventArgs e)
        {
            try
            {
                     BancoDeDadosPadraoDataContext contexto = new BancoDeDadosPadraoDataContext();
                    var value = Convert.ToInt32(txtIdFormaPagamento.Text);
                    var itensFormaPagamento = (from f in contexto.FORMAPAGAMENTO
                                               where f.ID == value
                                               select f)
                                            .FirstOrDefault();

                    if (itensFormaPagamento != null)
                    {
                        contexto.FORMAPAGAMENTO.DeleteOnSubmit(itensFormaPagamento);

                        contexto.SubmitChanges();
                    }

                    carregarDados();
                

            }
            catch (Exception ex)
            {
                LogRegistro.registrarLog(ex.Message + Environment.NewLine + ex.StackTrace + Environment.NewLine, "PetShopBairro.Web", caminhoLog, dataExecucao);
                divMensagem.Visible = true;
                divMensagem.Attributes["class"] += " alert-danger ";
                divMensagem.InnerHtml += ex.Message;

            }
        }

        protected void btnExcluirItenVenda_Click(object sender, EventArgs e)
        {
            try
            {
                
                    BancoDeDadosPadraoDataContext contexto = new BancoDeDadosPadraoDataContext();
                    var value = Convert.ToInt32(txtItenVenda.Text);
                    var itensvenda = (from i in contexto.ITENSVENDA
                                      where i.ID == value
                                      select i)
                                            .FirstOrDefault();

                    if (itensvenda != null)
                    {
                        if (!string.IsNullOrEmpty(itensvenda.IDPRODUTOLOCALESTOQUE.ToString()))
                        {
                            incluirQuatidadeItensAoEstoque(itensvenda.QUANTIDADE, itensvenda.IDPRODUTOLOCALESTOQUE);
                            contexto.ITENSVENDA.DeleteOnSubmit(itensvenda);
                        }
                        else
                        {
                            contexto.ITENSVENDA.DeleteOnSubmit(itensvenda);
                        }

                        contexto.SubmitChanges();
                    }

                    carregarDados();             

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