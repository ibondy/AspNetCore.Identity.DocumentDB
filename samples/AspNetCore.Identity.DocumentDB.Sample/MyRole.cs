using Microsoft.AspNetCore.Identity.DocumentDB;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCore.Identity.DocumentDB.Sample
{
    public class MyRole : IdentityRole
    {
        [JsonProperty(PropertyName = "CustomRoleProperty")]
        public string CustomRoleProperty { get; set; }
    }
}
