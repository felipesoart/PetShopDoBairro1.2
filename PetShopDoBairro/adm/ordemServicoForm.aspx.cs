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
    public partial class ordemServicoForm : System.Web.UI.Page
    {
        private string caminhoLog = ConfigurationManager.AppSettings["LogRegistro"];
        DateTime dataExecucao = DateTime.Now;

        private int _idOrdemServico = -1;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request["id"] != null)
            {
                int.TryParse(Request["id"].ToString(), out _idOrdemServico);

                if (!Page.IsPostBack)
                {
                    carregarDados();
                }
            }
            if (!Page.IsPostBack)
            {
                if (ckbAvaria.Checked)
                {
                    divAnexo.Visible = true;
                }
            }
            else
            {
                if (ckbAvaria.Checked)
                {
                    divAnexo.Visible = true;
                }               
            }
        }

        private void carregarDados()
        {
            BancoDeDadosPadraoDataContext contexto = new BancoDeDadosPadraoDataContext();
            var ordemServico = (from o in contexto.ORDEMSERVICO
                                where o.ID == _idOrdemServico
                                select o).FirstOrDefault();
            if (ordemServico != null)
            {
                txtDefeitoRepassadoCliente.Text = ordemServico.DEFEITOREPASSADOPELOCLIENTE;
                txtDefeitoApresentado.Text = ordemServico.DEFEITOAPRESENTADO;
                ckbAvaria.Checked = ordemServico.AVARIA;

                var idEquipamento = ordemServico.IDEQUIPAMENTO;

                    var objEquipamentoCliente = (from e in contexto.EQUIPAMENTO
                                                 where e.ID == idEquipamento
                                                 select new { Id = e.ID, DESCRICAO = "Nº de Série:" + e.NUMEROSERIE + " - CPF/CNPJ:" + e.USUARIO.CPFCNPJ + " - " + e.USUARIO.NOME, IdCliente = e.USUARIO.ID })
                           .FirstOrDefault();

                if (objEquipamentoCliente != null)
                {
                    txtEquipamentoCliente.Text = objEquipamentoCliente.DESCRICAO;
                    hdEquipamentoCliente.Value = objEquipamentoCliente.Id.ToString();
                }


                carregarAnexos(ordemServico.ORDEMSERVICOANEXO);
            }
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                BancoDeDadosPadraoDataContext contexto = new BancoDeDadosPadraoDataContext();
                ORDEMSERVICO ordemServico = null;
                if (_idOrdemServico == -1)
                {
                    ordemServico = new ORDEMSERVICO();
                    contexto.ORDEMSERVICO.InsertOnSubmit(ordemServico);

                    ordemServico.CRIADOPOR = Session["nomeUsuario"].ToString();
                    ordemServico.CRIADOEM = DateTime.Now;
                }
                else
                {
                    ordemServico = (from o in contexto.ORDEMSERVICO
                                    where o.ID == _idOrdemServico
                                   select o).FirstOrDefault();

                    ordemServico.MODIFICADOPOR = Session["nomeUsuario"].ToString();
                    ordemServico.MODIFICADOEM = DateTime.Now;
                }

                ordemServico.DEFEITOREPASSADOPELOCLIENTE = txtDefeitoRepassadoCliente.Text;
                ordemServico.DEFEITOAPRESENTADO = txtDefeitoApresentado.Text;
                ordemServico.AVARIA = ckbAvaria.Checked;
                ordemServico.IDEQUIPAMENTO = Convert.ToInt32(hdEquipamentoCliente.Value);

                if (ckbAvaria.Checked)
                {
                    if (!fileAnexo.HasFile)
                    {
                        throw new Exception("Por favor insira a imagem!");
                    }
                }

                if (fileAnexo.HasFile)
                {
                    foreach (HttpPostedFile file in fileAnexo.PostedFiles)
                    {
                        string fileName = Path.GetFileName(file.FileName);
                        int posicaoUltimoPonto = fileName.LastIndexOf('.');

                        string nomeArquivoOriginal = file.FileName.Substring(0, posicaoUltimoPonto);
                        var texto = AdmUtil.removerAcentos(nomeArquivoOriginal);

                        string nomeArquivoAjustado = texto.Replace(" ", "_").Replace(",", ".");
                        string extensaoArquivo = file.FileName.Substring(posicaoUltimoPonto + 1);
                        string nomeArquivoFinal = nomeArquivoAjustado + "-" + DateTime.Now.ToString("yyy-MM-dd-HH-mm-ss") + "." + extensaoArquivo;
                        file.SaveAs(Server.MapPath("~/OrdemServicoAnexo/") + nomeArquivoFinal);

                        ORDEMSERVICOANEXO anexo = new ORDEMSERVICOANEXO();
                        anexo.ARQUIVO = nomeArquivoFinal;
                        if (Request["id"] != null)
                        {
                            anexo.IDORDEMSERVICO = int.Parse(Request["id"]);
                        }

                        ordemServico.ORDEMSERVICOANEXO.Add(anexo);
                    }
                }

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

        protected void rptAnexos_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Download")
            {
                FileInfo arquivo = new FileInfo(Server.MapPath("~/OrdemServicoAnexo/") + e.CommandArgument);

                Response.Clear();
                Response.AddHeader("Content-Disposition", "attachment; filename=" + arquivo.Name);
                Response.AddHeader("Content-Length", arquivo.Length.ToString());
                Response.ContentType = "application/octet-stream";
                Response.WriteFile(arquivo.FullName);

                Response.End();
            }

            if (e.CommandName == "Excluir")
            {
                BancoDeDadosPadraoDataContext contexto = new BancoDeDadosPadraoDataContext();
                var valueCommandArgument = Convert.ToInt32(e.CommandArgument);
                var anexo = (from o in contexto.ORDEMSERVICOANEXO
                             where o.ID == valueCommandArgument
                             select o)
                                        .FirstOrDefault();

                if (anexo != null)
                {
                    if (File.Exists(Server.MapPath("~/OrdemServicoAnexo/") + anexo.ARQUIVO))
                    {
                        File.Delete(Server.MapPath("~/OrdemServicoAnexo/") + anexo.ARQUIVO);
                    }

                    contexto.ORDEMSERVICOANEXO.DeleteOnSubmit(anexo);

                    contexto.SubmitChanges();

                    carregarDados();
                }
            }
        }

        protected void carregarAnexos(IList<ORDEMSERVICOANEXO> anexos)
        {
            rptAnexos.DataSource = anexos;
            rptAnexos.DataBind();
        }

        [WebMethod]
        public static string[] pesquisaEquipamentoCliente(string prefixo)
        {
            List<string> equipamentoCliente = new List<string>();
            //  prefixo = prefixo.Replace(".", string.Empty).Replace("-", string.Empty).Replace("/", string.Empty).Replace("-", string.Empty);
            BancoDeDadosPadraoDataContext contexto = new BancoDeDadosPadraoDataContext();

            var objEquipamentoCliente = (from e in contexto.EQUIPAMENTO
                                where e.NUMEROSERIE.Contains(prefixo) || 
                                e.USUARIO.CPFCNPJ.Replace(".", string.Empty).Replace("-", string.Empty).Replace("/", string.Empty).Replace("-", string.Empty).Contains(prefixo)
                                select new { Id = e.ID, DESCRICAO = "Nº de Série: " + e.NUMEROSERIE + " - CPF/CNPJ: " + e.USUARIO.CPFCNPJ + " - " + e.USUARIO.NOME, IdCliente = e.USUARIO.ID })
                            .Distinct().OrderBy(c => c.DESCRICAO).Take(25);
            
            if (objEquipamentoCliente.Count() > 0)
            {
                foreach (var item in objEquipamentoCliente)
                {
                    equipamentoCliente.Add(string.Format("{0};{1}", item.DESCRICAO, item.Id));
                }
            }

            return equipamentoCliente.ToArray();
        }

    }
}