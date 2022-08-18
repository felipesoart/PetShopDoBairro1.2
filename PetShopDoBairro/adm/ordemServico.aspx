<%@ Page Title="" Language="C#" MasterPageFile="~/adm/MasterAdm.Master" AutoEventWireup="true" CodeBehind="ordemServico.aspx.cs" Inherits="PetShopDoBairro.adm.ordemServico" %>

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
       
                <h6 class="m-0 font-weight-bold text-primary">Lista da Ordem Servico</h6>                 

        </div>

        <div class="card-body">
            <div class="table-responsive">
                <a href="ordemServicoForm.aspx" class="btn btn-primary">Novo</a>
                 <asp:Button runat="server" ID="btExportar" Text="Exportar CSV" CausesValidation="false" style="float: right; display: inline-block; text-align: right; margin-left: 10px; "
                    CssClass="btn btn-primary" OnClick="btExportar_Click"></asp:Button>
                <table class="table table-bordered" id="dataTable" width="100%" cellspacing="0">
                    <thead>
                        <tr>
                            <th>Número de Série do equipamento</th>
                            <th>Cliente</th>
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
                        <asp:Repeater runat="server" ID="rptOrdemServicos" OnItemCommand="rptOrdemServicos_ItemCommand1">
                            <ItemTemplate>
                                <tr>
                                    <td>
                                        <a href=' <%# "ordemServicoForm.aspx?id=" + Eval("o.ID") %>'>
                                            <%# Eval("numeroSerieEquipamento") %>
                                        </a>
                                    </td>

                                    <td><%# Eval("nomeUsuario") %></td>
                                    <td>
                                        <asp:Button runat="server" ID="btExcluir" Text="Excluir" OnClientClick="return confirm('Você tem certeza que deseja excluir?');" CommandName="Excluir"
                                            CommandArgument='<%# Eval("o.ID") %>' class="btn-u btn-u-default"></asp:Button></td>

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
