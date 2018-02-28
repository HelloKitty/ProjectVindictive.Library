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
	public class WorldInfoController : AuthorizationReadyController
	{
		/// <summary>
		/// Logging serivce.
		/// </summary>
		private ILogger<WorldInfoController> Logger { get; }

		// GET api/values
		/// <inheritdoc />
		public WorldInfoController(IClaimsPrincipalReader haloLiveUserManager, ILogger<WorldInfoController> logger) 
			: base(haloLiveUserManager)
		{
			if(logger == null) throw new ArgumentNullException(nameof(logger));

			Logger = logger;
		}

		/// <summary>
		/// POST request that requests an a download URL for a world.
		/// The user must be authorized.
		/// </summary>
		/// <returns>A <see cref="WorldDownloadURLResponse"/> that either contains error information or the upload URL if it was successful.</returns>
		[HttpPost]
		[Authorize]
		public async Task<IActionResult> RequestWorldDownloadUrl([FromServices] IReadOnlyWorldEntryRepository worldEntryRepository, [FromServices] IStorageUrlBuilder urlBuilder, 
			[FromBody] WorldDownloadURLRequest downloadRequest)
		{
			if(worldEntryRepository == null) throw new ArgumentNullException(nameof(worldEntryRepository));

			//TODO: We want to rate limit access to this API
			//TODO: We should use both app logging but also another logging service that always gets hit

			//TODO: Consolidate this shared logic between controllers
			if(Logger.IsEnabled(LogLevel.Information))
				Logger.LogInformation($"Recieved {nameof(RequestWorldDownloadUrl)} request from {HaloLiveUserManager.GetUserName(User)}:{HaloLiveUserManager.GetUserId(User)}.");

			int userId;

			if(!int.TryParse(HaloLiveUserManager.GetUserId(User), out userId))
			{
				if(Logger.IsEnabled(LogLevel.Error))
					Logger.LogError($"Error: Encountered authorized user with unparsable UserId from User: {HaloLiveUserManager.GetUserName(User)}.");

				return Json(new WorldDownloadURLResponse(WorldDownloadURLResponseCode.AuthorizationFailed));
			}

			//TODO: We should probably check the flags of world to see if it's private (IE hidden from user). Or if it's unlisted or removed.
			//It's possible a user is requesting a world that doesn't exist
			//Could be malicious or it could have been deleted for whatever reason
			if(!await worldEntryRepository.HasEntry(downloadRequest.WorldId))
				return Json(new WorldDownloadURLResponse(WorldDownloadURLResponseCode.NoWorld));

			//We can get the URL from the urlbuilder if we provide the world storage GUID
			string downloadUrl = await urlBuilder.BuildRetrivalUrl(UserContentType.World, (await worldEntryRepository.GetWorldEntry(downloadRequest.WorldId)).StorageGuid);

			//TODO: Should we both validating S3 availability?
			if(String.IsNullOrEmpty(downloadUrl))
			{
				if(Logger.IsEnabled(LogLevel.Error))
					Logger.LogError($"Failed to create world upload URL for {HaloLiveUserManager.GetUserName(User)}:{HaloLiveUserManager.GetUserId(User)} with ID: {downloadRequest.WorldId}.");

				return Json(new WorldDownloadURLResponse(WorldDownloadURLResponseCode.WorldDownloadServiceUnavailable));
			}

			if(Logger.IsEnabled(LogLevel.Information))
				Logger.LogInformation($"Success. Sending {HaloLiveUserManager.GetUserName(User)} URL: {downloadUrl}");

			return Json(new WorldDownloadURLResponse(downloadUrl));
		}
	}
}
