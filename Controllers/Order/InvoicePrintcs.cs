using App.Handlers;
using Microsoft.AspNetCore.Mvc;

namespace App.Controllers.Order
{
    public class OrderController : Controller
    {
        public async Task<IActionResult> PrintInvoice(string orderId)
        {
            try
            {
                var getOrder = await Query.FetchOneById<Entities.Order>(orderId);
                var customerId = getOrder.CustomerID;
                var customer = await Query.FetchOneById<Entities.User>(customerId);

                var content = await InvoiceContentPdfcs.OneHistoryPdfContent(getOrder, customer);

                var stream = new MemoryStream(PdfHandler.HtmlToPdf(content));
                return new FileStreamResult(stream, "application/pdf");

            }
            catch
            {
                ModelState.AddModelError(string.Empty, "An error occurred while fetching the order");
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
