using UnityEngine;
using TMPro;

// 재화 표시
public class MoneyDisplay : MonoBehaviour
{
    private TextMeshProUGUI moneyText;

    private void Start()
    {
        moneyText = GetComponent<TextMeshProUGUI>();
        UpdateMoneyText();
    }

    private void Update()
    {
        UpdateMoneyText();
    }

    private void UpdateMoneyText()
    {
        if (GameManager.Instance != null && moneyText != null)
        {
            moneyText.text = GameManager.Instance.PlayerManager.Money.ToString("N0");  // N0 포맷은 천 단위 구분자를 추가합니다
        }
    }
}
