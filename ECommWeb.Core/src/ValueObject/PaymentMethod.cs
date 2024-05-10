using System.Text.Json.Serialization;

namespace ECommWeb.Core.src.ValueObject;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum PaymentMethod
{
    debitcard,
    creditcard,
    cash
}
