using MarketPlace.Enums;
using MarketPlace.Interfaces;
using MarketPlace.Models;
using MarketPlace.Services;
using Microsoft.Extensions.DependencyInjection;

namespace MarketPlace.Test;

public class Tests
{
    [Test]
    [TestCase(3)]
    public void AddProductToBasket_ShouldWork(int productsNr)
    {
        Basket basket = new();
        for (int i = 0; i < productsNr; i++)
        {
            basket.AddProductToBasket(GenerateRandomProducts(1).First());
        }
        Assert.That(basket.GetProducts(), Has.Count.EqualTo(productsNr));
    }

    [Test]
    [TestCase(5)]
    public void AddProductsToBasket_ShouldWork(int productsNr)
    {
        Basket basket = new();
        basket.AddProductsToBasket(GenerateRandomProducts(productsNr));
        Assert.That(basket.GetProducts(), Has.Count.EqualTo(productsNr));
    }
    
    [Test]
    public void TryRemoveProductFromBasket_ShouldWork()
    {
        Basket basket = new();
        var product = new Product("5000112637922", 20);
        basket.AddProductToBasket(product);
        var removingResult = basket.TryRemoveProductFromBasket(product);

        Assert.Multiple(() =>
        {
            Assert.That(removingResult, Is.True);
            Assert.That(basket.GetProducts(), Has.Count.EqualTo(0));
        });
    }

    [Test]
    public void TryRemoveProductFromBasket_ShouldNotWork()
    {
        Basket basket = new();
        var product1 = new Product("5000112637922", 20);
        var product2 = new Product("8711000530085", 20);
        basket.AddProductToBasket(product1);
        var removingResult = basket.TryRemoveProductFromBasket(product2);

        Assert.Multiple(() =>
        {
            Assert.That(removingResult, Is.False);
            Assert.That(basket.GetProducts(), Has.Count.EqualTo(1));
        });
    }

    [Test]
    [TestCase(30)]
    public void CalculatePrice_ShouldWorkForComboCampaign(int campaignPrice)
    {
        const int campaignProductsNr = 6;
        var products = GenerateRandomProducts(20);
        var campaigns = new List<Campaign>
        {
            new(CampaignType.Combo, products.Take(campaignProductsNr).Select(p => p.EAN).ToList(), 2, campaignPrice),
        };

        var basketService = GetBasketService();
        Basket basket = new();
        basketService?.SetCampaigns(campaigns);

        for (var i = 0; i < campaignProductsNr; i++)
        {
            basket.AddProductToBasket(products.Skip(i).First());
        }

        var calculatePrice = basketService?.CalculatePrice(basket);
        Assert.That(calculatePrice, Is.EqualTo(campaignProductsNr * campaignPrice / 2));
    }

    [Test]
    public void CalculatePrice_ShouldWorkForVolumeCampaign()
    {
        const int campaignPrice = 25;
        const int campaignProductsNr = 6;
        var products = GenerateRandomProducts(10);
        var productInCampaign = products.First();

        var campaigns = new List<Campaign>
        {
            new(CampaignType.Volume, new List<string> { productInCampaign.EAN }, 2, campaignPrice),
        };

        var basketService = GetBasketService();
        Basket basket = new();
        basketService?.SetCampaigns(campaigns);

        basket.AddProductToBasket(productInCampaign);
        basket.AddProductToBasket(productInCampaign);
        basket.AddProductToBasket(productInCampaign);

        var calculatePrice = basketService?.CalculatePrice(basket);
        Assert.That(calculatePrice, Is.EqualTo(productInCampaign.Price + campaignPrice));
    }

    [Test]
    public void CalculatePrice_ShouldWorkForBothCampaigns()
    {
        const int comboCampaignPrice = 25;
        const int volumeCampaignPrice = 30;
        
        var comboCampaignProducts = GenerateRandomProducts(5);
        var volumeCampaignProduct = GenerateRandomProducts(1).First();
        var noCampaignProducts = GenerateRandomProducts(10);

        var campaigns = new List<Campaign>
        {
            new(CampaignType.Combo, comboCampaignProducts.Select(p => p.EAN).ToList(), 2, comboCampaignPrice),
            new(CampaignType.Volume, new List<string> { volumeCampaignProduct.EAN }, 2, volumeCampaignPrice),
        };

        var basketService = GetBasketService();
        Basket basket = new();
        basketService?.SetCampaigns(campaigns);
        
        // Add all products to the basket
        basket.AddProductsToBasket(noCampaignProducts);
        basket.AddProductsToBasket(comboCampaignProducts);
        basket.AddProductToBasket(volumeCampaignProduct);
        basket.AddProductToBasket(volumeCampaignProduct);
        basket.AddProductToBasket(volumeCampaignProduct);

        var calculatePrice = basketService?.CalculatePrice(basket);
        var expectedValue =  noCampaignProducts.Sum(p => p.Price)
                  + (volumeCampaignPrice + volumeCampaignProduct.Price)
                             + (comboCampaignPrice * 2 + comboCampaignProducts.Min(p => p.Price));
        
        Assert.That(calculatePrice, Is.EqualTo(expectedValue));
    }

    private List<Product> GenerateRandomProducts(int number)
    {
        Random random = new();
        List<Product> randomProducts = new();
        for (var i = 0; i < number; i++)
        {
            randomProducts.Add(new Product(GenerateRandomEAN(random), random.Next(30, 99)));
        }

        return randomProducts;
    }

    private string GenerateRandomEAN(Random random)
    {
        var randomEAN = string.Empty;
        for (var i = 0; i < 10; i++)
        {
            randomEAN += random.Next(1, 9);
        }

        return randomEAN;
    }
    
    public IBasketService? GetBasketService()
    {
        ServiceCollection services = new();
        services.AddTransient<IBasketService, BasketService>();
        var serviceProvider = services.BuildServiceProvider();
        return serviceProvider.GetService<IBasketService>();
    }
}