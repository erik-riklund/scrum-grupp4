using App.Entities;
using App.Controllers;

namespace App.Handlers
{
    public class OrderPdfContent
    {
        public static string PdfContent(Order order, User customer, App.Entities.Model hatmodel)
        {
            string content = "<div><h1>Delivery to:</h1>" +
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
                "<div><h1>Order information</h1>" +
                "<table border='1'>" +
                "<tr><th>Model Name</th><th>Description</th><th>Price</th><th>Size</th><th>Quantity</th></tr>";


            foreach (var hat in order.Hats)
            {
                if (hat.Model.ID == hatmodel.ID)
                {
                    content += "<tr>" +
                        "<td>" + hatmodel.ModelName + "</td>" +
                        "<td>" + hatmodel.Description + "</td>" +
                        "<td>" + hat.Price + "</td>" +
                        "<td>" + hat.Size + "</td>" +
                        "<td>1</td>" +
                        "</tr>";
                }
            }

            content += "</table>" +
                "<h2>Total Quantity: " + order.Hats.Count() + "</h2>" + 
                "<h2>Total Price: " + order.Hats.Sum(p => p.Price) + "</h2>"; 

            return content;
        }

        public async static Task<string> OneHistoryPdfContent(Order order, User customer, string imageUrl)
        {
            string content = "<div><h1>Order from:</h1>" +
                "<h2> Customerid: " + customer.ID+"</h2>"+
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
                "<div><h1>Order information</h1>" +
                "<h2>Orderid: "+order.ID+ "</h2>" +
                "<table border='1'>" +
                "<tr><th>Model Name</th><th>Description</th><th>Productcode</th><th>Price</th><th>Size</th><th>Image</th></tr>";


            foreach (var hat in order.Hats)
            {

                content += "<tr>" +
                        "<td>" + hat.Model.ModelName + "</td>" +
                        "<td>" + hat.Description + "</td>" +
                        "<td>" + hat.Model.ProductCode+ "</td>"+
                        "<td>" + hat.Price + "</td>" +
                        "<td>" + hat.Size + "</td>" +
                "<td><img src=\"" + imageUrl + "\" alt=\"Hat Image\"></td>" +
                        "<td>1</td>" +
                        "</tr>";
               
            }

            content += "</table>" +
                "<h2>Total Quantity: " + order.Hats.Count() + "</h2>" +
                "<h2>Total Price: " + order.Hats.Sum(p => p.Price) + "</h2>";

            return content;
        }




    }
}
