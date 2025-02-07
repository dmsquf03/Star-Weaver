using UnityEngine;

public class GemGrid : ItemGridBase<GemItem>
{
    [SerializeField] private GemData gemData;
    
    protected override void LoadItems()
    {
        displayItems.Clear();
        for (int i = 0; i < PlayerData.Instance.gemsP.Length; i++)
        {
            if (PlayerData.Instance.gemsP[i])
            {
                displayItems.Add(GemItem.FromData(gemData.gems[i], true));
            }
        }
    }
}
