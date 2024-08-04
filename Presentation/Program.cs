
using Application.Services.Abstract;
using Application.Services.Concrete;
using Core.Constants;
using Core.Constants.Operations;
using System.Threading;
using Data;
using Data.UnitOfWork.Concrete;

public static class Program
{
    private static readonly UnitOfWork _unitOfWork;
    private static readonly AdminService _adminService;
    private static readonly SellerService _sellerService;
    private static readonly CustomerService _customerService;

    static Program()
    {
        _unitOfWork= new UnitOfWork();
        _adminService = new AdminService(_unitOfWork);
        _sellerService = new SellerService(_unitOfWork);
        _customerService = new CustomerService(_unitOfWork);


    }
    public static void Main()
    {
        DbInitializer.SeedData();
        MainMenu();



    }
    public static void MainMenu()
    {
    menuSection:
        Console.WriteLine(" ");
        Console.WriteLine("Which one is your Account type?");
        Console.WriteLine(" ");
        Console.WriteLine("1.Admin");
        Console.WriteLine("2.Seller");
        Console.WriteLine("3.Customer");
        Console.WriteLine("0.Exit");
        string userInput = Console.ReadLine();
        int choice;
        bool isSucceeded = int.TryParse(userInput, out choice);
        if (!isSucceeded)
        {
            Messages.InvalidInputMessage("Choice");
            goto menuSection;
        }
        switch (choice)
        {
            case 1:
                ShowAdminMenu();
                break;
            case 2:
                ShowSellerMenu();
                break;
            case 3:
                ShowCustomerMenu();
                break;
            case 0:
                return;
            default:
                Messages.InvalidInputMessage("Choice");
                goto menuSection;

        }
    }
    public static void ShowAdminMenu()
    {
        if (_adminService.AdminLogin())
        {

            while (true)
            {
                Thread.Sleep(2300);
                Console.WriteLine(" ");
                Console.WriteLine("---Menu---");
                Console.WriteLine(" ");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Seller Operations:");
                Console.ResetColor();
                Console.WriteLine("1.Create Seller");
                Console.WriteLine("2.Get All Sellers");
                Console.WriteLine("3.Delete Seller ");
                Console.WriteLine(" ");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Customer Operations:");
                Console.ResetColor();
                Console.WriteLine("4.Create Customer");
                Console.WriteLine("5.Get All Customers");
                Console.WriteLine("6.Delete Customer");
                Console.WriteLine(" ");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Category Operations:");
                Console.ResetColor();
                Console.WriteLine("7.Add Category");
                Console.WriteLine("8.See All Categories");
                Console.WriteLine("9.Delete Category");
                Console.WriteLine(" ");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Order Operations:");
                Console.ResetColor();
                Console.WriteLine("10.See All Orders ");
                Console.WriteLine("11.See Order Of Seller");
                Console.WriteLine("12.See Order Of Customer");
                Console.WriteLine("13.See Order Of Date : (dd.mm.yyyy)");
                Console.WriteLine(" ");
                Console.WriteLine("0.Exit");
                Console.WriteLine("Choose from Menu:");

            UserInputSection:
                string userInput = Console.ReadLine();
                int choice;
                bool isSucceeded = int.TryParse(userInput, out choice);
                if (!isSucceeded)
                {
                    Messages.InvalidInputMessage("Choice");
                    goto UserInputSection;
                }
                switch ((AdminOperations)choice)
                {
                    case AdminOperations.CreateSeller:
                        _adminService.CreateSeller();
                        break;
                    case AdminOperations.GetAllSellers:
                        _adminService.GetSellers();
                        break;
                    case AdminOperations.DeleteSeller:
                        _adminService.DeleteSeller();
                        break;
                    case AdminOperations.CreateCustomer:
                        _adminService.CreateCustomer();
                        break;
                    case AdminOperations.GetAllCustomers:
                        _adminService.GetCustomers();
                        break;
                    case AdminOperations.DeleteCustomer:
                        _adminService.DeleteCustomer();
                        break;
                    case AdminOperations.AddCategory:
                        _adminService.AddCategory();
                        break;
                    case AdminOperations.SeeAllCategories:
                        _adminService.SeeAllCategories();
                        break;
                    case AdminOperations.DeleteCategory:
                        _adminService.DeleteCategory();
                        break;
                    case AdminOperations.SeeAllOrders:
                        _adminService.SeeOrders();
                        break;
                    case AdminOperations.SeeOrderOfSeller:
                        _adminService.SeeOrderBySeller();
                        break;
                    case AdminOperations.SeeOrderOfCustomer:
                        _adminService.SeeOrderByCustomer();
                        break;
                    case AdminOperations.SeeOrderOfDate:
                        _adminService.SeeOrderByDate();
                        break;
                    case AdminOperations.Exit:
                        MainMenu();
                        break;
                    default:
                        Messages.InvalidInputMessage("Choice");
                        break;

                }
            }
        }
        else
        {
            MainMenu();
        }
    }
    public static void ShowSellerMenu()
    {

        if (_sellerService.SellerLogin())
        {
            while (true)
            {
                Thread.Sleep(2300);
                Console.WriteLine(" ");
                Console.WriteLine("--Menu--");
                Console.WriteLine("1.Get All Products");
                Console.WriteLine("2.Get My Products");
                Console.WriteLine("3.See My Bank Account");
                Console.WriteLine("4.Add Product");
                Console.WriteLine("5.Update Product");
                Console.WriteLine("6.Change Product Quantity");
                Console.WriteLine("7.Delete Product");
                Console.WriteLine("8.See Who Purchased Products");
                Console.WriteLine("9.See Purchased Product For Date");
                Console.WriteLine("10.Filter Product For Name");
                Console.WriteLine("11.See Total Income");
                Console.WriteLine("0.Exit");
                Console.WriteLine(" ");
                Console.WriteLine("Choose From Menu");

                string input = Console.ReadLine();
                int choice;
                bool isSucceeded = int.TryParse(input, out choice);
                if (isSucceeded)
                {
                    switch ((SellerOperations)choice)
                    {
                        case SellerOperations.GetAllProducts:
                            _sellerService.GetProducts();
                            break;
                        case SellerOperations.GetMyProducts:
                            _sellerService.GetMyProducts();
                            break;
                        case SellerOperations.SeeMyBankAccount:
                            _sellerService.SeeMyBankAccount();
                            break;
                        case SellerOperations.Add:
                            _sellerService.AddProduct();
                            break;
                        case SellerOperations.UpdateProduct:
                            _sellerService.UpdateProduct();
                            break;
                        case SellerOperations.ChangeProductQuantity:
                            _sellerService.ChangeProductQuantity();
                            break;
                        case SellerOperations.Delete:
                            _sellerService.DeleteProduct();
                            break;
                        case SellerOperations.SeeWhoPurchased:
                            _sellerService.SeeWhoPurchasedProduct();
                            break;
                        case SellerOperations.SeeProductForDate:
                            _sellerService.SeePurchasedProductForDate();
                            break;
                        case SellerOperations.Filter:
                            _sellerService.FilterForName();
                            break;
                        case SellerOperations.SeeIncome:
                            _sellerService.SeeTotalIncome();
                            break;
                        case SellerOperations.Exit:
                            MainMenu();
                            break;
                        default:
                            Messages.InvalidInputMessage("choice");
                            break;
                    }

                }


            }

        }
        else
        {
            MainMenu();
        }


    }
    public static void ShowCustomerMenu()
    {
        if (_customerService.CustomerLogin())
        {
            while (true)
            {
                Thread.Sleep(2300);
                Console.WriteLine(" ");
                Console.WriteLine("--Menu--");
                Console.WriteLine("1.See My Bank Account");
                Console.WriteLine("2.Buy Product");
                Console.WriteLine("3.See Purchased Products");
                Console.WriteLine("4.See Purchased Products By Date");
                Console.WriteLine("5.Filter");
                Console.WriteLine("0.Exit");
                Console.WriteLine(" ");
                Console.WriteLine("Choose From Menu");

                string input = Console.ReadLine();
                int choice;
                bool isSucceeded = int.TryParse(input, out choice);
                if (isSucceeded)
                {
                    switch ((CustomerOperations)choice)
                    {
                        case CustomerOperations.SeeMyBankAccount:
                            _customerService.SeeMyBankAccount();
                            break;
                        case CustomerOperations.Buy:
                            _customerService.BuyProduct();
                            break;
                        case CustomerOperations.SeePurchasedProducts:
                            _customerService.SeePurchasedProducts();
                            break;
                        case CustomerOperations.SeePurchasedProductsByDate:
                            _customerService.SeePurchasedProductsByDate();
                            break;
                        case CustomerOperations.Filter:
                            _customerService.Filter();
                            break;
                        case CustomerOperations.Exit:
                            MainMenu();
                            break;
                        default:
                            Messages.InvalidInputMessage("choice");
                            break;
                    }

                }


            }

        }
        else
        {
            MainMenu();
        }


    }
}