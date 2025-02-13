using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class YarnPreview : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Image expectedYarnImage;
    [SerializeField] private Material colorChangerMaterial;
    
    [Header("Basic Yarn Sprites")]
    [SerializeField] private Sprite[] basicYarnSprites;

    [Header("Gem Yarn Sprites (분홍젬)")]
    [SerializeField] private Sprite pinkGemBasicSprite;
    [SerializeField] private Sprite pinkGemPearlSprite;
    [SerializeField] private Sprite pinkGemSparkleSprite;

    [Header("Gem Yarn Sprites (노란젬)")]
    [SerializeField] private Sprite yellowGemBasicSprite;
    [SerializeField] private Sprite yellowGemPearlSprite;
    [SerializeField] private Sprite yellowGemSparkleSprite;

    [Header("Gem Yarn Sprites (청록젬)")]
    [SerializeField] private Sprite greenGemBasicSprite;
    [SerializeField] private Sprite greenGemPearlSprite;
    [SerializeField] private Sprite greenGemSparkleSprite;

    private Material instancedMaterial;
    private YarnData currentYarnData;
    private System.Collections.Generic.List<DyeItem> selectedDyes = new System.Collections.Generic.List<DyeItem>();

    private void Awake()
    {
        instancedMaterial = new Material(colorChangerMaterial);
        expectedYarnImage.material = instancedMaterial;
        
        currentYarnData = new YarnData
        {
            mainType = "기본",
            subType = -1,
            color = Color.white
        };

        expectedYarnImage.sprite = basicYarnSprites[0];
    }

    public void UpdatePreview(GemItem selectedGem = null, SubMaterialItem selectedSubMaterial = null, System.Collections.Generic.List<DyeItem> dyes = null)
    {
        selectedDyes = dyes ?? new System.Collections.Generic.List<DyeItem>();

        // 1. 실 스프라이트 결정
        Sprite targetSprite = DetermineYarnSprite(selectedGem, selectedSubMaterial);
        expectedYarnImage.sprite = targetSprite;

        // 2. YarnData 정보 업데이트
        UpdateYarnData(selectedGem, selectedSubMaterial);

        // 3. 염색약 적용
        ApplyDyeColor();
    }

    private void ApplyDyeColor()
    {
        if (selectedDyes.Count == 0)
        {
            instancedMaterial.SetFloat("_UseSecondColor", 0);
            instancedMaterial.SetColor("_Color1", Color.clear);
            instancedMaterial.SetColor("_Color2", Color.clear);
        }
        else if (selectedDyes.Count == 1)
        {
            instancedMaterial.SetFloat("_UseSecondColor", 0);
            instancedMaterial.SetColor("_Color1", selectedDyes[0].color);
            instancedMaterial.SetFloat("_Color1SatMult", selectedDyes[0].saturationMultiplier);
            instancedMaterial.SetFloat("_Color1ValMult", selectedDyes[0].valueMultiplier);
            instancedMaterial.SetColor("_Color2", Color.clear);
        }
        else if (selectedDyes.Count == 2)
        {
            instancedMaterial.SetFloat("_UseSecondColor", 1);
            instancedMaterial.SetColor("_Color1", selectedDyes[0].color);
            instancedMaterial.SetFloat("_Color1SatMult", selectedDyes[0].saturationMultiplier);
            instancedMaterial.SetFloat("_Color1ValMult", selectedDyes[0].valueMultiplier);
            instancedMaterial.SetColor("_Color2", selectedDyes[1].color);
            instancedMaterial.SetFloat("_Color2SatMult", selectedDyes[1].saturationMultiplier);
            instancedMaterial.SetFloat("_Color2ValMult", selectedDyes[1].valueMultiplier);
        }
    }

    private Sprite DetermineYarnSprite(GemItem gem, SubMaterialItem subMaterial)
    {
        if (gem == null)
        {
            int subIndex = subMaterial != null ? subMaterial.index + 1 : 0;
            return basicYarnSprites[subIndex];
        }

        switch (gem.index)
        {
            case 0: // 분홍젬
                if (subMaterial == null) return pinkGemBasicSprite;
                return subMaterial.index == 0 ? pinkGemPearlSprite : pinkGemSparkleSprite;

            case 1: // 노란젬
                if (subMaterial == null) return yellowGemBasicSprite;
                return subMaterial.index == 0 ? yellowGemPearlSprite : yellowGemSparkleSprite;

            case 2: // 청록젬
                if (subMaterial == null) return greenGemBasicSprite;
                return subMaterial.index == 0 ? greenGemPearlSprite : greenGemSparkleSprite;

            default:
                return basicYarnSprites[0];
        }
    }

    private void UpdateYarnData(GemItem gem, SubMaterialItem subMaterial)
    {
        currentYarnData.mainType = gem != null ? gem.name : "기본";
        currentYarnData.subType = subMaterial != null ? subMaterial.index : -1;
        currentYarnData.color = selectedDyes.Count > 0 ? selectedDyes[0].color : Color.white;
    }
}