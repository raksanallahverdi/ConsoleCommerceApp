using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Core.Constants;

public class Messages
{
    public static void InvalidInputMessage(string title) {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"{title} is invalid ");
        Console.ResetColor();

    }
    public static void CancelMessage(string title)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"{title} canceled ");
        Console.ResetColor();

    }
    public static void InputMessage(string title) => Console.WriteLine($"Please enter {title}");
    public static void NotFoundMessage(string title) 
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"{title} is not FOUND ");
        Console.ResetColor();

    }
    public static void NotEnoughItem(string title)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"Unfortunately,Not enough {title} ");
        Console.ResetColor();

    }
    public static void AlreadyExistError(string title) {

        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"{title} is already exist");
        Console.ResetColor();

    }
    public static void SuccessMessage(string title, string process)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"{title} successfully {process}");
        Console.ResetColor();
    }

    public static void ErrorOccuredMessage() { Console.WriteLine("Error Occured"); }

    public static void WantToChangeMessage(string title) => Console.WriteLine($"Do you want to change {title}, Y or N?");

}
