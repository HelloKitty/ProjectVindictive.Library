using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HaloLive.Hosting;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ProjectVindictive
{
	[Route("api/[controller]")]
	public class WorldController : AuthorizationReadyController
	{
		/// <summary>
		/// Logging serivce.
		/// </summary>
		private ILogger<WorldController> Logger { get; }

		// GET api/values
		/// <inheritdoc />
		public WorldController(IClaimsPrincipalReader haloLiveUserManager, ILogger<WorldController> logger) 
			: base(haloLiveUserManager)
		{
			if(logger == null) throw new ArgumentNullException(nameof(logger));

			Logger = logger;
		}
		
		/// <summary>
		/// POST request that requests an upload URL for a world.
		/// The user must be authorized.
		/// </summary>
		/// <returns>A <see cref="UploadUrlResponseModel"/> that either contains error information or the upload URL if it was successful.</returns>
		[HttpPost]
		[Authorize]
		public async Task<IActionResult> RequestWorldUploadUrl([FromServices] IWorldEntryRepository worldEntryRepository, [FromServices] IUploadUrlBuilder urlBuilder)
		{
			if(worldEntryRepository == null) throw new ArgumentNullException(nameof(worldEntryRepository));

			//TODO: We want to rate limit access to this API
			//TODO: We should use both app logging but also another logging service that always gets hit

			if(Logger.IsEnabled(LogLevel.Information))
				Logger.LogInformation($"Recieved {nameof(RequestWorldUploadUrl)} request from {HaloLiveUserManager.GetUserName(User)}:{HaloLiveUserManager.GetUserId(User)}.");

			int userId;

			if(!int.TryParse(HaloLiveUserManager.GetUserId(User), out userId))
			{
				if(Logger.IsEnabled(LogLevel.Error))
					Logger.LogError($"Error: Encountered authorized user with unparsable UserId from User: {HaloLiveUserManager.GetUserName(User)}.");

				return new JsonResult(UploadUrlResponseModel.CreateFailure("Failed to authorize action.", UploadUrlResponseCode.AuthorizationFailed));
			}

			Guid worldGuid = Guid.NewGuid();

			//TODO: Check if the result is valid? We should maybe return bool from this API
			//The idea is to create an entry which will contain a GUID. From that GUID we can then generate the upload URL
			await worldEntryRepository.AddWorldEntry(userId, this.HttpContext.Connection.RemoteIpAddress.ToString(), worldGuid); //TODO: Ok to just provide a guid right?

			string uploadUrl = await urlBuilder.BuildUploadUrl(UserContentType.World, worldGuid);

			if(String.IsNullOrEmpty(uploadUrl))
			{
				if(Logger.IsEnabled(LogLevel.Error))
					Logger.LogError($"Failed to create world upload URL for {HaloLiveUserManager.GetUserName(User)}:{HaloLiveUserManager.GetUserId(User)} with GUID: {worldGuid}.");

				return new JsonResult(UploadUrlResponseModel.CreateFailure("Upload service unavailable.", UploadUrlResponseCode.UploadUnavailable));
			}

			return new JsonResult(UploadUrlResponseModel.CreateSuccess(uploadUrl));
		}
	}
}
