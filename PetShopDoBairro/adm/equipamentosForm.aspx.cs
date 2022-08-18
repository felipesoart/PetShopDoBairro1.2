using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PetShopDoBairro.adm
{
    public partial class equipamentosForm : System.Web.UI.Page
    {
        private string caminhoLog = ConfigurationManager.AppSettings["LogRegistro"];
        DateTime dataExecucao = DateTime.Now;

        private int _idEquipamento = -1;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request["id"] != null)
            {
                int.TryParse(Request["id"].ToString(), out _idEquipamento);

                if (!Page.IsPostBack)
                {

                    BancoDeDadosPadraoDataContext contexto = new BancoDeDadosPadraoDataContext();
                    var equipamento = (from eq in contexto.EQUIPAMENTO
                                   where eq.ID == _idEquipamento
                                   select eq).FirstOrDefault();
                    if (equipamento != null)
                    {
                        txtNumeroSerie.Text = equipamento.NUMEROSERIE;
                        txtNumeroTomabamento.Text = equipamento.NUMEROTOMABAMENTO;
                        txtMarca.Text = equipamento.MARCA;
                        txtTipo.Text = equipamento.TIPO;
                        txtModelo.Text = equipamento.MODELO;
                        ddlCliente.SelectedValue = equipamento.IDUSUARIO.ToString();

                        var manutenções = (from m in contexto.MANUTENCAOEQUIPAMENTO
                                       where m.IDEQUIPAMENTO == _idEquipamento
                                       select m).ToList();

                        rptManutencao.DataSource = manutenções;
                        rptManutencao.DataBind();
                    }
                }
            }
            if (!Page.IsPostBack)
            {
                carregarClientes();
            }
        }

        private void carregarClientes()
        {
            try
            {
                BancoDeDadosPadraoDataContext contexto = new BancoDeDadosPadraoDataContext();
                var clientes = (from u in contexto.USUARIO
                               where (u.TIPOPESSOA == "2" || u.TIPOPESSOA == "3") && u.ATIVO
                                select new { u, NomeCpfCnpj = u.CPFCNPJ + " - " + u.NOME, IdUsuario = u.ID }).ToList();

                if (clientes != null)
                {
                    ddlCliente.DataSource = clientes;
                    ddlCliente.DataTextField = "NomeCpfCnpj";
                    ddlCliente.DataValueField = "IdUsuario";
                    ddlCliente.DataBind();
                }
                ddlCliente.Items.Insert(0, new ListItem("SELECIONE", "0"));
            }
            catch (Exception ex)
            {
                LogRegistro.registrarLog(ex.Message + Environment.NewLine + ex.StackTrace + Environment.NewLine, "PetShopBairro.Web", caminhoLog, dataExecucao);
                divMensagem.Visible = true;
                divMensagem.Attributes["class"] += " alert-danger ";
                divMensagem.InnerHtml += ex.Message;
            }
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                BancoDeDadosPadraoDataContext contexto = new BancoDeDadosPadraoDataContext();
                EQUIPAMENTO equipamento = null;
                if (_idEquipamento == -1)
                {
                    equipamento = new EQUIPAMENTO();
                    contexto.EQUIPAMENTO.InsertOnSubmit(equipamento);

                    equipamento.CRIADOPOR = Session["nomeUsuario"].ToString();
                    equipamento.CRIADOEM = DateTime.Now;
                }
                else
                {
                    equipamento = (from eq in contexto.EQUIPAMENTO
                                   where eq.ID == _idEquipamento
                                   select eq).FirstOrDefault();

                    equipamento.MODIFICADOPOR = Session["nomeUsuario"].ToString();
                    equipamento.MODIFICADOEM = DateTime.Now;
                }

                equipamento.NUMEROSERIE = txtNumeroSerie.Text;
                equipamento.NUMEROTOMABAMENTO = txtNumeroTomabamento.Text;
                equipamento.MARCA = txtMarca.Text;
                equipamento.TIPO = txtTipo.Text;
                equipamento.MODELO = txtModelo.Text;

                equipamento.IDUSUARIO = Convert.ToInt32(ddlCliente.SelectedValue);

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