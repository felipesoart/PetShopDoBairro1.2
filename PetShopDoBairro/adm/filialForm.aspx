<%@ Page Title="" Language="C#" MasterPageFile="~/adm/MasterAdm.Master" AutoEventWireup="true" CodeBehind="filialForm.aspx.cs" Inherits="PetShopDoBairro.adm.filialForm" %>
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
            <h6 class="m-0 font-weight-bold text-primary">Cadastro da Filial</h6>
        </div>
        <div class="card-body">
            <div class="form-group">
                <label>Nome Fantasia</label>
                <asp:TextBox runat="server" class="form-control" ID="txtNomeFantasia" />
            </div>
            <div class="form-group">
                <label>Razão Social</label>
                <asp:TextBox runat="server" class="form-control" ID="txtRazaoSocial" />
            </div>
            <div class="form-group">
                <label>CNPJ</label>
                <asp:TextBox runat="server" class="form-control" ID="txtCNPJ" />
            </div>
            <div class="form-group">
                <label>Endereço</label>
                <asp:TextBox runat="server" class="form-control" ID="txtEndereco" />
            </div>
            <div class="form-group">
                <label>Bairro</label>
                <asp:TextBox runat="server"  class="form-control" ID="txtBairro" />
            </div>
            <div class="form-group">
                <label>Cidade</label>
                <asp:TextBox runat="server" class="form-control" ID="txtCidade" />
            </div>
            <div class="form-group">
                <label>Estado</label>
                <asp:TextBox runat="server" class="form-control" ID="txtEstado" />
            </div>
            <div class="form-group">
                <label>Cep</label>
                <asp:TextBox runat="server" class="form-control" ID="txtCEP" />
            </div>
            <div class="form-group">
                <label>Telefone</label>
                <asp:TextBox runat="server" class="form-control" ID="txtTelefone" />
            </div>
            <div class="form-group">
                <label>Telefone Secundário</label>
                <asp:TextBox runat="server" class="form-control" ID="txtTelefone2" />
            </div>
            <div class="form-group">
                <label>E-mail</label>
                <asp:TextBox runat="server" type="email" class="form-control" ID="txtEmail" />
            </div>
            <div class="form-group">
                <asp:Button runat="server" ID="btnSalvar" Text="Salvar" class="btn btn-primary" OnClick="btnSalvar_Click" />
            </div>

        </div>
    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceScript" runat="server">
</asp:Content>
