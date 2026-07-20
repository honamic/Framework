namespace Honamic.Framework.EntityFramework;

public static class CustomDbFunctionsExtensions
{
    // Stub mapped to SQL Server's built-in SWITCHOFFSET via HasDbFunction
    // Never actually executed in .NET; EF Core only uses it to translate LINQ calls to SQL.
    public static DateTimeOffset SwitchOffset(DateTimeOffset dateTimeOffset, string offset) =>
        throw new NotSupportedException("This method is for SQL translation only and cannot be executed client-side.");


    public static string FormatOffset(TimeSpan offset)
    {
        var sign = offset < TimeSpan.Zero ? "-" : "+";
        return $"{sign}{Math.Abs(offset.Hours):D2}:{Math.Abs(offset.Minutes):D2}";
    }
}
