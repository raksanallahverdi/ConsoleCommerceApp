namespace Application.Services.Concrete;
using Application.Services.Abstract;
using Core.Constants;
using Data.UnitOfWork.Concrete;
using Core.Entities;
using Microsoft.AspNetCore.Identity;
using System.Globalization;
using Data.UnitOfWork;

public class SellerService : ISellerService
{
    private readonly UnitOfWork _unitOfWork;
    public int sellerId;
    public SellerService(UnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;

    }
    public void SeeMyBankAccount()
    {
        var seller = _unitOfWork.Sellers.GetAll().FirstOrDefault(c => c.Id == sellerId);


        Console.WriteLine($"My Bank Account: {seller.MyBankAccount}$");

    }
    public bool SellerLogin()
    {
        Messages.InputMessage("Email");
        string userEmail = Console.ReadLine();
        Messages.InputMessage("Password");
        string password = Console.ReadLine();

        var seller = _unitOfWork.Sellers.GetAll().FirstOrDefault(x => x.Email == userEmail);

        if (seller == null)
        {
            Messages.InvalidInputMessage("Email or Password");
            return false;
        }

        PasswordHasher<Seller> passwordHasher = new PasswordHasher<Seller>();
        var result = passwordHasher.VerifyHashedPassword(seller, seller.Password, password);

        if (result == PasswordVerificationResult.Failed)
        {
            Messages.InvalidInputMessage("Email or Password");
            return false;
        }
        Console.WriteLine(" ");
        Messages.SuccessMessage("Login", "Done");
        sellerId = seller.Id;
        return true;



    }
    public void GetProducts()
    {
        if (_unitOfWork.Products.GetAll().Count == 0)
        {
            Messages.NotFoundMessage("Product");
        }
        foreach (var product in _unitOfWork.Products.GetAll())
        {
            Console.WriteLine($"{product.Id}.{product.Name},  Price:{product.Price}$,  Quantity:{product.Quantity},Category:{product.Category.Name}, CreatedAt: {product.CreatedAt}, Seller:{product.Seller.Name} {product.Seller.Surname}{(product.UpdatedAt.HasValue ? $", UpdatedAt: {product.UpdatedAt}" : "")}");
        }
    }
    public void GetMyProducts() {

        foreach (var existProduct in _unitOfWork.Products.GetAll().Where(x => x.SellerId == sellerId))
        {
            Console.WriteLine($"{existProduct.Id}.{existProduct.Name},  Price:{existProduct.Price}$,  Quantity:{existProduct.Quantity},Category: {existProduct.Category.Name}, CreatedAt: {existProduct.CreatedAt}, Seller:{existProduct.Seller.Name} {existProduct.Seller.Surname}{(existProduct.UpdatedAt.HasValue ? $", UpdatedAt: {existProduct.UpdatedAt}" : "")}");
        }
    }
    public void AddProduct()
    {
    ProductNameSection:
        Messages.InputMessage("Product Name");
        string productName = Console.ReadLine();
        if (string.IsNullOrEmpty(productName))
        {
            Messages.InvalidInputMessage("Product Name");
            goto ProductNameSection;
        }

    ProductPriceSection:
        Messages.InputMessage("Product Price");
        string productPriceInput = Console.ReadLine();
        int productPrice;
        bool isSucceeded = int.TryParse(productPriceInput, out productPrice);
        if (!isSucceeded || productPrice <= 0)
        {
            Messages.InvalidInputMessage("Product Price");
            goto ProductPriceSection;
        }

    ProductQuantitySection:
        Messages.InputMessage("Product Quantity");
        string productQuantityInput = Console.ReadLine();
        int productQuantity;
        isSucceeded = int.TryParse(productQuantityInput, out productQuantity);
        if (!isSucceeded || productQuantity < 1)
        {
            Messages.InvalidInputMessage("Product Quantity");
            goto ProductQuantitySection;
        }


    CategoryIdSection:
        foreach (var category in _unitOfWork.Categories.GetAll())
        {
            Console.WriteLine($"{category.Id}. {category.Name}");
        }
        Messages.InputMessage("Category Id");
        string categoryIdInput = Console.ReadLine();
        int categoryId;
        isSucceeded = int.TryParse(categoryIdInput, out categoryId);
        var existCategory = _unitOfWork.Categories.Get(categoryId);
        if (!isSucceeded || existCategory == null)
        {
            Messages.InvalidInputMessage("Category Id");
            goto CategoryIdSection;
        }

        var seller = _unitOfWork.Sellers.Get(sellerId);
        if (seller == null)
        {
            Messages.NotFoundMessage("Seller");
            return;
        }


        Product newProduct = new Product
        {
            Name = productName,
            Price = productPrice,
            Quantity = productQuantity,
            CreatedAt = DateTime.Now,
            SellerId = sellerId,
            CategoryId = categoryId,
            Seller = seller,
            Category = existCategory

        };

        _unitOfWork.Products.Add(newProduct);
        Messages.SuccessMessage("Product", "Added");
        _unitOfWork.Commit();
    }
    public void ChangeProductQuantity()
    {
    inputProductId:
        GetProducts();
        Messages.InputMessage("Product Id");
        string productIdInput = Console.ReadLine();
        int productId;
        bool isSucceeded = int.TryParse(productIdInput, out productId);
        if (!isSucceeded || productId <= 0)
        {
            Messages.InvalidInputMessage("Product Id");
            goto inputProductId;
        }

        Product existingProduct = _unitOfWork.Products.Get(productId);
        if (existingProduct == null)
        {
            Messages.NotFoundMessage("Product");
            return;
        }

    inputNewQuantity:
        Messages.InputMessage("New Quantity");
        string newQuantityInput = Console.ReadLine();
        int newQuantity;
        isSucceeded = int.TryParse(newQuantityInput, out newQuantity);
        if (!isSucceeded || newQuantity < 0)
        {
            Messages.InvalidInputMessage("Product Quantity");
            goto inputNewQuantity;
        }

        existingProduct.Quantity = newQuantity;

        _unitOfWork.Products.Update(existingProduct);
        Messages.SuccessMessage("Product Quantity", "Updated");
        _unitOfWork.Commit();
    }
    public void DeleteProduct()
    {
        GetProducts();

    inputProductId:
        Messages.InputMessage("Product Id");
        string productIdInput = Console.ReadLine();
        int productId;
        bool isSucceeded = int.TryParse(productIdInput, out productId);
        if (!isSucceeded || productId <= 0)
        {
            Messages.InvalidInputMessage("Product Id");
            goto inputProductId;
        }

        Product product = _unitOfWork.Products.Get(productId);
        if (product == null)
        {
            Messages.NotFoundMessage("Product");
            return;
        }

        _unitOfWork.Products.Delete(product);
        Messages.SuccessMessage("Product", "Deleted");
        _unitOfWork.Commit();
    }
    public void UpdateProduct()
    {
        foreach (var existProduct in _unitOfWork.Products.GetAll().Where(x => x.SellerId == sellerId))
        {
            Console.WriteLine($"{existProduct.Id}.{existProduct.Name},  Price:{existProduct.Price}$,  Quantity:{existProduct.Quantity},Category: {existProduct.Category.Name}, CreatedAt: {existProduct.CreatedAt}, Seller:{existProduct.Seller.Name} {existProduct.Seller.Surname}{(existProduct.UpdatedAt.HasValue ? $", UpdatedAt: {existProduct.UpdatedAt}" : "")}");
        }


    inputProductId:
        Messages.InputMessage("Product Id");
        string productIdInput = Console.ReadLine();
        int productId;
        bool isSucceeded = int.TryParse(productIdInput, out productId);
        if (!isSucceeded || productId <= 0)
        {
            Messages.InvalidInputMessage("Product Id");
            goto inputProductId;
        }

        Product product = _unitOfWork.Products.Get(productId);
        if (product == null || product.SellerId != sellerId)
        {
            Messages.NotFoundMessage("Product");
            return;
        }

    inputProductPrice:
        Messages.InputMessage("Price");
        string userAnswer = Console.ReadLine();
        decimal productPrice;
        isSucceeded = decimal.TryParse(userAnswer, out productPrice);
        if (!isSucceeded || productPrice <= 0)
        {
            Messages.InvalidInputMessage("Price");
            goto inputProductPrice;
        }
    inputProductCategory:
        foreach (var category in _unitOfWork.Categories.GetAll())
        {
            Console.WriteLine($"{category.Id}.{category.Name}");
        }

        Messages.InputMessage("Category Id");
        string categoryIdInput = Console.ReadLine();
        int categoryId;
        isSucceeded = int.TryParse(categoryIdInput, out categoryId);
        var existCategory = _unitOfWork.Categories.Get(categoryId);
        if (!isSucceeded || existCategory == null)
        {
            Messages.InvalidInputMessage("Category Id");
            goto inputProductCategory;
        }
    inputProductQuantity:
        Messages.InputMessage("Quantity");
        string categoryQuantityInput = Console.ReadLine();
        int productQuantity;
        isSucceeded= int.TryParse(categoryQuantityInput, out productQuantity);
        if (!isSucceeded || productQuantity < 0)
        {
            Messages.InvalidInputMessage("Quantity");
        }
        product.Quantity = productQuantity;
        product.Price = productPrice;
        product.CategoryId = categoryId;
        product.Category = existCategory;
        product.UpdatedAt= DateTime.Now;
        _unitOfWork.Products.Update(product);
        _unitOfWork.Commit();
        Messages.SuccessMessage("Product", "Updated");





    }

