using UnityEngine;

public class SubMaterialItem : ItemBase
{
  public string description;

  public static SubMaterialItem FromData(SubMaterialObjectData data, PlayerData.PlayerSubMaterialData playerData)
  {
    return new SubMaterialItem
    {
      index = data.index,
      sprite = data.sprite,
      name = data.name,
      description = data.description,
      unlocked = playerData.unlocked,
      count = playerData.count
    };
  }
}
