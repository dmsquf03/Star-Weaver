using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WoolObtainAnim : MonoBehaviour
{
    [Header("Animation Settings")]
    public float fadeTime = 1f;
    [SerializeField] private float moveDistance = 10f;

    [Header("UI References")]
    [SerializeField] private CanvasGroup canvasGroup;

    private float time;
    private RectTransform rectTransform;
    private Vector2 startPosition;
    private bool isPlaying;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        if(canvasGroup == null) canvasGroup = GetComponent<CanvasGroup>();
    }

    public void PlayAnim()
    {
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

    void Update()
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
            if (canvasGroup != null)
            {
                canvasGroup.alpha = 1f - progress;
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
