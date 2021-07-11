using CodeBase.Data;

namespace CodeBase.Infrastructure.Services.PresistenProgress
{
  public interface IPersistentProgressService : IService
  {
    PlayerProgress Progress { get; set; }
  }
}