using UnityEngine;
using System.Collections.Generic;

public class MaterialGrid : ItemGridBase<ItemBase>
{
    [SerializeField] private DyeData dyeData;
    [SerializeField] private SubMaterialData subMaterialData;
    
    protected override void LoadItems()
    {
        displayItems.Clear();
        
        // 염색약 로드
        for (int i = 0; i < PlayerData.Instance.dyesP.Length; i++)
        {
            var dyePlayerData = PlayerData.Instance.dyesP[i];
            if (dyePlayerData.unlocked && dyePlayerData.count > 0)
            {
                displayItems.Add(DyeItem.FromData(dyeData.dyes[i], dyePlayerData));
            }
        }
        
        // 부재료 로드
        for (int i = 0; i < PlayerData.Instance.subMaterialsP.Length; i++)
        {
            var subPlayerData = PlayerData.Instance.subMaterialsP[i];
            if (subPlayerData.unlocked && subPlayerData.count > 0)
            {
                displayItems.Add(SubMaterialItem.FromData(subMaterialData.subMaterials[i], subPlayerData));
            }
        }
    }
}
