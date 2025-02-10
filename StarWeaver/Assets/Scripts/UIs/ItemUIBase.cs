using UnityEngine;
using UnityEngine.UI;
using TMPro;

public abstract class ItemUIBase<T> : MonoBehaviour where T : ItemBase
{
   [SerializeField] protected Image icon;
   [SerializeField] protected Image checkMark;
   [SerializeField] protected Button button;
   [SerializeField] protected TextMeshProUGUI countText; // 선택 사항
   protected T item;
   protected bool isSelected;
   protected ItemGridBase<T> grid;
   
   public virtual void Setup(T item, ItemGridBase<T> grid, bool isSelected = false)
   {
        this.item = item;
        this.grid = grid;
        this.isSelected = isSelected;
        icon.sprite = item.sprite;
        checkMark.gameObject.SetActive(false);

        if(countText != null)
        {
          countText.text = item.count.ToString("D2");
        }
        
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => {
            Debug.Log("Button clicked from base!");
            OnClick();
        });

        UpdateSelection(isSelected);
   }
   
   protected virtual void OnClick()
   {
        Debug.Log("OnClick called in base class");
        bool newState = !isSelected;
        if (grid.OnItemSelected(item, newState))
        {
            isSelected = newState;
            UpdateSelection(isSelected);
        }
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
