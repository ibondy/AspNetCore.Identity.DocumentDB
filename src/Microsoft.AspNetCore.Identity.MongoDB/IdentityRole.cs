namespace Microsoft.AspNetCore.Identity.DocumentDB
{
    using Microsoft.Azure.Documents;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    public class IdentityRole : Document
	{
		public IdentityRole()
		{
		}

		public IdentityRole(string roleName) : this()
		{
			Name = roleName;
		}

        [JsonProperty(PropertyName = "Name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "NormalizedName")]
        public string NormalizedName { get; set; }

		public override string ToString() => Name;
	}
}