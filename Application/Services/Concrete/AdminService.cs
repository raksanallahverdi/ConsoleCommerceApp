namespace Application.Services.Concrete;
using Application.Services.Abstract;
using Core.Constants;
using Core.Entities;
using Data.UnitOfWork.Concrete;
using Microsoft.AspNetCore.Identity;
using System.Globalization;
using System.Text.RegularExpressions;

public class AdminService : IAdminService
{

    private readonly UnitOfWork _unitOfWork;
    public AdminService()
    {
        _unitOfWork = new UnitOfWork();

    }
    public bool AdminLogin()
    {
        Messages.InputMessage("Admin Name");
        string adminName = Console.ReadLine();
        Messages.InputMessage("Password");
        string password = Console.ReadLine();

        var admin = _unitOfWork.Admins.GetAll().FirstOrDefault(x => x.Name == adminName);

        if (admin == null)
        {
            Messages.InvalidInputMessage("Email or Password");
            return false;
        }

        PasswordHasher<Admin> passwordHasher = new PasswordHasher<Admin>();
        var result = passwordHasher.VerifyHashedPassword(admin, admin.Password, password);

        if (result == PasswordVerificationResult.Failed)
        {
            Messages.InvalidInputMessage("Email or Password");
            return false;
        }
        Console.WriteLine(" ");
        Messages.SuccessMessage("Login", "Done");
        return true;

    }
    public void GetSellers()
    {
        foreach (var seller in _unitOfWork.Sellers.GetAll())
        {
            Console.WriteLine($"{seller.Id}.{seller.Name} {seller.Surname},Email: {seller.Email}, Pin: {seller.Pin}, Serial Number: {seller.SerialNumber}");
        }
        if (_unitOfWork.Sellers.GetAll().Count == 0)
        {
            Messages.NotFoundMessage("Seller");
        }
    }
    public void CreateSeller()
    {
    inputSellerName:
        Messages.InputMessage("New Seller Name");
        string newSellerName = Console.ReadLine();
        if (string.IsNullOrEmpty(newSellerName))
        {
            Messages.InvalidInputMessage("Seller Name");
            goto inputSellerName;
        }
    inputSellerSurname:
        Messages.InputMessage("New Seller Surname");
        string newSellerSurname = Console.ReadLine();
        if (string.IsNullOrEmpty(newSellerSurname))
        {
            Messages.InvalidInputMessage("Seller Surname");
            goto inputSellerSurname;
        }
    inputSellerPhoneNumber:
        Messages.InputMessage("New Seller PhoneNumber");
        string newPhoneNumber = Console.ReadLine();
        if (string.IsNullOrEmpty(newPhoneNumber))
        {
            Messages.InvalidInputMessage("Phone Number");
            goto inputSellerPhoneNumber;
        }
    inputSellerEmail:
        Messages.InputMessage("New Email");
        string sellerEmail = Console.ReadLine();
        bool isEmail = Regex.IsMatch(sellerEmail, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
        if (string.IsNullOrEmpty(sellerEmail) || !isEmail)
        {
            Messages.InvalidInputMessage("New Seller Email");
            goto inputSellerEmail;
        }
        var existedEmail = _unitOfWork.Sellers.GetAll().FirstOrDefault(x => x.Email == sellerEmail);
        if (existedEmail != null)
        {
            Messages.AlreadyExistError("This Email");
            goto inputSellerEmail;


        }
    inputSellerPin:
        Messages.InputMessage("Pin (7 Digits)");
        string sellerPin = Console.ReadLine();
        if (string.IsNullOrEmpty(sellerPin) || sellerPin.Length != 7)
        {
            Messages.InvalidInputMessage("Seller Pin");
            goto inputSellerPin;
        }
    inputSellerSerialNumber:
        Messages.InputMessage("Serial Number (9 Digits)");
        string sellerSerialNumber = Console.ReadLine();
        if (string.IsNullOrEmpty(sellerSerialNumber) || sellerSerialNumber.Length != 9)
        {
            Messages.InvalidInputMessage("Serial Number");
            goto inputSellerSerialNumber;
        }
    inputSellerPassword:
        Messages.InputMessage("Password (Min 8 digits) ");
        string sellerPassword = Console.ReadLine();
        if (string.IsNullOrEmpty(sellerPassword) || sellerPassword.Length < 8)
        {
            Messages.InvalidInputMessage("Password");
        }
        PasswordHasher<Seller> passwordHasher = new PasswordHasher<Seller>();



        Seller seller = new Seller
        {
            Name = newSellerName,
            Surname = newSellerSurname,
            PhoneNumber = newPhoneNumber,
            Email = sellerEmail,
            Pin = sellerPin,
            SerialNumber = sellerSerialNumber
        };

        seller.Password = passwordHasher.HashPassword(seller, sellerPassword);

        _unitOfWork.Sellers.Add(seller);
        Messages.SuccessMessage("Seller", "Added");
        _unitOfWork.Commit();

    }
    public void DeleteSeller()
    {
        GetSellers();
    sellerIdSection:
        Messages.InputMessage("Seller Id");
        string userInput = Console.ReadLine();
        int choice;
        bool isSucceeded = int.TryParse(userInput, out choice);
        if (!isSucceeded)
        {
            Messages.InvalidInputMessage("Seller Id");
            goto sellerIdSection;
        }
        Seller seller = _unitOfWork.Sellers.Get(choice);
        if (seller == null)
        {
            Messages.NotFoundMessage("Seller");
        }
        _unitOfWork.Sellers.Delete(seller);
        Messages.SuccessMessage("Seller", "Deleted");
        _unitOfWork.Commit();


    }
    public void GetCustomers()
    {
        if (_unitOfWork.Customers.GetAll().Count == 0)
        {
            Messages.NotFoundMessage("Customer");
        }
        foreach (var customer in _unitOfWork.Customers.GetAll())
        {
            Console.WriteLine($"{customer.Id}.{customer.Name} {customer.Surname}, Email: {customer.Email},Pin: {customer.Pin},Serial Number {customer.SerialNumber}");
        }

    }
    public void CreateCustomer()
    {
    inputCustomerName:
        Messages.InputMessage("New Customer Name");
        string newCustomerName = Console.ReadLine();
        if (string.IsNullOrEmpty(newCustomerName))
        {
            Messages.InvalidInputMessage("Customer Name");
            goto inputCustomerName;
        }
    inputCustomerSurname:
        Messages.InputMessage("New Customer Surname");
        string newCustomerSurname = Console.ReadLine();
        if (string.IsNullOrEmpty(newCustomerSurname))
        {
            Messages.InvalidInputMessage("Customer Surname");
            goto inputCustomerSurname;
        }
    inputCustomerPhoneNumber:
        Messages.InputMessage("New Customer PhoneNumber");
        string newPhoneNumber = Console.ReadLine();
        if (string.IsNullOrEmpty(newPhoneNumber))
        {
            Messages.InvalidInputMessage("Phone Number");
            goto inputCustomerPhoneNumber;
        }
    inputCustomerEmail:
        Messages.InputMessage("New Email");
        string customerEmail = Console.ReadLine();
        bool isEmail = Regex.IsMatch(customerEmail, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
        if (string.IsNullOrEmpty(customerEmail) || !isEmail)
        {
            Messages.InvalidInputMessage("New Customer Email");
            goto inputCustomerEmail;
        }
        var existedEmail=_unitOfWork.Customers.GetAll().FirstOrDefault(x => x.Email == customerEmail);
        if (existedEmail != null)
        {
            Messages.AlreadyExistError("This Email");
            goto inputCustomerEmail;

        }

    inputCustomerPin:
        Messages.InputMessage("Pin (7 Digits)");
        string customerPin = Console.ReadLine();
        if (string.IsNullOrEmpty(customerPin) || customerPin.Length != 7)
        {
            Messages.InvalidInputMessage("Customer Pin");
            goto inputCustomerPin;
        }
    inputCustomerSerialNumber:
        Messages.InputMessage("Serial Number (9 Digits)");
        string customerSerialNumber = Console.ReadLine();
        if (string.IsNullOrEmpty(customerSerialNumber) || customerSerialNumber.Length != 9)
        {
            Messages.InvalidInputMessage("Serial Number");
            goto inputCustomerSerialNumber;
        }

    inputCustomerPassword:
        Messages.InputMessage("Password (Min 8 digits) ");
        string customerPassword = Console.ReadLine();
        if (string.IsNullOrEmpty(customerPassword) || customerPassword.Length < 8)
        {
            Messages.InvalidInputMessage("Password");
        }
        PasswordHasher<Customer> passwordHasher = new PasswordHasher<Customer>();


        Customer customer = new Customer
        {
            Name = newCustomerName,
            Surname = newCustomerSurname,
            PhoneNumber = newPhoneNumber,
            Email = customerEmail,
            Pin = customerPin,
            SerialNumber = customerSerialNumber
        };
        customer.Password = passwordHasher.HashPassword(customer, customerPassword);

        _unitOfWork.Customers.Add(customer);
        Messages.SuccessMessage("Customer", "Added");
        _unitOfWork.Commit();
    }
    public void DeleteCustomer()
    {
        GetCustomers();
    customerIdSection:
        Messages.InputMessage("Customer Id");
        string userInput = Console.ReadLine();
        int choice;
        bool isSucceeded = int.TryParse(userInput, out choice);
        if (!isSucceeded)
        {
            Messages.InvalidInputMessage("Customer Id");
            goto customerIdSection;
        }
        Customer customer = _unitOfWork.Customers.Get(choice);
        if (customer == null)
        {
            Messages.NotFoundMessage("Customer");
            return;
        }
        _unitOfWork.Customers.Delete(customer);
        Messages.SuccessMessage("Customer", "Deleted");
        _unitOfWork.Commit();
    }

    public void AddCategory()
    {
    CategoryNameSection:
        Messages.InputMessage("Category Name");
        string categoryName = Console.ReadLine();
        Category existCategory = _unitOfWork.Categories.GetAll().FirstOrDefault(c => c.Name == categoryName);
        if (string.IsNullOrEmpty(categoryName))
        {
            Messages.InvalidInputMessage("Category Name");
            goto CategoryNameSection;
        }
        if (existCategory != null)
        {
            Messages.AlreadyExistError("Category");
            goto CategoryNameSection;
        }
        Category category = new Category
        {
            Name = categoryName,
            CreatedAt = DateTime.Now
        };
        _unitOfWork.Categories.Add(category);
        Messages.SuccessMessage("Category", "Added");
        _unitOfWork.Commit();

    }
    public void SeeAllCategories()
    {
        foreach (var category in _unitOfWork.Categories.GetAll())
        {
            Console.WriteLine($"{category.Id}.{category.Name}");
        }
        if (_unitOfWork.Categories.GetAll().Count == 0)
        {
            Messages.NotFoundMessage("Category");
        }

    }
    public void DeleteCategory()
    {
        SeeAllCategories();
    CategoryIdSection:
        Messages.InputMessage("Category Id");
        string userInput = Console.ReadLine();
        int choice;
        bool isSucceeded = int.TryParse(userInput, out choice);
        if (!isSucceeded)
        {
            Messages.InvalidInputMessage("Category Id");
            goto CategoryIdSection;
        }
        Category category = _unitOfWork.Categories.Get(choice);
        if (category == null)
        {
            Messages.NotFoundMessage("Category");
        }
        _unitOfWork.Categories.Delete(category);
        Messages.SuccessMessage("Category", "Deleted");
        _unitOfWork.Commit();


    }

    public void SeeOrders()
    {
        foreach (var order in _unitOfWork.Orders.GetAll())
        {
            Console.WriteLine($"{order.Id}.{order.Product.Name}({order.Product.Price}$), Quantity:{order.ProductQuantity},Total Price {order.TotalPrice},Seller:{order.Seller.Name} {order.Seller.Surname},Customer:{order.Customer.Name} {order.Customer.Surname},Date {order.CreatedAt}");
        }
        if (_unitOfWork.Orders.GetAll().Count == 0)
        {
            Messages.NotFoundMessage("Order");
        }
    }
    public void SeeOrderBySeller()
    {
        GetSellers();
    sellerIdSection:
        Messages.InputMessage("Seller Id");
        string userInput = Console.ReadLine();
        int choice;
        bool isSucceeded = int.TryParse(userInput, out choice);
        if (!isSucceeded)
        {
            Messages.InvalidInputMessage("Seller Id");
            goto sellerIdSection;
        }
        Seller seller = _unitOfWork.Sellers.Get(choice);
        if (seller == null)
        {
            Messages.NotFoundMessage("Seller");
        }
        foreach (var order in _unitOfWork.Orders.GetAll())
        {
            if (order.SellerId == choice)
            {
                Console.WriteLine($"Orders Of - {order.Seller.Name} {order.Seller.Surname}:");
                Console.WriteLine($"{order.Id}.{order.Product.Name}({order.Product.Price}$), Quantity:{order.ProductQuantity},Total Income: {order.TotalPrice},Customer:{order.Customer.Name} {order.Customer.Surname},Date {order.CreatedAt}");
            }
        }

    }
    public void SeeOrderByCustomer()
    {
        GetCustomers();
    customerIdSection:
        Messages.InputMessage("Customer Id");
        string userInput = Console.ReadLine();
        int choice;
        bool isSucceeded = int.TryParse(userInput, out choice);
        if (!isSucceeded)
        {
            Messages.InvalidInputMessage("Customer Id");
            goto customerIdSection;
        }
        Customer customer = _unitOfWork.Customers.Get(choice);
        if (customer == null)
        {
            Messages.NotFoundMessage("Customer");
        }
        foreach (var order in _unitOfWork.Orders.GetAll())
        {
            if (order.CustomerId == choice)
            {
                Console.WriteLine($"Orders Of - {order.Customer.Name} {order.Customer.Surname}:");
                Console.WriteLine($"{order.Id}.{order.Product.Name}({order.Product.Price}$), Quantity:{order.ProductQuantity},Total Income: {order.TotalPrice},Seller:{order.Seller.Name} {order.Seller.Surname},Date {order.CreatedAt}");
            }
        }

    }
    public void SeeOrderByDate()
    {
    DateSection:
        Console.WriteLine("Please enter Date (dd.mm.yyy)");
        string adminInput = Console.ReadLine();
        DateOnly adminDate;
        bool isSucceeded = DateOnly.TryParseExact(adminInput, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out adminDate);

        if (!isSucceeded)
        {
            Messages.InvalidInputMessage("Date");
            goto DateSection;
        }

        foreach (var order in _unitOfWork.Orders.GetAll())
        {
            DateOnly orderDate = DateOnly.FromDateTime(order.CreatedAt);

            if (adminDate == orderDate)
            {
                Console.WriteLine($"{order.Id}.{order.Product.Name}({order.Product.Price}$), Quantity:{order.ProductQuantity},Total Price {order.TotalPrice},Seller:{order.Seller.Name} {order.Seller.Surname},Customer:{order.Customer.Name} {order.Customer.Surname},Date {order.CreatedAt}");
            }



        }
    }
}
