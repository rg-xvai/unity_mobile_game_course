namespace CodeBase.Infrastructure.Services
{
  public interface IRandomService : IService
  {
    int Next(int min, int max);
  }
}