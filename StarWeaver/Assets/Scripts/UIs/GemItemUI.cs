using UnityEngine;

public class GemItemUI : ItemUIBase<GemItem>
{
    public override void Setup(GemItem item, ItemGridBase<GemItem> grid, bool isSelected = false)
    {
      base.Setup(item, grid, isSelected);

      button.onClick.RemoveAllListeners();
      button.onClick.AddListener(() => {
        Debug.Log("Button actually clicked!..");
        OnClick();
      });
      Debug.Log("Button click listener added");
    }

    protected override void OnClick()
    {
        Debug.Log("GemItemUI OnClick called");
        base.OnClick();
    }
}
