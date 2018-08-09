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

        [JsonIgnore]
        public override string Id { get { return RoleId ?? DocId; } set { DocId = value; } }

        // TODO make the field name "partition" configurable
        [JsonProperty("partition", NullValueHandling = NullValueHandling.Ignore)]
        internal virtual string RoleId { get; set; }

        [JsonProperty(PropertyName = "id")]
        internal string DocId { get; set; }

        [JsonProperty(PropertyName = "type")]
        public virtual TypeEnum Type => TypeEnum.Role;

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "normalizedName")]
        public string NormalizedName { get; set; }

        public override string ToString() => Name;
    }
}