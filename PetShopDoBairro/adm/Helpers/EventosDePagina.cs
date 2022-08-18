﻿using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PetShopDoBairro.adm.Helpers
{
    public class EventosDePagina : PdfPageEventHelper
    {

        private PdfContentByte wdc;
        private BaseFont fonteBaseRodape { get; set; }
        private iTextSharp.text.Font fonteRodape { get; set; }
        public int totalPaginas { get; set; }
        public EventosDePagina(int totalPaginas)
        {
            fonteBaseRodape = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);
            fonteRodape = new iTextSharp.text.Font(fonteBaseRodape, 8f, iTextSharp.text.Font.NORMAL, BaseColor.Black);
            this.totalPaginas = totalPaginas;
        }

        public override void OnOpenDocument(PdfWriter writer, Document document)
        {
            base.OnOpenDocument(writer, document);
            this.wdc = writer.DirectContent;
        }
        public override void OnEndPage(PdfWriter writer, Document document)
        {
            base.OnEndPage(writer, document);
            var textoMomentoGeracao = $"Gerado em {DateTime.Now.ToShortDateString()} às {DateTime.Now.ToShortTimeString()}";

            AdicionarMomentoGeracaoRelatorio(writer, document, textoMomentoGeracao);
            AdicionarNumerosDasPaginas(writer, document);
        }

        private void AdicionarMomentoGeracaoRelatorio(PdfWriter writer, Document document, string textoMomentoGeracao)
        {
            wdc.BeginText();
            wdc.SetFontAndSize(fonteRodape.BaseFont, fonteRodape.Size);
            wdc.SetTextMatrix(document.LeftMargin, document.BottomMargin * 0.75f);
            wdc.ShowText(textoMomentoGeracao);
            wdc.EndText();
        }

        private void AdicionarNumerosDasPaginas(PdfWriter writer, Document document)
        {
            int paginaAtual = writer.PageNumber;
            var textoPaginacao = $"Página {paginaAtual} de {totalPaginas}";

            float larguraTextoPaginacao = fonteBaseRodape.GetWidthPoint(textoPaginacao, fonteRodape.Size);

            var tamanhoPagina = document.PageSize;

            wdc.BeginText();
            wdc.SetFontAndSize(fonteRodape.BaseFont, fonteRodape.Size);
            wdc.SetTextMatrix(tamanhoPagina.Width - document.RightMargin - larguraTextoPaginacao, document.BottomMargin * 0.75f);
            wdc.ShowText(textoPaginacao);
            wdc.EndText();
        }
    }
}