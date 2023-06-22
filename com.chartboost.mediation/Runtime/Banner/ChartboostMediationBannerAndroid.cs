#if UNITY_ANDROID
using System;
using Chartboost.Interfaces;
using Chartboost.Platforms.Android;
using UnityEngine;

namespace Chartboost.Banner
{
    /// <summary>
    /// Chartboost Mediation banner object for Android.
    /// </summary>
    public class ChartboostMediationBannerAndroid : ChartboostMediationBannerBase
    {
        private readonly AndroidJavaObject _androidAd;

        public ChartboostMediationBannerAndroid(string placementName, ChartboostMediationBannerAdSize size) : base(placementName, size)
        {
            LogTag = "ChartboostMediationBanner (Android)";
            using var unityBridge = ChartboostMediationAndroid.GetUnityBridge();
            _androidAd = unityBridge.CallStatic<AndroidJavaObject>("getBannerAd", placementName, (int)size);
        }

        /// <inheritdoc cref="IChartboostMediationAd.SetKeyword"/>>
        public override bool SetKeyword(string keyword, string value)
        {
            base.SetKeyword(keyword, value);
            return _androidAd.Call<bool>("setKeyword", keyword, value);
        }

        /// <inheritdoc cref="IChartboostMediationAd.RemoveKeyword"/>>
        public override string RemoveKeyword(string keyword)
        {
            base.RemoveKeyword(keyword);
            return _androidAd.Call<string>("removeKeyword", keyword);
        }

        /// <inheritdoc cref="IChartboostMediationAd.Destroy"/>>
        public override void Destroy()
        {
            base.Destroy();
            _androidAd.Call("destroy");
        }

        /// <inheritdoc cref="IChartboostMediationBannerAd.Load(Chartboost.Banner.ChartboostMediationBannerAdScreenLocation)"/>>
        public override void Load(ChartboostMediationBannerAdScreenLocation location)
        {
            base.Load(location);
            _androidAd.Call("load", (int)location);
        }

        /// <inheritdoc cref="IChartboostMediationBannerAd.Load"/>>
        public override void Load(float x, float y, int width, int height)
        {
            base.Load(x, y, width, height);
            _androidAd.Call("load", x, Screen.height - y, width, height);   // Android measures pixels from top whereas Unity measures pixels from bottom of screen
        }

        /// <inheritdoc cref="IChartboostMediationBannerAd.SetVisibility"/>>
        public override void SetVisibility(bool isVisible)
        {
            base.SetVisibility(isVisible);
            _androidAd.Call("setBannerVisibility", isVisible);
        }

        /// <inheritdoc cref="IChartboostMediationBannerAd.ClearLoaded"/>>
        public override void ClearLoaded()
        {
            base.ClearLoaded();
            _androidAd.Call("clearLoaded");
        }

        /// <inheritdoc cref="IChartboostMediationBannerAd.Remove"/>>
        public override void Remove()
        {
            //android doesn't have a remove method. Instead, calling destroy
            Destroy();
        }

        /// <inheritdoc cref="IChartboostMediationBannerAd.EnableDrag"/>>
        public override void EnableDrag(Action<float, float> onDrag = null)
        {
            base.EnableDrag(onDrag);
            _androidAd.Call("enableDrag", new BannerDragEventListener(onDrag));
        }

        /// <inheritdoc cref="IChartboostMediationBannerAd.DisableDrag"/>>
        public override void DisableDrag()
        {
            base.DisableDrag();
            _androidAd.Call("disableDrag");
        }
    }
}

public class BannerDragEventListener : AndroidJavaProxy
{
    private Action<float, float> _onDrag;
    
    public BannerDragEventListener(Action<float, float> onDrag) : base("com.chartboost.mediation.unity.IBannerDragEventListener")
    {
        _onDrag = onDrag;
    }

    private void OnDrag(float x, float y)
    {
        _onDrag?.Invoke(x, Screen.height - y);
    }
}
#endif
