// ReSharper disable InconsistentNaming
using UnityEngine.Scripting;

namespace Helium.Interfaces
{
    /// <summary>
    /// All Banner placement callbacks.
    /// </summary>
    public interface IBannerEvents
    {
        /// <summary>
        /// Called after a banner has been loaded from the Helium API
        /// servers and cached locally.
        /// </summary>
        [Preserve]
        public event HeliumPlacementEvent DidLoadBanner;

        /// <summary>
        /// Called after a banner has been displayed on screen.
        /// </summary>
        [Preserve]
        public event HeliumPlacementEvent DidShowBanner;

        /// <summary>
        /// Called after a banner ad has been clicked.
        /// Implement to be notified of when a banner ad has been clicked for a given placement
        /// </summary>
        [Preserve]
        public event HeliumPlacementEvent DidClickBanner;

        /// <summary>
        /// Determines an ad visibility on the screen.
        /// Implement to be notified of when a banner ad has been become visible on the screen.
        /// </summary>
        [Preserve]
        public event HeliumPlacementEvent DidRecordImpressionBanner;
        
        /// <summary>
        /// Called with bid information after an banner has been loaded from the Helium API
        /// servers and cached locally.
        /// </summary>
        [Preserve]
        public event HeliumBidEvent DidWinBidBanner;
    }
}
