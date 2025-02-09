using UnityEngine;

public class GemGrid : ItemGridBase<GemItem>
{
    [SerializeField] private GemData gemData;
    
    protected override void LoadItems()
    {
        displayItems.Clear();

        var playerGemData = GameManager.Instance.PlayerManager.PlayerData.gemsP;
        for (int i = 0; i < playerGemData.Length; i++)
        {
            var gemPlayerData = playerGemData[i];
            if (gemPlayerData)
            {
                displayItems.Add(GemItem.FromData(gemData.gems[i], true));
            }
        }
    }
}
