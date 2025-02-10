using UnityEngine;
using System.Collections.Generic;

public class MaterialGrid : ItemGridBase<ItemBase>
{
    [SerializeField] private DyeData dyeData;
    [SerializeField] private SubMaterialData subMaterialData;

    private const int MAX_DYE_SELECTION = 2;
    public const int MAX_SUBMATERIAL_SELECTION = 1;
    private List<DyeItem> selectedDyes = new List<DyeItem>();
    private SubMaterialItem selectedSubMaterial = null;
    
    protected override void LoadItems()
    {

        displayItems.Clear();
        
        // 염색약 로드
        var playerDyeData = GameManager.Instance.PlayerManager.PlayerData.dyesP;
        Debug.Log($"Total dyes in player data: {playerDyeData.Length}");

        for (int i = 0; i < playerDyeData.Length; i++)
        {  
            var dyePlayerData = playerDyeData[i];
            Debug.Log($"Checking dye {i}: unlocked={dyePlayerData?.unlocked}, count={dyePlayerData?.count}");

            if (dyePlayerData != null && dyePlayerData.unlocked && dyePlayerData.count > 0)  // null 체크 추가
            {
                Debug.Log($"Adding dye {i} to display items");
                displayItems.Add(DyeItem.FromData(dyeData.dyes[i], dyePlayerData));
            }
        }
    
        // 부재료 로드
        var playerSubData = GameManager.Instance.PlayerManager.PlayerData.subMaterialsP;
        for (int i = 0; i < playerSubData.Length; i++)
        {
            var subPlayerData = playerSubData[i];
            if (subPlayerData != null && subPlayerData.unlocked && subPlayerData.count > 0)  // null 체크 추가
            {
                displayItems.Add(SubMaterialItem.FromData(subMaterialData.subMaterials[i], subPlayerData));
            }
        }

        Debug.Log($"Loaded items: Dyes={displayItems.Count}");  // 디버그 로그 추가
    }

    public override bool OnItemSelected(ItemBase item, bool isSelected)
    {
        if (item is DyeItem dyeItem)
        {
            // 염색약 선택 로직
            if (isSelected)
            {
                if (selectedDyes.Count >= MAX_DYE_SELECTION)
                {
                    Debug.Log($"Cannot select more than {MAX_DYE_SELECTION} dyes");
                    return false;
                }
                selectedDyes.Add(dyeItem);
            }
            else
            {
                selectedDyes.Remove(dyeItem);
            }
        }
        else if (item is SubMaterialItem subItem)
        {
            // 부재료 선택 로직
            if (isSelected)
            {
                if (selectedSubMaterial != null)
                {
                    Debug.Log("Cannot select more than 1 sub material");
                    return false;
                }
                selectedSubMaterial = subItem;
            }
            else
            {
                selectedSubMaterial = null;
            }
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
            
            // 선택 상태 확인
            bool isSelected = false;
            if (item is DyeItem dyeItem)
                isSelected = selectedDyes.Contains(dyeItem);
            else if (item is SubMaterialItem subItem)
                isSelected = selectedSubMaterial == subItem;
                
            itemUI.Setup(item, this, isSelected);  // 선택 상태 전달
        }
    }

    public override void ResetSelection()
    {
        selectedDyes.Clear();
        selectedSubMaterial = null;
        UpdateDisplay();
    }
}
