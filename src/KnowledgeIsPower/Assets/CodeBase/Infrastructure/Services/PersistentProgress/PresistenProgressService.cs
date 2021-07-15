using CodeBase.Data;

namespace CodeBase.Infrastructure.Services.PresistenProgress
{
  public class PersistentProgressService : IPersistentProgressService
  {
    public PlayerProgress Progress { get; set; }
  }
}