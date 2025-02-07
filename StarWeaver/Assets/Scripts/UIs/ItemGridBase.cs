using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
public abstract class ItemGridBase<T> : MonoBehaviour where T : ItemBase
{
    [SerializeField] protected Transform itemGrid;
    [SerializeField] protected GameObject itemPrefab;
    [SerializeField] protected Button nextButton;
    [SerializeField] protected Button prevButton;
    
    protected List<T> displayItems = new List<T>();
    protected int currentPage = 0;
    protected int itemsPerPage = 4;
    
    protected virtual void Start()
    {
        LoadItems();
        UpdateDisplay();
        UpdateNavigationButtons();

      if (nextButton) nextButton.onClick.AddListener(NextPage);
      if (prevButton) prevButton.onClick.AddListener(PrevPage);
    }

    protected abstract void LoadItems();
    
    protected virtual void UpdateDisplay()
    {
       foreach (Transform child in itemGrid) 
       {
           Destroy(child.gameObject);
       }
       
       int startIdx = currentPage * itemsPerPage;
       for (int i = 0; i < itemsPerPage && startIdx + i < displayItems.Count; i++)
       {
           GameObject obj = Instantiate(itemPrefab, itemGrid);
           var itemUI = obj.GetComponent<ItemUIBase<T>>();
           itemUI.Setup(displayItems[startIdx + i]);
       }
    }

    protected void NextPage()
   {
       if ((currentPage + 1) * itemsPerPage < displayItems.Count)
       {
           currentPage++;
           UpdateDisplay();
           UpdateNavigationButtons();
       }
   }
   
   protected void PrevPage()
   {
       if (currentPage > 0)
       {
           currentPage--;
           UpdateDisplay();
           UpdateNavigationButtons();
       }
   }
   
   protected void UpdateNavigationButtons()
   {
       if (prevButton) prevButton.interactable = currentPage > 0;
       if (nextButton) nextButton.interactable = (currentPage + 1) * itemsPerPage < displayItems.Count;
   }
}
