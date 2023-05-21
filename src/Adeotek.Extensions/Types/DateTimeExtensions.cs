namespace Adeotek.Extensions.Types;

public static class DateTimeExtensions
{
    public static DateTime ToUtc(DateTime input)
        => input.Kind switch
        {
            DateTimeKind.Unspecified => DateTime.SpecifyKind(input, DateTimeKind.Utc),
            DateTimeKind.Local => input.ToUniversalTime(),
            _ => input
        };
}