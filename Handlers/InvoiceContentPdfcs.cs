using App.Entities;
using System.Security.Cryptography;

namespace App.Handlers
{
    public class InvoiceContentPdfcs
    {
        public async static Task<string> OneHistoryPdfContent(Order order, User customer)
        {
            string content = "<div><h1>Invoice</h1>" +
                "<h2> Customerid: " + customer.ID + "</h2>" +
                "<h2>" + customer.FirstName + " " + customer.LastName + "</h2>" +
                "<h2>" + customer.Address.StreetAddress + "</h2>" +
                "<h2>" + customer.Email + "</h2>" +
                "<h2>" + customer.Address.ZipCode + " " + customer.Address.City + "</h2>" +
                "<h2>" + customer.Address.Country + "</h2>" +
                "<h2>" + customer.PhoneNumber + "</h2></div>" +
                "<div><h1>From:</h1>" +
                "<h2>Otto's Fashionable Hats</h2>" +
                "<h2>Hattmakaregatan 1</h2>" +
                "<h2>70001 Hattstaden</h2>" +
                "<h2>Hattlandet</h2></div>" +
                "<div><h2>Order information</h2>" +
                "<h3>Orderid: " + order.ID + "</h3>" +
                "<h3>"+ order.OrderDate+ "</h3>"+
                "<table border='1'>" +
                "<tr><th>Model Name</th><th>Description</th><th>Productcode</th><th>Price</th><th>Size</th><th>Image</th></tr>";


            foreach (var hat in order.Hats)
            {

                content += "<tr>" +
                        "<td>" + hat.Model.ModelName + "</td>" +
                        "<td>" + hat.Description + "</td>" +
                        "<td>" + hat.Model.ProductCode + "</td>" +
                        "<td>" + hat.Price + "</td>" +
                        "<td>" + hat.Size + "</td>" +
                        "<td>1</td>" +
                        "</tr>";

            }

            content += "</table>" +
                "<h3>Total Quantity: " + order.Hats.Count() + "</h3>" +
                "<h3>Total Price: " + order.Hats.Sum(p => p.Price) + "</h3>"+
                "<h3>OCR: " + order.ID + "</h2>"+
                "<h3>Bank account: 7771-2921 Name:Otto's Fashionable Hats </h3>"+
                "<h3>Pay in 30 days from order date</h3>";
            ;

            return content;
        }
    }
}
