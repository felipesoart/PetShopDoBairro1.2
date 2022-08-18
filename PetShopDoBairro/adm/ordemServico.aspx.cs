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
    public partial class ordemServico : System.Web.UI.Page
    {
        private string caminhoLog = ConfigurationManager.AppSettings["LogRegistro"];
        DateTime dataExecucao = DateTime.Now;
        

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                carregarDados();
                //GeradorDePdf gerador = new GeradorDePdf();
                //gerador.DesserializerPessoas();
                //gerador.GerarRelatorioEmPDF(5);
            }
            
        }

        private void carregarDados()
        {
            BancoDeDadosPadraoDataContext contexto = new BancoDeDadosPadraoDataContext();

            var ordemServicos = (from o in contexto.ORDEMSERVICO
                                 select new
                                 {
                                     o,
                                     nomeUsuario = o.EQUIPAMENTO.USUARIO.CPFCNPJ + " - " + o.EQUIPAMENTO.USUARIO.NOME,
                                     numeroSerieEquipamento = o.EQUIPAMENTO.NUMEROSERIE + " - " + o.EQUIPAMENTO.MODELO
                                 }).ToList();

            rptOrdemServicos.DataSource = ordemServicos;
            rptOrdemServicos.DataBind();
        }

        protected void rptOrdemServicos_ItemCommand1(object source, RepeaterCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Excluir")
                {
                    BancoDeDadosPadraoDataContext contexto = new BancoDeDadosPadraoDataContext();
                    var valueCommandArgument = Convert.ToInt32(e.CommandArgument);
                    var ordemServico = (from o in contexto.ORDEMSERVICO
                                        where o.ID == valueCommandArgument
                                        select o)
                                            .FirstOrDefault();

                    if (ordemServico != null)
                    {
                        contexto.ORDEMSERVICO.DeleteOnSubmit(ordemServico);

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

                if (ex.Message.Contains("FK_ORDEMSERVICOANEXO_ORDEMSERVICO"))
                {
                    divMensagem.InnerHtml += "Existe arquivos em anexo para esta ordem de serviço, por favor excluir os anexos antes de excluir a ordem de serviço!";
                }
                else
                {
                    divMensagem.InnerHtml += ex.Message;
                }
            }
        }

        protected void btExportar_Click(object sender, EventArgs e)
        {
            BancoDeDadosPadraoDataContext contexto = new BancoDeDadosPadraoDataContext();
            var objetos = (from o in contexto.ORDEMSERVICO
                           select o)
                           .OrderByDescending(c => c.ID)
                           .ToList();

            var sb = new StringBuilder();
            sb.AppendLine("ID ORDEMSERVICO,DATA CRIAÇÃO,DEFEITO REPASSADO PELO CLIENTE,DEFEITO APRESENTADO,EXISTE ALGUM TIPO DE AVARIA?,NÚMERO DE SÉRIE DO EQUIPAMENTO,MARCA,TIPO,MODELO,CPF/CNPJ,NOME,RAZÃO SOCIAL");

            foreach (var item in objetos)
            {
                sb.AppendLine("\"" + item.ID + "\"," +
                   "\"" + item.CRIADOEM.ToString("dd/MM/yyyy") + "\", " +
                   "\"" + item.DEFEITOREPASSADOPELOCLIENTE + "\"," +
                   "\"" + item.DEFEITOAPRESENTADO + "\"," +
                   "\"" + (item.AVARIA == true ? "Sim" : "Não") + "\"," +
                   "\"" + item.EQUIPAMENTO.NUMEROSERIE + "\"," +
                   "\"" + item.EQUIPAMENTO.MARCA + "\"," +
                   "\"" + item.EQUIPAMENTO.TIPO + "\"," +
                   "\"" + item.EQUIPAMENTO.MODELO + "\"," +
                   "\"" + item.EQUIPAMENTO.USUARIO.CPFCNPJ + "\"," +
                   "\"" + item.EQUIPAMENTO.USUARIO.NOME + "\"," +
                   "\"" + item.EQUIPAMENTO.USUARIO.RAZAOSOCIAL + "\"," 
                   );
            }

            string attachment = "attachment; filename=Ordens de Serviço.csv";
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


       
    }
}