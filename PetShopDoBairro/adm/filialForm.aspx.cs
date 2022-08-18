using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PetShopDoBairro.adm
{
    public partial class filialForm : System.Web.UI.Page
    {
        private int _idFilial = -1;
        private int _idEmpresa = -1;

        private string caminhoLog = ConfigurationManager.AppSettings["LogRegistro"];
        DateTime dataExecucao = DateTime.Now;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request["idEmpresa"] != null)
            {
                if (!int.TryParse(Request["idEmpresa"].ToString(), out _idEmpresa))
                {
                    Response.Write("Empresa invalida");
                }  
            }


                if (Request["idFilial"] != null)
            {
                int.TryParse(Request["idFilial"].ToString(), out _idFilial);

                if (!Page.IsPostBack)
                {

                    BancoDeDadosPadraoDataContext contexto = new BancoDeDadosPadraoDataContext();
                    var filial = (from emp in contexto.FILIAL
                                   where emp.ID == _idFilial
                                   select emp).FirstOrDefault();
                    if (filial != null)
                    {
                        txtNomeFantasia.Text = filial.NOMEFANTASIA;
                        txtRazaoSocial.Text = filial.RAZAOSOCIAL;
                        txtCNPJ.Text = filial.CNPJ;
                        txtEndereco.Text = filial.ENDERECO;
                        txtBairro.Text = filial.BAIRRO;
                        txtCidade.Text = filial.CIDADE;
                        txtEstado.Text = filial.ESTADO;
                        txtCEP.Text = filial.CEP;
                        txtTelefone.Text = filial.TELEFONE;
                        txtTelefone2.Text = filial.TELEFONE2;
                        txtEmail.Text = filial.EMAIL;
                                               
                    }
                }
            }
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            BancoDeDadosPadraoDataContext contexto = new BancoDeDadosPadraoDataContext();
            try
            {
                FILIAL filial = null;
            if (_idFilial == -1)
            {
                filial = new FILIAL();
                contexto.FILIAL.InsertOnSubmit(filial);
            }
            else
            {
                filial = (from f in contexto.FILIAL
                           where f.ID == _idFilial
                           select f).FirstOrDefault();
            }

            filial.NOMEFANTASIA = txtNomeFantasia.Text;
            filial.RAZAOSOCIAL = txtRazaoSocial.Text;
            filial.CNPJ = txtCNPJ.Text;
            filial.ENDERECO = txtEndereco.Text;
            filial.BAIRRO = txtBairro.Text;
            filial.CIDADE = txtCidade.Text;
            filial.ESTADO = txtEstado.Text;
            filial.CEP = txtCEP.Text;
            filial.TELEFONE = txtTelefone.Text;
            filial.TELEFONE2 = txtTelefone2.Text;
            filial.EMAIL = txtEmail.Text;
            filial.IDEMPRESA = Convert.ToInt32(Request["idEmpresa"].ToString());

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