    public void FilterForName()
    {
        foreach (var product in _unitOfWork.Products.GetAll())
        {
            Console.WriteLine($"{product.Id}.{product.Name}, Price:{product.Price}$,Quantity:{product.Quantity},CreatedAt: {product.CreatedAt}");

        }
    nameInputSection:
        Messages.InputMessage("Word");
        string filteredWord = Console.ReadLine();
        string filterWord = filteredWord.Trim();
        if (string.IsNullOrEmpty(filterWord))
        {
            Messages.InvalidInputMessage("Word");
            goto nameInputSection;
        }
        var validProducts = _unitOfWork.Products.GetAll().Where(p => p.Name.Contains(filterWord, StringComparison.OrdinalIgnoreCase)).ToList();
        if (validProducts.Count == 0)
        {
            Messages.NotFoundMessage("Product");
            return;
        }

        foreach (var product in validProducts)
        {
            Console.WriteLine($"{product.Id}.{product.Name}, Price:{product.Price}$,Quantity:{product.Quantity},CreatedAt: {product.CreatedAt}");
        }
    }

    public void SeeWhoPurchasedProduct()
    {
        foreach (var order in _unitOfWork.Orders.GetAll())
        {
            if (order.SellerId == sellerId)
            {
                Console.WriteLine($"{order.Customer.Name} {order.Customer.Surname} purchased {order.ProductQuantity} {order.Product.Name} from you at: {order.CreatedAt},Your Income: {order.TotalPrice}$");
            }
        }

    }
    public void SeePurchasedProductForDate()
    {
    DateSection:
        Console.WriteLine("Please enter Date (dd.mm.yyy)");
        string customerInput = Console.ReadLine();
        DateOnly customerDate;
        bool isSucceeded = DateOnly.TryParseExact(customerInput, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out customerDate);

        if (!isSucceeded)
        {
            Messages.InvalidInputMessage("Date");
            goto DateSection;
        }

        foreach (var order in _unitOfWork.Orders.GetAll())
        {
            if (order.SellerId == sellerId)
            {
                DateOnly orderDate = DateOnly.FromDateTime(order.CreatedAt);

                if (customerDate == orderDate)
                {
                    Console.WriteLine($"{order.Id}.{order.Product.Name}({order.Product.Price}$), Quantity:{order.ProductQuantity},Total Income {order.TotalPrice},Customer:{order.Customer.Name} {order.Customer.Surname},Date {order.CreatedAt}");
                }


            }
        }
    }
    public void SeeTotalIncome()
    {
        decimal total = 0;
        foreach (var order in _unitOfWork.Orders.GetAll())
        {
            if (order.SellerId == sellerId)
            {
                total += order.TotalPrice;
            }
        }
        Console.WriteLine($"Your Total Income: {total}$");
    }

}
