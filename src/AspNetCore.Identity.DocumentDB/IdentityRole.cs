namespace Microsoft.AspNetCore.Identity.DocumentDB
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    public class IdentityRole : IdentityClaimStore
    {
        public IdentityRole()
        {
        }

        public IdentityRole(string roleName) : this()
        {
            Name = roleName;
        }

        // TODO make the field name "partition" configurable
        [JsonProperty("partition")]
        public virtual string PartitionKey { get { return Id; } }

        [JsonProperty(PropertyName = "type")]
        public virtual TypeEnum Type { get { return TypeEnum.Role; } }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "normalizedName")]
        public string NormalizedName { get; set; }

        public override string ToString() => Name;
    }
}