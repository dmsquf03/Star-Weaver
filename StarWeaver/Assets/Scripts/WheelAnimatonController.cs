using UnityEngine;

public class WheelAnimatonController : MonoBehaviour
{
    private Animator animator;
    
    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // 가공 시작할 때 호출
    public void StartProcessing()
    {
        animator.SetBool("IsYarnProcessing", true);
    }

    // 가공 종료할 때 호출
    public void StopProcessing()
    {
        animator.SetBool("IsYarnProcessing", false);
    }
}
