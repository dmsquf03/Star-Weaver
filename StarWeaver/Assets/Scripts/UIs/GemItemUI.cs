using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GemItemUI : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private Image checkMark;
    [SerializeField] private Button button;
    
    private GemItem item;
    private GemGrid grid;
    private bool isSelected;

    public void Setup(GemItem item, GemGrid grid, bool isSelected)
    {
        this.item = item;
        this.grid = grid;
        this.isSelected = isSelected;
        
        icon.sprite = item.sprite;
        checkMark.gameObject.SetActive(isSelected);
        
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(OnClick);

        UpdateVisual();
    }

    private void OnClick()
    {
        Debug.Log($"GemItemUI OnClick - Current state: {isSelected}");
        bool newState = !isSelected;
        if (grid.OnItemSelected(item, newState))
        {
            isSelected = newState;
            UpdateVisual();
        }
    }

    private void UpdateVisual()
    {
        checkMark.gameObject.SetActive(isSelected);
        Color color = icon.color;
        color.a = isSelected ? 0.6f : 1f;
        icon.color = color;
    }
}
