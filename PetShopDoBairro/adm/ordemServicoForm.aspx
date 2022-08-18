<%@ Page Title="" Language="C#" MasterPageFile="~/adm/MasterAdm.Master" AutoEventWireup="true" CodeBehind="ordemServicoForm.aspx.cs" Inherits="PetShopDoBairro.adm.ordemServicoForm" %>

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
                <label for="txtEquipamentoCliente">Equipamento - Cliente</label>
                <asp:TextBox runat="server" ID="txtEquipamentoCliente" CssClass="form-control" placeholder="Buscar pelo número de série do equipamento ou Cpf/Cnpj."></asp:TextBox>
                <asp:HiddenField ID="hdEquipamentoCliente" runat="server" />
            </div>

            <div class="form-group">
                <label>Defeito repassado pelo cliente</label>
                <asp:TextBox ID="txtDefeitoRepassadoCliente" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="5"></asp:TextBox>
                <p class="countdown help-block"></p>
            </div>
            <div class="form-group">
                <label>Defeito apresentado</label>
                <asp:TextBox ID="txtDefeitoApresentado" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="5"></asp:TextBox>
                <p class="countdown help-block"></p>
            </div>

            <div class="form-group">
                <asp:CheckBox runat="server" ID="ckbAvaria" Checked="false" AutoPostBack="true" />
                <label>Existe algum tipo de avaria?</label>
            </div>

            <div class="form-group col-lg-12" runat="server" id="divAnexo" visible="false">
                <div class="panel panel-u margin-bottom-20">
                    <div class="panel-heading">
                        <h5 class="panel-title"><i class="fa fa-tasks"></i> Anexos de Avaria</h5>
                    </div>
                    <div class="panel-body">
                        <div class="form-group">
                            <p>
                                Você pode escolher mais de um arquivo por vez.
                            <br />
                                Importante: o tamanho e a quantidade dos anexos pode tornar tudo mais lento. Tenha Cuidado!
                            </p>
                            <asp:FileUpload runat="server" ID="fileAnexo" AllowMultiple="true" ToolTip="Escolher um ou mais arquivos" CssClass="form-control" />
                        </div>
                        <div class="form-group">
                            <table class="table">
                                <thead>
                                    <tr>
                                        <th>Ação</th>
                                        <th>Arquivo</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <asp:Repeater runat="server" ID="rptAnexos" OnItemCommand="rptAnexos_ItemCommand">
                                        <ItemTemplate>
                                            <tr>
                                                <td>
                                                    <asp:Button runat="server" ID="btExcluir" Text="Excluir" OnClientClick="return confirm('Você tem certeza que deseja excluir?');" CommandName="Excluir" CommandArgument='<%# Eval("Id") %>' class="btn-u btn-u-default"></asp:Button></td>

                                                <td>
                                                    <asp:LinkButton ID="LinkDownloadAnexo" runat="server" CommandArgument='<%# Eval("Arquivo") %>' CommandName="Download" Text='<%# Eval("Arquivo") %>'> </asp:LinkButton></td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>
            </div>

            <div class="form-group">
                <asp:Button runat="server" ID="btnSalvar" Text="Salvar" class="btn btn-primary" OnClick="btnSalvar_Click" />
            </div>

        </div>
    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceScript" runat="server">
    <script>
     
        //autocomplit
        $(document).ready(function () {

            $('#<%=txtEquipamentoCliente.ClientID %>').autocomplete({
                  source: function (request, response) {
                      $.ajax({
                          url: 'ordemServicoForm.aspx/pesquisaEquipamentoCliente',
                          data: "{ 'prefixo': '" + request.term + "'}",
                          dataType: "json",
                          type: "POST",
                          contentType: "application/json; charset=utf-8",
                          success: function (data) {
                              response($.map(data.d, function (item) {
                                  return {
                                      label: item.split(';')[0],
                                      val: item.split(';')[1]
                                  }
                              }))
                          },
                      });
                  },
                  select: function (e, i) {
                      $("[id$=hdEquipamentoCliente]").val(i.item.val);
                  },
                  minLength: 3,
              });

              var load = $('#<%=txtEquipamentoCliente.ClientID %>').val()
              var idHdCliente = $('#<%=hdEquipamentoCliente.ClientID %>').val()
              if (load != "" && idHdCliente == "") {
                  $('#<%=txtEquipamentoCliente.ClientID %>').addClass("ui-autocomplete-loading")
            }

        });
    </script>
</asp:Content>
