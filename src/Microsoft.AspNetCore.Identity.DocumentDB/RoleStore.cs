
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
// I'm using async methods to leverage implicit Task wrapping of results from expression bodied functions.

namespace Microsoft.AspNetCore.Identity.DocumentDB
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Azure.Documents;
    using Azure.Documents.Client;

    /// <summary>
    ///     Note: Deleting and updating do not modify the roles stored on a user document. If you desire this dynamic
    ///     capability, override the appropriate operations on RoleStore as desired for your application. For example you could
    ///     perform a document modification on the users collection before a delete or a rename.
    ///     When passing a cancellation token, it will only be used if the operation requires a database interaction.
    /// </summary>
    /// <typeparam name="TRole">Needs to extend the provided IdentityRole type.</typeparam>
    public class RoleStore<TRole> : IQueryableRoleStore<TRole>
		// todo IRoleClaimStore<TRole>
		where TRole : IdentityRole
	{
        private readonly DocumentClient _Client;
        private readonly DocumentCollection _Roles; // DocumentCollection of TRole

        public RoleStore(DocumentClient documentClient, DocumentCollection roles) // DocumentCollection of TRole
        {
            _Client = documentClient;
            _Roles = roles;
		}

        public virtual void Dispose()
		{
			// no need to dispose of anything, mongodb handles connection pooling automatically
		}

		public virtual async Task<IdentityResult> CreateAsync(TRole role, CancellationToken token)
		{
			var result = await _Client.CreateDocumentAsync(_Roles.DocumentsLink, role);
            role.Id = result.Resource.Id;
            role.ResourceId = result.Resource.ResourceId;

            return IdentityResult.Success;
		}

		public virtual async Task<IdentityResult> UpdateAsync(TRole role, CancellationToken token)
		{
			var result = await _Client.ReplaceDocumentAsync(GetRoleUri(role.Id), role);
			// todo low priority result based on replace result
			return IdentityResult.Success;
		}

		public virtual async Task<IdentityResult> DeleteAsync(TRole role, CancellationToken token)
		{
			var result = await _Client.DeleteDocumentAsync(GetRoleUri(role.Id));
			// todo low priority result based on delete result
			return IdentityResult.Success;
		}

		public virtual async Task<string> GetRoleIdAsync(TRole role, CancellationToken cancellationToken)
			=> role.Id;

		public virtual async Task<string> GetRoleNameAsync(TRole role, CancellationToken cancellationToken)
			=> role.Name;

		public virtual async Task SetRoleNameAsync(TRole role, string roleName, CancellationToken cancellationToken)
			=> role.Name = roleName;

		// note: can't test as of yet through integration testing because the Identity framework doesn't use this method internally anywhere
		public virtual async Task<string> GetNormalizedRoleNameAsync(TRole role, CancellationToken cancellationToken)
			=> role.NormalizedName;

		public virtual async Task SetNormalizedRoleNameAsync(TRole role, string normalizedName, CancellationToken cancellationToken)
			=> role.NormalizedName = normalizedName;

		public virtual async Task<TRole> FindByIdAsync(string roleId, CancellationToken token)
			=> _Client.CreateDocumentQuery<TRole>(_Roles.DocumentsLink)
                .Where(r => r.Id == roleId)
                .AsEnumerable()
                .FirstOrDefault();

		public virtual async Task<TRole> FindByNameAsync(string normalizedName, CancellationToken token)
			=> _Client.CreateDocumentQuery<TRole>(_Roles.DocumentsLink)
                .Where(r => r.NormalizedName == normalizedName)
                .AsEnumerable()
                .FirstOrDefault();

		public virtual IQueryable<TRole> Roles
			=> _Client.CreateDocumentQuery<TRole>(_Roles.DocumentsLink).AsQueryable();

        private string GetRoleUri(string documentId)
        {
            return string.Format("{0}/docs/{1}", _Roles.AltLink, documentId);
        }
    }
}