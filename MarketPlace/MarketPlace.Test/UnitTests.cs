using MarketPlace.Models;

namespace MarketPlace.Test;

public class Tests
{
    [Test]
    public void AddProductToBasket_ShouldWork()
    {
        Basket basket = new();
        basket.AddProductToBasket(new Product("5000112637922", 20));
        basket.AddProductToBasket(new Product("5000112637922", 20));
        basket.AddProductToBasket(new Product("8711000530085", 30));
        Assert.That(basket.GetProducts(), Has.Count.EqualTo(3));
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
}