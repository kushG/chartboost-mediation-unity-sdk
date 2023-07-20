using System;

namespace Chartboost.Banner
{
    /// <summary>
    /// Interface implemented by all Chartboost Mediation banners. 
    /// </summary>
    public interface IChartboostMediationBannerAd
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="location"></param>
        void Load(ChartboostMediationBannerAdScreenLocation location);
        
        /// <summary>
        /// Loads the banner ad at specified position and size
        /// </summary>
        /// <param name="x"> Distance from left of screen in pixels </param>
        /// <param name="y"> Distance from bottom of screen in pixels </param>
        /// <param name="width"> width in pixels </param>
        /// <param name="height"> height in pixels </param>
        void Load(float x, float y, int width, int height); 


        /// <summary>This method changes the visibility of the banner ad.</summary>
        /// <param name="isVisible">Specify if the banner should be visible.</param>
        void SetVisibility(bool isVisible);
        
        /// <summary>
        /// If an advertisement has been loaded, clear it. Once cleared, a new
        /// load can be performed.
        /// </summary>
        void ClearLoaded();
        
        /// <summary>
        /// Remove the banner.
        /// </summary>
        [Obsolete("Remove has been deprecated, please use Destroy instead.")] 
        void Remove();
        
        /// <summary>
        /// Enables dragging of this banner ad object
        /// </summary>
        /// <param name="onDrag"> Callback that receives screen coordinates of ad object, when dragged</param>
        void EnableDrag(Action<float, float> onDrag = null);

        /// <summary>
        /// Disables dragging of this banner ad object
        /// </summary>
        void DisableDrag();
    }
}
