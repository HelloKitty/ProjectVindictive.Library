using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using HaloLive.Models.Authentication;
using HaloLive.Network;
using TypeSafe.Http.Net;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using Unitysync.Async;

namespace ProjectVindictive
{
	public class BundleLoadingTestDemo : MonoBehaviour
	{
		/// <summary>
		/// The ID to use for the test.
		/// </summary>
		public string WorldId { get; set; }

		public string AuthToken { get; set; }

		private UnityWebRequest Request;

		public string UserName { get; set; }

		public string Password { get; set; }

		public void Test()
		{
			//https://stackoverflow.com/questions/4926676/mono-https-webrequest-fails-with-the-authentication-or-decryption-has-failed
			ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;

			//TODO: Service discovery
			IAuthenticationService authService = TypeSafeHttpBuilder<IAuthenticationService>.Create()
				.RegisterDefaultSerializers()
				.RegisterDotNetHttpClient("https://localhost:5001/")
				.RegisterJsonNetSerializer()
				.Build();

			authService.TryAuthenticate(new AuthenticationRequestModel(UserName, Password))
				.UnityAsyncContinueWith(this, ar => WorldServiceClientFactory.Create().GetWorldDownloadUrl(new WorldDownloadURLRequest(int.Parse(WorldId)), $"Bearer {ar.AccessToken}"))
				.UnityAsyncContinueWith(this, r =>
				{
					Debug.Log($"Result: {r.ResultCode} Download URL: {r.DownloadURL}");
					StartCoroutine(AssetBundleRequest(r.DownloadURL));
				});
		}

		//TODO: This is all test garbage. Don't use this.
		IEnumerator AssetBundleRequest(string url)
		{
			using(UnityWebRequest uwr = UnityWebRequest.GetAssetBundle(url))
			{
				yield return uwr.SendWebRequest();

				if(uwr.isNetworkError || uwr.isHttpError)
				{
					Debug.Log($"Failed to download bundle. Error: {uwr.error}");
				}
				else
				{
					// Get downloaded asset bundle
					AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(uwr);

					string[] paths = bundle.GetAllScenePaths();

					foreach(string p in paths)
						Debug.Log($"Found Scene in Bundle: {p}");

					Debug.Log("Loading first found scene.");

					SceneManager.LoadSceneAsync(System.IO.Path.GetFileNameWithoutExtension(paths.First())).allowSceneActivation = true;
				}
			}
		}

		//https://stackoverflow.com/questions/4926676/mono-https-webrequest-fails-with-the-authentication-or-decryption-has-failed
		private bool MyRemoteCertificateValidationCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslpolicyerrors)
		{
			return true;
		}
	}
}
