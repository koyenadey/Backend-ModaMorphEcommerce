using System.Text.Json.Serialization;

namespace ECommWeb.Core.src.ValueObject
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Role
    {
        Admin,
        Customer,
    }
}