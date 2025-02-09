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

        if (nextButton != null)
        {
            Debug.Log("Next button found, adding listener");  // 디버그 로그 추가
            nextButton.onClick.RemoveAllListeners();  // 기존 리스너 제거
            nextButton.onClick.AddListener(() => {
                Debug.Log("Next button clicked - from listener");
                NextPage();
            });
        }
        else
        {
            Debug.LogError("Next button is null!");
        }
    
        if (prevButton != null)
        {
            Debug.Log("Prev button found, adding listener");  // 디버그 로그 추가
            prevButton.onClick.RemoveAllListeners();  // 기존 리스너 제거
            prevButton.onClick.AddListener(PrevPage);
        }
        else
        {
            Debug.LogError("Prev button is null!");
        }
    }

    protected abstract void LoadItems();
    
    protected virtual void UpdateDisplay()
    {
        if (itemGrid == null)
        Debug.LogError("itemGrid is null!");
        if (itemPrefab == null)
        Debug.LogError("itemPrefab is null!");

       foreach (Transform child in itemGrid) 
       {
           Destroy(child.gameObject);
       }
       
       int startIdx = currentPage * itemsPerPage;
       for (int i = 0; i < itemsPerPage && startIdx + i < displayItems.Count; i++)
       {
           GameObject obj = Instantiate(itemPrefab, itemGrid);
           if (obj == null)
                Debug.LogError("Failed to instantiate prefab");

           var itemUI = obj.GetComponent<ItemUIBase<T>>();
           if (itemUI == null)
                Debug.LogError("ItemUIBase component not found on instantiated prefab");

           itemUI.Setup(displayItems[startIdx + i]);
       }
    }

    protected void NextPage()
   {
        Debug.Log($"NextPage called. Current page: {currentPage}, Items per page: {itemsPerPage}, Total items: {displayItems.Count}");
        // 조건 검사 부분을 분리해서 로깅
        bool canMoveNext = (currentPage + 1) * itemsPerPage < displayItems.Count;
        Debug.Log($"Can move to next page? {canMoveNext}");
        Debug.Log($"Next page calculation: ({currentPage} + 1) * {itemsPerPage} < {displayItems.Count}");
        
        if (canMoveNext)
        {
           currentPage++;
           Debug.Log($"Moving to page {currentPage}");
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
        if (nextButton != null)
        {
            nextButton.interactable = (currentPage + 1) * itemsPerPage < displayItems.Count;
            Debug.Log($"Next button interactable: {nextButton.interactable}");
        }
        if (prevButton != null)
        {
            prevButton.interactable = currentPage > 0;
            Debug.Log($"Prev button interactable: {prevButton.interactable}");
        }
    }
}
