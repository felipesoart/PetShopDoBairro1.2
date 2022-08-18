<%@ Page Title="" Language="C#" MasterPageFile="~/adm/MasterAdm.Master" AutoEventWireup="true" CodeBehind="vendasForm.aspx.cs" Inherits="PetShopDoBairro.adm.vendasForm" %>

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
    <%-- OPEN ITENS DA VENDA --%>
    <div class="card shadow" id="divItenVenda" runat="server" style="margin-bottom: 20px;">
        <div class="card-header py-3">
            <h6 class="m-0 font-weight-bold text-primary">Cadastro de itens</h6>
        </div>
        <div class="card-body">
            <div class="row">
                <div class="form-group col-lg-2">
                    <label>Tipo de Venda</label>
                    <asp:DropDownList ID="ddlTipovenda" runat="server" CssClass="form-control" AutoPostBack="true">
                        <asp:ListItem Value="0" Text="Entrada"></asp:ListItem>
                        <asp:ListItem Value="1" Text="Despesa"></asp:ListItem>
                    </asp:DropDownList>
                    <asp:HiddenField runat="server" ID="hdfCodigo" />
                </div>
            </div>
            <div class="row">
                <div class="col-lg-4 " id="DivLocalEstoque" runat="server">
                    <label for="ddlLocalEstoque" class="control-label">Local Estoque</label>
                    <asp:DropDownList runat="server" ID="ddlLocalEstoque" DataTextField="NOME" DataValueField="ID" required="true"
                        CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlLocalEstoque_SelectedIndexChanged">
                    </asp:DropDownList>
                </div>

                <div class="col-lg-8 " id="divProdutoLocalEstoque" runat="server">
                    <label for="ddlProdutosLocalEstoque" class="control-label">Produto Estoque</label>
                    <asp:DropDownList runat="server" ID="ddlProdutosLocalEstoque" DataTextField="NOME" DataValueField="ID" required="true"
                        CssClass="form-control" OnSelectedIndexChanged="ddlProdutosLocalEstoque_SelectedIndexChanged">
                    </asp:DropDownList>
                    <asp:HiddenField runat="server" ID="hdfProdutosLocalEstoqueValorVenda" />
                </div>

                <div class="form-group col-lg-6" id="divDecricaoDespesa" runat="server">
                    <label>Descrição Despesa</label>
                    <asp:TextBox runat="server" class="form-control" ID="txtdecricaoDespesa" />
                </div>
                <div class="form-group col-lg-2" id="divValorGasto" runat="server">
                    <label>Valor Gasto</label>
                    <asp:TextBox ID="txtValorGasto" runat="server" CssClass="form-control"></asp:TextBox>
                </div>

            </div>
            <div class="row" style="padding-top: 15px;" id="divQuantidade" runat="server">
                <div class="form-group col-lg-4">
                    <label>Quantidade</label>
                    <asp:TextBox ID="txtQuantidade" min="1" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
            <div class="form-group">
                <asp:Button runat="server" ID="btnAdicionarItem" Text="Adicionar item" class="btn btn-primary" OnClick="btnAdicionarItem_Click" />
            </div>

        </div>
    </div>

    <!-- Page Heading -->

    <!-- DataTales Example -->
    <div class="card shadow mb-4">
        <div class="card-header py-3">
            <h6 class="m-0 font-weight-bold text-primary">Lista de itens da venda</h6>
        </div>
        <div class="card-body">
            <div class="table-responsive">
                <table class="table table-bordered" id="dataTable" width="100%" cellspacing="0">
                    <thead>
                        <tr>
                            <th>ID</th>
                            <th>Nome Produto</th>
                            <th>Descrição</th>
                            <th>Quantidade</th>
                            <th>Valor Item</th>
                            <th>Valor Total Item</th>
                            <th>Local Estoque</th>

                        </tr>
                    </thead>
                    <tbody>
                        <asp:Repeater runat="server" ID="rptItensVenda">
                            <ItemTemplate>
                                <tr>
                                    <td><%# Eval("ID") %></td>
                                    <td><%# Eval("PRODUTOLOCALESTOQUE.PRODUTO.NOME") %></td>
                                    <td><%# Eval("DESCRICAO") %></td>
                                    <td><%# Eval("QUANTIDADE", "{0:n2}")  %></td>
                                    <td><%# Eval("PRODUTOLOCALESTOQUE.VALORVENDA", "{0:c2}")  %></td>
                                    <td><%# Eval("VALOR", "{0:c2}")  %></td>
                                    <td><%# Eval("PRODUTOLOCALESTOQUE.LOCALESTOQUE.NOME") %></td>

                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>

                    </tbody>
                </table>
                <div class="card-body" id="divExcluirItenVenda" runat="server" visible="false">
                    <div class="row">
                        <div class="form-group col-lg-4">
                            <label>ID</label>
                            <asp:TextBox runat="server" class="form-control" ID="txtItenVenda" />
                        </div>
                        <div class="form-group col-lg-4" style="padding-top: 29px;">
                            <asp:Button runat="server" ID="btnExcluirItenVenda" Text="Excluir Item da Venda" class="btn btn-primary" OnClick="btnExcluirItenVenda_Click" />
                        </div>
                    </div>
                </div>
                <div class="form-group" id="divTotalItemVenda" runat="server" visible="false">
                    <h4 id="lbItens" runat="server" style="font-weight: bold; color: red;"></h4>
                    <asp:HiddenField ID="hdfValorTotalItens" runat="server" />
                </div>
            </div>
        </div>
    </div>
    <%-- END ITENS DA VENDA --%>

    <%-- OPEN ITENS DA FORMA PAGAMENTO --%>
    <div class="card shadow" runat="server" id="divFormaPagamento" style="margin-bottom: 20px;">
        <div class="card-header py-3">
            <h6 class="m-0 font-weight-bold text-primary">Cadastro de forma de pagamento</h6>
        </div>
        <div class="card-body">
            <div class="row">
                <div class="form-group col-lg-3">
                    <label>Forma de pagamento</label>
                    <asp:DropDownList ID="ddlMeioFormaPagamento" runat="server" CssClass="form-control" required="true">
                        <asp:ListItem Value="0" Text="Cartão de Crédito"></asp:ListItem>
                        <asp:ListItem Value="1" Text="Cartão de Débito"></asp:ListItem>
                        <asp:ListItem Value="2" Text="Pix"></asp:ListItem>
                        <asp:ListItem Value="3" Text="Dinheiro"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="row">
                <div class="form-group col-lg-4">
                    <label>Quantidade de Parcelas</label>
                    <asp:DropDownList ID="ddlQuantidadeParcelas" runat="server" CssClass="form-control">
                        <asp:ListItem Selected="True" Value="0" Text="À Vista"></asp:ListItem>
                        <asp:ListItem Value="1" Text="2x"></asp:ListItem>
                        <asp:ListItem Value="2" Text="3x"></asp:ListItem>
                        <asp:ListItem Value="3" Text="4x"></asp:ListItem>
                        <asp:ListItem Value="4" Text="5x"></asp:ListItem>
                        <asp:ListItem Value="5" Text="6x"></asp:ListItem>
                        <asp:ListItem Value="6" Text="7x"></asp:ListItem>
                        <asp:ListItem Value="7" Text="8x"></asp:ListItem>
                        <asp:ListItem Value="8" Text="9x"></asp:ListItem>
                        <asp:ListItem Value="9" Text="10x"></asp:ListItem>
                        <asp:ListItem Value="10" Text="11x"></asp:ListItem>
                        <asp:ListItem Value="11" Text="12x"></asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="form-group col-lg-4" id="div6" runat="server">
                    <label>Valor Acrescimo</label>
                    <asp:TextBox ID="txtValorAcrescimo" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
                <div class="form-group col-lg-4" id="div9" runat="server">
                    <label>Valor Desconto</label>
                    <asp:TextBox ID="txtValorDesconto" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
                <div class="form-group col-lg-4" id="div10" runat="server">
                    <label>Valor da forma de pagamento</label>
                    <asp:TextBox ID="txtValorFormaPagamento" runat="server" CssClass="form-control"></asp:TextBox>
                </div>

                <div class="form-group col-lg-4" id="div7" runat="server">
                    <label>NSU</label>
                    <asp:TextBox runat="server" class="form-control" ID="txtNsu" />
                </div>
                <div class="form-group col-lg-4" id="div8" runat="server">
                    <label>Cod. Autorização</label>
                    <asp:TextBox runat="server" class="form-control" ID="txtCodAutorizacao" />
                </div>
                <div class="form-group col-lg-12" id="div11" runat="server">
                    <label>Descrição forma pagamento</label>
                    <asp:TextBox runat="server" class="form-control" ID="txtDescricao" />
                </div>

                <div class="form-group">
                    <asp:Button runat="server" ID="btnAdicionarItemFormaPagamento" Text="Adicionar item" class="btn btn-primary" OnClick="btnAdicionarItemFormaPagamento_Click" />
                </div>

            </div>
        </div>
    </div>
    <!-- DataTales Example -->
    <div class="card shadow mb-4">
        <div class="card-header py-3">
            <h6 class="m-0 font-weight-bold text-primary">Lista de forma de pagamento</h6>
        </div>
        <div class="card-body">
            <div class="table-responsive">
                <table class="table table-bordered" id="dataTable2" width="100%" cellspacing="0">
                    <thead>
                        <tr>
                            <th>ID</th>
                            <th>Descrição</th>
                            <th>NSU</th>
                            <th>Autorização</th>
                            <th>Valor Forma Pagamento</th>
                            <th>Valor Acrescimo</th>
                            <th>Valor desconto</th>
                            <th>Local Estoque</th>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:Repeater runat="server" ID="rptItensFormaPagamento">
                            <ItemTemplate>
                                <tr>
                                    <td><%# Eval("ID") %></td>
                                    <td><%# Eval("DESCRICAO") %></td>
                                    <td><%# Eval("NSU") %></td>
                                    <td><%# Eval("CODAUTORIZACAO") %></td>
                                    <td><%# Eval("VALORFORMAPAGAMENTO", "{0:c2}")  %></td>
                                    <td><%# Eval("VALORACRESCIMO", "{0:c2}")  %></td>
                                    <td><%# Eval("VALORDESCONTO", "{0:c2}")  %></td>
                                    <td><%# Eval("LOCALESTOQUE.NOME") %></td>

                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>

                    </tbody>
                </table>
                <div class="card-body" id="divExcluirFormaPagamento" runat="server" visible="false">
                    <div class="row">
                        <div class="form-group col-lg-4">
                            <label>ID</label>
                            <asp:TextBox runat="server" class="form-control" ID="txtIdFormaPagamento" />
                        </div>
                        <div class="form-group col-lg-4" style="padding-top: 29px;">
                            <asp:Button runat="server" ID="btnExcluirFormaPagamento" Text="Excluir Forma Pagamento" class="btn btn-primary" OnClick="btnExcluirFormaPagamento_Click" />
                        </div>
                    </div>
                </div>
                <div class="form-group" id="divTotalFormaPagamento" runat="server" visible="false">
                    <h4 id="lbFormaPagamento" runat="server" style="font-weight: bold; color: red;"></h4>
                    <asp:HiddenField ID="hdfValorFormaPagamento" runat="server" />
                </div>
            </div>
        </div>
    </div>

    <%-- END ITENS DA FORMA PAGAMENTO --%>

    <!-- DataTales Example -->
    <div class="card shadow" runat="server" id="div2" style="margin-bottom: 20px;">
        <div class="card-header py-3">
            <h6 class="m-0 font-weight-bold text-primary">Cadastro de forma de pagamento</h6>
        </div>
        <div class="card-body">
            <div class="row">
                <div class="col-lg-4 " id="Div1" runat="server">
                    <label for="ddlLocalEstoque" class="control-label">Local Estoque</label>
                    <asp:DropDownList runat="server" ID="ddlLocalEstoqueVenda" DataTextField="NOME" DataValueField="ID" required="true"
                        CssClass="form-control" AutoPostBack="true">
                    </asp:DropDownList>
                </div>

                <div class="form-group" runat="server" id="divGerarVenda" style="margin-top: 32px;">
                    <asp:Button runat="server" ID="btnGerarVenda" Text="Gerar Venda" class="btn btn-primary" OnClick="btnGerarVenda_Click" />
                </div>
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
        // Call the dataTables jQuery plugin
        $(document).ready(function () {
            $('#dataTable2').DataTable();
        });
    </script>
</asp:Content>
