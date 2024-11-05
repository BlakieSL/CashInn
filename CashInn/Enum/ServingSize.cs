using System.Text.Json.Serialization;

namespace CashInn.Enum;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ServingSize
{
    Small = 1,
    Medium = 2,
    Big = 3
}