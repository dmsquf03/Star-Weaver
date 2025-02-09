using UnityEngine;
using UnityEngine.UI;
using TMPro;

public abstract class ItemUIBase<T> : MonoBehaviour where T : ItemBase
{
   [SerializeField] protected Image icon;
   [SerializeField] protected Image checkMark;
   [SerializeField] protected Button button;
   [SerializeField] protected TextMeshProUGUI countText;
   protected T item;
   
   public virtual void Setup(T item)
   {
        this.item = item;
        icon.sprite = item.sprite;
        checkMark.gameObject.SetActive(false);
        countText.text = item.count.ToString("D2");
       
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => {
            Debug.Log("Button clicked from base!");
            OnClick();
        });
   }
   
   protected virtual void OnClick()
   {
        Debug.Log("OnClick called in base class");
        bool isSelected = !checkMark.gameObject.activeSelf;
        UpdateSelection(isSelected);
   }
   
   protected virtual void UpdateSelection(bool isSelected)
   {    
        Debug.Log($"Updating selection: {isSelected}");
        checkMark.gameObject.SetActive(isSelected);
        Color color = icon.color;
        color.a = isSelected ? 0.6f : 1f;
        icon.color = color;
   }
}
