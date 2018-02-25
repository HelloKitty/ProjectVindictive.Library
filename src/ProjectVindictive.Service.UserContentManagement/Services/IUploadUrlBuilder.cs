using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectVindictive
{
	/// <summary>
	/// Contract for a type that can build or create upload URLs.
	/// </summary>
	public interface IUploadUrlBuilder
	{
		/// <summary>
		/// Builds an upload URL for the provided <see cref="contentType"/> and the key.
		/// </summary>
		/// <param name="contentType">The type of the uploaded content.</param>
		/// <param name="key">The unique key for the content.</param>
		/// <returns>A built URL that can be uploaded to.</returns>
		Task<string> BuildUploadUrl(UserContentType contentType, Guid key);
	}
}
