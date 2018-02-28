using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using ProjectVindictive.SDK;
using TypeSafe.Http.Net;

namespace ProjectVindictive.Tests.S3PreSignedUrlTestClient
{
	public class Program
	{
		private static string AuthToken = "Bearer eyJhbGciOiJSUzI1NiIsImtpZCI6IjYyN0YyQUFDMTZERTlENjNDMkY3NDQyQzk1OUFBNjEyQjIyOTlENDciLCJ0eXAiOiJKV1QifQ.eyJzdWIiOiIxIiwibmFtZSI6ImFkbWluIiwidG9rZW5fdXNhZ2UiOiJhY2Nlc3NfdG9rZW4iLCJqdGkiOiI5NGUxNTBkMS01ZjRmLTQxZDktOTQ4ZC1lNzQyY2VlZDdjMDQiLCJhdWQiOiJhdXRoLXNlcnZlciIsIm5iZiI6MTUxOTMxMjUzNiwiZXhwIjoxNTE5MzE2MTM2LCJpYXQiOjE1MTkzMTI1MzYsImlzcyI6Imh0dHBzOi8vbG9jYWxob3N0OjUwMDEvIn0.WGWNBRRlzgcot5u7rKdT15jOw4zvClge8VTJwqMlKcDe7QHpJo97y7iHc4q7baJ-Hum_eTmw0M_p-W4_O_lQwglx1uibd__7MztG1Y-h4224EwZ7XdPQzoqej60vtbB83yzgUQ60UEau2EbpqZL_IBcWdK2S-pGaaj_t2YV5rzS2YxMOGG66H3dgn9WR57Wu__Q_lmgBPI1-Je_et10n6Lvwqna1Ci2Tb4Dhz5U6ljvL6vvdXH3gFHBddbVPgRIyt3Tas-PUFIgu_YtCu0x8-cmlTHfEZfDXsZVn1pJ581fWm3Z_GCKDgxwXBLKIFoDOMSiW98HEopwoWr71wJDjD2iMGyYnKrMchf7oVfkWHPps_iad2BkwTgRFecPQOxIhTAG0lmtaMwj5oAUT3Ik2ns10ThXUbEdqOwXjzjW6bqWvvdDOCaa9AUhnNYJbF2f3bPXLbxjBwRU9KKYwXyDaglSR6M9uxnoza3H_94zoStHQcv05Jx25zoM656QdqjA6JtkyRnRdZiklx5R5nglhW5tLbKbagtwdI5XTlSX1_zn3AXYWQP1yT-Zay3K65Qw-mMA2OGL-38S4mtJGdzYvL9RPQ6t7IdXXa5jNL76rbUXQx_NbKm7xNFMx3AyqjO39z-xbA7cdi7X92IZp908HubXbDewwVPTWRUjswkCehYw";

		static void Main(string[] args)
		{
			ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;

			IUserContentManagementServiceClient ucmService = TypeSafeHttpBuilder<IUserContentManagementServiceClient>.Create()
				.RegisterDefaultSerializers()
				.RegisterDotNetHttpClient("https://localhost.:5002/")
				.RegisterJsonNetSerializer()
				.Build();

			Console.WriteLine("Sending request");

			RequestedUrlResponseModel responseModel = ucmService.GetNewWorldUploadUrl(AuthToken)
				.Result;

			Console.WriteLine($"URL: {responseModel.UploadUrl}");

			IWorldServiceClient worldService = TypeSafeHttpBuilder<IWorldServiceClient>.Create()
				.RegisterDefaultSerializers()
				.RegisterDotNetHttpClient("https://localhost.:5003/")
				.RegisterJsonNetSerializer()
				.Build();

			WorldDownloadURLResponse downloadResponse = worldService.GetWorldDownloadUrl(new WorldDownloadURLRequest(64), AuthToken).Result;

			Console.WriteLine($"Download URL Response: {downloadResponse.ResultCode} {downloadResponse.DownloadURL}");

			Console.ReadKey();
		}

		//https://stackoverflow.com/questions/4926676/mono-https-webrequest-fails-with-the-authentication-or-decryption-has-failed
		private static bool MyRemoteCertificateValidationCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslpolicyerrors)
		{
			TypeSafeHttpBuilder<IUserContentManagementServiceClient>.Create()
				.RegisterDefaultSerializers()
				.RegisterDotNetHttpClient("https://localhost.:5002/")
				.RegisterJsonNetSerializer()
				.Build();

			return true;
		}
	}
}
