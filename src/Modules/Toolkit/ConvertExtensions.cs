using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Eva.ToolKit;

public static class ConvertExtensions
{
    public static T? ConvertJsonToObject<T>(this string? json) where T : class
    {
        if (string.IsNullOrWhiteSpace(json)) return null;

        return JsonConvert.DeserializeObject<T>(json);
    }

    public static object? ConvertJsonToObject(this string? json)
    {
        if (string.IsNullOrWhiteSpace(json)) return null;

        return JsonConvert.DeserializeObject(json);
    }

    public static string? ConvertObjectToJson(this object? obj, IContractResolver? contractResolver = null)
    {
        if (obj == null) return null;

        var settings = new JsonSerializerSettings
        {
            ContractResolver = contractResolver ?? new DefaultContractResolver()
        };

        return JsonConvert.SerializeObject(obj, settings);
    }
}