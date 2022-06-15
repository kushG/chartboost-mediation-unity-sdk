using UnityEngine;
#if UNITY_IPHONE
using System;
using System.Runtime.InteropServices;
#endif

namespace Helium
{
	public class HeliumInterstitialAd {

		#if UNITY_IPHONE
		// Extern functions
		[DllImport("__Internal")]
		private static extern bool _heliumSdkInterstitialSetKeyword(IntPtr uniqueID, string keyword, string value);
		[DllImport("__Internal")]
		private static extern string _heliumSdkInterstitialRemoveKeyword(IntPtr uniqueID, string keyword);
		[DllImport("__Internal")]
		private static extern void _heliumSdkInterstitialAdLoad(IntPtr uniqueID);
		[DllImport("__Internal")]
		private static extern bool _heliumSdkInterstitialClearLoaded(IntPtr uniqueID);
		[DllImport("__Internal")]
		private static extern void _heliumSdkInterstitialAdShow(IntPtr uniqueID);
		[DllImport("__Internal")]
		private static extern bool _heliumSdkInterstitialAdReadyToShow(IntPtr uniqueID);
		[DllImport("__Internal")]
		private static extern void _heliumSdkFreeInterstitialAdObject(IntPtr uniqueID);
		#endif

		// Class variables
		#if UNITY_IPHONE
		private readonly IntPtr _uniqueId;
		#endif

		#if UNITY_IPHONE
		public HeliumInterstitialAd(IntPtr uniqueId)
		{
			_uniqueId = uniqueId;
		}
		#elif UNITY_ANDROID
		private readonly AndroidJavaObject _androidAd;
		public HeliumInterstitialAd(AndroidJavaObject ad)
		{
			_androidAd = ad;
		}
		#endif

		// Class functions

		/// <summary>
		/// Set a keyword/value pair on the advertisement. If the keyword has previously been
		/// set, then the value will be replaced with the new value.  These values will be
		/// used upon the loading of the advertisement.
		/// </summary>
		/// <param name="keyword">The keyword (maximum of 64 characters)</param>
		/// <param name="value">The value (maximum of 256 characters)</param>
		/// <returns>true if the keyword was successfully set, else false</returns>
		public bool SetKeyword(string keyword, string value)
        {
			#if UNITY_IPHONE
			return _heliumSdkInterstitialSetKeyword(_uniqueId, keyword, value);
			#elif UNITY_ANDROID
			return _androidAd.Call<bool>("setKeyword", keyword, value);
			#else
			return false;
			#endif
		}

		/// <summary>
		/// Remove a keyword from the advertisement.
		/// </summary>
		/// <param name="keyword">The keyword to remove.</param>
		/// <returns>The currently set value, else null</returns>
		public string RemoveKeyword(string keyword)
        {
			#if UNITY_IPHONE
			return _heliumSdkInterstitialRemoveKeyword(_uniqueId, keyword);
			#elif UNITY_ANDROID
			return _androidAd.Call<string>("removeKeyword", keyword);
			#else
			return null;
			#endif
		}

		/// <summary>
		/// Load the advertisement.
		/// </summary>
		public void Load() {
			#if UNITY_IPHONE
			System.GC.Collect(); // make sure previous i12 ads get destructed if necessary
			_heliumSdkInterstitialAdLoad(_uniqueId);
			#elif UNITY_ANDROID
			_androidAd.Call("load");
			#endif
		}

		/// <summary>
		/// If an advertisement has been loaded, clear it. Once cleared, a new
		/// load can be performed.
		/// </summary>
		/// <returns>true if successfully cleared</returns>
		public bool ClearLoaded() {
			#if UNITY_IPHONE
			return _heliumSdkInterstitialClearLoaded(_uniqueId);
			#elif UNITY_ANDROID
			return _androidAd.Call<bool>("clearLoaded");
			#else
			return false;
			#endif
		}

		/// <summary>
		/// Show a previously loaded advertisement.
		/// </summary>
		public void Show() {
			#if UNITY_IPHONE
			_heliumSdkInterstitialAdShow(_uniqueId);
			#elif UNITY_ANDROID
			_androidAd.Call("show");
			#endif
		}

		/// <summary>
		/// Indicates if an advertisement is ready to show.
		/// </summary>
		/// <returns>True if ready to show.</returns>
		public bool ReadyToShow() {
			#if UNITY_IPHONE
			return _heliumSdkInterstitialAdReadyToShow(_uniqueId);
			#elif UNITY_ANDROID
			return _androidAd.Call<bool>("readyToShow");
			#else
			return false;
			#endif
		}

		/// <summary>
		/// Destroy the advertisement to free up memory resources.
		/// </summary>
		public void Destroy()
		{
			#if UNITY_ANDROID
			_androidAd.Call("destroy");
			#endif
		}

		~HeliumInterstitialAd() {
			#if UNITY_IPHONE
			_heliumSdkFreeInterstitialAdObject(_uniqueId);
			#endif
		}
	}
}
