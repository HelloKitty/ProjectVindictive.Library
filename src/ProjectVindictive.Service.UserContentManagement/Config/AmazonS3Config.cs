using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ProjectVindictive
{
	/// <summary>
	/// Configuration for the Amazon S3.
	/// </summary>
	[JsonObject]
	public sealed class AmazonS3Config
	{
		//TODO: We probably need to support multiple buckets at some point
		/// <summary>
		/// The name of the bucket to use
		/// </summary>
		[JsonProperty]
		public string BucketName { get; set; }

		public AmazonS3Config()
		{
			
		}
	}
}
