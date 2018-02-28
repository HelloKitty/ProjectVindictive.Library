﻿using System;
using System.Collections.Generic;
using System.Text;
using Amazon;
using Amazon.Runtime;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ProjectVindictive
{
	public static class ProjectVindictiveAmazonS3Extensions
	{
		/// <summary>
		/// Adds the an AmazonS3 <see cref="IStorageUrlBuilder"/> registeration ot the container.
		/// Additionally sets up S3 communication and credentials for the application.
		/// Requires <see cref="AmazonS3Config"/> in the configuration.
		/// </summary>
		/// <param name="services">The service container.</param>
		/// <param name="configuration">A configuration object with <see cref="AmazonS3Config"/> registered.</param>
		public static IServiceCollection AddS3Service(this IServiceCollection services, IConfigurationRoot configuration)
		{
			//To communicate with all S3 regions we'll need to enable signature version 4 which uses a newer
			//signature computation. Some regions don't support older versions so this is REQUIRED
			AWSConfigsS3.UseSignatureVersion4 = true;

			//TODO: Handle credentials properly
			//TODO: Don't let this go into prod.
			Amazon.Util.ProfileManager.RegisterProfile("ProjectVindictive.UCM", "AKIAI3VALKRBX7IWIYKQ", "+XJ+9+iJGt8MUYU4QixpchSiehyc5IjUHXBqmStT");

			services.RegisterConfigOptions<AmazonS3Config>(configuration);
			services.AddSingleton<IStorageUrlBuilder, AmazonS3URLBuilder>();

			//TODO: Handle this differently
			services.AddSingleton<AWSCredentials>(sp => new StoredProfileAWSCredentials("ProjectVindictive.UCM"));

			return services;
		}
	}
}
