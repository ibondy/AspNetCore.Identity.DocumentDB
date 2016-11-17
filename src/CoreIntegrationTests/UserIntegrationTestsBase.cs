namespace IntegrationTests
{
    using System;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.DocumentDB;
    using Microsoft.Extensions.DependencyInjection;
    using NUnit.Framework;
    using Microsoft.Azure.Documents.Client;
    using System.Linq;
    using Microsoft.Azure.Documents;
    using System.Diagnostics;
    using System.Threading.Tasks;

    public class UserIntegrationTestsBase : AssertionHelper
    {
        protected DocumentClient Client;
        protected Database Database;
        protected DocumentCollection Users;
        protected DocumentCollection Roles;

        // note: for now we'll have interfaces to both the new and old apis for DocumentDB, that way we don't have to update all the tests at once and risk introducing bugs
        protected IServiceProvider ServiceProvider;

        // Default settings to connect with DocumentDB Emulator:
        private const string endpointUrl = "https://localhost:8081";
        private const string primaryKey = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";
        private static readonly string databaseName = "AspDotNetCore.Identity.DocumentDB.Test";

        [SetUp]
        public async Task BeforeEachTest()
        {
            ConnectionPolicy.Default.EnableEndpointDiscovery = false;

            Client = new DocumentClient(new Uri(endpointUrl), primaryKey, connectionPolicy: ConnectionPolicy.Default);
            Database = await Client.ReadDatabaseAsync(UriFactory.CreateDatabaseUri(databaseName))
                ?? (await Client.CreateDatabaseAsync(new Database { Id = databaseName })).Resource;
            // Client.CreateDatabaseQuery().Where(d => d.Id == databaseName).AsEnumerable().FirstOrDefault()

            Users = Client.CreateDocumentCollectionQuery(Database.SelfLink).Where(c => c.Id.Equals("users")).AsEnumerable().FirstOrDefault();
            Roles = Client.CreateDocumentCollectionQuery(Database.SelfLink).Where(c => c.Id.Equals("roles")).AsEnumerable().FirstOrDefault();

            if (Users != null) { await Client.DeleteDocumentCollectionAsync(Users.SelfLink); }
            if (Roles != null) { await Client.DeleteDocumentCollectionAsync(Roles.SelfLink); }

            Users = Client.CreateDocumentCollectionAsync(Database.SelfLink, new DocumentCollection { Id = "users" }).Result;
            Roles = Client.CreateDocumentCollectionAsync(Database.SelfLink, new DocumentCollection { Id = "roles" }).Result;

            ServiceProvider = CreateServiceProvider<IdentityUser, IdentityRole>();
        }

        protected UserManager<IdentityUser> GetUserManager()
            => ServiceProvider.GetService<UserManager<IdentityUser>>();

        protected RoleManager<IdentityRole> GetRoleManager()
            => ServiceProvider.GetService<RoleManager<IdentityRole>>();

        protected IServiceProvider CreateServiceProvider<TUser, TRole>(Action<IdentityOptions> optionsProvider = null)
            where TUser : IdentityUser
            where TRole : IdentityRole
        {
            var services = new ServiceCollection();
            optionsProvider = optionsProvider ?? (options => { });
            services.AddIdentity<TUser, TRole>(optionsProvider)
                .AddDefaultTokenProviders()
                .RegisterDocumentDBStores<TUser, TRole>(Client, Database.SelfLink);

            services.AddLogging();

            return services.BuildServiceProvider();
        }
    }
}