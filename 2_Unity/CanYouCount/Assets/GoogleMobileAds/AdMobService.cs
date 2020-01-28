using System;
using GoogleMobileAds.Api;
using UnityEngine;

public class AdMobService : MonoBehaviour
{
    private const string _devAndroidInterstitialAdUnitId = "ca-app-pub-3940256099942544/1033173712";
    private const string _devAndroidAppId = "ca-app-pub-3940256099942544~3347511713";
    private const string _devAndroidBannerAdUnitId = "ca-app-pub-3940256099942544/6300978111";
    private const string Unknown = "unexpected_platform";
    private const string KEY = "Ad_Count_Key";

    [Header("App Id")]
    [SerializeField]
    private string _androidAppId = "ca-app-pub-2541894783789070~5988919935";
    [SerializeField]
    private string _iOSAppId = "ca-app-pub-2541894783789070~8073524777";

    [Header("Ad Unit Id")]
    [SerializeField]
    private string _androidAdUnitId = "ca-app-pub-2541894783789070/5797348249";
    [SerializeField]
    private string _iOSAdUnitId = "ca-app-pub-2541894783789070/9545021568";
    [SerializeField]
    private int _timesBeforeAd = 3;

    string adUnitId = Unknown;
    string appId = Unknown;
    private BannerView _bannerView = null;
    private InterstitialAd _interstitial = null;

    /// <summary>
    /// Requests the interstitial.
    /// </summary>
    public void RequestInterstitial()
    {
        if (this._interstitial.IsLoaded() && ShowAdIfNeeded())
        {
            this._interstitial.Show();
        }
    }

    private bool ShowAdIfNeeded() 
    {
        int currentCount = PlayerPrefs.GetInt(KEY, 0);
        bool shouldShow = currentCount >= 3;
        int valueToSetTo = shouldShow ? 0 : currentCount + 1;

        PlayerPrefs.SetInt(KEY, valueToSetTo);
        return shouldShow;
    }


    private void Awake()
    {
        Initialize();
    }

    private void AttachDeleagates()
    {
        // Called when an ad request has successfully loaded.
        this._bannerView.OnAdLoaded += this.HandleOnAdLoaded;
        // Called when an ad request failed to load.
        this._bannerView.OnAdFailedToLoad += this.HandleOnAdFailedToLoad;
        // Called when an ad is clicked.
        this._bannerView.OnAdOpening += this.HandleOnAdOpened;
        // Called when the user returned from the app after an ad click.
        this._bannerView.OnAdClosed += this.HandleOnAdClosed;
        // Called when the ad click caused the user to leave the application.
        this._bannerView.OnAdLeavingApplication += this.HandleOnAdLeavingApplication;
    }

    private void Initialize()
    {
#if UNITY_ANDROID
        appId = _androidAppId;
#endif

#if UNITY_IPHONE
        appId = _iOSAppId;
#endif

        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(appId);
    }

    private void OnDestroy()
    {
        // Called when an ad request has successfully loaded.
        this._bannerView.OnAdLoaded -= this.HandleOnAdLoaded;
        // Called when an ad request failed to load.
        this._bannerView.OnAdFailedToLoad -= this.HandleOnAdFailedToLoad;
        // Called when an ad is clicked.
        this._bannerView.OnAdOpening -= this.HandleOnAdOpened;
        // Called when the user returned from the app after an ad click.
        this._bannerView.OnAdClosed -= this.HandleOnAdClosed;
        // Called when the ad click caused the user to leave the application.
        this._bannerView.OnAdLeavingApplication -= this.HandleOnAdLeavingApplication;
        _bannerView.Destroy();
    }

    private void HandleOnAdLoaded(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdLoaded event received");
    }

    private void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        MonoBehaviour.print("HandleFailedToReceiveAd event received with message: "
                            + args.Message);
    }

    private void HandleOnAdOpened(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdOpened event received");
    }

    private void HandleOnAdClosed(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdClosed event received");
    }

    private void HandleOnAdLeavingApplication(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdLeavingApplication event received");
    }

    private void Start()
    {
        this.RequestBanner();

        SetupInterstitial();
    }

    private void SetupInterstitial()
    {
        // Initialize an InterstitialAd.
        this._interstitial = new InterstitialAd(adUnitId);
        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the interstitial with the request.
        this._interstitial.LoadAd(request);
    }

    private void RequestBanner()
    {
#if UNITY_ANDROID
        adUnitId = _androidAdUnitId;
#endif

#if UNITY_IPHONE
        adUnitId = _iOSAdUnitId;
#endif

        // Create a 320x50 banner at the top of the screen.
        this._bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Bottom);
        AttachDeleagates();
        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();

        // Load the banner with the request.
        this._bannerView.LoadAd(request);
    }

}
