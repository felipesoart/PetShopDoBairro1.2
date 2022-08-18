<%@ Page Title="" Language="C#" MasterPageFile="~/adm/MasterAdm.Master" AutoEventWireup="true" CodeBehind="produtos.aspx.cs" Inherits="PetShopDoBairro.adm.produtos" %>

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
   
    <!-- DataTales Example -->
    <div class="card shadow mb-4">
        <div class="card-header py-3">
       
                <h6 class="m-0 font-weight-bold text-primary">Lista de Produtos</h6>                 

        </div>

        <div class="card-body">
            <div class="table-responsive">
                <a href="produtosForm.aspx" class="btn btn-primary">Novo</a>                 
                <table class="table table-bordered" id="dataTable" width="100%" cellspacing="0">
                    <thead>
                        <tr>                           
                            <th>Nome</th>
                            <th>Código</th>
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
                        <asp:Repeater runat="server" ID="rptProdutos" OnItemCommand="rptProdutos_ItemCommand1">
                            <ItemTemplate>
                                <tr>
                                    <td>
                                        <a href=' <%# "produtosForm.aspx?id=" + Eval("ID") %>'>
                                            <%# Eval("NOME") %>
                                        </a>
                                    </td>

                                    <td><%# Eval("CODIGO") %></td>
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
