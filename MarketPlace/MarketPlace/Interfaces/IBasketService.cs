using MarketPlace.Models;

namespace MarketPlace.Interfaces;

public interface IBasketService
{
    public void SetCampaigns(List<Campaign> campaigns);
    public decimal CalculatePrice(Basket basket);
}