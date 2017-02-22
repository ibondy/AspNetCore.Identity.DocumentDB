namespace Tests
{
    using Microsoft.AspNetCore.Identity.DocumentDB;
    using Newtonsoft.Json;
    using NUnit.Framework;

    // todo low - validate all tests work
    [TestFixture]
    public class IdentityUserTests : AssertionHelper
    {
        // No ID is expected behavior; DocumentDB generates unique ID on document creation

        [Test]
        public void Create_NoPassword_DoesNotSerializePasswordField()
        {
            // if a particular consuming application doesn't intend to use passwords, there's no reason to store a null entry except for padding concerns, if that is the case then the consumer may want to create a custom class map to serialize as desired.

            var user = new IdentityUser();

            user = SerializeAndDeserialize(user);

            Expect(user.PasswordHash, Is.Null);
        }

        [Test]
        public void Create_NullLists_DoesNotSerializeNullLists()
        {
            // serialized nulls can cause havoc in deserialization, overwriting the constructor's initial empty list 
            var user = new IdentityUser();
            user.Roles = null;
            user.Tokens = null;
            user.Logins = null;
            user.Claims = null;

            user = SerializeAndDeserialize(user);

            Expect(user.Roles.Count, Is.Zero);
            Expect(user.Tokens.Count, Is.Zero);
            Expect(user.Logins.Count, Is.Zero);
            Expect(user.Claims.Count, Is.Zero);
        }

        [Test]
        public void Create_NewIdentityUser_ListsNotNull()
        {
            var user = new IdentityUser();

            user = SerializeAndDeserialize(user);

            Expect(user.Logins, Is.Empty);
            Expect(user.Tokens, Is.Empty);
            Expect(user.Roles, Is.Empty);
            Expect(user.Claims, Is.Empty);
        }

        private T SerializeAndDeserialize<T>(T obj)
        {
            var userJson = JsonConvert.SerializeObject(obj);
            obj = JsonConvert.DeserializeObject<T>(userJson);

            return obj;
        }
    }
}