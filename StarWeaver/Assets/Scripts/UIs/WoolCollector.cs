using UnityEngine;
using UnityEngine.UI;

public class WoolCollector : MonoBehaviour
{
  public Image gaugeImage; // 노란 원형 게이지
  public int clicksToFill = 30; // 양털 1개당 필요 클릭 횟수

  private void Start()
  {
    UpdateGauge();
  }
  public void OnBtnClicked()
    {
        if(GameManager.Instance.PlayerManager.CurrentClicks < clicksToFill)
        {
            GameManager.Instance.PlayerManager.AddClick();
            UpdateGauge();
        }
        if(GameManager.Instance.PlayerManager.CurrentClicks >= clicksToFill)
        {
            CollectWool();
        }
    }

    private void UpdateGauge()
    {
        gaugeImage.fillAmount = (float)GameManager.Instance.PlayerManager.CurrentClicks / clicksToFill;
    }

    private void CollectWool()
    {
        GameManager.Instance.PlayerManager.AddWool();
        GameManager.Instance.PlayerManager.ResetClicks();
        gaugeImage.fillAmount = 0;

        Debug.Log("양털 수: " + GameManager.Instance.PlayerManager.Wool);
    }
}
