using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RequiredMaterialSlot : MonoBehaviour
{
    [SerializeField] private Image itemIcon;
    [SerializeField] private Image plusIcon;
    [SerializeField] private TextMeshProUGUI requiredText;

    private ItemBase currentItem;
    private bool isPlaceholder = false;
    
    public void SetItem(ItemBase item, int requiredCount, int ownedCount)
    {
        if(item != null)
        {
            currentItem = item;
            itemIcon.sprite = item.sprite;
            itemIcon.gameObject.SetActive(true);
            plusIcon.gameObject.SetActive(false);
            requiredText.text = $"{ownedCount}/{requiredCount}";
            requiredText.gameObject.SetActive(true);
            isPlaceholder = false;
        }
    }

    public void SetAsPlaceholder()
    {
        itemIcon.gameObject.SetActive(false);
        plusIcon.gameObject.SetActive(true);
        requiredText.gameObject.SetActive(false);
        isPlaceholder = true;
    }
}
