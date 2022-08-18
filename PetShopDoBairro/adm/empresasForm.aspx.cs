using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PetShopDoBairro.adm
{
    public partial class empresasForm : System.Web.UI.Page
    {
        private string caminhoLog = ConfigurationManager.AppSettings["LogRegistro"];
        DateTime dataExecucao = DateTime.Now;

        private int _idEmpresa = -1;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request["id"] != null)
            {
                int.TryParse(Request["id"].ToString(), out _idEmpresa);

                if (!Page.IsPostBack)
                {

                    BancoDeDadosPadraoDataContext contexto = new BancoDeDadosPadraoDataContext();
                    var empresa = (from emp in contexto.EMPRESA
                                   where emp.ID == _idEmpresa
                                   select emp).FirstOrDefault();
                    if (empresa != null)
                    {
                        txtNomeFantasia.Text = empresa.NOMEFANTASIA;
                        txtRazaoSocial.Text = empresa.RAZAOSOCIAL;
                        txtCNPJ.Text = empresa.CNPJ;
                        txtEndereco.Text = empresa.ENDERECO;
                        txtBairro.Text = empresa.BAIRRO;
                        txtCidade.Text = empresa.CIDADE;
                        txtEstado.Text = empresa.ESTADO;
                        txtCEP.Text = empresa.CEP;
                        txtTelefone.Text = empresa.TELEFONE;
                        txtTelefone2.Text = empresa.TELEFONE2;
                        txtEmail.Text = empresa.EMAIL;

                        var filiais = (from f in contexto.FILIAL
                                       where f.IDEMPRESA == _idEmpresa
                                       select f).ToList();

                        rptFilial.DataSource = filiais;
                        rptFilial.DataBind();
                    }
                }
            }
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                BancoDeDadosPadraoDataContext contexto = new BancoDeDadosPadraoDataContext();
                EMPRESA empresa = null;
                if (_idEmpresa == -1)
                {
                    empresa = new EMPRESA();
                    contexto.EMPRESA.InsertOnSubmit(empresa);
                }
                else
                {
                    empresa = (from emp in contexto.EMPRESA
                               where emp.ID == _idEmpresa
                               select emp).FirstOrDefault();
                }

                empresa.NOMEFANTASIA = txtNomeFantasia.Text;
                empresa.RAZAOSOCIAL = txtRazaoSocial.Text;
                empresa.CNPJ = txtCNPJ.Text;
                empresa.ENDERECO = txtEndereco.Text;
                empresa.BAIRRO = txtBairro.Text;
                empresa.CIDADE = txtCidade.Text;
                empresa.ESTADO = txtEstado.Text;
                empresa.CEP = txtCEP.Text;
                empresa.TELEFONE = txtTelefone.Text;
                empresa.TELEFONE2 = txtTelefone2.Text;
                empresa.EMAIL = txtEmail.Text;

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