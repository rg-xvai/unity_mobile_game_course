using System;

namespace CodeBase.Infrastructure.Services.Ads
{
  public interface IAdsService : IService
  {
    event Action RecordedVideoReady;
    bool IsRewardedVideoReady { get; }
    int Reward { get; }
    void Initialize();
    void ShowRewardedVideo(Action onVideoFinished);
  }
}