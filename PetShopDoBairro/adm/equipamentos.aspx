<%@ Page Title="" Language="C#" MasterPageFile="~/adm/MasterAdm.Master" AutoEventWireup="true" CodeBehind="equipamentos.aspx.cs" Inherits="PetShopDoBairro.adm.equipamentos" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">

      <!-- Page Heading -->
   
    <!-- DataTales Example -->
    <div class="card shadow mb-4">
        <div class="card-header py-3">
            <h6 class="m-0 font-weight-bold text-primary">Cadastro da Empresa</h6>
        </div>
        <div class="card-body">
            <div class="table-responsive">
                <a href="equipamentosForm.aspx" class="btn btn-primary">Novo</a>
                <table class="table table-bordered" id="dataTable" width="100%" cellspacing="0">
                    <thead>
                        <tr>
                            <th>Cliente</th> 
                            <th>Número de Série</th>
                            <th>Marca</th>
                            <th>Modelo</th>
                                                       
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
                        <asp:Repeater runat="server" ID="rptEquipamentos">
                            <ItemTemplate>
                                <tr>
                                    <td>
                                        <a href=' <%# "equipamentosForm.aspx?id=" + Eval("eq.ID") %>'> 
                                            <%# Eval("nomeUsuario") %>
                                        </a>
                                    </td>
                                        
                                    <td><%# Eval("eq.NUMEROSERIE") %></td>
                                    <td><%# Eval("eq.MARCA") %></td>
                                    <td><%# Eval("eq.MODELO") %></td>                                  
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
