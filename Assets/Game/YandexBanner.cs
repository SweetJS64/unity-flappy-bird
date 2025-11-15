using UnityEngine;
using YandexMobileAds;
using YandexMobileAds.Base;

public class YandexBanner : MonoBehaviour
{
    [SerializeField] private string ProductionAdUnitId = "R-M-17615282-1";
    private Banner _banner;

    private void Start()
    {
        var adUnitId =
#if UNITY_EDITOR
            "demo-banner-yandex";
#else
            ProductionAdUnitId;
#endif
        
        var widthDp = GetScreenWidthDp();

        var size = BannerAdSize.StickySize(widthDp);

        _banner = new Banner(adUnitId, size, AdPosition.BottomCenter);

        var request = new AdRequest.Builder().Build();

        _banner.OnAdLoaded += (s, e) => Debug.Log("[Yandex Ads] Banner loaded");
        _banner.OnAdFailedToLoad += (s, e) =>
            Debug.LogError("[Yandex Ads] Banner failed: " + (e?.Message ?? "unknown"));
        _banner.OnReturnedToApplication += (s, e) => Debug.Log("[Yandex Ads] Returned");
        _banner.OnLeftApplication += (s, e) => Debug.Log("[Yandex Ads] Left");

        _banner.LoadAd(request);
        _banner.Show();
    }

    private void OnDestroy()
    {
        _banner?.Destroy();
        _banner = null;
    }

    private int GetScreenWidthDp()
    {
        var dpi = Screen.dpi;
        if (dpi <= 0f) return 360;
        return Mathf.RoundToInt(Screen.width * 160f / dpi);
    }
} 