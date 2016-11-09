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

    public class UserIntegrationTestsBase : AssertionHelper
	{
        protected DocumentClient Client;
		protected Database Database;
		protected DocumentCollection Users;
		protected DocumentCollection Roles;

		// note: for now we'll have interfaces to both the new and old apis for MongoDB, that way we don't have to update all the tests at once and risk introducing bugs
		protected IServiceProvider ServiceProvider;

        // TODO to execute unit tests following variables must be set:
        private const string endpointUrl = "";
        private const string primaryKey = "";
        private static readonly string databaseName = "";

		[SetUp]
		public void BeforeEachTest()
		{
            Client = new DocumentClient(new Uri(endpointUrl), primaryKey);
            Database = Client.CreateDatabaseQuery().Where(d => d.Id == databaseName).AsEnumerable().FirstOrDefault()
                ?? Client.CreateDatabaseAsync(new Database { Id = databaseName }).Result;


            Users = Client.CreateDocumentCollectionQuery(Database.SelfLink).Where(c => c.Id.Equals("users")).AsEnumerable().FirstOrDefault();
            Roles = Client.CreateDocumentCollectionQuery(Database.SelfLink).Where(c => c.Id.Equals("roles")).AsEnumerable().FirstOrDefault();

            if (Users != null) { Client.DeleteDocumentCollectionAsync(Users.SelfLink); }
            if (Roles != null) { Client.DeleteDocumentCollectionAsync(Roles.SelfLink); }

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