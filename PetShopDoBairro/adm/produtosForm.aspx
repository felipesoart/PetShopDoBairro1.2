<%@ Page Title="" Language="C#" MasterPageFile="~/adm/MasterAdm.Master" AutoEventWireup="true" CodeBehind="produtosForm.aspx.cs" Inherits="PetShopDoBairro.adm.produtosForm" %>

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
            <h6 class="m-0 font-weight-bold text-primary">Cadastro de Produto</h6>
        </div>
        <div class="card-body">
                       
            <div class="form-group">
                <label>Nome</label>
                <asp:TextBox ID="txtNome" runat="server" CssClass="form-control"></asp:TextBox>                
            </div>
            <div class="form-group">
                <label>Codigo</label>
                <asp:TextBox ID="txtCodigo" runat="server" CssClass="form-control"></asp:TextBox>                
            </div>

            <div class="form-group">
                <asp:Button runat="server" ID="btnSalvar" Text="Salvar" class="btn btn-primary" OnClick="btnSalvar_Click" />
            </div>

        </div>
    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceScript" runat="server">
   
</asp:Content>
