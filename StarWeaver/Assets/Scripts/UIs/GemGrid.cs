using UnityEngine;

public class GemGrid : ItemGridBase<GemItem>
{
    [SerializeField] private GemData gemData;
    [SerializeField] private SpinningWheelManager spinningWheelManager;
    private GemItem selectedGem = null;
    
    protected override void Start()
    {
        base.Start();
        selectedGem = null;
        
        if (spinningWheelManager == null)
        {
            Debug.LogError("SpinningWheelManager is not assigned!");
        }
    }

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
        if (spinningWheelManager == null) return false;

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
            if (selectedGem == item)
            {
                selectedGem = null;
            }
            else
            {
                return false; // 선택되지 않은 아이템을 해제하려고 할 때
            }
        }

        // SpinningWheelManager에 알림
        spinningWheelManager.SetSelectedGem(selectedGem);
        
        UpdateDisplay();
        return true;
    }

    protected override void UpdateDisplay()
    {
        foreach (Transform child in itemGrid) 
        {
            Destroy(child.gameObject);
        }
        
        int startIdx = currentPage * itemsPerPage;
        for (int i = 0; i < itemsPerPage && startIdx + i < displayItems.Count; i++)
        {
            GameObject obj = Instantiate(itemPrefab, itemGrid);
            var itemUI = obj.GetComponent<GemItemUI>();  // GemItemUI로 변경
            var item = displayItems[startIdx + i];
            
            // 현재 선택된 젬과 같은지 확인
            bool isSelected = (selectedGem != null && selectedGem.index == item.index);
            itemUI.Setup(item, this, isSelected);
        }
    }

    public override void ResetSelection()
    {
        selectedGem = null;
        if (spinningWheelManager != null)
        {
            spinningWheelManager.SetSelectedGem(null);
        }
        UpdateDisplay();
    }
}