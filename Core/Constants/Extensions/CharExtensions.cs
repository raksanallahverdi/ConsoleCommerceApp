namespace Core.Constants.Extensions;

public static class CharExtensions
{
    public static bool isValidChoice(this char choice)
    {
        if (choice.ToString().ToLower() == "y" || choice.ToString().ToLower() == "n")
            return true;
        return false;



    }
}