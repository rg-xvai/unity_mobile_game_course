using CodeBase.Infrastructure.Services.Ads;
using CodeBase.Infrastructure.Services.PresistenProgress;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Windows
{
  public class RewardedAdItem : MonoBehaviour
  {
    public Button ShowAdButton;
    public GameObject[] AdActiveObjects;
    public GameObject[] AdInactiveObjects;

    private IAdsService _adsService;
    private IPersistentProgressService _progressService;

    public void Construct(IAdsService adsService, IPersistentProgressService progressService)
    {
      _adsService = adsService;
      _progressService = progressService;
    }

    public void Initialize()
    {
      ShowAdButton.onClick.AddListener(OnShowAdClicked);

      RefreshAvailableAds();
    }

    public void Subscribe() =>
      _adsService.RecordedVideoReady += RefreshAvailableAds;

    public void Cleanup() =>
      _adsService.RecordedVideoReady -= RefreshAvailableAds;

    private void OnShowAdClicked() =>
      _adsService.ShowRewardedVideo(OnVideoFinished);

    private void OnVideoFinished() =>
      _progressService.Progress.WorldData.LootData.Add(_adsService.Reward);

    private void RefreshAvailableAds()
    {
      bool videoReady = _adsService.IsRewardedVideoReady;

      foreach (GameObject activeObject in AdActiveObjects)
        activeObject.SetActive(videoReady);

      foreach (GameObject activeObject in AdInactiveObjects)
        activeObject.SetActive(!videoReady);
    }
  }
}