using UnityEngine;

public class MaterialItemUI : ItemUIBase<ItemBase>
{
  public override void Setup(ItemBase item, ItemGridBase<ItemBase> grid, bool isSelected = false)
  {
    base.Setup(item, grid, isSelected);

    // 버튼 클릭 이벤트
    button.onClick.RemoveAllListeners();
    button.onClick.AddListener(() => {
      Debug.Log("Button actually clicked!..");
      OnClick();
    });
    Debug.Log("Button click listener added");

    /*// 디버그: 연결 확인
    Debug.Log($"Setup item: {item.name}");
    Debug.Log($"Icon RectTransform size: {icon.rectTransform.sizeDelta}");
    Debug.Log($"Icon sprite size: {(item.sprite != null ? item.sprite.rect.size.ToString() : "null")}");
    Debug.Log($"Icon color: {icon.color}");
    */
  }

  protected override void OnClick()
    {
        Debug.Log("MaterialItemUI OnClick called");
        base.OnClick();
    }
}
