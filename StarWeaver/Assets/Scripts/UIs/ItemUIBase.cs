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
   protected bool isSelected;
   protected MaterialGrid materialGrid;
   
   public virtual void Setup(T item, MaterialGrid grid, bool isSelected = false)
   {
        this.item = item;
        this.materialGrid = grid;
        this.isSelected = isSelected;
        icon.sprite = item.sprite;
        checkMark.gameObject.SetActive(false);
        countText.text = item.count.ToString("D2");
       
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
        if (materialGrid.OnItemSelected(item, newState))
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
