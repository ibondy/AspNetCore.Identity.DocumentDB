
## AspNetCore.Identity.DocumentDB [![AppVeyor build status](https://ci.appveyor.com/api/projects/status/b27a4wvconad0c5k?svg=true)](https://ci.appveyor.com/project/FelschR/aspnetcore-identity-documentdb) [![Travis CI build status](https://travis-ci.org/FelschR/aspnetcore-identity-documentdb.svg?branch=netcore)](https://travis-ci.org/FelschR/aspnetcore-identity-documentdb?branch=netcore)

**Notice: this is a fork of aspnet-identity-mongo and is still under development.**
**Basic features already work.**
**The documentation was not updated yet and mostly reflects the mongodb version.**
**If you have questions regarding implemetation feel free to contact me.**

This is a DocumentDB provider for the ASP.NET Core Identity framework. This was ported from the MongoDB ASP.NET Core Identity framework (Microsoft.AspNetCore.Identity.MongoDB NuGet package) by g0t4.

This project has extensive test coverage. 

If you want something easy to setup, this adapter is for you. I do not intend to cover every possible desirable configuration, if you don't like my decisions, write your own adapter. Use this as a learning tool to make your own adapter. These adapters are not complicated, but trying to make them configurable would become a complicated mess. And would confuse the majority of people that want something simple to use. So I'm favoring simplicity over making every last person happy.

## Usage

- Reference this package in project.json: Microsoft.AspNetCore.Identity.DocumentDB
- Then, in ConfigureServices--or wherever you are registering services--include the following to register both the Identity services and DocumentDB stores:

```csharp
services.AddIdentityWithDocumentDBStores(documentClient, databaseLink);
```

- If you want to customize what is registered, refer to the tests for further options (CoreTests/DocumentDBIdentityBuilderExtensionsTests.cs)
- Remember with the Identity framework, the whole point is that both a `UserManager` and `RoleManager` are provided for you to use, here's how you can resolve instances manually. Of course, constructor injection is also available.

```csharp
var userManager = provider.GetService<UserManager<IdentityUser>>();
var roleManager = provider.GetService<RoleManager<IdentityRole>>();
```

- The following methods help create indexes that will boost lookups by UserName, Email and role Name. These have changed since Identity v2 to refer to Normalized fields. I dislike this aspect of Core Identity, but it is what it is. Basically these three fields are stored in uppercase format for case insensitive searches.

```csharp
	IndexChecks.EnsureUniqueIndexOnNormalizedUserName(users);
	IndexChecks.EnsureUniqueIndexOnNormalizedEmail(users);
	IndexChecks.EnsureUniqueIndexOnNormalizedRoleName(roles);
```

What frameworks are targeted, with rationale:

- Microsoft.AspNetCore.Identity - supports net451 and netstandard1.3
- Microsoft.Azure.DocumentDB v1.10.0 - supports net45 (netstandard support is in development)
- Thus, the lowest common denominators are net451 (of net45 and net451)

## Building instructions

run commands in [](build.sh)
