using UnityEngine;
using UnityEngine.SceneManagement;

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

    [SerializeField]
    private Vector3[] pointListData;  // 메인씬 플레이어 이동 복귀 위치 저장

    // PointList property 추가
    public Vector3[] PointList
    {
        get 
        { 
            Debug.Log($"Trying to access PointList. Instance is null? {Instance == null}");
            Debug.Log($"pointListData is null? {pointListData == null}");
            if (pointListData == null || pointListData.Length == 0)
            {
                Debug.LogWarning("PointList is not initialized!");
                pointListData = new Vector3[0];

                // 기본값 설정 (필요한 경우)
                pointListData[0] = new Vector3(-2, -11, -11);
                pointListData[1] = new Vector3(-4, -13, -13);
                pointListData[2] = new Vector3(1, -12, -12);
            }
            return pointListData;
        }
        set { pointListData = value; }
    }

     private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeManagers();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeManagers()
    {
        #if UNITY_EDITOR
        PlayerManager = new PlayerManager(useTestData);
        #else
        PlayerManager = new PlayerManager(false);
        #endif

        InventoryManager = new InventoryManager();
        
        // PointList 초기화 확인
        if (pointListData == null)
        {
            pointListData = new Vector3[3];
        }
    }

    // 씬 로드 이벤트 감지
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"Scene loaded: {scene.name}");
        Debug.Log($"GameManager Instance exists? {Instance != null}");
        Debug.Log($"PointList data exists? {pointListData != null}");
        
        if (scene.name == "main")
        {
            if (pointListData == null)
            {
                Debug.LogError("PointList was null on main scene load!");
                InitializeManagers();
            }
        }
    }

    // 디버그용 메서드
    public void LogPointListStatus()
    {
        Debug.Log($"PointList status - Is null: {pointListData == null}, Length: {(pointListData != null ? pointListData.Length : 0)}");
        if (pointListData != null)
        {
            for (int i = 0; i < pointListData.Length; i++)
            {
                Debug.Log($"Point {i}: {pointListData[i]}");
            }
        }
    }
}
