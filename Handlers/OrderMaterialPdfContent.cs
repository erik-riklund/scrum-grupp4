using App.Entities;
using MongoDB.Entities;

namespace App.Handlers
{
    public class OrderMaterialPdfContent
    {
        public async static Task<string> PdfContent(string supplierID, string materialID, Double amount, DateTime orderDate)
        {
            var material = await Query.FetchOneById<Material>(materialID);
            var supplier = await Query.FetchOneById<Supplier>(supplierID);
            string content = 
                "<H2>Order Date" + orderDate+ "</h2>"+
                "<div><h1>Order from:</h1>" +
                    "<h2>" + supplier.Name + "</h2>" +
                    "<h2>" + supplier.TelephoneNumber + "</h2>" +
                    "<h2>" + supplier.Email + "</h2>" +
                    "<h2>" + supplier.Address.ZipCode + " " + supplier.Address.City + "</h2>" +
                    "<h2>" + supplier.Address.Country + "</h2>" +
                    "<div><h1>From:</h1>" +
                    "<h2>Otto's Fashionable Hats</h2>" +
                    "<h2>Hattmakaregatan 1</h2>" +
                    "<h2>70001 Hattstaden</h2>" +
                    "<h2>Hattlandet</h2></div>" +
                    "<div><h1>Order information</h1>";


            content += "<div><h2>Material to be ordered:</h2></div>" +
                "<h2>" + material.Name + "</h2>" +
                "<h2>" + material.Price + " SEK </h2>" +
                "<h2>" + amount + " " + material.Unit + "</h2>";
           
            return content;
        }

       
        }
}
