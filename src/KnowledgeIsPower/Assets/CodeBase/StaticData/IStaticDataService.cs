using CodeBase.Infrastructure.Services;

namespace CodeBase.StaticData
{
  public interface IStaticDataService : IService
  {
    void LoadMonsters();
    MonsterStaticData ForMonster(MonsterTypeId typeId);
    LevelStaticData ForLevel(string sceneKey);
  }
}