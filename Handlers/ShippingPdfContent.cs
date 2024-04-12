using App.Entities;
using iText.StyledXmlParser.Jsoup.Parser;
using System;
using System.Security.Cryptography;
using static iText.Kernel.Pdf.Colorspace.PdfShading;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace App.Handlers
{
    public class ShippingPdfContent
    {
        public static string PdfContent(Order order, User customer, Shipping shipping)
        {
            string content = "<div><h1>Delivery to:</h1>" +
                "<h2>" + customer.FirstName + " " + customer.LastName + "</h2>" +
                "<h2>" + customer.Address.StreetAddress + "</h2>" +
                "<h2>" + customer.Address.ZipCode + " " + customer.Address.City + "</h2>" +
                "<h2>" + customer.Address.Country + "</h2>" +
                "<h2>" + customer.PhoneNumber + "</h2></div>"+
                "<div><h1>From:</h1>" +
                "<h2>Otto's Fashionable Hats</h2>" +
                "<h2>Hattmakaregatan 1</h2>" +
                "<h2>70001 Hattstaden</h2>" +
                "<h2>Hattlandet</h2></div>" +
                "<div><h1>Package information</h1>" +
                "<h2>Quantity: " + shipping.PackageQuantity + " package</h2>" +
                "<h2>Size: " + shipping.PackageSize + "</h2>" +
                "<h2>Content: " + shipping.PackageContent + "</h2>" +
                "<h2>Weight: " + shipping.PackageWeight + "</h2>" +
                "<h2>Shippingcompany " + shipping.ShippingCompany + "</h2>"+
                "<h2>ShippingPayment " + shipping.Payment + "</h2></div>";

            return content;
        }

    }
}
