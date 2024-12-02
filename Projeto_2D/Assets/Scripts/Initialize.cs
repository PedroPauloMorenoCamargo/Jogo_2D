using UnityEngine;
using UnityEngine.Advertisements;

public class AdsInitializer : MonoBehaviour, IUnityAdsInitializationListener
{
    [SerializeField] private string _androidGameId;
    [SerializeField] private string _iOSGameId;
    [SerializeField] private bool _testMode = true;
    private string _gameId;

    public RewardedAdsButton rewardedAdsButton; // Reference to the rewarded ads button.

    void Awake()
    {
        InitializeAds();
    }

    public void InitializeAds()
    {
#if UNITY_IOS
        _gameId = _iOSGameId;
#elif UNITY_ANDROID
        _gameId = _androidGameId;
#elif UNITY_EDITOR
        _gameId = _androidGameId; // For testing in the editor.
#endif

        if (!Advertisement.isInitialized && Advertisement.isSupported)
        {
            Debug.Log("Initializing Unity Ads...");
            Advertisement.Initialize(_gameId, _testMode, this);
        }
        else
        {
            Debug.Log("Unity Ads already initialized or not supported.");
        }
    }

    public void OnInitializationComplete()
    {
        Debug.Log("Unity Ads initialization complete.");
        rewardedAdsButton.LoadAd(); // Start loading the first ad.
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.LogError($"Unity Ads Initialization Failed: {error} - {message}");
    }
}
