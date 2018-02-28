using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TypeSafe.Http.Net;

namespace ProjectVindictive
{
	/// <summary>
	/// Contract for a client that can communicate with
	/// the world service.
	/// </summary>
	[Header("User-Agent", "Client 0.0.1")]
	public interface IWorldServiceClient
	{
		//TODO: Path change to /download
		/// <summary>
		/// Gets the unique user-specific expiring download URL for the requested world
		/// listed in the <see cref="WorldDownloadURLRequest"/>.
		/// </summary>
		/// <param name="request">The request for the world download.</param>
		/// <param name="authToken">The authentication token.</param>
		/// <returns>A response to the download request.</returns>
		[Post("/api/worldinfo")] //we use POST due to complex model needed to query. We don't need caching anyway but this is KIND OF like a get. Though unique each call.
		Task<WorldDownloadURLResponse> GetWorldDownloadUrl([JsonBody] WorldDownloadURLRequest request, [DynamicHeader("Authorization")] string authToken);
	}
}
