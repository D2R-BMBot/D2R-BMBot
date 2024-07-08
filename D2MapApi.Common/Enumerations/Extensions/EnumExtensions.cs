using System.Text;

namespace D2MapApi.Common.Enumerations.Extensions;

public static class EnumExtensions
{
    public static string ToFriendlyString(this Enum p_value)
    {
        var rawValue = Enum.GetName(p_value.GetType(), p_value);
        
        var friendlyValue = rawValue?.Replace("_", " ") ?? string.Empty;

        var friendlyValueParts = friendlyValue.Split(" ");

        var friendlyValueBuilder = new StringBuilder();
        
        foreach (var part in friendlyValueParts)
        {
            var builder = new StringBuilder();
            for (var i = 0; i < part.Length; ++i)
            {
                builder.Append(i == 0 ? char.ToUpperInvariant(part[i]) : char.ToLowerInvariant(part[i]));
            }

            friendlyValueBuilder.Append(builder);
            friendlyValueBuilder.Append(' ');
        }
        
        return friendlyValueBuilder.ToString().TrimEnd();
    }
}