using UnityEngine;

public class DyeItem : ItemBase
{
  public Color color { get; set; }
  public float saturationMultiplier { get; set; }
  public float valueMultiplier { get; set; }

  public static DyeItem FromData(DyeObjectData data, PlayerData.PlayerDyeData playerData)
  {
    Color dyeColor = data.color;
    dyeColor.a = 1f;

    return new DyeItem{
      index = data.index,
      sprite = data.sprite,
      name = data.name,
      color = dyeColor,
      saturationMultiplier = data.saturationMultiplier,
      valueMultiplier = data.valueMultiplier,
      unlocked = playerData.unlocked,
      count = playerData.count
    };
  }
}
