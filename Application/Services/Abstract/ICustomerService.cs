namespace Application.Services.Abstract;

public interface ICustomerService
{
    public void BuyProduct();
    public void SeePurchasedProducts();
    public void SeePurchasedProductsByDate();
    public void Filter();

}
