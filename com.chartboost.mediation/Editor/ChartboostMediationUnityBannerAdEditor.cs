using System;
using System.Runtime.Remoting.Messaging;
using Chartboost.AdFormats.Banner.Unity;
using UnityEditor;

namespace Chartboost.Editor
{
    [CustomEditor(typeof(ChartboostMediationUnityBannerAd))]
    public class ChartboostMediationUnityBannerAdEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var unityBannerAd = target as ChartboostMediationUnityBannerAd;
            unityBannerAd.AdjustSize();
        }
    }
}