<%@ Page Title="" Language="C#" MasterPageFile="~/adm/MasterAdm.Master" AutoEventWireup="true" CodeBehind="estoqueProdutosForm.aspx.cs" Inherits="PetShopDoBairro.adm.estoqueProdutosForm" %>

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
            <h6 class="m-0 font-weight-bold text-primary">Cadastro de Estoque Produtos</h6>
        </div>
        <div class="card-body col-lg-12">
            <div class="row">
                <div class="col-lg-6 " id="DivLocalEstoque" runat="server">
                    <label for="ddlLocalEstoque" class="control-label">Local Estoque</label>
                    <asp:DropDownList runat="server" ID="ddlLocalEstoque" DataTextField="NOME" DataValueField="ID" required="true"
                        CssClass="form-control">
                        <%--AutoPostBack="true" OnSelectedIndexChanged="ddlMeusClientes_SelectedIndexChanged">--%>
                    </asp:DropDownList>
                </div>

                <div class="col-lg-6 " id="dvNomeProdutoCodigo" runat="server">
                    <label for="ddlNomeProdutoCodigo" class="control-label">Produto</label>
                    <asp:DropDownList runat="server" ID="ddlProduto" DataTextField="NomeProdutoCodigo" DataValueField="Id" required="true"
                        CssClass="form-control">
                        <%--AutoPostBack="true" OnSelectedIndexChanged="ddlMeusClientes_SelectedIndexChanged">--%>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="row" style="padding-top: 10px;">
                <div class="form-group col-lg-4" >
                    <label>Valor Custo</label>
                    <asp:TextBox ID="txtValorCusto" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
                <div class="form-group col-lg-4">
                    <label>Valor Venda</label>
                    <asp:TextBox ID="txtValorVenda" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
                <div class="form-group col-lg-4">
                    <label>Quantidade</label>
                    <asp:TextBox ID="txtQuantidade" min="0" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
            <div class="form-group">
                <asp:Button runat="server" ID="btnSalvar" Text="Salvar" class="btn btn-primary" OnClick="btnSalvar_Click" />
            </div>
        </div>
    </div>


</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceScript" runat="server">
</asp:Content>
