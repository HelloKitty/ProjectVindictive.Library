using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TypeSafe.Http.Net;

namespace ProjectVindictive
{
	public sealed class WorldServiceClientFactory
	{
		public static IWorldServiceClient Create()
		{
			return Create(@"https://localhost:5003/");
		}

		public static IWorldServiceClient Create(string baseUrl)
		{
			if(string.IsNullOrWhiteSpace(baseUrl)) throw new ArgumentException("Value cannot be null or whitespace.", nameof(baseUrl));

			return TypeSafeHttpBuilder<IWorldServiceClient>.Create()
				.RegisterDefaultSerializers()
				.RegisterDotNetHttpClient(baseUrl)
				.RegisterJsonNetSerializer()
				.Build();
		}
	}
}
