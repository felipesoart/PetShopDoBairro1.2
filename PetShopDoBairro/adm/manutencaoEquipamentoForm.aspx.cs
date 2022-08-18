using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PetShopDoBairro.adm
{
    public partial class manutencaoEquipamentoForm : System.Web.UI.Page
    {
        private int _idManutencao = -1;
        private int _idEquipamento = -1;

        private string caminhoLog = ConfigurationManager.AppSettings["LogRegistro"];
        DateTime dataExecucao = DateTime.Now;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request["idEquipamento"] != null)
            {
                if (!int.TryParse(Request["idEquipamento"].ToString(), out _idEquipamento))
                {
                    Response.Write("Equipamento invalida");
                }  
            }


                if (Request["idManutencao"] != null)
            {
                int.TryParse(Request["idManutencao"].ToString(), out _idManutencao);

                if (!Page.IsPostBack)
                {

                    BancoDeDadosPadraoDataContext contexto = new BancoDeDadosPadraoDataContext();
                    var manutencao = (from m in contexto.MANUTENCAOEQUIPAMENTO
                                   where m.ID == _idManutencao
                                   select m).FirstOrDefault();
                    if (manutencao != null)
                    {
                        txtDescricao.Text = manutencao.DESCRICAO;                      
                                               
                    }
                }
            }
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            BancoDeDadosPadraoDataContext contexto = new BancoDeDadosPadraoDataContext();
            try
            {
                MANUTENCAOEQUIPAMENTO manutencao = null;
            if (_idManutencao == -1)
            {
                manutencao = new MANUTENCAOEQUIPAMENTO();
                contexto.MANUTENCAOEQUIPAMENTO.InsertOnSubmit(manutencao);

                    manutencao.CRIADOPOR = Session["nomeUsuario"].ToString();
                    manutencao.CRIADOEM = DateTime.Now;
                }
            else
            {
                manutencao = (from m in contexto.MANUTENCAOEQUIPAMENTO
                              where m.ID == _idManutencao
                           select m).FirstOrDefault();

                    manutencao.MODIFICADOPOR = Session["nomeUsuario"].ToString();
                    manutencao.MODIFICADOEM = DateTime.Now;
                }

            manutencao.DESCRICAO = txtDescricao.Text;
           
            manutencao.IDEQUIPAMENTO = Convert.ToInt32(Request["idEquipamento"].ToString());

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