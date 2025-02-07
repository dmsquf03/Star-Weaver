using UnityEngine;

public class DyeItem : ItemBase
{
  public Color color;

  public static DyeItem FromData(DyeObjectData data, PlayerData.PlayerDyeData playerData)
  {
    return new DyeItem{
      index = data.index,
      sprite = data.sprite,
      name = data.name,
      unlocked = playerData.unlocked,
      count = playerData.count
    };
  }
}
