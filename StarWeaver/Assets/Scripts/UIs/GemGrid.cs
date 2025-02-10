using UnityEngine;

public class GemGrid : ItemGridBase<GemItem>
{
    [SerializeField] private GemData gemData;
    private GemItem selectedGem = null;
    
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

    public override bool OnItemSelected(GemItem item, bool isSelected)
    {
        if (isSelected)
        {
            if (selectedGem != null)
            {
                Debug.Log("Cannot select more than 1 gem");
                return false;
            }
            selectedGem = item;
        }
        else
        {
            selectedGem = null;
        }
        return true;
    }

    public override void ResetSelection()
    {
        selectedGem = null;
        UpdateDisplay();
    }
}
