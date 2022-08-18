<%@ Page Title="" Language="C#" MasterPageFile="~/adm/MasterAdm.Master" AutoEventWireup="true" CodeBehind="empresas.aspx.cs" Inherits="PetShopDoBairro.adm.empresas" %>
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
                <a href="empresasForm.aspx" class="btn btn-primary">Novo</a>
                <table class="table table-bordered" id="dataTable" width="100%" cellspacing="0">
                    <thead>
                        <tr>
                            <th>Razão Social</th>
                            <th>Nome Fantasia</th>
                            <th>CNPJ</th>
                            <th>Endereço</th>
                            
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
                        <asp:Repeater runat="server" ID="rptEmpresas">
                            <ItemTemplate>
                                <tr>
                                    <td>
                                        <a href=' <%# "empresasForm.aspx?id=" + Eval("ID") %>'> 
                                            <%# Eval("RAZAOSOCIAL") %> 
                                        </a>
                                    </td>
                                        
                                    <td><%# Eval("NOMEFANTASIA") %></td>
                                    <td><%# Eval("CNPJ") %></td>
                                    <td><%# Eval("ENDERECO") %></td>                                  
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
