using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Documents.Client;
using Microsoft.AspNetCore.Identity;
using Microsoft.Azure.Documents;

namespace AspNetCore.Identity.DocumentDB.Sample
{
    public class Startup
    {
        // endpoint & key for DocumentDB Emulator:
        Uri endpoint = new Uri("https://localhost:8081");
        static readonly string key = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";
        static readonly string databaseId = "AspDotNetCore.Identity.DocumentDB.Test";

        DocumentClient client;

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();

            // TODO remove next line; currently DocumentDB Emulator hangs if not set
            ConnectionPolicy.Default.EnableEndpointDiscovery = false;
            client = new DocumentClient(endpoint, key);
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();

            // make sure the database exists!
            Database db = client.CreateDatabaseQuery().Where(d => d.Id == databaseId).AsEnumerable().FirstOrDefault()
                ?? client.CreateDatabaseAsync(new Database { Id = databaseId }).Result;

            // add Identity Server with DocumentDB stores (will store data in collections "users" and "roles")
            // services.AddIdentityWithDocumentDBStoresUsingCustomTypes<MyUser, MyRole>(client, databaseLink);

            // variant 2:
            services.AddIdentity<MyUser, MyRole>()
                .RegisterDocumentDBStores<MyUser, MyRole>(client, (p) => new DocumentCollection { Id = "userCollection" });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            // call this before UseMvc()
#if NETCOREAPP1_0
            app.UseIdentity();
#elif NETCOREAPP2_0
            app.UseAuthentication();
#endif

            app.UseMvc();
        }
    }
}
