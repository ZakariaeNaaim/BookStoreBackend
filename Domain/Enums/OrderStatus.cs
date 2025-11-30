using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Domain.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum OrderStatus
    {
        Pending,
        Approved,
        Processing,
        Shipped,
        Canceled,
        Refunded
    }
}
