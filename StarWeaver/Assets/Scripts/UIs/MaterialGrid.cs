using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class MaterialGrid : ItemGridBase<ItemBase>
{
    [SerializeField] private DyeData dyeData;
    [SerializeField] private SubMaterialData subMaterialData;
    [SerializeField] private SpinningWheelManager spinningWheelManager;

    private const int MAX_DYE_SELECTION = 2;
    private const int MAX_SUBMATERIAL_SELECTION = 1;
    
    // 외부에서 Material Grid 업데이트하기 위한 메서드
    public void RefreshItems()
    {
        LoadItems();
        UpdateDisplay();
    }
    protected override void LoadItems()
    {
        displayItems.Clear();
        
        // 염색약 로드
        var playerDyeData = GameManager.Instance.PlayerManager.PlayerData.dyesP;
        for (int i = 0; i < playerDyeData.Length; i++)
        {  
            var dyePlayerData = playerDyeData[i];
            if (dyePlayerData != null && dyePlayerData.unlocked && dyePlayerData.count > 0)
            {
                displayItems.Add(DyeItem.FromData(dyeData.dyes[i], dyePlayerData));
            }
        }
    
        // 부재료 로드
        var playerSubData = GameManager.Instance.PlayerManager.PlayerData.subMaterialsP;
        for (int i = 0; i < playerSubData.Length; i++)
        {
            var subPlayerData = playerSubData[i];
            if (subPlayerData != null && subPlayerData.unlocked && subPlayerData.count > 0)
            {
                displayItems.Add(SubMaterialItem.FromData(subMaterialData.subMaterials[i], subPlayerData));
            }
        }
    }

    public override bool OnItemSelected(ItemBase item, bool isSelected)
    {
        if (spinningWheelManager == null)
        {
            Debug.LogError("SpinningWheelManager reference is missing!");
            return false;
        }

        if (isSelected)
        {
            // 선택 제한 체크
            if (item is DyeItem)
            {
                int currentDyeCount = selectedItems.Count(x => x is DyeItem);
                if (currentDyeCount >= MAX_DYE_SELECTION)
                {
                    Debug.Log($"Cannot select more dyes. Current: {currentDyeCount}, Max: {MAX_DYE_SELECTION}");
                    return false;
                }
            }
            else if (item is SubMaterialItem)
            {
                if (selectedItems.Any(x => x is SubMaterialItem))
                {
                    Debug.Log("Cannot select more submaterials");
                    return false;
                }
            }

            // SpinningWheelManager에 추가
            if (!spinningWheelManager.OnItemSelected(item, true)) return false;
            
            selectedItems.Add(item);
        }
        else
        {
            selectedItems.Remove(item);
            spinningWheelManager.OnItemSelected(item, false);
        }

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
            var itemUI = obj.GetComponent<ItemUIBase<ItemBase>>();
            var item = displayItems[startIdx + i];
            bool isSelected = selectedItems.Contains(item);
            itemUI.Setup(item, this, isSelected);
        }
    }

    public override void ResetSelection()
    {
        selectedItems.Clear();
        UpdateDisplay();
    }
}