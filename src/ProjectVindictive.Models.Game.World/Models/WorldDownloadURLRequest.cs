using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace ProjectVindictive
{
	/// <summary>
	/// Request model containing all the information required
	/// to get a world download URL.
	/// </summary>
	[JsonObject]
	public sealed class WorldDownloadURLRequest
	{
		//TODO: Implement region and variant handling? HD vs SD and NA vs EU etc
		/// <summary>
		/// The ID of the world to request a download URL for.
		/// </summary>
		[JsonProperty]
		public int WorldId { get; private set; }

		public WorldDownloadURLRequest(int worldId)
		{
			if(worldId < 0) throw new ArgumentOutOfRangeException(nameof(worldId));

			WorldId = worldId;
		}

		/// <summary>
		/// Serializer ctor. DO NOT CALL
		/// </summary>
		public WorldDownloadURLRequest()
		{
			
		}
	}
}
