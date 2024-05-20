using System;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Mediation;

public class UnityAdsManager : MonoBehaviour
{
    private string gameId;
    private string adUnitId;
    public float adInterval = 300.0f; // Time interval between ads in seconds
    private float timeSinceLastAd;

    private IInterstitialAd interstitialAd;

    async void Start()
    {
        timeSinceLastAd = 0.0f;

#if UNITY_ANDROID
        gameId = "5622038"; // Replace with your actual Android Game ID
        adUnitId = "Interstitial_Android"; // Replace with your actual Android Ad Unit ID
#elif UNITY_IOS
        gameId = "5622039"; // Replace with your actual iOS Game ID
        adUnitId = "Interstitial_iOS"; // Replace with your actual iOS Ad Unit ID
#else
        Debug.LogError("Unsupported platform");
        return;
#endif

        try
        {
            // Initialize the Unity Services core
            await UnityServices.InitializeAsync();

            // Create an instance of the interstitial ad
            interstitialAd = MediationService.Instance.CreateInterstitialAd(adUnitId);

            // Subscribe to events
            interstitialAd.OnClosed += OnAdClosed;
            interstitialAd.OnFailedShow += OnAdFailedShow;
            interstitialAd.OnLoaded += OnAdLoaded;
            interstitialAd.OnFailedLoad += OnAdFailedLoad;

            // Load the ad
            await interstitialAd.LoadAsync();
        }
        catch (Exception e)
        {
            Debug.LogError($"Unity Services initialization failed: {e}");
        }
    }

    void Update()
    {
        timeSinceLastAd += Time.deltaTime;

        if (timeSinceLastAd >= adInterval && interstitialAd.AdState == AdState.Loaded)
        {
            ShowAd();
            timeSinceLastAd = 0.0f;
        }
    }

    public void ShowAd()
    {
        if (interstitialAd.AdState == AdState.Loaded)
        {
            interstitialAd.ShowAsync();
        }
        else
        {
            Debug.Log("Advertisement not ready");
        }
    }

    private void OnAdClosed(object sender, EventArgs e)
    {
        Debug.Log("Ad closed");
        interstitialAd.LoadAsync();
    }

    private void OnAdFailedShow(object sender, ShowErrorEventArgs e)
    {
        Debug.LogError($"Ad failed to show: {e.Message}");
    }

    private void OnAdLoaded(object sender, EventArgs e)
    {
        Debug.Log("Ad loaded");
    }

    private void OnAdFailedLoad(object sender, LoadErrorEventArgs e)
    {
        Debug.LogError($"Ad failed to load: {e.Message}");
    }

    private void OnDestroy()
    {
        // Clean up events
        if (interstitialAd != null)
        {
            interstitialAd.OnClosed -= OnAdClosed;
            interstitialAd.OnFailedShow -= OnAdFailedShow;
            interstitialAd.OnLoaded -= OnAdLoaded;
            interstitialAd.OnFailedLoad -= OnAdFailedLoad;
        }
    }
}
