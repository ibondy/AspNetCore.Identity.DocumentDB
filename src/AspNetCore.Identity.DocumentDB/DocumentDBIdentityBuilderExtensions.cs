// ReSharper disable once CheckNamespace - Common convention to locate extensions in Microsoft namespaces for simplifying autocompletion as a consumer.

using System.Diagnostics.CodeAnalysis;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.DocumentDB;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;

namespace Microsoft.Extensions.DependencyInjection
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public static class DocumentDBIdentityBuilderExtensions
    {
        /// <summary>
        ///     This method only registers DocumentDB stores, you also need to call AddIdentity.
        ///     Consider using AddIdentityWithDocumentDBStores.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="documentClient">Must be an initialized DocumentClient</param>
        /// <param name="databaseLink">Must contain the database link</param>
        public static IdentityBuilder RegisterDocumentDBStores<TUser, TRole>(this IdentityBuilder builder, DocumentClient documentClient, string databaseLink)
            where TRole : IdentityRole
            where TUser : IdentityUser
        {
            if (documentClient == null)
            {
                throw new ArgumentException("documentClient cannot be null");
            }

            var collectionId = databaseLink.Substring(4);
            documentClient.CreateDatabaseIfNotExistsAsync(collectionId).Wait(30 * 1000); //30 seconds

            return builder.RegisterDocumentDBStores<TUser, TRole>(
                documentClient,
                p => documentClient.CreateDocumentCollectionQuery(databaseLink).Where(c => c.Id.Equals("users")).AsEnumerable().FirstOrDefault()
                        ?? documentClient.CreateDocumentCollectionAsync(databaseLink, new DocumentCollection { Id = "users" }).Result);
        }

        /// <summary>
        ///     If you want control over creating the users and roles collections, use this overload.
        ///     This method only registers DocumentDB stores, you also need to call AddIdentity.
        /// </summary>
        /// <typeparam name="TUser"></typeparam>
        /// <typeparam name="TRole"></typeparam>
        /// <param name="builder"></param>
        /// <param name="documentClient"></param>
        /// <param name="collectionFactory">Function containing DocumentCollection</param>
        public static IdentityBuilder RegisterDocumentDBStores<TUser, TRole>(this IdentityBuilder builder,
            DocumentClient documentClient,
            Func<IServiceProvider, DocumentCollection> collectionFactory)
            where TRole : IdentityRole
            where TUser : IdentityUser
        {
            if (typeof(TUser) != builder.UserType)
            {
                var message = "User type passed to RegisterDocumentDBStores must match user type passed to AddIdentity. "
                              + $"You passed {builder.UserType} to AddIdentity and {typeof(TUser)} to RegisterDocumentDBStores, "
                              + "these do not match.";
                throw new ArgumentException(message);
            }
            if (typeof(TRole) != builder.RoleType)
            {
                var message = "Role type passed to RegisterDocumentDBStores must match role type passed to AddIdentity. "
                              + $"You passed {builder.RoleType} to AddIdentity and {typeof(TRole)} to RegisterDocumentDBStores, "
                              + "these do not match.";
                throw new ArgumentException(message);
            }

            builder.Services.AddSingleton<IUserStore<TUser>>(p => new UserStore<TUser>(documentClient, collectionFactory(p)));
            builder.Services.AddSingleton<IRoleStore<TRole>>(p => new RoleStore<TRole>(documentClient, collectionFactory(p)));

            return builder;
        }

        /// <summary>
        ///     This method only registers DocumentDB stores, you also need to call AddIdentity.
        ///     Consider using AddIdentityWithDocumentDBStores.
        /// </summary>
        /// <typeparam name="TUser">The type associated with user identity information.</typeparam>
        /// <typeparam name="TRole">The type associated with role identity information.</typeparam>
        /// <param name="builder">The <see cref="IdentityBuilder"/> to build upon.</param>
        /// <param name="options">The options for creating the DocumentDB client.</param>
        /// <returns>The <see cref="IdentityBuilder"/> with the DocumentDB settings applied.</returns>
        /// <remarks>
        ///     This does <b>not</b> register the options with the ASP.Net Core options framework.
        /// </remarks>
        public static IdentityBuilder RegisterDocumentDBStores<TUser, TRole>(
            this IdentityBuilder builder,
            Action<DocumentDbOptions> options)
            where TRole : IdentityRole
            where TUser : IdentityUser
        {
            var dbOptions = new DocumentDbOptions();
            options(dbOptions);

            var client = new DocumentClient(new Uri(dbOptions.DocumentUrl), dbOptions.DocumentKey, dbOptions.ConnectionPolicy);

            return builder.RegisterDocumentDBStores<TUser, TRole>(client, dbOptions.CollectionId);
        }

        /// <summary>
        ///     This method registers identity services and DocumentDB stores using the IdentityUser and IdentityRole types.
        /// </summary>
        /// <param name="service">The <see cref="IdentityBuilder"/> to build upon.</param>
        /// <param name="options">The options for creating the DocumentDB client.</param>
        /// <returns>The <see cref="IdentityBuilder"/> with the DocumentDB settings applied.</returns>
        public static IdentityBuilder AddIdentityWithDocumentDBStores(
            this IServiceCollection service,
            Action<DocumentDbOptions> options)
        {
            return service.AddIdentity<IdentityUser, IdentityRole>()
                .RegisterDocumentDBStores<IdentityUser, IdentityRole>(options);
        }

        /// <summary>
        ///     This method allows you to customize the user and role type when registering identity services
        ///     and DocumentDB stores.`
        /// </summary>
        /// <typeparam name="TUser">The type associated with user identity information.</typeparam>
        /// <typeparam name="TRole">The type associated with role identity information.</typeparam>
        /// <param name="service">The <see cref="IdentityBuilder"/> to build upon.</param>
        /// <param name="options">The options for creating the DocumentDB client.</param>
        /// <returns>The <see cref="IdentityBuilder"/> with the DocumentDB settings applied.</returns>
        public static IdentityBuilder AddIdentityWithDocumentDBStores<TUser, TRole>(
            this IServiceCollection service,
            Action<DocumentDbOptions> options)
            where TUser : IdentityUser
            where TRole : IdentityRole
        {
            return service.AddIdentity<TUser, TRole>()
                .RegisterDocumentDBStores<TUser, TRole>(options);
        }

        /// <summary>
        ///     This method registers identity services and DocumentDB stores using the IdentityUser and IdentityRole types.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="documentClient">Must be an initialized DocumentClient</param>
        /// <param name="databaseLink">Must contain the database link</param>
        public static IdentityBuilder AddIdentityWithDocumentDBStores(this IServiceCollection services, DocumentClient documentClient, string databaseLink)
        {
            return services.AddIdentityWithDocumentDBStoresUsingCustomTypes<IdentityUser, IdentityRole>(documentClient, databaseLink);
        }

        /// <summary>
        ///     This method allows you to customize the user and role type when registering identity services
        ///     and DocumentDB stores.
        /// </summary>
        /// <typeparam name="TUser"></typeparam>
        /// <typeparam name="TRole"></typeparam>
        /// <param name="services"></param>
        /// <param name="documentClient">Must be an initialized DocumentClient</param>
        /// <param name="databaseLink">Must contain the database link</param>
        public static IdentityBuilder AddIdentityWithDocumentDBStoresUsingCustomTypes<TUser, TRole>(this IServiceCollection services, DocumentClient documentClient, string databaseLink)
            where TUser : IdentityUser
            where TRole : IdentityRole
        {
            return services.AddIdentity<TUser, TRole>()
                .RegisterDocumentDBStores<TUser, TRole>(documentClient, databaseLink);
        }

        private static async Task CreateDatabaseIfNotExistsAsync(this IDocumentClient client, string databaseId)
        {
            try
            {
                await client.ReadDatabaseAsync(UriFactory.CreateDatabaseUri(databaseId));
            }
            catch (DocumentClientException ex)
            {
                if (ex.StatusCode == HttpStatusCode.NotFound)
                    await client.CreateDatabaseAsync(new Database { Id = databaseId });
                else
                    throw;
            }
        }
    }
}