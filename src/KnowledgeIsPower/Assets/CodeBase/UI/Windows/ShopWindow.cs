using TMPro;

namespace CodeBase.UI.Windows
{
  public class ShopWindow : WindowBase
  {
    public TextMeshProUGUI SkullText;

    protected override void Initialize() =>
      RefreshSkullText();

    protected override void SubscribeUpdates() =>
      Progress.WorldData.LootData.Changed += RefreshSkullText;

    protected override void Cleanup()
    {
      base.Cleanup();
      Progress.WorldData.LootData.Changed -= RefreshSkullText;
    }

    private void RefreshSkullText() => 
      SkullText.text = Progress.WorldData.LootData.Collected.ToString();
  }
}