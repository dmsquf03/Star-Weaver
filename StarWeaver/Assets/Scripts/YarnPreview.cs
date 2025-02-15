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
    private List<DyeItem> selectedDyes = new List<DyeItem>();
    private SubMaterialItem currentSubMaterial;
    private GemItem currentGem;

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

    public void UpdatePreview(GemItem selectedGem = null, SubMaterialItem selectedSubMaterial = null, List<DyeItem> dyes = null)
    {
        selectedDyes = dyes ?? new List<DyeItem>();
        currentSubMaterial = selectedSubMaterial;
        currentGem = selectedGem;

        // 1. 실 스프라이트 결정
        Sprite targetSprite = DetermineYarnSprite(selectedGem, selectedSubMaterial);
        expectedYarnImage.sprite = targetSprite;

        // 2. YarnData 정보 업데이트
        UpdateYarnData(selectedGem, selectedSubMaterial);

        // 3. 염색약 적용
        ApplyDyeColor();
    }

    public string GetYarnName()
    {
        string name = "";

        // 1. 부재료 description
        if (currentSubMaterial != null)
        {
            name += currentSubMaterial.description + " ";
        }

        // 2. 염색약 색상
        if (selectedDyes.Count > 0)
        {
            if (selectedDyes.Count == 1)
            {
                name += GetColorName(selectedDyes[0]) + "색 ";
            }
            else if (selectedDyes.Count == 2)
            {
                name +=  GetMixedColorName(selectedDyes[0], selectedDyes[1]) + "색 ";
            }
        }

        // 3. 젬에 따른 실 종류
        if (currentGem != null)
        {
            name += currentGem.yarnType + " ";
        }

        // 기본 실
        if (string.IsNullOrEmpty(name.Trim()))
        {
            return "양털 실";
        }

        // 4. "실" 추가
        name += "실";

        return name;
    }

    private string GetColorName(DyeItem dye)
    {
        return dye.name.Replace("색 염색약", "");
    }

    // 염색약 조합에 따른 이름
    private string GetMixedColorName(DyeItem dye1, DyeItem dye2)
    {
        // 빨강 + 노랑 = 주황
        if ((GetColorName(dye1) == "빨간" && GetColorName(dye2) == "노란") ||
            (GetColorName(dye1) == "노란" && GetColorName(dye2) == "빨간"))
        {
            return "주황";
        }
        // 빨강 + 파랑 = 보라
        if ((GetColorName(dye1) == "빨간" && GetColorName(dye2) == "파란") ||
            (GetColorName(dye1) == "파란" && GetColorName(dye2) == "빨간"))
        {
            return "보라";
        }
        // 빨강 + 하양 = 분홍
        if ((GetColorName(dye1) == "빨간" && GetColorName(dye2) == "하얀") ||
            (GetColorName(dye1) == "하얀" && GetColorName(dye2) == "빨간"))
        {
            return "분홍";
        }
        // 빨강 + 검정 = 와인색
        if ((GetColorName(dye1) == "빨간" && GetColorName(dye2) == "검은") ||
            (GetColorName(dye1) == "검은" && GetColorName(dye2) == "빨간"))
        {
            return "와인";
        }
        
        // 노랑 + 파랑 = 초록
        if ((GetColorName(dye1) == "노란" && GetColorName(dye2) == "파란") ||
            (GetColorName(dye1) == "파란" && GetColorName(dye2) == "노란"))
        {
            return "초록";
        }
        // 노랑 + 하양 = 크림
        if ((GetColorName(dye1) == "노란" && GetColorName(dye2) == "하얀") ||
            (GetColorName(dye1) == "하얀" && GetColorName(dye2) == "노란"))
        {
            return "크림";
        }
        // 노랑 + 검정 = 올리브
        if ((GetColorName(dye1) == "노란" && GetColorName(dye2) == "검은") ||
            (GetColorName(dye1) == "검은" && GetColorName(dye2) == "노란"))
        {
            return "올리브";
        }

        // 파랑 + 하양 = 하늘
        if ((GetColorName(dye1) == "파란" && GetColorName(dye2) == "하얀") ||
            (GetColorName(dye1) == "하얀" && GetColorName(dye2) == "파란"))
        {
            return "하늘";
        }
        // 파랑 + 검정 = 남색
        if ((GetColorName(dye1) == "파란" && GetColorName(dye2) == "검은") ||
            (GetColorName(dye1) == "검은" && GetColorName(dye2) == "파란"))
        {
            return "남";
        }

        // 하양 + 검정 = 회색
        if ((GetColorName(dye1) == "하얀" && GetColorName(dye2) == "검은") ||
            (GetColorName(dye1) == "검은" && GetColorName(dye2) == "하얀"))
        {
            return "회";
        }

        // 기타 조합의 경우 첫 번째 색상으로 표시
        return GetColorName(dye1);
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

    // 현재 스프라이트 반환
    public Sprite GetCurrentYarnSprite()
    {
        return expectedYarnImage.sprite;
    }

    // 현재 Material 반환
    public Material GetCurrentMaterial()
    {
        return instancedMaterial;
    }
}