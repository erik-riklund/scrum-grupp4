using App.Models;
using Microsoft.AspNetCore.Mvc;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System.IO;
using System.Reflection.Metadata;


namespace App.Controllers
{
    public class PrintController : Controller
    {

        //public void GeneratePDFOrder(string filePath, string name, string email, string address)
        //{
        //    // Create a new PDF document
        //    PdfDocument document = new PdfDocument();

        //    // Add a page to the document
        //    PdfPage page = document.AddPage();

        //    // Create a graphics object for the page
        //    XGraphics gfx = XGraphics.FromPdfPage(page);

        //    // Create a font
        //    XFont font = new XFont("Arial", 12);

        //    // Draw form fields onto the PDF
        //    gfx.DrawString($"Name: {name}", font, XBrushes.Black, new XPoint(50, 50));
        //    gfx.DrawString($"Email: {email}", font, XBrushes.Black, new XPoint(50, 70));
        //    gfx.DrawString($"Address: {address}", font, XBrushes.Black, new XPoint(50, 90));

        //    // Save the PDF document to a file
        //    document.Save(filePath);

        //    // Close the document
        //    document.Close();
        //}
        public static MemoryStream GeneratePDFShipping(PrintOrderViewModel model, Entities.Order order, Entities.User user)
        {
            // Create a new PDF document
            PdfDocument document = new PdfDocument();

            // Add a page to the document
            PdfPage page = document.AddPage();

            // Create a graphics object for the page
            XGraphics gfx = XGraphics.FromPdfPage(page);

            // Create a font
            XFont font = new XFont("Arial", 12);

            // Draw form fields onto the PDF
            gfx.DrawString($"Delivery to: {""}", font, XBrushes.Black, new XPoint(50, 50));
            gfx.DrawString($"Name: {user.FirstName + " " + user.LastName}", font, XBrushes.Black, new XPoint(50, 50));
            gfx.DrawString($"Email: {user.Email}", font, XBrushes.Black, new XPoint(50, 70));
            gfx.DrawString($"Phone: {user.PhoneNumber}", font, XBrushes.Black, new XPoint(50, 50));
            gfx.DrawString($"Address: {user.Address.StreetAddress + " " +
                user.Address.ZipCode + " " + user.Address.City + " " + user.Address.Country}", font, XBrushes.Black, new XPoint(50, 90));
            gfx.DrawString($"Sender: {""}", font, XBrushes.Black, new XPoint(50, 50));

            gfx.DrawString($"PackageSize: {model.PackageSize}", font, XBrushes.Black, new XPoint(50, 50));



            MemoryStream stream = new MemoryStream();
            document.Save(stream, false);
            stream.Position = 0;


            // Close the document
            document.Close();

            return stream;
        }

        protected void DownloadPDF_Click(PdfDocument document)
        {
            MemoryStream pdfStream = new MemoryStream();
            DownloadPDF(pdfStream);



        }

        protected FileContentResult DownloadPDF(MemoryStream pdfStream)
        {
            // Skicka PDF-dokumentet till klienten för nedladdning


            return new FileContentResult(pdfStream.ToArray(), "application/pdf")
            {
                FileDownloadName = "MyPdf.pdf"
            };


        }
    }
}

