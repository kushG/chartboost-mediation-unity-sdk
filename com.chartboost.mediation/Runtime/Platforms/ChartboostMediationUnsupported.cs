namespace Chartboost.Platforms
{
    public class ChartboostMediationUnsupported : ChartboostMediationExternal
    {
        private static string _userIdentifier;
        
        public ChartboostMediationUnsupported()
        {
            LogTag = "ChartboostMediation (Unsupported)";
        }

        public override void Init()
        {
            base.Init();
            IsInitialized = true;
        }

        public override void InitWithAppIdAndSignature(string appId, string appSignature)
        {
            base.InitWithAppIdAndSignature(appId, appSignature);
            IsInitialized = true;
        }

        public override void SetUserIdentifier(string userIdentifier)
        {
            base.SetUserIdentifier(userIdentifier);
            _userIdentifier = userIdentifier;
        }
        
        public override string GetUserIdentifier()
        {
            base.GetUserIdentifier();
            return _userIdentifier;
        }
    }
}