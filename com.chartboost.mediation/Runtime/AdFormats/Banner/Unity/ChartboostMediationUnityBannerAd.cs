using System.Collections.Generic;
using Chartboost.Banner;
using Chartboost.Placements;
using Chartboost.Utilities;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;
using Logger = Chartboost.Utilities.Logger;

namespace Chartboost.AdFormats.Banner.Unity
{
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(DragObject))]
    [RequireComponent(typeof(Image))]
    public partial class ChartboostMediationUnityBannerAd : MonoBehaviour, IChartboostMediationBannerEvents
    {
        // new API
        public Dictionary<string, string> keywords;

        [Header("Config")]
        [Tooltip("Auto loads this ad after Chartboost Mediation SDK is initialized")]
        public bool autoLoadOnInit = true;
        
        [Tooltip("Placement identifier for banner")]
        public string bannerPlacementName;

        [SerializeField] [Tooltip("Size of the banner")]
        private ChartboostMediationBannerAdSize _size;
        
        [SerializeField] [Tooltip("If enabled, this Gameobject can be dragged on screen")]
        private bool _draggable = true;
        
        [Header("Edit Mode Only")] [SerializeField]
        private ReferenceDevice _referenceDevice;
        
        private ChartboostMediationBannerAd _ad;
        private RectTransform _rectTransform;
        private DragObject _dragger;
        private Image _outline;
        private Canvas _canvas;
        
        private const string LogTag = "UnityBannerAd";

        
        // current
        public event ChartboostMediationPlacementLoadEvent DidLoadBanner;
        public event ChartboostMediationPlacementEvent DidClickBanner;
        public event ChartboostMediationPlacementEvent DidRecordImpressionBanner;
        
        // TODO (new API)
        // TODO: Remove these delegates from here once bannerloadrequest is created
        public delegate void ChartboostMediationBannerAdEventWithError(IChartboostMediationBannerAd ad, ChartboostMediationError? error);
        public delegate void ChartboostMediationBannerAdEvent(IChartboostMediationBannerAd bannerAd);
        public event ChartboostMediationBannerAdEvent DidClick;
        public event ChartboostMediationBannerAdEvent DidRecordImpression;
        public event ChartboostMediationBannerAdEventWithError DidClose;

        #region Unity Lifecycle Event

        private void Awake()
        {
            _canvas = GetComponentInParent<Canvas>();
            _dragger = GetComponent<DragObject>();
            _outline = GetComponent<Image>();
            _rectTransform = GetComponent<RectTransform>();
            
            ChartboostMediation.DidStart += ChartboostMediationOnDidStart;
            
            // TODO: Event subscriptions will happen from load request in new API
            ChartboostMediation.DidLoadBanner += ChartboostMediationOnDidLoadBanner;
            ChartboostMediation.DidClickBanner += ChartboostMediationOnDidClickBanner;
            ChartboostMediation.DidRecordImpressionBanner += ChartboostMediationOnDidRecordImpressionBanner;            
        }

        private void Start()
        {
            Draggable = _draggable;
            AdjustSize();
        }

        private void OnDestroy()
        {
            // TODO 
            ChartboostMediation.DidLoadBanner -= ChartboostMediationOnDidLoadBanner;
            ChartboostMediation.DidClickBanner -= ChartboostMediationOnDidClickBanner;
            ChartboostMediation.DidRecordImpressionBanner -= ChartboostMediationOnDidRecordImpressionBanner;
        }

        #endregion

        #region Public API

        /// <summary>
        /// Size of the Banner Ad
        /// </summary>
        public ChartboostMediationBannerAdSize Size
        {
            get => _size;
            set
            {
                _size = value;
                AdjustSize();
            }
        }

        /// <summary>
        /// Enables/Disables dragging capabilities of Banner Ad
        /// </summary>
        public bool Draggable
        {
            get => _draggable;
            set
            {
                _draggable = value;
                _dragger.enabled = value;

                if (_draggable)
                {
                    BannerAd?.EnableDrag(OnBannerDrag);
                }
                else
                {
                    BannerAd?.DisableDrag();
                }
            }
        }

        public void Load()
        {
            // Current API
            var lp = _rectTransform.LayoutParams();
            BannerAd?.Load(lp.x, lp.y, lp.width, lp.height);
            
            // TODO: New API
            // var loadRequest = new ChartboostMediationBannerAdLoadRequest(bannerPlacementName, size, keywords);
            // loadRequest.DidClick += ChartboostMediationOnDidClickBanner;
            // loadRequest.DidRecordImpression += ChartboostMediationOnDidRecordImpressionBanner;
            //
            // var result = await ChartboostMediation.LoadBannerAd(loadRequest);
            // _ad = result.AD;
        }
        
        public void ShowOutline(bool show)
        {
            _outline.enabled = show;
        }

        #region Base Methods
        
        /// <inheritdoc cref="ChartboostMediationBannerBase.SetKeyword"/>>
        public bool SetKeyword(string keyword, string value) 
            => BannerAd != null && BannerAd.SetKeyword(keyword, value);
        
        /// <inheritdoc cref="ChartboostMediationBannerBase.RemoveKeyword"/>>
        public string RemoveKeyword(string keyword) 
            => BannerAd?.RemoveKeyword(keyword);
        
        /// <inheritdoc cref="ChartboostMediationBannerBase.Destroy"/>>
        public void Destroy() 
            => BannerAd?.Destroy();

        /// <inheritdoc cref="ChartboostMediationBannerBase.SetVisibility"/>>
        public void SetVisibility(bool isVisible) 
            =>BannerAd?.SetVisibility(isVisible);

        /// <inheritdoc cref="ChartboostMediationBannerBase.ClearLoaded"/>>
        public void ClearLoaded() 
            => BannerAd?.ClearLoaded();

        /// <inheritdoc cref="ChartboostMediationBannerBase.Remove"/>>
        public void Remove()
        {
            BannerAd?.Remove();
            Destroy(gameObject);
        }

        #endregion
        
        #endregion

        #region Callbacks
        
        private void ChartboostMediationOnDidStart(string error)
        {
            if (!string.IsNullOrEmpty(error))
                return;

            if (autoLoadOnInit)
            {
                Load();
            }
        }
        
        private void OnBannerDrag(float x, float y)
        {
            _canvas ??= GetComponentInParent<Canvas>();
            var rt = _rectTransform == null ? GetComponent<RectTransform>() : _rectTransform;

            // adjust x,y based on anchor position
            // TODO: Not handling the case when custom anchors are in use (i.e when anchorMin and anchorMax are not equal)
            if (rt.anchorMin == rt.anchorMax)
            {
                var anchor = rt.anchorMin;
                x -= Screen.width * anchor.x;
                y -= Screen.height * anchor.y;
            }

            // convert in canvas scale
            var canvasScale = _canvas.transform.localScale;
            x /= canvasScale.x;
            y /= canvasScale.x;

            // x,y obtained from native is for top left corner (x = 0,y = 1)
            // RectTransform pivot may or may not be top-left (it's usually at center)
            var pivot = rt.pivot;
            var newX = x + (_size.GetDimensions().Item1 * ScalingFactor * (pivot.x - 0f)); // top-left x is 0
            var newY = y + (_size.GetDimensions().Item2 * ScalingFactor * (pivot.y - 1f)); // top-left y is 1

            rt.anchoredPosition = new Vector2(newX, newY);
        }

        #region Current API
        
        private void ChartboostMediationOnDidLoadBanner(string placement, string loadid, BidInfo bidinfo, string error)
        {
            if(placement != bannerPlacementName)
                return;
            
            DidLoadBanner?.Invoke(placement, loadid, bidinfo, error);
            
            if(Draggable)
                BannerAd?.EnableDrag(OnBannerDrag);
            
            // NO need to show outline once an ad is loaded
            ShowOutline(false);
        }
        
        private void ChartboostMediationOnDidClickBanner(string placement, string error)
        {
            if(placement != bannerPlacementName)
                return;
            
            DidClickBanner?.Invoke(placement, error);
        }
        
        private void ChartboostMediationOnDidRecordImpressionBanner(string placement, string error)
        {
            if(placement != bannerPlacementName)
                return;
            
            DidRecordImpressionBanner?.Invoke(placement, error);
        }
        
        #endregion

        #region New API (TODO for later)

        public void ChartboostMediationDidClick(IChartboostMediationBannerAd ad)
        {
            DidClick?.Invoke(ad);
        }
        
        public void ChartboostMediationDidRecordImpression(IChartboostMediationBannerAd ad)
        {
            DidRecordImpression?.Invoke(ad);
        }
        
        public void ChartboostMediationDiDClose(IChartboostMediationBannerAd ad, ChartboostMediationError error)
        {
            DidClose?.Invoke(ad, error);
        }

        #endregion

        #endregion

        #region Utils
        
        /// <summary>
        /// Calculates and sets width and height of this Gameobject based on its size />
        /// </summary>
        public void AdjustSize()
        {
            if (_rectTransform == null)
            {
                _rectTransform = GetComponent<RectTransform>();
            }
            
            _rectTransform.sizeDelta = new Vector2(Size.GetDimensions().Item1 * ScalingFactor,
                Size.GetDimensions().Item2 * ScalingFactor);
        }
        
        [CanBeNull]
        private ChartboostMediationBannerAd BannerAd
        {
            get
            {
                _ad ??= ChartboostMediation.GetBannerAd(bannerPlacementName, _size);

                if (_ad != null) return _ad;
                Logger.LogError(LogTag, "BannerAd not found") ;
                return null;
            }
        }
        private float ScalingFactor
        {
            get
            {
                var scaleFactor = 2.5f;
                var canvasScaler = GetComponentInParent<CanvasScaler>();
                if (canvasScaler == null)
                    return scaleFactor;

#if UNITY_EDITOR
                // Banner sizes are usually in `dp` or `points` but Unity works with pixels so there is no way of
                // visualizing the exact size of banner in pixels in Editor.
                // Hence we use a reference device whose scale factor is already determined
                scaleFactor = _referenceDevice.GetUIScaleFactor();
#else
                scaleFactor = ChartboostMediation.GetUIScaleFactor();
#endif
                if (canvasScaler.uiScaleMode == CanvasScaler.ScaleMode.ScaleWithScreenSize)
                {
                    scaleFactor /= canvasScaler.transform.localScale.x;
                }
                
                return scaleFactor;
            }
        }
        

        #endregion
    }
}