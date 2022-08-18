<%@ Page Title="" Language="C#" MasterPageFile="~/adm/MasterAdm.Master" AutoEventWireup="true" CodeBehind="usuarioForm.aspx.cs" Inherits="PetShopDoBairro.adm.usuarioForm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">

    <div class="row">      
        <div class="row">
            <div class="col-lg-12">
                <div class="alert alert-dismissable" runat="server" id="divMensagem" visible="false" enableviewstate="false">
                    <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
                </div>
            </div>
        </div>
    </div>


    <div class="card shadow">
        <div class="card-header py-3">
            <h6 class="m-0 font-weight-bold text-primary">Cadastro do Usuário</h6>
        </div>
        <div class="card-body">

            <div class="form-group">
                <label>Tipo de pessoa</label>
                <asp:DropDownList ID="ddlTipoPessoa" runat="server" CssClass="form-control" AutoPostBack="true">
                    <asp:ListItem id="liGestor" runat="server" Value="0" Text="Gestor"></asp:ListItem>
                    <asp:ListItem id="liFunci" runat="server" Value="1" Text="Funcionário"></asp:ListItem>
                    <asp:ListItem Value="2" Text="Pessoa Física"></asp:ListItem>
                    <asp:ListItem Value="3" Text="Pessoa Jurídica"></asp:ListItem>                   
                </asp:DropDownList>
            </div>

            <div class="form-group">
                <label>Nome</label>
                <asp:TextBox runat="server" class="form-control" ID="txtNome" />
            </div>
            <div class="form-group">
                <label>CPF/CNPJ</label>
                <asp:TextBox runat="server" class="form-control" ID="txtCPFCNPJ" />
            </div>

            <div runat="server" id="divPessoaJuridica">
                <div class="form-group">
                    <label>Razão Social</label>
                    <asp:TextBox runat="server" class="form-control" ID="txtRazaoSocial" />
                </div>
                <div class="form-group">
                    <label>Inscrição Estadual</label>
                    <asp:TextBox runat="server" class="form-control" ID="txtInscricaoEstadual" />
                </div>

                <div class="form-group">
                    <label>Inscrição Municipal</label>
                    <asp:TextBox runat="server" class="form-control" ID="txtInscricaoMunicipal" />
                </div>
            </div>
            <div class="form-group">
                <label>E-mail</label>
                <asp:TextBox runat="server" type="email" class="form-control" ID="txtEmail" placeholder="nome@exemplo.com" />
            </div>
            <div class="form-group">
                <label>Senha</label>
                <asp:TextBox runat="server" TextMode="Password" class="form-control" ID="txtSenha" />
            </div>

            <div class="form-group">
                <label>Cidade</label>
                <asp:TextBox runat="server" class="form-control" ID="txtCidade" />
            </div>

             <div class="form-group">
                <label>CEP</label>
                <asp:TextBox runat="server" class="form-control" ID="txtCEP" />
            </div>

             <div class="form-group">
                <label>Bairro</label>
                <asp:TextBox runat="server" class="form-control" ID="txtBairro" />
            </div>

            <div class="form-group">
                <label>Endereço</label>
                <asp:TextBox runat="server" class="form-control" ID="txtEndereco" />
            </div>

            <div class="form-group">
                <label>Telefone/Celular/Whatsapp</label>
                <asp:TextBox runat="server" class="form-control" ID="txtTelCellWhat" />
            </div>

            <div class="form-group">
                <asp:CheckBox runat="server" ID="ckbInforWhat" Checked="true" />
                <label>Aceita receber informações pelo whatsapp?</label>
            </div>

            <div class="form-group">
                <asp:CheckBox runat="server" ID="ckbInforEmail" Checked="true" />
                <label>Aceita receber informações por e-mail?</label>
            </div>

             <div class="form-group">
                <label>Como conheceu a loja?</label>
                <asp:DropDownList ID="ddlConheceuLoja" runat="server" CssClass="form-control">
                    <asp:ListItem Value="0" Text="Indicação de um amigo"></asp:ListItem>
                    <asp:ListItem Value="1" Text="Redes sociais"></asp:ListItem>
                    <asp:ListItem Value="2" Text="Google"></asp:ListItem>
                    <asp:ListItem Value="3" Text="N/A"></asp:ListItem>
                </asp:DropDownList>
            </div>

            <div class="form-group">
                <asp:CheckBox runat="server" ID="ckbAtivo" Checked="true" />
                <label>Ativo</label>
            </div>
            <div class="form-group">
                <asp:Button runat="server" ID="btnSalvar" Text="Salvar" class="btn btn-primary" OnClick="btnSalvar_Click" />
            </div>

        </div>
    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceScript" runat="server">
</asp:Content>
