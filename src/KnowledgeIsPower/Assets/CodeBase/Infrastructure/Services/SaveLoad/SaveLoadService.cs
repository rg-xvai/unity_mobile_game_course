using CodeBase.Data;
using UnityEngine;

namespace CodeBase.Infrastructure.Services.SaveLoad
{
  public class SaveLoadService : ISaveLoadService
  {
    private const string ProgressKey = "Progress";

    public void SaveProgress()
    {
    }

    public PlayerProgress LoadProgress() => 
      PlayerPrefs.GetString(ProgressKey)?
        .ToDeserialized<PlayerProgress>();
  }
}