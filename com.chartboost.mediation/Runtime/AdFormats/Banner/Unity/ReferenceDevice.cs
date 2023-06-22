namespace Chartboost.AdFormats.Banner.Unity
{
    public enum ReferenceDevice
    {
        // TODO: Add more devices
        #if UNITY_ANDROID
        GooglePixel5,
        GooglePixel5XL,
        GooglePixel6,
        GooglePixel6XL,
        #elif  UNITY_IOS
        IPhone12,
        Iphone13,
        Iphone14
        #endif
    }
    
    
}