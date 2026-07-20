using System.Globalization;

namespace Honamic.Framework.Queries;

public static class QueryCalendarConverter
{
    private static readonly string[] PersianMonthNames =
    {
        "فروردین", "اردیبهشت", "خرداد", "تیر", "مرداد", "شهریور",
        "مهر", "آبان", "آذر", "دی", "بهمن", "اسفند"
    };

    private static readonly string[] HijriMonthNames =
    {
        "محرم", "صفر", "ربیع الأول", "ربیع الآخر", "جمادی الأولی", "جمادی الآخرة",
        "رجب", "شعبان", "رمضان", "شوال", "ذو القعدة", "ذو الحجة"
    };

    private static readonly HijriCalendar HijriCalendar = new();

    public static (int Year, int Month, int Day) ToCalendarDate(DateTime date, QueryCalendarType calendarType)
    {
        switch (calendarType)
        {
            case QueryCalendarType.Persian:
                var persianCalendar = new PersianCalendar();
                return (persianCalendar.GetYear(date), persianCalendar.GetMonth(date), persianCalendar.GetDayOfMonth(date));

            case QueryCalendarType.Hijri:
                return (HijriCalendar.GetYear(date), HijriCalendar.GetMonth(date), HijriCalendar.GetDayOfMonth(date));

            default:
                return (date.Year, date.Month, date.Day);
        }
    }

    public static string GetMonthName(int month, QueryCalendarType calendarType) => calendarType switch
    {
        QueryCalendarType.Persian => PersianMonthNames[month - 1],
        QueryCalendarType.Hijri => HijriMonthNames[month - 1],
        _ => CultureInfo.InvariantCulture.DateTimeFormat.GetMonthName(month),
    };
}

public enum QueryCalendarType
{
    Gregorian = 0,
    Persian = 1,
    Hijri = 2,
}