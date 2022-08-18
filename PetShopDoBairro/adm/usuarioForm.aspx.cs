using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PetShopDoBairro.adm
{
    public partial class usuarioForm : System.Web.UI.Page
    {
        private int _idUsuario = -1;
        private string caminhoLog = ConfigurationManager.AppSettings["LogRegistro"];
        DateTime dataExecucao = DateTime.Now;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request["id"] != null)
            {
                int.TryParse(Request["id"].ToString(), out _idUsuario);

                if (!Page.IsPostBack)
                {

                    BancoDeDadosPadraoDataContext contexto = new BancoDeDadosPadraoDataContext();
                    var usuario = (from u in contexto.USUARIO
                                   where u.ID == _idUsuario
                                   select u).FirstOrDefault();
                    if (usuario != null)
                    {
                        txtNome.Text = usuario.NOME;
                        txtCPFCNPJ.Text = usuario.CPFCNPJ;
                        txtEmail.Text = usuario.EMAIL;
                        txtSenha.Text = usuario.SENHA;
                        ddlTipoPessoa.SelectedValue = usuario.TIPOPESSOA;

                        txtRazaoSocial.Text = usuario.RAZAOSOCIAL;
                        txtInscricaoEstadual.Text = usuario.INSCRICAOESTADUAL;
                        txtInscricaoMunicipal.Text = usuario.INSCRICAOMUNICIPAL;
                        txtEndereco.Text = usuario.ENDERECO;
                        txtCEP.Text = usuario.CEP;
                        txtBairro.Text = usuario.BAIRRO;
                        txtCidade.Text = usuario.CIDADE;
                        txtTelCellWhat.Text = usuario.TELCELLWHAT;                      

                        ddlConheceuLoja.SelectedValue = usuario.CONHECEUATRAVES;

                        ckbInforWhat.Checked = usuario.RECEBERINFORWHATSAPP;
                        ckbInforEmail.Checked = usuario.RECEBERINFOREMAIL;
                        ckbAtivo.Checked = usuario.ATIVO;
                    }
                }
            }
            if (ddlTipoPessoa.SelectedValue=="3")//PessoaJuridica
            {
                divPessoaJuridica.Visible = true;
            }
            else
            {
                divPessoaJuridica.Visible = false;
            }

            if (Session["tipoPessoa"].ToString() == "1")//Funcionario
            {
                liGestor.Enabled = false;
                liFunci.Enabled = false;
            }
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            BancoDeDadosPadraoDataContext contexto = new BancoDeDadosPadraoDataContext();
            try
            {
                USUARIO usuario = null;
                if (_idUsuario == -1)//novo usuario
                {
                    usuario = new USUARIO();
                    contexto.USUARIO.InsertOnSubmit(usuario);

                    usuario.CRIADOPOR = Session["nomeUsuario"].ToString();
                    usuario.CRIADOEM = DateTime.Now;
                }
                else
                {
                    usuario = (from u in contexto.USUARIO
                               where u.ID == _idUsuario
                               select u).FirstOrDefault();

                    usuario.MODIFICADOPOR = Session["nomeUsuario"].ToString();
                    usuario.MODIFICADOEM = DateTime.Now;
                }

                usuario.NOME = txtNome.Text;
                usuario.CPFCNPJ = txtCPFCNPJ.Text;
                usuario.EMAIL = txtEmail.Text;
                usuario.SENHA = txtSenha.Text;
                usuario.TIPOPESSOA = ddlTipoPessoa.SelectedValue;

                usuario.RAZAOSOCIAL = txtRazaoSocial.Text;
                usuario.INSCRICAOESTADUAL = txtInscricaoEstadual.Text;
                usuario.INSCRICAOMUNICIPAL = txtInscricaoMunicipal.Text;
                usuario.ENDERECO = txtEndereco.Text;
                usuario.CEP = txtCEP.Text;
                usuario.BAIRRO = txtBairro.Text;
                usuario.CIDADE = txtCidade.Text;
                usuario.TELCELLWHAT = txtTelCellWhat.Text;

                usuario.CONHECEUATRAVES = ddlConheceuLoja.SelectedValue;

                usuario.RECEBERINFORWHATSAPP = ckbInforWhat.Checked;
                usuario.RECEBERINFOREMAIL = ckbInforEmail.Checked;

                usuario.ATIVO = ckbAtivo.Checked;               

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