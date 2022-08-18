using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;

namespace PetShopDoBairro.adm.Helpers
{
    public class GeradorDePdf
    {
        //URL VIDEO EXPLICATIVO = https://youtu.be/Gm2pJfCJyUw
        // link github = https://github.com/barrosebs/geradorPDF

        private string caminhoLog = ConfigurationManager.AppSettings["LogRegistro"];
        DateTime dataExecucao = DateTime.Now;

        private List<ORDEMSERVICO> listaOrdemServico = new List<ORDEMSERVICO>();
        static BaseFont fonteBase = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);

        public void DesserializerPessoas()
        {
            BancoDeDadosPadraoDataContext contexto = new BancoDeDadosPadraoDataContext();
            try
            {

                List<ORDEMSERVICO> ordemServicos = (from o in contexto.ORDEMSERVICO
                                                    select o).ToList();

                if (ordemServicos.Count > 0)
                {
                    listaOrdemServico.AddRange(ordemServicos);
                }
                else
                {
                    throw new Exception("Não foi possível carregar o Arquivo!");
                }
            }
            catch (Exception ex)
            {
                LogRegistro.registrarLog(ex.Message + Environment.NewLine + ex.StackTrace + Environment.NewLine, "PetShopBairro.Web", caminhoLog, dataExecucao);
                throw;
            }
        }

