namespace MarketPlace.Models;

public class Product
{
    public string EAN { get; set; }
    public decimal Price { get; set; }
    
    public Product(string ean, decimal price)
    {
        EAN = ean;
        Price = price;
    }
}