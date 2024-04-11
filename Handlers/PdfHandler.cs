using iText.Kernel.Pdf;
using iText.Html2pdf;
using Microsoft.AspNetCore.Mvc;

namespace App.Handlers
{
  public static class PdfHandler
  {
    public static byte[] HtmlToPdf(string content)
    {
      var stream = new MemoryStream();
      var writer = new PdfWriter(stream);
      var document = new PdfDocument(writer);

      writer.SetCloseStream(false);
      document.SetCloseWriter(false);

      HtmlConverter.ConvertToPdf(content, writer, new ConverterProperties());

      var bytes = stream.ToArray();

      writer.Close();
      stream.Close();

      return bytes;
    }

    public static FileStreamResult ToStream(byte[] content)
    {
      return new FileStreamResult(new MemoryStream(content), "application/pdf");
    }
  }
}