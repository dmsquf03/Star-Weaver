using UnityEngine;
using System.Collections.Generic;

public class MaterialGrid : ItemGridBase<ItemBase>
{
    [SerializeField] private DyeData dyeData;
    [SerializeField] private SubMaterialData subMaterialData;
    
    protected override void LoadItems()
    {
        Debug.Log("Starting LoadItems...");
    
    if (itemGrid == null)
        Debug.LogError("Item Grid is null!");
    if (itemPrefab == null)
        Debug.LogError("Item Prefab is null!");
    if (nextButton == null)
        Debug.LogError("Next Button is null!");
    if (prevButton == null)
        Debug.LogError("Prev Button is null!");
    if (dyeData == null)
        Debug.LogError("DyeData is null!");
    if (subMaterialData == null)
        Debug.LogError("SubMaterialData is null!");
        
    displayItems.Clear();
    
    if (GameManager.Instance == null)
    {
        Debug.LogError("GameManager.Instance is null!");
        return;
    }
    
    if (GameManager.Instance.PlayerManager == null)
    {
        Debug.LogError("PlayerManager is null!");
        return;
    }
    
    if (GameManager.Instance.PlayerManager.PlayerData == null)
    {
        Debug.LogError("PlayerData is null!");
        return;
    }

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
}
