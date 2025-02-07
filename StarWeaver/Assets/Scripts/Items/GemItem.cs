using UnityEngine;

public class GemItem : ItemBase
{
    public static GemItem FromData(GemObjectData data, bool unlocked)
    {
        return new GemItem
        {
            index = data.index,
            sprite = data.sprite,
            name = data.name,
            unlocked = unlocked,
            count = unlocked ? 1 : 0
        };
    }
}
