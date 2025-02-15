using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpinningWheelProcess : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image progressImage;        // 원형 게이지 바
    [SerializeField] private Image yarnImage;           // 중앙 실 이미지
    [SerializeField] private TextMeshProUGUI timerText; // 남은 시간 표시
    [SerializeField] private TextMeshProUGUI quantityText; // 설정된 제작 수량
    [SerializeField] private Button determineButton;    // 결정 버튼
    [SerializeField] private Button obtainButton;       // 획득 버튼
    [SerializeField] private GameObject dontTouchPopUp; // 클릭 금지 팝업

    [Header("Obtain Animation")]
    [SerializeField] private ObtainAnim obtainAnim;

    [Header("References")]
    [SerializeField] private SpinningWheelManager spinningWheelManager;
    [SerializeField] private WheelAnimatonController wheelManager;

    private float totalTime;
    private float remainingTime;
    private string yarnName;
    private int quantity;
    private bool isProcessing;

    public void StartProcess(Sprite yarnSprite, Material yarnMaterial, float processTime, string name, int count)
    {
        // UI 설정
        yarnImage.sprite = yarnSprite;
        yarnImage.material = yarnMaterial;
        
        // 데이터 설정
        totalTime = processTime;
        remainingTime = processTime;
        yarnName = name;
        quantity = count;
        isProcessing = true;

        // 버튼 상태 초기화
        if (obtainButton != null)
        {
          obtainButton.gameObject.SetActive(true);
          obtainButton.interactable = false;
        }
        if (determineButton != null) determineButton.gameObject.SetActive(false);

        // 제작 수량 표시
        if (quantityText != null)
        {
          quantityText.text = quantity.ToString();
        }

        // 초기 UI 상태
        progressImage.fillAmount = 0f;  // 시작은 빈 상태
    }

    private void Update()
    {
        if (!isProcessing) return;

        remainingTime -= Time.deltaTime;
        float progress = 1f - (remainingTime / totalTime);
        
        if (remainingTime <= 0)
        {
            CompleteProcess();
        }
        else
        {
            UpdateProgressUI(progress);
            UpdateTimerText();
        }
    }

    private void UpdateProgressUI(float progress)
    {
        progressImage.fillAmount = progress;
    }

    private void UpdateTimerText()
    {
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(remainingTime / 60f);
            int seconds = Mathf.FloorToInt(remainingTime % 60f);
            timerText.text = string.Format("{0:0}분 {1:00}초", minutes, seconds);
        }
    }

    private void CompleteProcess()
    {
        isProcessing = false;
        progressImage.fillAmount = 1f;

        // 버튼 상태 변경
        if (obtainButton != null) 
        {
          obtainButton.interactable = true;
        }

        if (timerText != null)
        {
          timerText.text = "0분 00초";
        }

        // Wheel 애니메이션 변경
        wheelManager.StopProcessing();
    }

    public void OnObtainButtonClick()
    {
        // 즉시 버튼 비활성화
        if (obtainButton != null)
        {
            obtainButton.interactable = false;
        }
        
        // 실을 인벤토리에 추가
        var yarn = new YarnData
        {
            name = yarnName,
            count = quantity
        };

        // 획득 애니메이션 재생
        if (obtainAnim != null)
        {
            obtainAnim.PlayAnim(yarnImage.sprite, yarnImage.material, quantity);
        }

        // 기존 실 존재 여부 확인
        var existingYarn = GameManager.Instance.PlayerManager.PlayerData.yarns
            .Find(y => y.name == yarnName);

        if (existingYarn != null)
        {
            existingYarn.count += quantity;
        }
        else
        {
            GameManager.Instance.PlayerManager.PlayerData.yarns.Add(yarn);
        }

        // 애니메이션 끝난 후, 상태 및 UI 초기화
        Invoke("ResetUIState", obtainAnim.fadeTime);
    }

    private void ResetUIState()
    {
        // 상태 초기화 (Process UI 비활성화)
        gameObject.SetActive(false);

        // 전체 UI 초기화
        if (spinningWheelManager != null)
        {
            spinningWheelManager.ResetToInitialState();
        }
    }
}