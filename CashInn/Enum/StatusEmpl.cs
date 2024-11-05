using System.Text.Json.Serialization;

namespace CashInn.Enum;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum StatusEmpl
{
    FullTime = 0,
    PartTime = 1
}