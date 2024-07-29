using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Safari.Net.Core.Extensions.EnumUtilities;

public static class EnumDisplay
{
    /// <summary>
    ///     Get the display name of an enum value. Default is an empty string.
    ///     Use the Display attribute to set the display name.
    /// </summary>
    /// <param name="enumValue">The enum value to get the display name for.</param>
    /// <returns>The display name of the enum value.</returns>
    public static string GetDisplayName(this Enum enumValue) =>
        enumValue
            .GetType()
            .GetMember(enumValue.ToString())
            .First()
            .GetCustomAttribute<DisplayAttribute>()
            ?.Name ?? string.Empty;

    /// <summary>
    ///     Get the display names list of an enum.
    /// </summary>
    /// <typeparam name="T">The enum type to get the display names for.</typeparam>
    /// <returns>The display names list of the enum.</returns>
    public static IEnumerable<string> GetEnumDisplayNames<T>()
        where T : Enum => GetEnumValues<T>().Select(e => e.GetDisplayName());

    /// <summary>
    ///     Get the values list of an enum.
    /// </summary>
    /// <typeparam name="T">The enum type to get the values for.</typeparam>
    /// <returns>The values list of the enum.</returns>
    public static IEnumerable<T> GetEnumValues<T>()
        where T : Enum => Enum.GetValues(typeof(T)).Cast<T>();
}
