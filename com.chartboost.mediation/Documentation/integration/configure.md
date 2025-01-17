# Configure Chartboost Mediation

## Test Mode
The Chartboost Mediation Unity SDK includes a Test Mode method that will allow you to test your partner integrations and get their test ads. To enable Test Mode, simply set the following method to true after starting the SDK. Remember to remove it or set the method to false before releasing your app.

```csharp
ChartboostMediation.SetTestMode(true);
// or
ChartboostMediation.SetTestMode(false);
```

## COPPA
COPPA Settings can be configured utilizing the following method:

```csharp
  ChartboostMediation.SetSubjectToCoppa(true);
  // or
  ChartboostMediation.SetSubjectToCoppa(false);
```

* By sending `SetSubjectToCoppa (true)`, you indicate that you want your content treated as child-directed for purposes of COPPA. We will take steps to disable interest-based advertising for such ad requests.

* By sending `SetSubjectToCoppa (false)`, you indicate that you don't want your content treated as child-directed for purposes of COPPA. You represent and warrant that your applications and services are not directed towards children and that you will not provide any information to Chartboost Mediation from a user under the age of 13.

## GDPR
```csharp
  ChartboostMediation.SetSubjectToGDPR(true);
  // or
  ChartboostMediation.SetSubjectToGDPR(false);
```

* By sending `SetSubjectToGDPR (true)`, you indicate that GDPR is applied to this user from your application.

* By sending `SetSubjectToGDPR (false)`, you indicate that GDPR is not applied to this user from your application.

```csharp
  ChartboostMediation.SetUserHasGivenConsent(true);
  // or
  ChartboostMediation.SetUserHasGivenConsent(false);
```

* By sending `SetUserHasGivenConsent (true)`, you indicate that this user from your application has given consent to share personal data for behavioral targeted advertising.

* By sending `SetUserHasGivenConsent (false)`, you indicate that this user from your application has not given consent to use its personal data for behavioral targeted advertising, so only contextual advertising is allowed.

## CCPA
```csharp
  ChartboostMediation.SetCCPAConsent(true);
  // or
  ChartboostMediation.SetCCPAConsent(false);
```

* By sending `SetCCPAConsent (true)`, you indicate that this user from your application has given consent to share personal data for behavioral targeted advertising under CCPA regulation.

* By sending `SetCCPAConsent (false)`, you indicate that this user from your application has not given consent to allow sharing personal data for behavioral targeted advertising under CCPA regulation.

> **Note** \
> Chartboost Mediation will send CCPA information to the bidding network and set the CCPA information for the adapters.

## Keywords
As of Chartboost Mediation 2.9.0, the Chartboost Mediation SDKs introduces keywords: key-value pairs to enable real-time targeting on line items.

### Set Keywords
To set keywords, you will need to first create a Chartboost Mediation ad object, then use the setKeyword method to add key-value keywords pair.

```csharp
// Create an Ad object.
ChartboostMediationInterstitialAd interstitialAd = ChartboostMediation.GetInterstitialAd(PLACEMENT_INTERSTITIAL);

ChartboostMediationRewardedAd rewardedAd = ChartboostMediation.GetRewardedAd(PLACEMENT_REWARDED);

ChartboostMediationBannerAd bannerAd = ChartboostMediation.GetBannerAd(PLACEMENT_BANNER, BANNER_SIZE);

// Set a Keyword
this.interstitialAd.SetKeyword("i12_keyword1", "i12_value1");
this.rewardedAd.SetKeyword("rwd_keyword1", "rwd_value1");
this.bannerAd.SetKeyword("bnr_keyword1", "bnr_value1");
```

### Remove Keywords
To remove keywords, simply use the removeKeyword method and pass the key you would like to remove.

```csharp
// Remove Keyword.
this.interstitialAd.RemoveKeyword("i12_keyword1");
this.rewardedAd.RemoveKeyword("rwd_keyword1");
this.bannerAd.RemoveKeyword("bnr_keyword1");
```

> **Warning** \
> Keywords has restrictions for setting keys and values. The maximum characters allowed for keys is 64 characters. The maximum characters for values is 256 characters.

### Setting User Identifier

The user identifier property is found on the ChartboostMediation class. This property may be set anytime after SDK initialization.

```csharp
ChartboostMediation.SetUserIdentifier("user");
```

### Setting Custom Data

The custom data property is found on the `ChartboostMediationRewardedAd` instance, and has a maximum character limit of `1000` characters. In the event that the limit is exceeded, the customData property will be set to null.

Custom data may be set at any time before calling `Show()`

```csharp
_rewardedAd = ChartboostMediation.GetRewardedAd(“placement”);
var bytesToEncode = Encoding.UTF8.GetBytes("{\"testkey\":\"testvalue\"}");
var encodedText = Convert.ToBase64String(bytesToEncode);
_rewardedAd.SetCustomData(encodedText);

_rewardedAd.Load();
```
