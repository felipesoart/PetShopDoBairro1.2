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
using ListItem = System.Web.UI.WebControls.ListItem;

namespace PetShopDoBairro.adm
{
    public partial class vendas : System.Web.UI.Page
    {
        private string caminhoLog = ConfigurationManager.AppSettings["LogRegistro"];
        DateTime dataExecucao = DateTime.Now;


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                carregarLocalEstoque();
            }

        }

        private void carregarDados()
        {
            BancoDeDadosPadraoDataContext contexto = new BancoDeDadosPadraoDataContext();

            var vendas = (from v in contexto.VENDA
                          select v).ToList();

            rptVendas.DataSource = vendas;
            rptVendas.DataBind();
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

                ddlLocalEstoque.Items.Insert(0, new ListItem("Todos...", "0"));

            }
        }

        protected void rptVendas_ItemCommand1(object source, RepeaterCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Excluir")
                {
                    BancoDeDadosPadraoDataContext contexto = new BancoDeDadosPadraoDataContext();
                    var valueCommandArgument = Convert.ToInt32(e.CommandArgument);
                    var venda = (from v in contexto.VENDA
                                 where v.ID == valueCommandArgument
                                 select v)
                                            .FirstOrDefault();

                    if (venda != null)
                    {
                        var formapagamento = (from f in contexto.FORMAPAGAMENTO
                                              where f.CODIGO == venda.CODIGO
                                              select f)
                                            .ToList();

                        contexto.FORMAPAGAMENTO.DeleteAllOnSubmit(formapagamento);

                        var itensvenda = (from i in contexto.ITENSVENDA
                                          where i.CODIGO == venda.CODIGO
                                          select i)
                                            .ToList();

                        foreach (var item in itensvenda)
                        {
                            if (!string.IsNullOrEmpty(item.IDPRODUTOLOCALESTOQUE.ToString()))
                            {
                                incluirQuatidadeItensAoEstoque(item.QUANTIDADE, item.IDPRODUTOLOCALESTOQUE);
                                contexto.ITENSVENDA.DeleteOnSubmit(item);
                            }
                            else
                            {
                                contexto.ITENSVENDA.DeleteOnSubmit(item);
                            }
                        }

                        contexto.VENDA.DeleteOnSubmit(venda);


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

        protected void btExportar_Click(object sender, EventArgs e)
        {
            BancoDeDadosPadraoDataContext contexto = new BancoDeDadosPadraoDataContext();

            int statusPossiveis = int.Parse(ddlEstado.SelectedValue);

            int produtosLocalEstoque = int.Parse(ddlLocalEstoque.SelectedValue);

            DateTime Inicio = DateTime.Parse("2022-01-01 00:00:00");
            DateTime Fim = DateTime.Now;
            if (!string.IsNullOrEmpty(txtInicio.Text) && !string.IsNullOrEmpty(txtFim.Text))
            {
                Inicio = DateTime.Parse(txtInicio.Text + " 00:00:00");
                Fim = DateTime.Parse(txtFim.Text + " 23:59:59");
            }


            List<VENDA> objetos = new List<VENDA>();
            if (produtosLocalEstoque == 0)
            {
                if (statusPossiveis != -1)
                {
                    objetos = (from r in contexto.VENDA
                               where r.TIPOVENDA == statusPossiveis
                                 && (r.CRIADOEM >= Inicio && r.CRIADOEM <= Fim)
                               select r)
                                                  .OrderByDescending(r => r.CRIADOEM)
                                                  .ToList();
                }
                else
                {
                    objetos = (from r in contexto.VENDA
                               where (r.CRIADOEM >= Inicio && r.CRIADOEM <= Fim)
                               select r)
                                                  .OrderByDescending(r => r.CRIADOEM)
                                                  .ToList();
                }

            }
            else
            {
                if (statusPossiveis != -1)
                {
                    objetos = (from r in contexto.VENDA
                               where r.TIPOVENDA == statusPossiveis
                                 && r.IDLOCALESTOQUE == produtosLocalEstoque
                                 && (r.CRIADOEM >= Inicio && r.CRIADOEM <= Fim)
                               select r)
                           .OrderByDescending(r => r.CRIADOEM)
                           .ToList();
                }
                else
                {
                    objetos = (from r in contexto.VENDA
                               where r.IDLOCALESTOQUE == produtosLocalEstoque
                                 && (r.CRIADOEM >= Inicio && r.CRIADOEM <= Fim)
                               select r)
                          .OrderByDescending(r => r.CRIADOEM)
                          .ToList();
                }
            }

            if (objetos != null)
            {
                if (!string.IsNullOrEmpty(txtInicio.Text) && !string.IsNullOrEmpty(txtFim.Text))
                {
                    if (Inicio.Date > Fim.Date)
                    {
                        throw new Exception("Data Inicio não pode ser maior que a Data Fim.");
                    }
                    else
                    {
                        objetos = objetos.Where(x => x.CRIADOEM >= Inicio && x.CRIADOEM <= Fim).ToList();
                    }
                }
            }

            var sb = new StringBuilder();           

            foreach (var itemVenda in objetos)
            {
                sb.AppendLine("\"ID VENDA\",\"DATA CRIACAO\",\"CODIGO\",\"VALOR TOTAL ITENS\",\"VALOR TOTAL FP\",\"TIPO VENDA\",\"LOCAL ESTOQUE\"");
                
                sb.AppendLine("\"" + itemVenda.ID + "\"," +
                   "\"" + itemVenda.CRIADOEM.ToString("dd/MM/yyyy") + "\"," +
                   "\"" + itemVenda.CODIGO + "\"," +
                   "\"" + Convert.ToDecimal(itemVenda.VALORTOTALITENS).ToString("C2") + "\"," +
                   "\"" + itemVenda.VALORTOTAL.ToString("C2") + "\"," +
                   "\"" + (itemVenda.TIPOVENDA == 0 ? "Entrada" : "Despesa") + "\"," +
                   "\"" + itemVenda.LOCALESTOQUE.NOME + "\","
                   );

                var objItem = (from i in contexto.ITENSVENDA
                               where i.CODIGO == itemVenda.CODIGO
                               select i)
                          .OrderByDescending(c => c.ID)
                          .ToList();

                sb.AppendLine("\"ID ITENS DA VENDA\",\"DATA CRIACAO\",\"CODIGO\",\"NOME PRODUTO\",\"DESCRICAO\",\"LOCAL ESTOQUE\",\"QUANTIDADE\",\"VALOR CUSTO\",\"VALOR VENDA\",\"VALOR TOTAL ITEM\"");

                foreach (var itemVendaItem in objItem)
                {
                    var NomeProduto = string.Empty;
                    var valorCusto = string.Empty;
                    var NomeLocalEstoque = string.Empty;
                    var valorItem = string.Empty;
                    if (string.IsNullOrEmpty(itemVendaItem.IDPRODUTOLOCALESTOQUE.ToString()))
                    {
                        NomeProduto = "";
                        valorCusto = (0.00).ToString("C2");
                        valorItem = (0.00).ToString("C2");
                        NomeLocalEstoque = (from l in contexto.LOCALESTOQUE
                                                where l.ID == itemVendaItem.IDLOCALESTOQUE
                                                select l.NOME).FirstOrDefault();
                    }
                    else
                    {
                        NomeProduto = itemVendaItem.PRODUTOLOCALESTOQUE.PRODUTO.NOME;
                        valorCusto = itemVendaItem.PRODUTOLOCALESTOQUE.VALORCUSTO.ToString("C2");
                        NomeLocalEstoque = itemVendaItem.PRODUTOLOCALESTOQUE.LOCALESTOQUE.NOME;
                        valorItem = itemVendaItem.PRODUTOLOCALESTOQUE.VALORVENDA.ToString("C2");
                    }  

                    sb.AppendLine("\"" + itemVendaItem.ID + "\"," +
                     "\"" + itemVendaItem.CRIADOEM.ToString("dd/MM/yyyy") + "\"," +
                     "\"" + itemVendaItem.CODIGO + "\"," +                    
                     "\"" + NomeProduto + "\"," +
                     "\"" + itemVendaItem.DESCRICAO + "\"," +
                     "\"" + NomeLocalEstoque + "\"," +
                     "\"" + itemVendaItem.QUANTIDADE.ToString("N2") + "\"," +
                     "\"" + valorCusto + "\"," +
                     "\"" + valorItem + "\"," +
                     "\"" + itemVendaItem.VALOR.ToString("C2") + "\","                     
                     );
                }

                var objFormaPagamento = (from f in contexto.FORMAPAGAMENTO
                               where f.CODIGO == itemVenda.CODIGO
                               select f)
                         .OrderByDescending(c => c.ID)
                         .ToList();

                
                sb.AppendLine("\"ID FORMA PAGAMENTO\",\"DATA CRIACOO\",\"CODIGO\",\"DESCRICAO\",\"NSU\",\"COD AUTORIZACAO\",\"VALOR ACRESCIMO\",\"VALOR FORMA PAGAMENTO\",\"VALOR DESCONTO\"");

                foreach (var itemFormaPagamento in objFormaPagamento)
                {

                    sb.AppendLine("\"" + itemFormaPagamento.ID + "\"," +
                     "\"" + itemFormaPagamento.CRIADOEM.ToString("dd/MM/yyyy") + "\"," +
                     "\"" + itemFormaPagamento.CODIGO + "\"," +
                     "\"" + itemFormaPagamento.DESCRICAO + "\"," +
                     "\"" + itemFormaPagamento.NSU + "\"," +
                     "\"" + itemFormaPagamento.CODAUTORIZACAO + "\"," +                    
                     "\"" + itemFormaPagamento.VALORACRESCIMO.ToString("C2") + "\"," +
                     "\"" + itemFormaPagamento.VALORFORMAPAGAMENTO.ToString("C2") + "\"," +
                     "\"" + itemFormaPagamento.VALORDESCONTO.ToString("C2") + "\"," 
                     );
                }
            }

            string attachment = "attachment; filename=VendaItensdaVendaeFormadePagamento.csv";
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ClearHeaders();
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.AddHeader("content-disposition", attachment);
            HttpContext.Current.Response.ContentType = "text/csv";
            HttpContext.Current.Response.AddHeader("Pragma", "public");
            HttpContext.Current.Response.ContentEncoding = Encoding.Default;
            HttpContext.Current.Response.Charset = Encoding.Default.EncodingName;

            HttpContext.Current.Response.Write(sb.ToString());
            HttpContext.Current.Response.End();
        }

        protected void btExportarVendas_Click(object sender, EventArgs e)
        {
            BancoDeDadosPadraoDataContext contexto = new BancoDeDadosPadraoDataContext();

            int statusPossiveis = int.Parse(ddlEstado.SelectedValue);

            int produtosLocalEstoque = int.Parse(ddlLocalEstoque.SelectedValue);

            DateTime Inicio = DateTime.Parse("2022-01-01 00:00:00");
            DateTime Fim = DateTime.Now;
            if (!string.IsNullOrEmpty(txtInicio.Text) && !string.IsNullOrEmpty(txtFim.Text))
            {
                Inicio = DateTime.Parse(txtInicio.Text + " 00:00:00");
                Fim = DateTime.Parse(txtFim.Text + " 23:59:59");
            }


            List<VENDA> objetos = new List<VENDA>();
            if (produtosLocalEstoque == 0)
            {
                if (statusPossiveis != -1)
                {
                    objetos = (from r in contexto.VENDA
                               where r.TIPOVENDA == statusPossiveis
                                 && (r.CRIADOEM >= Inicio && r.CRIADOEM <= Fim)
                               select r)
                                                  .OrderByDescending(r => r.CRIADOEM)
                                                  .ToList();
                }
                else
                {
                    objetos = (from r in contexto.VENDA
                               where (r.CRIADOEM >= Inicio && r.CRIADOEM <= Fim)
                               select r)
                                                  .OrderByDescending(r => r.CRIADOEM)
                                                  .ToList();
                }

            }
            else
            {
                if (statusPossiveis != -1)
                {
                    objetos = (from r in contexto.VENDA
                               where r.TIPOVENDA == statusPossiveis
                                 && r.IDLOCALESTOQUE == produtosLocalEstoque
                                 && (r.CRIADOEM >= Inicio && r.CRIADOEM <= Fim)
                               select r)
                           .OrderByDescending(r => r.CRIADOEM)
                           .ToList();
                }
                else
                {
                    objetos = (from r in contexto.VENDA
                               where r.IDLOCALESTOQUE == produtosLocalEstoque
                                 && (r.CRIADOEM >= Inicio && r.CRIADOEM <= Fim)
                               select r)
                          .OrderByDescending(r => r.CRIADOEM)
                          .ToList();
                }
            }

            if (objetos != null)
            {
                if (!string.IsNullOrEmpty(txtInicio.Text) && !string.IsNullOrEmpty(txtFim.Text))
                {
                    if (Inicio.Date > Fim.Date)
                    {
                        throw new Exception("Data Inicio não pode ser maior que a Data Fim.");
                    }
                    else
                    {
                        objetos = objetos.Where(x => x.CRIADOEM >= Inicio && x.CRIADOEM <= Fim).ToList();
                    }
                }
            }

            var sb = new StringBuilder();

                sb.AppendLine("\"ID VENDA\",\"DATA CRIACAO\",\"CODIGO\",\"VALOR TOTAL ITENS\",\"VALOR TOTAL FP\",\"TIPO VENDA\",\"LOCAL ESTOQUE\"");

            foreach (var itemVenda in objetos)
            {

                sb.AppendLine("\"" + itemVenda.ID + "\"," +
                   "\"" + itemVenda.CRIADOEM.ToString("dd/MM/yyyy") + "\"," +
                   "\"" + itemVenda.CODIGO + "\"," +
                    "\"" + Convert.ToDecimal(itemVenda.VALORTOTALITENS).ToString("C2") + "\"," +
                   "\"" + itemVenda.VALORTOTAL.ToString("C2") + "\"," +
                   "\"" + (itemVenda.TIPOVENDA == 0 ? "Entrada" : "Despesa") + "\"," +
                   "\"" + itemVenda.LOCALESTOQUE.NOME + "\","
                   );
            }

            string attachment = "attachment; filename=Vendas.csv";
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ClearHeaders();
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.AddHeader("content-disposition", attachment);
            HttpContext.Current.Response.ContentType = "text/csv";
            HttpContext.Current.Response.AddHeader("Pragma", "public");
            HttpContext.Current.Response.ContentEncoding = Encoding.Default;
            HttpContext.Current.Response.Charset = Encoding.Default.EncodingName;

            HttpContext.Current.Response.Write(sb.ToString());
            HttpContext.Current.Response.End();

        }

        protected void btExportarItensVendas_Click(object sender, EventArgs e)
        {
            BancoDeDadosPadraoDataContext contexto = new BancoDeDadosPadraoDataContext();
              
            int produtosLocalEstoque = int.Parse(ddlLocalEstoque.SelectedValue);

            DateTime Inicio = DateTime.Parse("2022-01-01 00:00:00");
            DateTime Fim = DateTime.Now;
            if (!string.IsNullOrEmpty(txtInicio.Text) && !string.IsNullOrEmpty(txtFim.Text))
            {
                Inicio = DateTime.Parse(txtInicio.Text + " 00:00:00");
                Fim = DateTime.Parse(txtFim.Text + " 23:59:59");
            }


            List<ITENSVENDA> objetos = new List<ITENSVENDA>();
            if (produtosLocalEstoque == 0)
            {
               
                    objetos = (from r in contexto.ITENSVENDA
                               where (r.CRIADOEM >= Inicio && r.CRIADOEM <= Fim)
                               select r)
                                                  .OrderByDescending(r => r.CRIADOEM)
                                                  .ToList();
               

            }
            else
            {               
                    objetos = (from r in contexto.ITENSVENDA
                               where r.IDLOCALESTOQUE == produtosLocalEstoque
                                 && (r.CRIADOEM >= Inicio && r.CRIADOEM <= Fim)
                               select r)
                          .OrderByDescending(r => r.CRIADOEM)
                          .ToList();                
            }

            if (objetos != null)
            {
                if (!string.IsNullOrEmpty(txtInicio.Text) && !string.IsNullOrEmpty(txtFim.Text))
                {
                    if (Inicio.Date > Fim.Date)
                    {
                        throw new Exception("Data Inicio não pode ser maior que a Data Fim.");
                    }
                    else
                    {
                        objetos = objetos.Where(x => x.CRIADOEM >= Inicio && x.CRIADOEM <= Fim).ToList();
                    }
                }
            }           

            var sb = new StringBuilder();

            sb.AppendLine("\"ID ITENS DA VENDA\",\"DATA CRIACAO\",\"CODIGO\",\"NOME PRODUTO\",\"DESCRICAO\",\"LOCAL ESTOQUE\",\"QUANTIDADE\",\"VALOR CUSTO\",\"VALOR VENDA\",\"VALOR TOTAL ITEM\"");

            foreach (var itemVendaItem in objetos)
            {
                var NomeProduto = string.Empty;
                var valorCusto = string.Empty;
                var NomeLocalEstoque = string.Empty;
                var valorItem = string.Empty;
                if (string.IsNullOrEmpty(itemVendaItem.IDPRODUTOLOCALESTOQUE.ToString()))
                {
                    NomeProduto = "";
                    valorCusto = (0.00).ToString("C2");
                    valorItem = (0.00).ToString("C2");
                    NomeLocalEstoque = (from l in contexto.LOCALESTOQUE
                                        where l.ID == itemVendaItem.IDLOCALESTOQUE
                                        select l.NOME).FirstOrDefault();
                }
                else
                {
                    NomeProduto = itemVendaItem.PRODUTOLOCALESTOQUE.PRODUTO.NOME;
                    valorCusto = itemVendaItem.PRODUTOLOCALESTOQUE.VALORCUSTO.ToString("C2");
                    NomeLocalEstoque = itemVendaItem.PRODUTOLOCALESTOQUE.LOCALESTOQUE.NOME;
                    valorItem = itemVendaItem.PRODUTOLOCALESTOQUE.VALORVENDA.ToString("C2");
                }

                sb.AppendLine("\"" + itemVendaItem.ID + "\"," +
                 "\"" + itemVendaItem.CRIADOEM.ToString("dd/MM/yyyy") + "\"," +
                 "\"" + itemVendaItem.CODIGO + "\"," +
                 "\"" + NomeProduto + "\"," +
                 "\"" + itemVendaItem.DESCRICAO + "\"," +
                 "\"" + NomeLocalEstoque + "\"," +
                 "\"" + itemVendaItem.QUANTIDADE.ToString("N2") + "\"," +
                 "\"" + valorCusto + "\"," +
                 "\"" + valorItem + "\"," +
                 "\"" + itemVendaItem.VALOR.ToString("C2") + "\","
                 );
            }

            string attachment = "attachment; filename=ItensdasVendas.csv";
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ClearHeaders();
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.AddHeader("content-disposition", attachment);
            HttpContext.Current.Response.ContentType = "text/csv";
            HttpContext.Current.Response.AddHeader("Pragma", "public");
            HttpContext.Current.Response.ContentEncoding = Encoding.Default;
            HttpContext.Current.Response.Charset = Encoding.Default.EncodingName;

            HttpContext.Current.Response.Write(sb.ToString());
            HttpContext.Current.Response.End();
        }

        protected void btExportarFormaPagaemnto_Click(object sender, EventArgs e)
        {
            BancoDeDadosPadraoDataContext contexto = new BancoDeDadosPadraoDataContext();

            int produtosLocalEstoque = int.Parse(ddlLocalEstoque.SelectedValue);

            DateTime Inicio = DateTime.Parse("2022-01-01 00:00:00");
            DateTime Fim = DateTime.Now;
            if (!string.IsNullOrEmpty(txtInicio.Text) && !string.IsNullOrEmpty(txtFim.Text))
            {
                Inicio = DateTime.Parse(txtInicio.Text + " 00:00:00");
                Fim = DateTime.Parse(txtFim.Text + " 23:59:59");
            }


            List<FORMAPAGAMENTO> objetos = new List<FORMAPAGAMENTO>();
            if (produtosLocalEstoque == 0)
            {

                objetos = (from r in contexto.FORMAPAGAMENTO
                           where (r.CRIADOEM >= Inicio && r.CRIADOEM <= Fim)
                           select r)
                                              .OrderByDescending(r => r.CRIADOEM)
                                              .ToList();

            }
            else
            {
                objetos = (from r in contexto.FORMAPAGAMENTO
                           where r.IDLOCALESTOQUE == produtosLocalEstoque
                             && (r.CRIADOEM >= Inicio && r.CRIADOEM <= Fim)
                           select r)
                      .OrderByDescending(r => r.CRIADOEM)
                      .ToList();
            }

            if (objetos != null)
            {
                if (!string.IsNullOrEmpty(txtInicio.Text) && !string.IsNullOrEmpty(txtFim.Text))
                {
                    if (Inicio.Date > Fim.Date)
                    {
                        throw new Exception("Data Inicio não pode ser maior que a Data Fim.");
                    }
                    else
                    {
                        objetos = objetos.Where(x => x.CRIADOEM >= Inicio && x.CRIADOEM <= Fim).ToList();
                    }
                }
            }

           
            var sb = new StringBuilder();

            sb.AppendLine("\"ID FORMA PAGAMENTO\",\"DATA CRIACOO\",\"CODIGO\",\"DESCRICAO\",\"NSU\",\"COD AUTORIZACAO\",\"VALOR ACRESCIMO\",\"VALOR FORMA PAGAMENTO\",\"VALOR DESCONTO\"");

            foreach (var itemFormaPagamento in objetos)
            {

                sb.AppendLine("\"" + itemFormaPagamento.ID + "\"," +
                 "\"" + itemFormaPagamento.CRIADOEM.ToString("dd/MM/yyyy") + "\"," +
                 "\"" + itemFormaPagamento.CODIGO + "\"," +
                 "\"" + itemFormaPagamento.DESCRICAO + "\"," +
                 "\"" + itemFormaPagamento.NSU + "\"," +
                 "\"" + itemFormaPagamento.CODAUTORIZACAO + "\"," +
                 "\"" + itemFormaPagamento.VALORACRESCIMO.ToString("C2") + "\"," +
                 "\"" + itemFormaPagamento.VALORFORMAPAGAMENTO.ToString("C2") + "\"," +
                 "\"" + itemFormaPagamento.VALORDESCONTO.ToString("C2") + "\","
                 );
            }        

        string attachment = "attachment; filename=FormasdePagamentos.csv";
        HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ClearHeaders();
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.AddHeader("content-disposition", attachment);
            HttpContext.Current.Response.ContentType = "text/csv";
            HttpContext.Current.Response.AddHeader("Pragma", "public");
            HttpContext.Current.Response.ContentEncoding = Encoding.Default;
            HttpContext.Current.Response.Charset = Encoding.Default.EncodingName;

            HttpContext.Current.Response.Write(sb.ToString());
            HttpContext.Current.Response.End();
        }

        protected void btPesquisar_Click(object sender, EventArgs e)
        {
            try
            {
                BancoDeDadosPadraoDataContext contexto = new BancoDeDadosPadraoDataContext();
               
                int statusPossiveis = int.Parse(ddlEstado.SelectedValue);

                int produtosLocalEstoque = int.Parse(ddlLocalEstoque.SelectedValue);                                                          

                DateTime Inicio = DateTime.Parse("2022-01-01 00:00:00");
                DateTime Fim = DateTime.Now;
                if (!string.IsNullOrEmpty(txtInicio.Text) && !string.IsNullOrEmpty(txtFim.Text))
                {
                    Inicio = DateTime.Parse(txtInicio.Text + " 00:00:00");
                    Fim = DateTime.Parse(txtFim.Text + " 23:59:59");
                }


                List<VENDA> objetos = new List<VENDA>();
                if (produtosLocalEstoque == 0)
                {
                    if (statusPossiveis != -1)
                    {
                        objetos = (from r in contexto.VENDA
                                   where r.TIPOVENDA == statusPossiveis
                                     && (r.CRIADOEM >= Inicio && r.CRIADOEM <= Fim)
                                   select r)
                                                      .OrderByDescending(r => r.CRIADOEM)
                                                      .ToList();
                    }
                    else
                    {
                        objetos = (from r in contexto.VENDA
                                   where (r.CRIADOEM >= Inicio && r.CRIADOEM <= Fim)
                                   select r)
                                                      .OrderByDescending(r => r.CRIADOEM)
                                                      .ToList();
                    }
                    
                }
                else
                {
                    if (statusPossiveis != -1)
                    {
                        objetos = (from r in contexto.VENDA
                                   where r.TIPOVENDA == statusPossiveis
                                     && r.IDLOCALESTOQUE == produtosLocalEstoque
                                     && (r.CRIADOEM >= Inicio && r.CRIADOEM <= Fim)
                                   select r)
                               .OrderByDescending(r => r.CRIADOEM)
                               .ToList();
                    }
                    else
                    {
                        objetos = (from r in contexto.VENDA
                                   where r.IDLOCALESTOQUE == produtosLocalEstoque
                                     && (r.CRIADOEM >= Inicio && r.CRIADOEM <= Fim)
                                   select r)
                              .OrderByDescending(r => r.CRIADOEM)
                              .ToList();
                    }
                }
                
                if (objetos != null)
                {                   
                    if (!string.IsNullOrEmpty(txtInicio.Text) && !string.IsNullOrEmpty(txtFim.Text))
                    {
                        if (Inicio.Date > Fim.Date)
                        {
                            throw new Exception("Data Inicio não pode ser maior que a Data Fim.");
                        }
                        else
                        {
                            objetos = objetos.Where(x => x.CRIADOEM >= Inicio && x.CRIADOEM <= Fim).ToList();
                        }
                    }

                    rptVendas.DataSource = objetos;
                    rptVendas.DataBind();
                }
            }
            catch (Exception ex)
            {
                LogRegistro.registrarLog(ex.Message + Environment.NewLine + ex.StackTrace + Environment.NewLine, "PetShopBairro.Web", caminhoLog, dataExecucao);
            }
        }
    }
}