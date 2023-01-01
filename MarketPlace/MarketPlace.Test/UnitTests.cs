using MarketPlace.Models;

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
}