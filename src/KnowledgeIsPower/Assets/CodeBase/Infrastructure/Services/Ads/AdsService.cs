using System;
using UnityEngine;
using UnityEngine.Advertisements;

namespace CodeBase.Infrastructure.Services.Ads
{
  public class AdsService : IAdsService, IUnityAdsListener
  {
    private const string AndroidGameId = "4335709";
    private const string IOSGameId = "4335708";

    private const string RewardedVideoAndroid = "Rewarded_Android";
    private const string RewardedVideoIOS = "Rewarded_iOS";

    public event Action RecordedVideoReady;

    private string _gameId;
    private string _rewardedVideo;
    private Action _onVideoFinished;

    public void Initialize()
    {
      switch (Application.platform)
      {
        case RuntimePlatform.Android:
          _gameId = AndroidGameId;
          _rewardedVideo = RewardedVideoAndroid;
          break;
        case RuntimePlatform.IPhonePlayer:
          _gameId = IOSGameId;
          _rewardedVideo = RewardedVideoIOS;
          break;
        case RuntimePlatform.WindowsEditor:
          _gameId = AndroidGameId;
          _rewardedVideo = RewardedVideoAndroid;
          break;
        default:
          Debug.Log("Unsupported platform for ads");
          break;
      }

      Advertisement.AddListener(this);
      Advertisement.Initialize(_gameId);
    }

    public void ShowRewardedVideo(Action onVideoFinished)
    {
      Advertisement.Show(_rewardedVideo);
      _onVideoFinished = onVideoFinished;
    }

    public bool IsRewardedVideoReady =>
      Advertisement.IsReady(_rewardedVideo);

    public void OnUnityAdsReady(string placementId)
    {
      Debug.Log($"OnUnityAdsReady {placementId}");

      if (placementId == _rewardedVideo)
        RecordedVideoReady?.Invoke();
    }

    public void OnUnityAdsDidError(string message) =>
      Debug.Log($"OnUnityAdsDidError {message}");

    public void OnUnityAdsDidStart(string placementId) =>
      Debug.Log($"OnUnityAdsDidStart {placementId}");

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
      switch (showResult)
      {
        case ShowResult.Failed:
          Debug.LogError($"OnUnityAdsDidFinish {showResult}");
          break;
        case ShowResult.Skipped:
          Debug.LogError($"OnUnityAdsDidFinish {showResult}");
          break;
        case ShowResult.Finished:
          _onVideoFinished?.Invoke();
          break;
        default:
          Debug.LogError($"OnUnityAdsDidFinish {showResult}");
          break;
      }

      _onVideoFinished = null;
    }
  }
}