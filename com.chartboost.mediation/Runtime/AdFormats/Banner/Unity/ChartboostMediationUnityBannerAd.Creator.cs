using Chartboost.Banner;
using Chartboost.Utilities;
using UnityEditor;
using UnityEngine;

namespace Chartboost.AdFormats.Banner.Unity
{
    public partial class ChartboostMediationUnityBannerAd : MonoBehaviour
    {
#if UNITY_EDITOR
        [MenuItem("Chartboost Mediation/UnityBannerAd/Create New")]
        [MenuItem("GameObject/Chartboost Mediation/UnityBannerAd")]
        public static void CreateAd()
        {
            Instantiate();
        }
#endif

        public static ChartboostMediationUnityBannerAd Instantiate(
            ChartboostMediationBannerAdSize size = ChartboostMediationBannerAdSize.Standard,
            ChartboostMediationBannerAdScreenLocation screenLocation = ChartboostMediationBannerAdScreenLocation.Center)
        {
            // Find canvas with highest sorting order
            var canvas =  ChartboostMediationUtils.GetCanvasWithHighestSortingOrder();
            
            // Instantiate inside this canvas
            var unityBannerAd = Instantiate(
                Resources.Load<ChartboostMediationUnityBannerAd>("Chartboost Mediation/Banner/Unity/CM_UnityBannerAd"), 
                canvas.transform);
            unityBannerAd.name = "CM_UnityBannerAd";
            unityBannerAd.Size = size;
            unityBannerAd.transform.localScale = Vector3.one;
            unityBannerAd.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

            PlaceUnityBannerAd(unityBannerAd, screenLocation);
            
            return unityBannerAd;
        }
        
        private static void PlaceUnityBannerAd(ChartboostMediationUnityBannerAd unityBannerAd,
            ChartboostMediationBannerAdScreenLocation screenLocation)
        {
            var anchor = Vector2.zero;
            var pivot = Vector2.zero;
            switch (screenLocation)
            {
                case ChartboostMediationBannerAdScreenLocation.TopLeft:
                    anchor = new Vector2(0, 1);
                    pivot = new Vector2(0, 1);
                    break;
                case ChartboostMediationBannerAdScreenLocation.TopCenter:
                    anchor = new Vector2(0.5f, 1);
                    pivot = new Vector2(0.5f, 1);
                    break;
                case ChartboostMediationBannerAdScreenLocation.TopRight:
                    anchor = new Vector2(1, 1);
                    pivot = new Vector2(1, 1);
                    break;
                case ChartboostMediationBannerAdScreenLocation.Center:
                    anchor = new Vector2(0.5f, .5f);
                    pivot = new Vector2(0.5f, 0.5f);
                    break;
                case ChartboostMediationBannerAdScreenLocation.BottomLeft:
                    anchor = new Vector2(0, 0);
                    pivot = new Vector2(0, 0);
                    break;
                case ChartboostMediationBannerAdScreenLocation.BottomCenter:
                    anchor = new Vector2(0.5f, 0);
                    pivot = new Vector2(0.5f, 0);
                    break;
                case ChartboostMediationBannerAdScreenLocation.BottomRight:
                    anchor = new Vector2(1, 0);
                    pivot = new Vector2(1, 0);
                    break;
            }

            var rect = unityBannerAd.GetComponent<RectTransform>();

            rect.anchorMin = rect.anchorMax = anchor;
            rect.pivot = pivot;
            rect.anchoredPosition = Vector2.zero;
        }
    }
}