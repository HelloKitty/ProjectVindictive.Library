using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Options;

namespace ProjectVindictive
{
	/// <summary>
	/// A <see cref="IUploadUrlBuilder"/> implementation that uses S3 to serve/build upload URLs.
	/// </summary>
	public sealed class AmazonS3URLBuilder : IUploadUrlBuilder
	{
		//TODO: Hide credentials from this config
		/// <summary>
		/// Amazon config settings.
		/// </summary>
		private IOptions<AmazonS3Config> AmazonConfig { get; }

		/// <summary>
		/// The credentials for the AWS client.
		/// </summary>
		private AWSCredentials AWSCredentials { get; }

		public AmazonS3URLBuilder(IOptions<AmazonS3Config> amazonConfig, AWSCredentials awsCredentials)
		{
			if(amazonConfig == null) throw new ArgumentNullException(nameof(amazonConfig));
			if(awsCredentials == null) throw new ArgumentNullException(nameof(awsCredentials));

			AmazonConfig = amazonConfig;
			AWSCredentials = awsCredentials;
		}

		/// <inheritdoc />
		public Task<string> BuildUploadUrl(UserContentType contentType, Guid key)
		{
			//TODO: Extrac region and other info into config
			using(IAmazonS3 client = new AmazonS3Client(AWSCredentials, new Amazon.S3.AmazonS3Config() { SignatureVersion = "V4", RegionEndpoint = RegionEndpoint.USEast2, SignatureMethod = SigningAlgorithm.HmacSHA256 }))
			{
				GetPreSignedUrlRequest request = new GetPreSignedUrlRequest()
				{
					//TODO: Use the content type to determine bucket
					BucketName = AmazonConfig.Value.BucketName,

					//We just use the GUID as the key. This should be enough
					Key = key.ToString(),

					//TODO: Handle expiry better. Research if this will cause expiry during uploads

					Expires = DateTime.Now.AddMinutes(5),

					Verb = HttpVerb.PUT
				};

				return Task.FromResult(client.GetPreSignedURL(request));
			}
		}
	}
}
