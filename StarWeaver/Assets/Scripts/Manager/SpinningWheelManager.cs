using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class SpinningWheelManager : MonoBehaviour
{
    [Header("Slots")]
    [SerializeField] private RequiredMaterialSlot[] materialSlots = new RequiredMaterialSlot[4];
    
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI expectedTimeText;
    [SerializeField] private TextMeshProUGUI quantityText;
    [SerializeField] private RectTransform itemInUI;
    [SerializeField] private RectTransform itemInBorder;
    [SerializeField] private Button decreaseButton;
    [SerializeField] private Button increaseButton;

    [Header("Preview")]
    [SerializeField] private YarnPreview yarnPreview;

    [Header("Layout Settings")]
    [SerializeField] private float slotWidth = 100f;
    [SerializeField] private float slotSpacing = 10f;
    [SerializeField] private float uiPadding = 20f;  // UI 좌우 여백

    [Header("Resources")]
    [SerializeField] private Sprite woolSprite;

    private WoolItem woolItem;
    private List<ItemBase> selectedMaterials = new List<ItemBase>();
    private GemItem selectedGem = null;  // 선택된 젬 별도 관리
    private int currentYarnQuantity = 0;

    [Header("Item Description PopUp")]
    [SerializeField] private GameObject itemDescriptionPopUp;
    [SerializeField] private Image itemIconImage;
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private Button expectedYarnButton;
    [SerializeField] private TextMeshProUGUI inventoryText;

    // Constants
    private const int MAX_SLOTS = 4;
    private const int MAX_YARN_QUANTITY = 5;
    private const int BASE_TIME = 20;
    private const int DYE_TIME = 15;
    private const int SUBMATERIAL_TIME = 25;
    private const float TIME_REDUCTION_PER_COUNT = 0.05f;

    private void Start()
    {
        InitializeWoolItem();
        InitializeSlots();
        
        // 수량 조절 버튼 이벤트 연결
        if (increaseButton != null)
            increaseButton.onClick.AddListener(IncreaseQuantity);
        if (decreaseButton != null)
            decreaseButton.onClick.AddListener(DecreaseQuantity);

        // Expected Yarn 버튼 이벤트 연결
        if (expectedYarnButton != null)
            expectedYarnButton.onClick.AddListener(ShowYarnDescription);
            
        UpdateUI();
    }

    private void InitializeWoolItem()
    {
        woolItem = new WoolItem
        {
            name = "양털",
            sprite = woolSprite,
            count = GameManager.Instance.PlayerManager.PlayerData.wool
        };
    }

    private void InitializeSlots()
    {
        // 첫 번째 슬롯(양털)과 두 번째 슬롯(+) 활성화
        materialSlots[0].gameObject.SetActive(true);
        materialSlots[1].gameObject.SetActive(true);
        
        UpdateWoolSlot();
        materialSlots[1].SetAsPlaceholder();
        
        // 나머지 슬롯 비활성화
        for (int i = 2; i < materialSlots.Length; i++)
        {
            materialSlots[i].gameObject.SetActive(false);
        }
    }

    public void SetSelectedGem(GemItem gem)
    {
        selectedGem = gem;
        UpdateUI();
    }

    private void UpdateWoolSlot()
    {
        int woolRequired = 5 * currentYarnQuantity;
        int woolOwned = GameManager.Instance.PlayerManager.PlayerData.wool;
        woolItem.count = woolOwned;
        materialSlots[0].SetItem(woolItem, woolRequired, woolOwned);
    }

    private void UpdateUI()
    {
        quantityText.text = currentYarnQuantity.ToString();
        UpdateRequiredCounts();
        UpdateExpectedTime();
        UpdateSlotsPosition();
        UpdateUISize();
        UpdateQuantityButtons();
        UpdateYarnPreview();
    }

    private void UpdateRequiredCounts()
    {
        // 양털 슬롯은 항상 업데이트
        UpdateWoolSlot();

        // 선택된 재료 슬롯 업데이트
        for (int i = 0; i < MAX_SLOTS - 1; i++)
        {
            if (i < selectedMaterials.Count)
            {
                // 선택된 아이템이 있는 경우
                var item = selectedMaterials[i];
                int required = GetRequiredCountForItem(item) * currentYarnQuantity;
                int owned = GetOwnedCountForItem(item);
                materialSlots[i + 1].gameObject.SetActive(true);
                materialSlots[i + 1].SetItem(item, required, owned);
            }
            else if (i == selectedMaterials.Count && selectedMaterials.Count < MAX_SLOTS - 1)
            {
                // 다음 빈 슬롯은 + 아이콘으로
                materialSlots[i + 1].gameObject.SetActive(true);
                materialSlots[i + 1].SetAsPlaceholder();
            }
            else
            {
                // 나머지 슬롯은 비활성화
                materialSlots[i + 1].gameObject.SetActive(false);
            }
        }
    }

    private void UpdateYarnPreview()
    {
        SubMaterialItem selectedSubMaterial = null;
        List<DyeItem> selectedDyes = new List<DyeItem>();

        foreach (var material in selectedMaterials)
        {
            if (material is SubMaterialItem sub)
                selectedSubMaterial = sub;
            else if (material is DyeItem dye)
                selectedDyes.Add(dye);
        }

        yarnPreview.UpdatePreview(selectedGem, selectedSubMaterial, selectedDyes);
    }

    private void UpdateSlotsPosition()
    {
        int activeSlots = GetActiveSlotCount();
        float totalWidth = (slotWidth * activeSlots) + (slotSpacing * (activeSlots - 1));
        float startX = -totalWidth / 2f;

        // 활성화된 모든 슬롯의 위치 조정
        int currentIndex = 0;
        for (int i = 0; i < materialSlots.Length; i++)
        {
            if (materialSlots[i].gameObject.activeSelf)
            {
                float xPos = startX + (currentIndex * (slotWidth + slotSpacing)) + (slotWidth / 2f);
                materialSlots[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(xPos, 0);
                currentIndex++;
            }
        }
    }

    private void UpdateUISize()
    {
        int activeSlots = GetActiveSlotCount();
        float contentWidth = (slotWidth * activeSlots) + (slotSpacing * (activeSlots - 1));
        float totalWidth = contentWidth + (uiPadding * 2);

        itemInUI.sizeDelta = new Vector2(totalWidth, itemInUI.sizeDelta.y);
        itemInBorder.sizeDelta = new Vector2(totalWidth, itemInBorder.sizeDelta.y);
    }

    private int GetActiveSlotCount()
    {
        return 1 + selectedMaterials.Count + (selectedMaterials.Count < MAX_SLOTS - 1 ? 1 : 0);
    }

    public void IncreaseQuantity()
    {
        if (currentYarnQuantity < MAX_YARN_QUANTITY)
        {
            currentYarnQuantity++;
            UpdateUI();
        }
    }

    public void DecreaseQuantity()
    {
        if (currentYarnQuantity > 0)
        {
            currentYarnQuantity--;
            UpdateUI();
        }
    }

    public bool CanAddMaterial(ItemBase item)
    {
        if (selectedMaterials.Count >= (MAX_SLOTS - 1)) return false;
        
        if (item is DyeItem)
        {
            int dyeCount = selectedMaterials.Count(x => x is DyeItem);
            return dyeCount < 2;
        }
        else if (item is SubMaterialItem)
        {
            return !selectedMaterials.Any(x => x is SubMaterialItem);
        }
        
        return false;
    }

    public bool OnItemSelected(ItemBase item, bool isSelected)
    {
        if (isSelected)
        {
            if (!CanAddMaterial(item)) return false;
            selectedMaterials.Add(item);
        }
        else
        {
            selectedMaterials.Remove(item);
        }

        UpdateUI();
        return true;
    }

    private int GetRequiredCountForItem(ItemBase item)
    {
        if (item is DyeItem)
        {
            return item.index > 2 ? 2 : 1;
        }
        else if (item is SubMaterialItem)
        {
            return 1;
        }
        return 0;
    }

    private int GetOwnedCountForItem(ItemBase item)
    {
        return item.count;
    }

    public float CalculateTotalTime(int yarnCount)
    {
        float baseTime = BASE_TIME;
        foreach (var item in selectedMaterials)
        {
            if (item is DyeItem)
                baseTime += DYE_TIME;
            else if (item is SubMaterialItem)
                baseTime += SUBMATERIAL_TIME;
        }

        float totalTime = baseTime * yarnCount;
        float reduction = 1f - (TIME_REDUCTION_PER_COUNT * (yarnCount - 1));
        return totalTime * reduction;
    }

    private void UpdateExpectedTime()
    {
        float totalSeconds = CalculateTotalTime(currentYarnQuantity);
        int minutes = Mathf.FloorToInt(totalSeconds / 60);
        int seconds = Mathf.FloorToInt(totalSeconds % 60);
        expectedTimeText.text = string.Format("{0}분 {1:D2}초", minutes, seconds);
    }

    private void UpdateQuantityButtons()
    {
        // 감소 버튼은 현재 수량이 1일 때 비활성화
        decreaseButton.interactable = currentYarnQuantity > 0;

        // 증가 버튼은 최대 수량이거나 재료가 부족할 때 비활성화
        bool canIncrease = currentYarnQuantity < MAX_YARN_QUANTITY && HasEnoughMaterialsForNextQuantity();
        increaseButton.interactable = canIncrease;
    }

    private bool HasEnoughMaterialsForNextQuantity()
    {
        int nextQuantity = currentYarnQuantity + 1;

        // 양털 체크
        int woolRequired = 5 * nextQuantity;
        woolItem.count = GameManager.Instance.PlayerManager.PlayerData.wool;
        if (woolItem.count < woolRequired)
        {
            Debug.Log($"Not enough wool. Required: {woolRequired}, Have: {woolItem.count}");
            return false;
        }

        // 선택된 재료들 체크
        foreach (var material in selectedMaterials)
        {
            int required = GetRequiredCountForItem(material) * nextQuantity;
            if (material is DyeItem dye)
            {
                material.count = GameManager.Instance.PlayerManager.PlayerData.dyesP[dye.index].count;
            }
            else if (material is SubMaterialItem sub)
            {
                material.count = GameManager.Instance.PlayerManager.PlayerData.subMaterialsP[sub.index].count;
            }

            if (material.count < required)
            {
                Debug.Log($"Not enough {material.name}. Required: {required}, Have: {material.count}");
                return false;
            }
        }

        return true;
    }

    // 실 설명창
    private void ShowYarnDescription()
    {
        if (itemDescriptionPopUp != null && itemIconImage != null && itemNameText != null)
        {
            // PopUp 활성화
            itemDescriptionPopUp.SetActive(true);

            // 아이콘 이미지, Material 설정
            itemIconImage.sprite = yarnPreview.GetCurrentYarnSprite();
            itemIconImage.material = yarnPreview.GetCurrentMaterial();
            
            // 이름 텍스트 설정
            string yarnName = yarnPreview.GetYarnName();
            itemNameText.text = yarnName;

            // 실 보유 수량 설정
            int count = GetInventoryCount(yarnName);
            inventoryText.text = count.ToString("D2");
        }
    }

    // 실 설명창 - 보유 수량 가져오기
    private int GetInventoryCount(string yarnName)
    {
        var yarns = GameManager.Instance.PlayerManager.PlayerData.yarns;
        var matchingYarn = yarns.Find(yarn => yarn.name == yarnName);
        return matchingYarn?.count ?? 0;
    }
}