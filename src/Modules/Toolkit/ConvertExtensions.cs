using Newtonsoft.Json;

namespace Eva.ToolKit;

public static class ConvertExtensions
{
    public static T? ConvertJsonToObject<T>(this string? json) where T : class
    {
        if (string.IsNullOrWhiteSpace(json))
        {
            return null;
        }

        return JsonConvert.DeserializeObject<T>(json);
    }

    public static object? ConvertJsonToObject(this string? json)
    {
        if (string.IsNullOrWhiteSpace(json))
        {
            return null;
        }

        return JsonConvert.DeserializeObject(json);
    }

    public static string? ConvertObjectToJson(this object? obj)
    {
        if (obj == null)
        {
            return null;
        }

        return JsonConvert.SerializeObject(obj);
    }
}