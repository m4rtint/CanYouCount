using GoogleMobileAds.Api;
using UnityEngine;

public class AdMobService : MonoBehaviour
{
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

    private const string Unknown = "unexpected_platform";
    private BannerView bannerView;

    public void Start()
    {
#if UNITY_ANDROID
            string appId = _androidAppId;
#elif UNITY_IPHONE
            string appId = _iOSAppId;
#else
            string appId = Unknown;
#endif

        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(appId);

        this.RequestBanner();
    }

    private void RequestBanner()
    {
#if UNITY_ANDROID
        string adUnitId = _androidAdUnitId;
#elif UNITY_IPHONE
            string adUnitId = _iOSAdUnitId;
#else
            string adUnitId = Unknown;
#endif

        // Create a 320x50 banner at the top of the screen.
        this.bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Bottom);
        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();

        // Load the banner with the request.
        this.bannerView.LoadAd(request);
    }
}
