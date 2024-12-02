using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;

public class RewardedAdsButton : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
    [SerializeField] private Button _showAdButton;
    [SerializeField] private string _androidAdUnitId = "Rewarded_Android";
    [SerializeField] private string _iOSAdUnitId = "Rewarded_iOS";
    private string _adUnitId;
    private bool _adLoaded = false; // Track ad loading state.

    public Player_Movement playerMovement; // Reference to player movement script.

    void Awake()
    {
#if UNITY_IOS
        _adUnitId = _iOSAdUnitId;
#elif UNITY_ANDROID
        _adUnitId = _androidAdUnitId;
#else
        _adUnitId = null; // Ads are not supported on this platform.
#endif

        _showAdButton.interactable = false; // Disable button initially.
        _showAdButton.onClick.AddListener(ShowAd);
    }

    public void LoadAd()
    {
        if (!string.IsNullOrEmpty(_adUnitId) && !_adLoaded)
        {
            Debug.Log($"Loading Ad: {_adUnitId}");
            Advertisement.Load(_adUnitId, this);
        }
        else if (_adLoaded)
        {
            Debug.Log("Ad already loaded.");
        }
        else
        {
            Debug.LogError("Ad Unit ID is null or empty.");
        }
    }

    public void ShowAd()
    {
        if (_adLoaded)
        {
            _showAdButton.interactable = false; // Disable button during ad show.
            Advertisement.Show(_adUnitId, this);
        }
        else
        {
            Debug.LogError("Ad not ready yet. Load the ad first.");
        }
    }

    public void OnUnityAdsAdLoaded(string adUnitId)
    {
        if (adUnitId == _adUnitId)
        {
            Debug.Log("Ad successfully loaded.");
            _adLoaded = true; // Mark the ad as loaded.
            _showAdButton.interactable = true; // Enable the button.
        }
    }

    public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
    {
        Debug.LogError($"Failed to load ad {adUnitId}: {error} - {message}");
        _adLoaded = false; // Ensure ad is marked as not loaded.
    }

    public void OnUnityAdsShowStart(string adUnitId)
    {
        Debug.Log($"Ad {adUnitId} started showing.");
    }

    public void OnUnityAdsShowClick(string adUnitId)
    {
        Debug.Log($"Ad {adUnitId} clicked.");
    }

    public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
    {
        if (adUnitId == _adUnitId && showCompletionState == UnityAdsShowCompletionState.COMPLETED)
        {
            Debug.Log("Unity Ads Rewarded Ad Completed. Granting reward.");
            playerMovement.hasWatchedAd = true; // Reward the player.
            _adLoaded = false; // Mark ad as not loaded after completion.
            LoadAd(); // Load a new ad for next time.
        }
        else
        {
            Debug.Log($"Ad {adUnitId} did not finish successfully.");
        }
    }

    public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
    {
        Debug.LogError($"Ad {adUnitId} failed to show: {error} - {message}");
        _adLoaded = false; // Reset ad state to allow reloading.
        LoadAd(); // Reload the ad.
    }

    void OnDestroy()
    {
        _showAdButton.onClick.RemoveAllListeners();
    }
}
