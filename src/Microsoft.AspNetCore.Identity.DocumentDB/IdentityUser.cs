namespace Microsoft.AspNetCore.Identity.DocumentDB
{
    using Azure.Documents;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Security.Claims;

    public class IdentityUser : Resource
    {
        public IdentityUser()
        {
            Roles = new List<string>();
            Logins = new List<IdentityUserLogin>();
            Claims = new List<IdentityUserClaim>();
            Tokens = new List<IdentityUserToken>();
        }

        [JsonProperty(PropertyName = "Type")]
        public virtual TypeEnum Type { get { return TypeEnum.User; } }

        [JsonProperty(PropertyName = "UserName")]
        public virtual string UserName { get; set; }

        [JsonProperty(PropertyName = "NormalizedUserName")]
        public virtual string NormalizedUserName { get; set; }

        /// <summary>
        ///     A random value that must change whenever a users credentials change
        ///     (password changed, login removed)
        /// </summary>
        [JsonProperty(PropertyName = "SecurityStamp")]
        public virtual string SecurityStamp { get; set; }

        [JsonProperty(PropertyName = "Email")]
        public virtual string Email { get; set; }

        [JsonProperty(PropertyName = "NormalizedEmail")]
        public virtual string NormalizedEmail { get; set; }

        [JsonProperty(PropertyName = "EmailConfirmed")]
        public virtual bool EmailConfirmed { get; set; }

        [JsonProperty(PropertyName = "PhoneNumber")]
        public virtual string PhoneNumber { get; set; }

        [JsonProperty(PropertyName = "PhoneNumberConfirmed")]
        public virtual bool PhoneNumberConfirmed { get; set; }

        [JsonProperty(PropertyName = "TwoFactorEnabled")]
        public virtual bool TwoFactorEnabled { get; set; }

        [JsonProperty(PropertyName = "LockoutEndDateUtc")]
        public virtual DateTime? LockoutEndDateUtc { get; set; }

        [JsonProperty(PropertyName = "LockoutEnabled")]
        public virtual bool LockoutEnabled { get; set; }

        [JsonProperty(PropertyName = "AccessFailedCount")]
        public virtual int AccessFailedCount { get; set; }

        [DefaultValue(default(List<string>))]
        [JsonProperty(PropertyName = "Roles", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Populate)]
        public virtual List<string> Roles { get; set; }

        public virtual void AddRole(string role)
        {
            Roles.Add(role);
        }

        public virtual void RemoveRole(string role)
        {
            Roles.Remove(role);
        }

        [JsonProperty(PropertyName = "PasswordHash", NullValueHandling = NullValueHandling.Ignore)]
        public virtual string PasswordHash { get; set; }

        [DefaultValue(default(List<IdentityUserLogin>))]
        [JsonProperty(PropertyName = "Logins", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Populate)]
        public virtual List<IdentityUserLogin> Logins { get; set; }

        public virtual void AddLogin(UserLoginInfo login)
        {
            Logins.Add(new IdentityUserLogin(login));
        }

        public virtual void RemoveLogin(string loginProvider, string providerKey)
        {
            Logins.RemoveAll(l => l.LoginProvider == loginProvider && l.ProviderKey == providerKey);
        }

        public virtual bool HasPassword()
        {
            return false;
        }

        [DefaultValue(default(List<IdentityUserClaim>))]
        [JsonProperty(PropertyName = "Claims", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Populate)]
        public virtual List<IdentityUserClaim> Claims { get; set; }

        public virtual void AddClaim(Claim claim)
        {
            Claims.Add(new IdentityUserClaim(claim));
        }

        public virtual void RemoveClaim(Claim claim)
        {
            Claims.RemoveAll(c => c.Type == claim.Type && c.Value == claim.Value);
        }

        public virtual void ReplaceClaim(Claim existingClaim, Claim newClaim)
        {
            var claimExists = Claims
                .Any(c => c.Type == existingClaim.Type && c.Value == existingClaim.Value);
            if (!claimExists)
            {
                // note: nothing to update, ignore, no need to throw
                return;
            }
            RemoveClaim(existingClaim);
            AddClaim(newClaim);
        }

        [DefaultValue(default(List<IdentityUserToken>))]
        [JsonProperty(PropertyName = "Tokens", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Populate)]
        public virtual List<IdentityUserToken> Tokens { get; set; }

        private IdentityUserToken GetToken(string loginProider, string name)
            => Tokens
                .FirstOrDefault(t => t.LoginProvider == loginProider && t.Name == name);

        public virtual void SetToken(string loginProider, string name, string value)
        {
            var existingToken = GetToken(loginProider, name);
            if (existingToken != null)
            {
                existingToken.Value = value;
                return;
            }

            Tokens.Add(new IdentityUserToken
            {
                LoginProvider = loginProider,
                Name = name,
                Value = value
            });
        }

        public virtual string GetTokenValue(string loginProider, string name)
        {
            return GetToken(loginProider, name)?.Value;
        }

        public virtual void RemoveToken(string loginProvider, string name)
        {
            Tokens.RemoveAll(t => t.LoginProvider == loginProvider && t.Name == name);
        }

        public override string ToString() => UserName;
    }
}