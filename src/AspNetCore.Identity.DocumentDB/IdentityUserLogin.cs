using Newtonsoft.Json;

namespace Microsoft.AspNetCore.Identity.DocumentDB
{
    public class IdentityUserLogin
    {
        private IdentityUserLogin()
        {
        }

        public IdentityUserLogin(string loginProvider, string providerKey, string providerDisplayName)
        {
            LoginProvider = loginProvider;
            ProviderDisplayName = providerDisplayName;
            ProviderKey = providerKey;
        }

        public IdentityUserLogin(UserLoginInfo login)
        {
            LoginProvider = login.LoginProvider;
            ProviderDisplayName = login.ProviderDisplayName;
            ProviderKey = login.ProviderKey;
        }

        [JsonProperty(PropertyName = "LoginProvider")]
        public string LoginProvider { get; set; }
        [JsonProperty(PropertyName = "ProviderDisplayName")]
        public string ProviderDisplayName { get; set; }
        [JsonProperty(PropertyName = "ProviderKey")]
        public string ProviderKey { get; set; }

        public UserLoginInfo ToUserLoginInfo()
        {
            return new UserLoginInfo(LoginProvider, ProviderKey, ProviderDisplayName);
        }
    }
}