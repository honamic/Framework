namespace Honamic.Framework.Utilities.Extensions;

public static class StringExtensions
{
    public static bool HasValue(this string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return false;
        }

        return true;
    }


    public static string? SeparateCamelCase(this string? stringValue)
    {
        if (stringValue is null)
            return stringValue;


        for (int i = 1; i < stringValue.Length; i++)
        {
            if (char.IsUpper(stringValue[i]))
            {
                stringValue = stringValue.Insert(i, " ");
                i++;
            }
        }

        return stringValue;
    }

}
