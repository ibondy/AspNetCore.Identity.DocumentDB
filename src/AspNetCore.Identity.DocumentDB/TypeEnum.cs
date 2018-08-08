using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace Microsoft.AspNetCore.Identity.DocumentDB
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum TypeEnum
    {
        [EnumMember(Value = "user")]
        User,

        [EnumMember(Value = "userMapping")]
        UserMapping,

        [EnumMember(Value = "role")]
        Role,

        [EnumMember(Value = "roleMapping")]
        RoleMapping
    }
}
