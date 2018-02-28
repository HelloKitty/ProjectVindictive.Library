﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using HaloLive.Models;
using Newtonsoft.Json;

namespace ProjectVindictive
{
	/// <summary>
	/// Response model used to respond to <see cref="WorldDownloadURLRequest"/>.
	/// Either contains the resource URL or will contain an error code.
	/// </summary>
	[JsonObject]
	public sealed class WorldDownloadURLResponse : ISucceedable, IResponseModel<WorldDownloadURLResponseCode>
	{
		/// <inheritdoc />
		[JsonIgnore]
		public bool isSuccessful { get; }

		/// <summary>
		/// A valid URL to download a world resource from.
		/// </summary>
		[JsonProperty(Required = Required.AllowNull)] //can be null if the request failed.
		public string DownloadURL { get; private set; }

		/// <inheritdoc />
		[JsonProperty]
		public WorldDownloadURLResponseCode ResultCode { get; private set; }

		/// <inheritdoc />
		public WorldDownloadURLResponse(WorldDownloadURLResponseCode resultCode)
		{
			if(!Enum.IsDefined(typeof(WorldDownloadURLResponseCode), resultCode)) throw new ArgumentOutOfRangeException(nameof(resultCode), "Value should be defined in the WorldDownloadURLResponseCode enum.");

			if(resultCode == WorldDownloadURLResponseCode.Success)
				throw new ArgumentException("Cannot provide Success to a failing response ctor.", nameof(resultCode));
		}

		/// <inheritdoc />
		public WorldDownloadURLResponse(string downloadUrl)
		{
			if(string.IsNullOrWhiteSpace(downloadUrl)) throw new ArgumentException("Value cannot be null or whitespace.", nameof(downloadUrl));

			DownloadURL = downloadUrl;
		}

		/// <summary>
		/// Serializer ctor. DO NOT CALL
		/// </summary>
		public WorldDownloadURLResponse()
		{
			
		}
	}
}
