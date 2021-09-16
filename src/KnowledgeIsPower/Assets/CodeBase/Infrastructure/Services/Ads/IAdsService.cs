﻿using System;

namespace CodeBase.Infrastructure.Services.Ads
{
  public interface IAdsService
  {
    event Action RecordedVideoReady;
    bool IsRewardedVideoReady { get; }
    void Initialize();
    void ShowRewardedVideo(Action onVideoFinished);
  }
}