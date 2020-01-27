using DonVo.Inventory.ViewModels.MaterialDistributionNoteViewModel;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;

namespace DonVo.Inventory.Infrastructures
{
    public class MaterialDistributionNoteReportPdfTemplate
    {
        public MemoryStream GeneratePdfTemplate(List<MaterialDistributionNoteReportViewModel> data, DateTime date, string unitName)
        {
            BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1250, BaseFont.NOT_EMBEDDED);
            BaseFont bf_bold = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1250, BaseFont.NOT_EMBEDDED);
            var normal_font = FontFactory.GetFont(BaseFont.HELVETICA, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 8);
            var bold_font = FontFactory.GetFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 8);

            Document document = new Document(PageSize.A4, 25, 25, 30, 30);
            MemoryStream stream = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(document, stream);
            writer.CloseStream = false;
            document.Open();
            PdfContentByte cb = writer.DirectContent;

            cb.BeginText();

            cb.SetFontAndSize(bf, 8);
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "PT DAN LIRIS", 15, 820, 0);
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "INDUSTRIAL & TRADING CO LTD", 15, 810, 0);

            cb.SetFontAndSize(bf_bold, 8);
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "FM.FP-00-GG-15-002", 485, 815, 0);

            cb.SetFontAndSize(bf_bold, 14);
            cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, "BON PENGANTAR Here", 300, 785, 0);

            cb.SetFontAndSize(bf, 8);
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Date", 15, 760, 0);
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, ":", 70, 760, 0);
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, date.ToString("dd-MM-yyyy"), 80, 760, 0);

            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "BAGIAN", 15, 750, 0);
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, ":", 70, 750, 0);
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, unitName, 80, 750, 0);

            cb.EndText();

            PdfPTable table = new PdfPTable(10);
            PdfPCell cell;
            table.TotalWidth = 565f;
            int rowsPerPage = 20;

            float[] widths = new float[] { 0.5f, 1.5f, 2f, 2f, 1.5f, 2.5f, 0.7f, 1.5f, 2f, 2.5f };
            table.SetWidths(widths);

            cell = new PdfPCell() { Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 2 };

            cell.Phrase = new Phrase("NO", bold_font);
            table.AddCell(cell);

            cell.Phrase = new Phrase("NO BON", bold_font);
            table.AddCell(cell);

            cell.Phrase = new Phrase("TIPE BON", bold_font);
            table.AddCell(cell);

            cell.Phrase = new Phrase("NO SPB", bold_font);
            table.AddCell(cell);

            cell.Phrase = new Phrase("NO SPP", bold_font);
            table.AddCell(cell);

            cell.Phrase = new Phrase("Item name", bold_font);
            table.AddCell(cell);

            cell.Phrase = new Phrase("GRD", bold_font);
            table.AddCell(cell);

            cell.Phrase = new Phrase("PIECE", bold_font);
            table.AddCell(cell);

            cell.Phrase = new Phrase("METER", bold_font);
            table.AddCell(cell);

            cell.Phrase = new Phrase("ASAL", bold_font);
            table.AddCell(cell);

            int Number = 1;
            double? TotalQuantity = 0, TotalReceivedLength = 0;

            int TotalRows = data.Count;

            foreach (var d in data)
            {
                cell = new PdfPCell() { Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 2 };
                cell.Phrase = new Phrase(Number.ToString(), normal_font);
                table.AddCell(cell);

                cell.Phrase = new Phrase(d.No, normal_font);
                table.AddCell(cell);

                cell.Phrase = new Phrase(d.Type, normal_font);
                table.AddCell(cell);

                cell.Phrase = new Phrase(d.MaterialRequestNoteNo, normal_font);
                table.AddCell(cell);

                cell.Phrase = new Phrase(d.ProductionOrderNo, normal_font);
                table.AddCell(cell);

                cell = new PdfPCell() { Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 2 };
                cell.Phrase = new Phrase(d.ProductName.Replace("Here ", "", StringComparison.CurrentCultureIgnoreCase), normal_font);
                table.AddCell(cell);

                cell = new PdfPCell() { Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 2 };
                cell.Phrase = new Phrase(d.Grade, normal_font);
                table.AddCell(cell);

                cell = new PdfPCell() { Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER, HorizontalAlignment = Element.ALIGN_RIGHT, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 2 };
                cell.Phrase = new Phrase(string.Format("{0:n}", d.Quantity), normal_font);
                table.AddCell(cell);

                cell.Phrase = new Phrase(string.Format("{0:n}", d.Length), normal_font);
                table.AddCell(cell);

                cell = new PdfPCell() { Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 2 };
                cell.Phrase = new Phrase(d.SupplierName, normal_font);
                table.AddCell(cell);

                TotalQuantity += d.Quantity;
                TotalReceivedLength += d.Length;

                if (Number == TotalRows)
                {
                    cell = new PdfPCell() { Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER, HorizontalAlignment = Element.ALIGN_RIGHT, VerticalAlignment = Element.ALIGN_MIDDLE, Colspan = 7, Phrase = new Phrase("Amount", normal_font), Padding = 2 };
                    table.AddCell(cell);

                    cell = new PdfPCell() { Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER, HorizontalAlignment = Element.ALIGN_RIGHT, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 2 };

                    cell.Phrase = new Phrase(string.Format("{0:n}", TotalQuantity), normal_font);
                    table.AddCell(cell);

                    cell.Phrase = new Phrase(string.Format("{0:n}", TotalReceivedLength), normal_font);
                    table.AddCell(cell);
                    table.AddCell(new Phrase("", normal_font));
                }

                if (Number % rowsPerPage == 0)
                {
                    if (Number == rowsPerPage)
                    {
                        table.WriteSelectedRows(0, -1, 15, 735, cb);
                    }
                    else
                    {
                        table.WriteSelectedRows(0, -1, 15, 815, cb);
                    }

                    for (var i = 0; i < rowsPerPage; i++)
                    {
                        table.DeleteLastRow();
                    }

                    if (Number != TotalRows)
                        document.NewPage();
                }

                Number++;
            }

            Number--;
            if (Number % rowsPerPage != 0)
            {
                if (Number < rowsPerPage)
                    table.WriteSelectedRows(0, -1, 15, 735, cb);
                else
                    table.WriteSelectedRows(0, -1, 15, 815, cb);
            }

            cb.BeginText();
            cb.SetTextMatrix(15, 23);

            cb.SetFontAndSize(bf, 8);
            cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, "Pengirim,", 130, 110, 0);
            cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, "(.................................)", 130, 50, 0);
            cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, "Bag. Gd Material", 130, 35, 0);

            cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, "Penerima,", 460, 110, 0);
            cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, "(.................................)", 460, 50, 0);
            cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, "Bag. PRODUCTION", 460, 35, 0);

            cb.EndText();

            document.Close();
            byte[] byteInfo = stream.ToArray();
            stream.Write(byteInfo, 0, byteInfo.Length);
            stream.Position = 0;
            return stream;
        }
    }
}
