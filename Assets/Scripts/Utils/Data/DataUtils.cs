using Newtonsoft.Json;
using Sirenix.Utilities;

public static class DataExtensions
{
    public static string ToJson(this object obj)
        => JsonConvert.SerializeObject(obj);
    public static T FromJson<T>(this string str)
        => str.IsNullOrWhitespace() 
            ? default 
            : JsonConvert.DeserializeObject<T>(str);
    
}