using Newtonsoft.Json;

namespace Microsoft.AspNetCore.Identity.DocumentDB
{
	/// <summary>
	///     Authentication token associated with a user
	/// </summary>
	public class IdentityUserToken
	{
        public IdentityUserToken()
        {
        }

        /// <summary>
        /// The provider that the token came from.
        /// </summary>
        [JsonProperty(PropertyName = "LoginProvider")]
        public string LoginProvider { get; set; }

        /// <summary>
        /// The name of the token.
        /// </summary>
        [JsonProperty(PropertyName = "Name")]
        public string Name { get; set; }

        /// <summary>
        /// The value of the token.
        /// </summary>
        [JsonProperty(PropertyName = "Value")]
        public string Value { get; set; }
	}
}