using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Identity.DocumentDB
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum TypeEnum { User, Role }
}
