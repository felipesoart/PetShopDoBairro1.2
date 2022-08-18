using PetShopDoBairro.adm;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PetShopDoBairro
{
    public partial class login : System.Web.UI.Page
    {
        private string caminhoLog = ConfigurationManager.AppSettings["LogRegistro"];
        DateTime dataExecucao = DateTime.Now;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnEntrar_Click(object sender, EventArgs e)
        {
            try
            {
                BancoDeDadosPadraoDataContext contexto = new BancoDeDadosPadraoDataContext();
                var usuario = (from u in contexto.USUARIO
                               where u.EMAIL == txtEmail.Text && u.SENHA == txtSenha.Text
                               && u.ATIVO
                               select u).FirstOrDefault();
                if (usuario != null && (usuario.TIPOPESSOA != "2" && usuario.TIPOPESSOA != "3"))
                {
                    //autenticado com sucesso
                    string usuarioEmail = txtEmail.Text;
                    Session["email"] = txtEmail.Text;
                    Session["nomeUsuario"] = usuario.NOME;
                    Session["tipoPessoa"] = usuario.TIPOPESSOA;

                    FormsAuthentication.RedirectFromLoginPage(usuarioEmail, true);
                    Response.Redirect("adm/BemVindo.aspx");                    
                }
                else
                {
                    throw new Exception("Usuário não permitido!"); 
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
    }
}