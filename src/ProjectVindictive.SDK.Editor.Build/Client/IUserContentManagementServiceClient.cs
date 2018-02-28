using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TypeSafe.Http.Net;

namespace ProjectVindictive.SDK
{
	//TODO: Automate user-agent SDK version headers
	[Header("User-Agent", "SDK 0.0.1")]
	public interface IUserContentManagementServiceClient
	{
		/// <summary>
		/// Attempts to get a new URL that can be used to upload the world.
		/// If successful the URl contained in the response will contain a valid upload
		/// URL which can be used to upload world content.
		/// </summary>
		/// <param name="authToken">The user authentication token.</param>
		/// <returns>A model representing the result of the world URL generation request.</returns>
		[Post("/api/World")]
		Task<RequestedUrlResponseModel> GetNewWorldUploadUrl([DynamicHeader("Authorization")] string authToken);
	}
}
