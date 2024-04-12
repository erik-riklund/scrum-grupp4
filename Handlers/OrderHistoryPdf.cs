using App.Entities;
using iText.IO.Codec;
using Microsoft.AspNetCore.Mvc;

namespace App.Handlers
{
    public class OrderHistoryPdf
    {
        public async Task<String> PdfContent(List<Order> order, DateTime dateFrom, DateTime dateTo)
        {
            string content =
                "<div><h1>Date from: "+dateFrom+" Date to: "+dateTo+"</h1></div>"+
                "<div><h1>From:</h1>" +
                "<h2>Otto's Fashionable Hats</h2>" +
                "<h2>Hattmakaregatan 1</h2>" +
                "<h2>70001 Hattstaden</h2>" +
                "<h2>Hattlandet</h2></div>" +
                "<div><h1>Order information</h1>" +
                "<table border='1'>" +
                "<tr><th>OrderDate</th><th>ModelId</th><th>Modelname</th><th>Price</th><th>Orderstatus</th><th>Quantity</th></tr>";

            List<double> sumPrice = new List<double>();
            foreach (var orders in order)
            {
                foreach (var hat in orders.Hats) 
                {
                    var hatModel = await Query.FetchOneById<Entities.Model>(hat.ModelID);
                    if (hat.ModelID == hatModel.ID)
                    {
                        content += "<tr>" +
                        "<td>" + orders.OrderDate + "</td>"+
                        "<td>" + hat.ModelID + "</td>" +
                        "<td>" + hatModel.ModelName + "</td>" +
                        "<td>" + hat.Price + "</td>" +
                        "<td>" + orders.Status + "</td>" +
                        "<td>" + orders.Hats.Count() + "</td>"+
                        "</tr>";
                        sumPrice.Add(hat.Price);

                    }
                }
                
            }

            content += "</table>" +
                "<h2>Total Price: " + sumPrice.Sum() + " SEK</h2>";

            return content;
        }

    }
}
