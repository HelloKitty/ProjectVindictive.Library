using System;
using HaloLive.Models;
using Newtonsoft.Json;

namespace ProjectVindictive
{
	/// <summary>
	/// Response model used for when a user requests an upload URL.
	/// </summary>
	[JsonObject]
	public sealed class UploadUrlResponseModel : IResponseModel<UploadUrlResponseCode>, ISucceedable
	{
		//TODO: Create response code system
		/// <summary>
		/// Indicates if the 
		/// </summary>
		[JsonIgnore]
		public bool isSuccessful => ResultCode == UploadUrlResponseCode.Success;

		/// <summary>
		/// The URL for the upload.
		/// </summary>
		[JsonProperty]
		public string UploadUrl { get; set; }

		/// <summary>
		/// If not-null then it contains the error message.
		/// </summary>
		[JsonProperty]
		public string ErrorMessage { get; set; }

		/// <inheritdoc />
		[JsonProperty]
		public UploadUrlResponseCode ResultCode { get; }

		private UploadUrlResponseModel(string errorMessage, UploadUrlResponseCode code)
		{
			if(string.IsNullOrWhiteSpace(errorMessage)) throw new ArgumentException("Value cannot be null or whitespace.", nameof(errorMessage));
			if(!Enum.IsDefined(typeof(UploadUrlResponseCode), code)) throw new ArgumentOutOfRangeException(nameof(code), "Value should be defined in the UploadUrlResponseCode enum.");

			ErrorMessage = errorMessage;
		}

		private UploadUrlResponseModel(string url)
		{
			if(string.IsNullOrWhiteSpace(url)) throw new ArgumentException("Value cannot be null or whitespace.", nameof(url));

			UploadUrl = url;
			ResultCode = UploadUrlResponseCode.Success;
		}

		public static UploadUrlResponseModel CreateSuccess(string url)
		{
			return new UploadUrlResponseModel(url);
		}

		public static UploadUrlResponseModel CreateFailure(string errorMessage, UploadUrlResponseCode code)
		{
			if(string.IsNullOrWhiteSpace(errorMessage)) throw new ArgumentException("Value cannot be null or whitespace.", nameof(errorMessage));
			if(!Enum.IsDefined(typeof(UploadUrlResponseCode), code)) throw new ArgumentOutOfRangeException(nameof(code), "Value should be defined in the UploadUrlResponseCode enum.");

			if(code == UploadUrlResponseCode.Success)
				throw new ArgumentException($"Cannot use {nameof(UploadUrlResponseCode.Success)} for failure.", nameof(code));

			return new UploadUrlResponseModel(errorMessage, code);
		}
	}
}
