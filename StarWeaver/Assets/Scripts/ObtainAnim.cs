using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ObtainAnim : MonoBehaviour
{
    [Header("Animation Settings")]
    public float fadeTime = 1f;
    [SerializeField] private float moveDistance = 20f;

    [Header("UI References")]
    [SerializeField] private Image yarnImage;
    [SerializeField] private TextMeshProUGUI numText;
    [SerializeField] private CanvasGroup canvasGroup;

    private float time;
    private RectTransform rectTransform;
    private Vector2 startPosition;
    private bool isPlaying;
    private Material materialInstance;  // 실 이미지의 Material 인스턴스

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        if (canvasGroup == null) canvasGroup = GetComponent<CanvasGroup>();
    }

     public void PlayAnim(Sprite yarnSprite, Material yarnMaterial, int quantity)
    {
         // Material 인스턴스 생성 및 설정
        if (yarnMaterial != null && yarnImage != null)
        {
            // 이전 Material 인스턴스 정리
            if (materialInstance != null)
            {
                Destroy(materialInstance);
            }
            
            materialInstance = new Material(yarnMaterial);
            materialInstance.SetFloat("_Alpha", 1f);  // 초기 알파값 설정
            yarnImage.material = materialInstance;
        }

        // UI 설정
        if (yarnImage != null)
        {
            yarnImage.sprite = yarnSprite;
        }
        if (numText != null)
        {
            numText.text = quantity.ToString();
        }

        // 애니메이션 초기화
        gameObject.SetActive(true);
        startPosition = rectTransform.anchoredPosition;
        time = 0f;
        isPlaying = true;
        
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 1f;
        }
    }

    private void Update()
    {
        if (!isPlaying) return;

        if (time < fadeTime)
        {
            time += Time.deltaTime;
            float progress = time / fadeTime;

            // 위로 올라감
            Vector2 newPosition = startPosition + Vector2.up * (moveDistance * progress);
            rectTransform.anchoredPosition = newPosition;

            // 페이드아웃
            float alphaValue = 1f - progress;

            if (canvasGroup != null)
            {
                canvasGroup.alpha = 1f - progress;
            }

            // 실 이미지 알파값 조정
            if (materialInstance != null)
            {
                materialInstance.SetFloat("_Alpha", alphaValue);
            }

            if (progress >= 1f)
            {
                OnAnimationComplete();
            }
        }
    }

    private void OnAnimationComplete()
    {
        isPlaying = false;
        gameObject.SetActive(false);
        ResetAnim();
    }

    public void ResetAnim()
    {
        time = 0f;
        if (rectTransform != null)
        {
            rectTransform.anchoredPosition = startPosition;
        }
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 1f;
        }
    }
}
