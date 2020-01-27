using DonVo.Inventory.ViewModels.FPReturnInvToPurchasingViewModel;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.IO;

namespace DonVo.Inventory.Infrastructures
{
    public class ReturnInvToPurchasingPdfTemplate
    {
        public MemoryStream GeneratePdfTemplate(ReturnInvToPurchasingViewModel viewModel, int offset)
        {
            //Declaring fonts.

            BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1250, BaseFont.NOT_EMBEDDED);
            BaseFont bf_bold = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1250, BaseFont.NOT_EMBEDDED);
            var normal_font = FontFactory.GetFont(BaseFont.HELVETICA, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 9);
            var bold_font = FontFactory.GetFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 9);

            //Creating page.

            Document document = new Document(PageSize.A5.Rotate());
            MemoryStream stream = new MemoryStream();

            PdfWriter writer = PdfWriter.GetInstance(document, stream);
            writer.CloseStream = false;

            document.Open();

            PdfContentByte cb = writer.DirectContent;

            //Set Header
            #region SetHeader

            cb.BeginText();
            cb.SetTextMatrix(15, 23);

            cb.SetFontAndSize(bf, 10);
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "PT DAN LIRIS", 20, 385, 0);
            
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Sukoharjo", 20, 370, 0);

            cb.SetFontAndSize(bf_bold, 14);
            cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, "BON Returns of Goods", 297, 360, 0);

            cb.SetFontAndSize(bf_bold, 9);
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "FM.FP-GB-15-005 / R2", 480, 385, 0);

            cb.SetFontAndSize(bf, 9);
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "KEPADA YTH. BAGIAN PURCHASE", 20, 335, 0);
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Telah Diretur Ke : " + viewModel.Supplier.Name, 20, 320, 0);

            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "No : " + viewModel.No, 480, 335, 0);

            cb.EndText();

            #endregion

            #region CreateTable
            PdfPTable table = new PdfPTable(5)
            {
                TotalWidth = 553f
            };

            float[] widths = new float[] { 3f, 10f, 5f, 5f, 10f };
            table.SetWidths(widths);

            var cell = new PdfPCell() { Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 5 };
            var rightCell = new PdfPCell() { Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER, HorizontalAlignment = Element.ALIGN_RIGHT, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 5 };
            var leftCell = new PdfPCell() { Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 5 };
            
            cell.Phrase = new Phrase("No", bold_font);
            table.AddCell(cell);

            cell.Phrase = new Phrase("Item name", bold_font);
            table.AddCell(cell);

            cell.Phrase = new Phrase("Piece", bold_font);
            table.AddCell(cell);

            cell.Phrase = new Phrase("Long Total", bold_font);
            table.AddCell(cell);

            cell.Phrase = new Phrase("Keterangan", bold_font);
            table.AddCell(cell);

            //Add all items.
            int index = 1;
            foreach (var item in viewModel.ReturnInvToPurchasingDetails)
            {
                
                cell.Phrase = new Phrase(index.ToString(), normal_font);
                table.AddCell(cell);

                leftCell.Phrase = new Phrase(item.Product.Name  , normal_font);
                table.AddCell(leftCell);

                rightCell.Phrase = new Phrase(string.Format("{0:n}", item.Quantity), normal_font);
                table.AddCell(rightCell);

                rightCell.Phrase = new Phrase(string.Format("{0:n}", item.NecessaryLength), normal_font);
                table.AddCell(rightCell);

                leftCell.Phrase = new Phrase(item.Description, normal_font);
                table.AddCell(leftCell);
                index++;
            }

            //Save tables.

            table.WriteSelectedRows(0, -1, 20, 310, cb);
            #endregion

            cb.SetFontAndSize(bf, 10);
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Sukoharjo : " + DateTime.UtcNow.AddHours(offset).ToString("dd-MM-yyyy"), 460, 260, 0);
            //Set footer
            #region Footer

            cb.BeginText();
            cb.SetTextMatrix(15, 23);

            //LEFT
            cb.SetFontAndSize(bf, 9);
            cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, "Yang Menyerahkan,", 180, 120, 0);
            cb.SetFontAndSize(bf, 9);
            cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, "(..........................)", 180, 50, 0);
            cb.SetFontAndSize(bf, 9);
            cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, "Bag. Warehouse Material", 180, 35, 0);


            //RIGHT
            cb.SetFontAndSize(bf, 9);
            cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, "Yang Menerima,", 420, 120, 0);
            cb.SetFontAndSize(bf, 9);
            cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, "(..........................)", 420, 50, 0);
            cb.SetFontAndSize(bf, 9);
            cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, "Bag. PURCHASE", 420, 35, 0);

            cb.EndText();

            #endregion

            document.Close();

            byte[] byteInfo = stream.ToArray();
            stream.Write(byteInfo, 0, byteInfo.Length);
            stream.Position = 0;

            return stream;
        }
    }
}
