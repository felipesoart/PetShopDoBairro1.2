<%@ Page Title="" Language="C#" MasterPageFile="~/adm/MasterAdm.Master" AutoEventWireup="true" CodeBehind="equipamentosForm.aspx.cs" Inherits="PetShopDoBairro.adm.equipamentosForm" %>

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
            <h6 class="m-0 font-weight-bold text-primary">Cadastro da Equipamentos</h6>
        </div>
        <div class="card-body">
            <div class="form-group">
                <label>Número de Série</label>
                <asp:TextBox runat="server" class="form-control" ID="txtNumeroSerie" />
            </div>
            <div class="form-group">
                <label>Número do Tomabamento</label>
                <asp:TextBox runat="server" class="form-control" ID="txtNumeroTomabamento" />
            </div>
            <div class="form-group">
                <label>Marca</label>
                <asp:TextBox runat="server" class="form-control" ID="txtMarca" />
            </div>
            <div class="form-group">
                <label>Tipo</label>
                <asp:TextBox runat="server" class="form-control" ID="txtTipo" />
            </div>
            <div class="form-group">
                <label>Modelo</label>
                <asp:TextBox runat="server" class="form-control" ID="txtModelo" />
            </div>

            <div class="form-group">
                <label>CLIENTE</label>
                <asp:DropDownList ID="ddlCliente" runat="server" CssClass="form-control"></asp:DropDownList>                
            </div>

            <div class="form-group">
                <asp:Button runat="server" ID="btnSalvar" Text="Salvar" class="btn btn-primary" OnClick="btnSalvar_Click" />
            </div>

        </div>
    </div>

    <!-- Page Heading -->
   
    <!-- DataTales Example -->
    <div class="card shadow mb-4">
        <div class="card-header py-3">
            <h6 class="m-0 font-weight-bold text-primary">Filiais</h6>
        </div>
        <div class="card-body">
            <div class="table-responsive">
                <a href='<%= "manutencaoEquipamentoForm.aspx?idEquipamento="+ Request["id"] %>' class="btn btn-primary">Novo</a>
                <table class="table table-bordered" id="dataTable" width="100%" cellspacing="0">
                    <thead>
                        <tr>
                            <th>#</th>                            
                            <th>Descrição</th>                            
                        </tr>
                    </thead>
                    <tbody>
                        <asp:Repeater runat="server" ID="rptManutencao">
                            <ItemTemplate>
                                <tr>
                                    <td>
                                        <a href=' <%# "manutencaoEquipamentoForm.aspx?idEquipamento=" + Eval("IDEQUIPAMENTO") + "&idManutencao=" + Eval("ID") %>'>
                                            <%# Eval("ID") %> 
                                        </a>
                                    </td>
                                    <td><%# Eval("DESCRICAO") %></td>
                                    
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