        public void GerarRelatorioEmPDF(int qtdPessoas)
        {
            var OrdemServicoSelecionadas = listaOrdemServico.Take(qtdPessoas).ToList();
            if (OrdemServicoSelecionadas.Count > 0)
            {
                //calculo da quantidade total de páginas
                int totalPaginas = 1;
                int totalLinhas = OrdemServicoSelecionadas.Count;
                if (totalLinhas > 24)
                {
                    totalPaginas += (int)Math.Ceiling((totalLinhas - 24) / 29F);
                }

                //configuracao do documento PDF
                var pxPorMm = 72 / 25.2F;
                var pdf = new Document(PageSize.A4, 15 * pxPorMm, 15 * pxPorMm, 15 * pxPorMm, 20 * pxPorMm);
                var nomeArquivo = $"C:\\ordemServico.{DateTime.Now.ToString("yyyyMMddHHmmss")}.pdf";  
               
                var arquivo = new FileStream(nomeArquivo, FileMode.Create);
                var writer = PdfWriter.GetInstance(pdf, arquivo);
                writer.PageEvent = new EventosDePagina(totalPaginas);
                pdf.Open();

                //adicao de um link
                var fonteLink = new iTextSharp.text.Font(fonteBase, 9.9F, Font.NORMAL, BaseColor.Blue);
                var link = new Chunk("Gerando relatório PDF - Edu.barros", fonteLink);
                link.SetAnchor("https://www.linkedin.com/in/barrosebs/");
                var larguraTextoLink = fonteBase.GetWidthPoint(link.Content, fonteLink.Size);
                var caixaTexto = new ColumnText(writer.DirectContent);
                caixaTexto.AddElement(link);
                caixaTexto.SetSimpleColumn(
                    pdf.PageSize.Width - pdf.RightMargin - larguraTextoLink,
                    pdf.PageSize.Height - pdf.TopMargin - (30 * pxPorMm),
                    pdf.PageSize.Width - pdf.RightMargin,
                    pdf.PageSize.Height - pdf.TopMargin - (18 * pxPorMm)
                    );
                caixaTexto.Go();

                //adição do título do relatório
                var fonteParagrafo = new iTextSharp.text.Font(fonteBase, 32, iTextSharp.text.Font.NORMAL, BaseColor.Black);
                var titulo = new Paragraph("Relatório de Pessoas \n\n", fonteParagrafo);
                titulo.Alignment = Element.ALIGN_LEFT;
                titulo.SpacingAfter = 4;
                pdf.Add(titulo);
                var caminhoImagem = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "C:\\Dev\\Cursos\\GeraPDF\\GerarPDF_ItextSharp\\\\img\\youtube.png");// logo
                if (File.Exists(caminhoImagem))
                {
                    iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(caminhoImagem);
                    float razaoAlturaLargura = logo.Width / logo.Height;
                    float alturaLogo = 50;
                    float larguraLogo = alturaLogo + razaoAlturaLargura;
                    logo.ScaleToFit(larguraLogo, alturaLogo);
                    var margemEsquerda = pdf.PageSize.Width - pdf.RightMargin - larguraLogo;
                    var margemTopo = pdf.PageSize.Height - pdf.TopMargin - 40;
                    logo.SetAbsolutePosition(margemEsquerda, margemTopo);
                    writer.DirectContent.AddImage(logo, false);
                }
                //add table of dada
                var tabela = new PdfPTable(5);
                float[] largurasColunas = { 0.6f, 2f, 1.5f, 1f, 1f }; //melhorar largura das colunas
                tabela.SetWidths(largurasColunas);
                tabela.DefaultCell.BorderWidth = 0;
                tabela.WidthPercentage = 100;

                CriarCelulaTexto(tabela, "Modelo", PdfCell.ALIGN_CENTER, true);
                CriarCelulaTexto(tabela, "Número de Série do equipamento", PdfCell.ALIGN_CENTER, true);
                CriarCelulaTexto(tabela, "CpfCnpj", PdfCell.ALIGN_CENTER, true);
                CriarCelulaTexto(tabela, "Valor", PdfCell.ALIGN_CENTER, true);
                CriarCelulaTexto(tabela, "Avaria?", PdfCell.ALIGN_CENTER, true);

                var valor = 0.00;
                foreach (var p in OrdemServicoSelecionadas)
                {
                    CriarCelulaTexto(tabela, p.EQUIPAMENTO.MODELO, PdfCell.ALIGN_CENTER);
                    CriarCelulaTexto(tabela, p.EQUIPAMENTO.NUMEROSERIE, PdfCell.ALIGN_CENTER);
                    //CriarCelulaTexto(tabela, p.Nome + " " + p.Sobrenome);
                    CriarCelulaTexto(tabela, p.EQUIPAMENTO.USUARIO.CPFCNPJ, PdfCell.ALIGN_CENTER);
                    CriarCelulaTexto(tabela, valor.ToString("C2"), PdfCell.ALIGN_RIGHT);
                    CriarCelulaTexto(tabela, p.AVARIA ? "Sim" : "Não", PdfCell.ALIGN_CENTER);
                }
                pdf.Add(tabela);
                pdf.Close();
                arquivo.Close();

                //abre o PDF no visualizador padrão

                var caminhoPDF = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, nomeArquivo);
                if (File.Exists(caminhoPDF))
                {
                    Process.Start(new ProcessStartInfo()
                    {
                        Arguments = $"/c start {caminhoPDF}",
                        FileName = "cmd.exe",
                        CreateNoWindow = true
                    });
                }
            }
        }


        //adicionando celular de titulos das colunas
        private void CriarCelulaTexto(PdfPTable tabela, string texto, int alinhamentoHortz = PdfPCell.ALIGN_LEFT,
                                                                    bool negrito = false, bool italico = false,
                                                                    int tamanhoFonte = 12, int alturaCelula = 25)
        {
            int estilo = iTextSharp.text.Font.NORMAL;
            if (negrito && italico)
            {
                estilo = iTextSharp.text.Font.BOLDITALIC;
            }
            else if (negrito)
            {
                estilo = iTextSharp.text.Font.BOLD;
            }
            else if (italico)
            {
                estilo = iTextSharp.text.Font.ITALIC;
            }
            var bgColor = iTextSharp.text.BaseColor.White;
            if (tabela.Rows.Count % 2 == 1)
            {
                bgColor = new BaseColor(0.85F, 0.95F, 0.95F);
            }
            var fonteCelula = new iTextSharp.text.Font(fonteBase, tamanhoFonte, estilo, BaseColor.Black);
            var celula = new PdfPCell(new Phrase(texto, fonteCelula));
            celula.HorizontalAlignment = PdfCell.ALIGN_LEFT;
            celula.VerticalAlignment = PdfCell.ALIGN_MIDDLE;
            celula.Border = 0;
            celula.BorderWidthBottom = 1;
            celula.FixedHeight = alturaCelula;
            celula.BackgroundColor = bgColor;
            celula.PaddingBottom = 5;
            tabela.AddCell(celula);

        }
    }
}