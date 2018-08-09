using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;

namespace AspNetCore.Identity.DocumentDB
{
    public static class Helper
    {
        /// <summary>
        /// Merges the base request options with the given ones and returns them.
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public static RequestOptions GetRequestOptions(object partitionKeyValue, RequestOptions options = null)
        {
            if (partitionKeyValue == null)
            {
                return options;
            }

            var partitionKey = new PartitionKey(partitionKeyValue);

            if (options != null)
            {
                options.PartitionKey = partitionKey;
            }
            else
            {
                options = new RequestOptions
                {
                    PartitionKey = partitionKey
                };
            }

            return options;
        }

        /// <summary>
        /// Merges the base feed options with the given ones and returns them.
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public static FeedOptions GetFeedOptions(object partitionKeyValue, FeedOptions options = null)
        {
            if (partitionKeyValue == null)
            {
                return options;
            }

            var partitionKey = new PartitionKey(partitionKeyValue);

            if (options != null)
            {
                options.PartitionKey = partitionKey;
            }
            else
            {
                options = new FeedOptions
                {
                    PartitionKey = partitionKey
                };
            }

            return options;
        }

        public static String GetEnumMemberValue<T>(T value)
            where T : struct, IConvertible
        {
            return typeof(T)
                .GetTypeInfo()
                .DeclaredMembers
                .SingleOrDefault(x => x.Name == value.ToString())
                ?.GetCustomAttribute<EnumMemberAttribute>(false)
                ?.Value;
        }
    }
}
