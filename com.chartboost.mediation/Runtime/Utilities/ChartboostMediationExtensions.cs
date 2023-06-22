using System;
using Chartboost.AdFormats.Banner.Unity;
using Chartboost.Banner;
using UnityEngine;

namespace Chartboost.Utilities
{
    public static class ChartboostMediationExtensions
    {
        public static LayoutParams LayoutParams(this RectTransform rectTransform)
        {
            Vector3[] corners = new Vector3[4];
            rectTransform.GetWorldCorners(corners);

            // corners[0] -> bottom-left
            // corners[1] -> top-left
            // corners[2] -> top-right
            // corners[3] -> bottom-right
            //    1           2
            //     _ _ _ _ _ _ 
            //    |           |
            //    |           |
            //    |           |
            //     - - - - - -
            //    0           3

            var lp = new LayoutParams
            {
                x = corners[0].x,   
                y = corners[1].y,   
                width = (int)(corners[2].x - corners[0].x),
                height = (int)(corners[1].y - corners[0].y)
            };
        
            return lp;
        }
        public static float GetUIScaleFactor(this ReferenceDevice device)
        {
            // TODO: Add more devices
            switch (device)
            {
                #if UNITY_ANDROID
                case ReferenceDevice.GooglePixel6: return 2.5f;
                case ReferenceDevice.GooglePixel5: return 2;
                case ReferenceDevice.GooglePixel5XL: return 2.5f;
                #elif UNITY_IOS
                case ReferenceDevice.Iphone13 : return 2;
                #endif
                default: return 2.5f;
            }
        }
        
        public static (float, float) GetDimensions(this ChartboostMediationBannerAdSize size)
        {
            return size switch
            {
                ChartboostMediationBannerAdSize.Standard => (320, 50),
                ChartboostMediationBannerAdSize.MediumRect => (300, 250),
                ChartboostMediationBannerAdSize.Leaderboard => (728, 90),
                _ => throw new ArgumentOutOfRangeException(nameof(size), size, null)
            };
        }
    }
}