using Application.Services.Abstract;
using Core.Constants;
using Core.Constants.Extensions;
using Core.Entities;
using Data.UnitOfWork;
using Data.UnitOfWork.Concrete;
using Microsoft.AspNetCore.Identity;
using System.Globalization;


namespace Application.Services.Concrete;

public class CustomerService : ICustomerService
{
    private readonly UnitOfWork _unitOfWork;
    public int customerId;
    public Product anyProduct;

    public CustomerService(UnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;

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
            return;
        }
        var cart = new List<(Product product, int quantity)>();
        decimal totalProductsPayment = 0;
        bool isSucceeded;

        while (true)
        {
            foreach (var product in _unitOfWork.Products.GetAll())
            {
                Console.WriteLine($"{product.Id}.{product.Name}-{product.Price}$, Quantity {product.Quantity}, Seller: {product.Seller.Name} {product.Seller.Surname}");
            }

        inputProductId:
            Messages.InputMessage("Product Id");
            string productIdInput = Console.ReadLine();
            isSucceeded = int.TryParse(productIdInput, out int productId);
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
            isSucceeded = int.TryParse(customerInput, out int quantity);
            if (!isSucceeded || quantity <= 0)
            {
                Messages.InvalidInputMessage("Quantity");
                goto inputProductQuantity;
            }
            if (existProduct.Quantity < quantity)
            {
                Messages.NotEnoughItem($"{existProduct.Name}");
                goto inputProductQuantity;
            }


            totalProductsPayment += quantity * existProduct.Price;
            if (totalProductsPayment > customer.MyBankAccount)
            {
                Messages.NotEnoughItem("Money in Your Bank Account");
                return;
            }

            cart.Add((existProduct, quantity));

            Console.WriteLine("Do you want to add more products? (Y/N)");
            string addMore = Console.ReadLine();
            if (addMore?.ToLower() != "y")
            {
                break;
            }
        }


    PaymentSection:
        Console.WriteLine($"Your Payment will be {totalProductsPayment} $, Do you want to Continue? Y or N ");
        string customerAnswer = Console.ReadLine();
        isSucceeded = char.TryParse(customerAnswer, out char choice);
        if (!isSucceeded || !choice.isValidChoice())
        {
            Messages.InvalidInputMessage("Choice");
            goto PaymentSection;
        }

        if (choice == 'y')
        {
            foreach (var (product, quantity) in cart)
            {
                anyProduct = product;
                int newProductQuantity = product.Quantity - quantity;

                product.Quantity = newProductQuantity;
                _unitOfWork.Products.Update(product);             

                Order order = new Order
                {
                    CreatedAt = DateTime.Now,
                    SellerId = product.SellerId,
                    Seller = product.Seller,
                    CustomerId = customerId,
                    Customer = customer,
                    ProductId = product.Id,
                    Product = product,
                    ProductQuantity = quantity,
                    TotalPrice = quantity * product.Price
                };
                _unitOfWork.Orders.Add(order);
            }

            customer.MyBankAccount -= totalProductsPayment;
            _unitOfWork.Customers.Update(customer);

            var seller = _unitOfWork.Sellers.GetAll().FirstOrDefault(s => s.Id == anyProduct.SellerId);

            if (seller == null)
            {
                Messages.NotFoundMessage("Seller");
                return;
            }

            seller.MyBankAccount += totalProductsPayment;
            _unitOfWork.Sellers.Update(seller);
            _unitOfWork.Commit();
            Messages.SuccessMessage("Products", "Purchased");
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
    public void SeeMyBankAccount()
    {
        var customer = _unitOfWork.Customers.GetAll().FirstOrDefault(c => c.Id == customerId);


        Console.WriteLine($"My Bank Account: {customer.MyBankAccount}$");

    }
    public void Filter()
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

}
