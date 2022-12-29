using MarketPlace.Enums;

namespace MarketPlace.Models;

public class Campaign
{
    public CampaignType Type { get; set; }

    public List<string> ProductEANs { get; set; }

    public int MinimumPurchaseQuantity { get; set; }

    public decimal Price { get; set; }

    public Campaign(CampaignType type, List<string> productEaNs, int minimumPurchaseQuantity, decimal price)
    {
        Type = type;
        ProductEANs = productEaNs;
        MinimumPurchaseQuantity = minimumPurchaseQuantity;
        Price = price;
    }
}