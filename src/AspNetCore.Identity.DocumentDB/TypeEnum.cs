using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Microsoft.AspNetCore.Identity.DocumentDB
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum TypeEnum { User, Role }
}
