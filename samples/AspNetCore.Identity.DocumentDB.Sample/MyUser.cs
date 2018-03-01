using Microsoft.AspNetCore.Identity.DocumentDB;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCore.Identity.DocumentDB.Sample
{
    public class MyUser : IdentityUser
    {
        [JsonProperty(PropertyName = "CustomProperty")]
        public string CustomProperty { get; set; }
    }
}
