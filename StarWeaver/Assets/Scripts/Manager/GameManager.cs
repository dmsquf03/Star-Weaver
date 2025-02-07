using UnityEngine;

// 게임 전체 관리 (싱글톤)
public class GameManager : MonoBehaviour
{
    // 테스트 데이터 사용 여부 설정
    #if UNITY_EDITOR
    [SerializeField] private bool useTestData = true;
    #endif
    public static GameManager Instance { get; private set; }
    public PlayerManager PlayerManager { get; private set; }
    public InventoryManager InventoryManager { get; private set; }
    
    public Vector3[] PointList;  // 메인씬 플레이어 이동 복귀 위치 저장

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            #if UNITY_EDITOR
            PlayerManager = new PlayerManager(useTestData);
            #else
            PlayerManager = new PlayerManager(false);
            #endif

            InventoryManager = new InventoryManager();
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
