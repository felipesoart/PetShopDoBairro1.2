<%@ Page Title="" Language="C#" MasterPageFile="~/adm/MasterAdm.Master" AutoEventWireup="true" CodeBehind="vendas.aspx.cs" Inherits="PetShopDoBairro.adm.vendas" %>

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
    <!-- Page Heading -->

    <div class="card shadow mb-4">
        <div class="card-header py-3">
            <h6 class="m-0 font-weight-bold text-primary">Pesquisar</h6>
        </div>
        <div class="card-body">
            <div class="table-responsive">
                <a href="vendasForm.aspx" class="btn btn-primary">Novo</a>
            </div>
            <div class="row">
                <div class="form-group col-lg-2">
                    <label for="ddlProduto">Local Estoque</label>
                    <asp:DropDownList runat="server" ID="ddlLocalEstoque" AutoPostBack="true" DataValueField="Id" CssClass="form-control" DataTextField="Nome">
                    </asp:DropDownList>
                </div>
                                             
                <div class="form-group col-lg-2">
                    <label>Tipo de Venda</label>
                    <asp:DropDownList ID="ddlEstado" runat="server" CssClass="form-control">
                        <asp:ListItem Selected="True" Value="-1" Text="Todos"></asp:ListItem>
                        <asp:ListItem Value="0" Text="Entrada"></asp:ListItem>
                        <asp:ListItem Value="1" Text="Despesa"></asp:ListItem>
                    </asp:DropDownList>
                </div>
          

                <div class="col-lg-4">
                    <div class="form-group">
                        <label>Data de Entrega Inicial</label>
                        <asp:TextBox type="date"
                            runat="server" ID="txtInicio" CssClass="form-control" />
                    </div>
                </div>
                <div class="col-lg-4">
                    <div class="form-group">
                        <label>Data Entrega Final</label>
                        <asp:TextBox type="date"
                            runat="server" ID="txtFim" CssClass="form-control" />
                    </div>
                </div>
                <div class="form-group col-lg-12">
                    <asp:Button runat="server" ID="btPesquisar" Text="Pesquisar" OnClick="btPesquisar_Click" class="btn-u" />
                    <asp:Button runat="server" ID="Button1" Text="Exportar CSV Formas de Pagamentos" CausesValidation="false" Style="float: right; display: inline-block; text-align: right; margin-left: 10px;"
                        CssClass="btn btn-primary" OnClick="btExportarFormaPagaemnto_Click"></asp:Button>
                    <asp:Button runat="server" ID="Button2" Text="Exportar CSV Itens das Vendas" CausesValidation="false" Style="float: right; display: inline-block; text-align: right; margin-left: 10px;"
                        CssClass="btn btn-primary" OnClick="btExportarItensVendas_Click"></asp:Button>
                    <asp:Button runat="server" ID="Button3" Text="Exportar CSV Vendas" CausesValidation="false" Style="float: right; display: inline-block; text-align: right; margin-left: 10px;"
                        CssClass="btn btn-primary" OnClick="btExportarVendas_Click"></asp:Button>
                    <asp:Button runat="server" ID="Button4" Text="Exportar CSV Detalhado" CausesValidation="false" Style="float: right; display: inline-block; text-align: right; margin-left: 10px;"
                        CssClass="btn btn-primary" OnClick="btExportar_Click"></asp:Button>
                </div>
            </div>

        </div>
    </div>
    <!-- DataTales Example -->
    <div class="card shadow mb-4">
        <div class="card-header py-3">
            <h6 class="m-0 font-weight-bold text-primary">Lista de Vendas</h6>
        </div>
        <div class="card-body">
            <div class="table-responsive">              
                <table class="table table-bordered" id="dataTable" width="100%" cellspacing="0">
                    <thead>
                        <tr>
                            <th>Data Hora</th>
                            <th>Código</th>
                            <th>Valor Total</th>
                            <th>Tipo da Venda</th>
                            <th>Local Estoque</th>
                            <th>Ação</th>

                        </tr>
                    </thead>
                    <%-- <tfoot>
                        <tr>
                            <th>Name</th>
                            <th>Position</th>
                            <th>Office</th>
                            <th>Age</th>
                            <th>Start date</th>
                            <th>Salary</th>
                        </tr>
                    </tfoot>--%>
                    <tbody>
                        <asp:Repeater runat="server" ID="rptVendas" OnItemCommand="rptVendas_ItemCommand1">
                            <ItemTemplate>
                                <tr>
                                    <td>
                                        <a href=' <%# "vendasForm.aspx?cod=" + Eval("CODIGO") %>'>
                                            <%# Eval("CRIADOEM", "{0:dd/MM/yyyy}") %>
                                        </a>
                                    </td>

                                    <td><%# Eval("CODIGO") %></td>
                                    <td><%# Eval("VALORTOTAL", "{0:c2}")  %></td>
                                    <td><%# Convert.ToString(Eval("TIPOVENDA")) == "0" ? "Entrada" : "Despesa" %></td>

                                    <td><%# Eval("LOCALESTOQUE.NOME") %></td>
                                    <td>
                                        <asp:Button runat="server" ID="btExcluir" Text="Excluir" OnClientClick="return confirm('Você tem certeza que deseja excluir?');" CommandName="Excluir"
                                            CommandArgument='<%# Eval("ID") %>' class="btn-u btn-u-default"></asp:Button></td>

                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>

                    </tbody>
                </table>
            </div>
        </div>
    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceScript" runat="server">
    <script>
        // Call the dataTables jQuery plugin
        $(document).ready(function () {
            $('#dataTable').DataTable();
        });
    </script>

</asp:Content>
