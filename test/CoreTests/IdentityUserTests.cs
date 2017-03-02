namespace Tests
{
    using Microsoft.AspNetCore.Identity.DocumentDB;
    using Newtonsoft.Json;
    using Xunit;

    // todo low - validate all tests work
    public class IdentityUserTests
    {
        // No ID is expected behavior; DocumentDB generates unique ID on document creation

        [Fact]
        public void Create_NoPassword_DoesNotSerializePasswordField()
        {
            // if a particular consuming application doesn't intend to use passwords, there's no reason to store a null entry except for padding concerns, if that is the case then the consumer may want to create a custom class map to serialize as desired.

            var user = new IdentityUser();

            user = SerializeAndDeserialize(user);

            Assert.Null(user.PasswordHash);
        }

        [Fact]
        public void Create_NullLists_DoesNotSerializeNullLists()
        {
            // serialized nulls can cause havoc in deserialization, overwriting the constructor's initial empty list 
            var user = new IdentityUser();
            user.Roles = null;
            user.Tokens = null;
            user.Logins = null;
            user.Claims = null;

            user = SerializeAndDeserialize(user);

            Assert.Empty(user.Roles);
            Assert.Empty(user.Tokens);
            Assert.Empty(user.Logins);
            Assert.Empty(user.Claims);
        }

        [Fact]
        public void Create_NewIdentityUser_ListsNotNull()
        {
            var user = new IdentityUser();

            user = SerializeAndDeserialize(user);

            Assert.Empty(user.Roles);
            Assert.Empty(user.Tokens);
            Assert.Empty(user.Logins);
            Assert.Empty(user.Claims);
        }

        private T SerializeAndDeserialize<T>(T obj)
        {
            var userJson = JsonConvert.SerializeObject(obj);
            obj = JsonConvert.DeserializeObject<T>(userJson);

            return obj;
        }
    }
}