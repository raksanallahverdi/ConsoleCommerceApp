using Application.Services.Abstract;
using Core.Constants;
using Core.Constants.Extensions;
using Core.Entities;
using Data.UnitOfWork.Concrete;
using Microsoft.AspNetCore.Identity;
using System.Globalization;


namespace Application.Services.Concrete;

public class CustomerService : ICustomerService
{
    private readonly UnitOfWork _unitOfWork;
    public int customerId;

    public CustomerService()
    {
        _unitOfWork = new UnitOfWork();

    }
    public bool CustomerLogin()
    {
        Messages.InputMessage("Email");
        string userEmail = Console.ReadLine();
        Messages.InputMessage("Password");
        string password = Console.ReadLine();

        var customer = _unitOfWork.Customers.GetAll().FirstOrDefault(x => x.Email == userEmail);

        if (customer == null)
        {
            Messages.InvalidInputMessage("Email or Password");
            return false;
        }

        PasswordHasher<Customer> passwordHasher = new PasswordHasher<Customer>();
        var result = passwordHasher.VerifyHashedPassword(customer, customer.Password, password);

        if (result == PasswordVerificationResult.Failed)
        {
            Messages.InvalidInputMessage("Email or Password");
            return false;
        }
        Console.WriteLine(" ");
        Console.ForegroundColor = ConsoleColor.Green;
        Messages.SuccessMessage("Login", "Done");
        Console.ResetColor();
        customerId = customer.Id;
        return true;



    }
    public void BuyProduct()
    {
        var customer = _unitOfWork.Customers.Get(customerId);
        if (customer == null)
        {
            Messages.NotFoundMessage("Customer");
            return;
        }
        if (_unitOfWork.Products.GetAll().Count == 0)
        {
            Messages.NotFoundMessage("Product");
        }

        foreach (var product in _unitOfWork.Products.GetAll())
        {
          
            Console.WriteLine($"{product.Id}.{product.Name}-{product.Price}$,Quantity{product.Quantity}," +
                $"Seller:{product.Seller.Name} {product.Seller.Surname}");
           
            
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
        Product existProduct = _unitOfWork.Products.Get(productId);
        if (existProduct == null)
        {
            Messages.NotFoundMessage("Product");
            return;
        }
    inputProductQuantity:
        Messages.InputMessage("Product Quantity");
        string customerInput = Console.ReadLine();
        int quantity;
        isSucceeded = int.TryParse(customerInput, out quantity);
        if (!isSucceeded || quantity <= 0)
        {
            Messages.InvalidInputMessage("Quantity");
            goto inputProductQuantity;
        }
        if (existProduct.Quantity < quantity)
        {
            Messages.NotEnoughItem($"{existProduct.Name}");
        }

    PaymentSection:
        decimal totalPaymentOfCustomer = quantity * existProduct.Price;
        Console.WriteLine($"Your Payment will be {totalPaymentOfCustomer} $,Do you want to Continue? Y or N ");
        string customerAnswer = Console.ReadLine();
        char choice;
        isSucceeded = char.TryParse(customerAnswer, out choice);
        if (!isSucceeded || !choice.isValidChoice())
        {
            Messages.InvalidInputMessage("Choice");
            goto PaymentSection;
        }
        int newProductQuantity = existProduct.Quantity - quantity;
        if (choice == 'y')
        {
            if (newProductQuantity > 0)
            {
                existProduct.Quantity = newProductQuantity;
                _unitOfWork.Products.Update(existProduct);
            }
            else
            {
                _unitOfWork.Products.Delete(existProduct);
            }
            Messages.SuccessMessage("Product", "Purchased");
            Order order = new Order
            {
                CreatedAt = DateTime.Now,
                SellerId = existProduct.SellerId,
                Seller = existProduct.Seller,
                CustomerId = customerId,
                Customer = customer,
                ProductId = existProduct.Id,
                Product = existProduct,
                ProductQuantity = quantity,
                TotalPrice = totalPaymentOfCustomer
            };
            _unitOfWork.Orders.Add(order);
            _unitOfWork.Commit();
        }
        else
        {
            Messages.CancelMessage("Purchase");
        }






    }
    public void SeePurchasedProducts()
    {
        foreach (var order in _unitOfWork.Orders.GetAll())
        {
            if (order.CustomerId == customerId)
            {
                Console.WriteLine($"You Purchased {order.Product.Name} ({order.Product.Price}$), Quantity:{order.ProductQuantity}, Total Payment {order.TotalPrice},Seller:{order.Seller.Name} {order.Seller.Surname},Date : {order.CreatedAt}");
            }

        }

    }
    public void SeePurchasedProductsByDate()
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
          

            if (order.CustomerId == customerId)
            {
                DateOnly orderDate = DateOnly.FromDateTime(order.CreatedAt);

                if (customerDate == orderDate)
                {
                   
                    Console.WriteLine($"{order.Id}.{order.Product.Name}({order.Product.Price}$), Quantity:{order.ProductQuantity},Total Payment {order.TotalPrice},Seller:{order.Seller.Name} {order.Seller.Surname},Date {order.CreatedAt}");
                }


            }
        }

    }
    public void Filter()
    {
        foreach(var product in _unitOfWork.Products.GetAll())
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

}
