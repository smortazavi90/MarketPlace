using MarketPlace.Models;
using MarketPlace.Enums;
using MarketPlace.Interfaces;

namespace MarketPlace.Services;

public class BasketService : IBasketService
{
    private List<Campaign> _campaigns { get; set; } = new();

    public void SetCampaigns(List<Campaign> campaigns)
    {
        _campaigns = campaigns;
    }
    
    public decimal CalculatePrice(Basket basket)
    {
        decimal totalPrice = 0;

        var products = basket.GetProducts().ToList();

        foreach (var campaign in _campaigns)
        {
            var campaignProducts = products.Where(p => campaign.ProductEANs.Contains(p.EAN)).ToList();

            if (!campaignProducts.Any())
            {
                continue;
            }

            if (campaign.Type is CampaignType.Volume)
            {
                // Calculate total price of products in a volume campaign
                totalPrice += CalculateVolumeCampaignsTotalPrice(campaignProducts, campaign);
            }
            else
            {
                // Calculate total price of products in a combo campaign
                totalPrice += CalculateComboCampaignsTotalPrice(campaignProducts, campaign);
            }

            campaignProducts.ForEach(p => products.Remove(p));
        }

        if (products.Any())
        {
            // Calculate price of products that are not in any campaign
            totalPrice += products.Sum(p => p.Price);
        }

        return totalPrice;
    }

    private decimal CalculateComboCampaignsTotalPrice(List<Product> products, Campaign campaign)
    {
        decimal totalPrice = 0;
        var campaignPriceUnits = products.Count / campaign.MinimumPurchaseQuantity; // number of times the campaign price gets counted 

        totalPrice += campaignPriceUnits * campaign.Price;

        var normalPriceProducts = products.Count - campaignPriceUnits * campaign.MinimumPurchaseQuantity; // number of products that should be counted with their own original price
        if (0 < normalPriceProducts)
        {
            // the cheapest products should be selected
            totalPrice += products.OrderBy(p => p.Price).Take(normalPriceProducts).Sum(p => p.Price);
        }

        return totalPrice;
    }

    private decimal CalculateVolumeCampaignsTotalPrice(List<Product> products, Campaign campaign)
    {
        decimal totalPrice = 0;   
        var productsQuantityPrice = products
            .GroupBy(p => p.EAN)
            .Select(p => new KeyValuePair<int, decimal>(p.Count(), p.First().Price))
            .ToList();

        foreach (var (productQuantity, productPrice) in productsQuantityPrice)
        {
            totalPrice += (productQuantity / campaign.MinimumPurchaseQuantity) * campaign.Price;
            totalPrice += (productQuantity % campaign.MinimumPurchaseQuantity) * productPrice;
        }

        return totalPrice;
    }
}