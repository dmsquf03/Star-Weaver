using UnityEngine;
using UnityEngine.UI;

public abstract class ItemUIBase<T> : MonoBehaviour where T : ItemBase
{
   [SerializeField] protected Image icon;
   [SerializeField] protected Image checkMark;
   [SerializeField] protected Button button;
   protected T item;
   
   public virtual void Setup(T item)
   {
       this.item = item;
       icon.sprite = item.sprite;
       checkMark.gameObject.SetActive(false);
       button.onClick.AddListener(OnClick);
   }
   
   protected virtual void OnClick()
   {
       bool isSelected = !checkMark.gameObject.activeSelf;
       UpdateSelection(isSelected);
   }
   
   protected virtual void UpdateSelection(bool isSelected)
   {
       checkMark.gameObject.SetActive(isSelected);
       Color color = icon.color;
       color.a = isSelected ? 0.6f : 1f;
       icon.color = color;
   }
}
