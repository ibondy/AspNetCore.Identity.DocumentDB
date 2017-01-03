using Microsoft.Azure.Documents.Client;

namespace Microsoft.AspNetCore.Identity.DocumentDB
{
    /// <summary>
    /// Options for connecting to a DocumentDB NOSQL database.
    /// </summary>
    public class DocumentDbOptions
    {
        /// <summary>
        /// The URL of the DocumentDB database (Found in Azure).
        /// </summary>
        public string DocumentUrl { get; set; }
        /// <summary>
        /// The DocuementDB access key for the database (Found in Azure).
        /// </summary>
        public string DocumentKey { get; set; }
        /// <summary>
        /// The name of the collection to house identity data.
        /// </summary>
        public string CollectionId { get; set; }

        /// <summary>
        /// The <see cref="ConnectionPolicy"/> for the DocumentDB client.
        /// </summary>
        public ConnectionPolicy ConnectionPolicy { get; set; }

        /// <summary>
        /// Defaults <see cref="DocumentUrl"/>, <see cref="DocumentKey"/>, and <see cref="ConnectionPolicy"/> to the
        /// DocumentDB Emulator defaults.
        /// </summary>
        public void EnableDocumentDbEmulatorSupport()
        {
            DocumentUrl = "https://localhost:8081";
            DocumentKey = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";
            ConnectionPolicy = new ConnectionPolicy {EnableEndpointDiscovery = false};
        }
    }
}
