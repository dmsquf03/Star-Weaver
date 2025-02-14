using UnityEngine;

public class GemItem : ItemBase
{
    public string yarnType {get; private set;}
    public static GemItem FromData(GemObjectData data, bool unlocked)
    {
        return new GemItem
        {
            index = data.index,
            sprite = data.sprite,
            name = data.name,
            yarnType = data.yarnType,
            unlocked = unlocked,
            count = unlocked ? 1 : 0
        };
    }
}
