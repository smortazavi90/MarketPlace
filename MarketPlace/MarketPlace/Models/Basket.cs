namespace MarketPlace.Models;

public class Basket
{
    private List<Product> Products { get; } = new();

    public List<Product> GetProducts()
    {
        return Products;
    }
    
    public void AddProductToBasket(Product product)
    {
        Products.Add(product);
    }
    
    public bool TryRemoveProductFromBasket(Product removingProduct)
    {
        Product? product = Products.SingleOrDefault(p => p.EAN.Equals(removingProduct.EAN, StringComparison.OrdinalIgnoreCase));
        if (product is null)
        {
            return false;
        }
        
        Products.Remove(product);
        return true;
    }
}