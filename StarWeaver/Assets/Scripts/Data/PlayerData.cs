using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class PlayerData
{
    public static readonly int MAX_MONEY = 999999; // 재화 최대
    public int money = 0;
    public int wool;                    // 보유 양털
    public int currentClicks;           // 현재 클릭 수
    
    
    // 젬 데이터
    public bool[] gemsP = new bool[] {false, false, false};   // 보유 젬 (0~2)
    
    // 염색약 데이터
    [System.Serializable]
    public class PlayerDyeData {
        public bool unlocked;           // 해금 여부
        public int count;               // 보유량
    }
    public PlayerDyeData[] dyesP = new PlayerDyeData[5];  // 0:red, 1:yellow, 2:blue, 3:white, 4:black
    
    // 부재료 데이터
    [System.Serializable]
    public class PlayerSubMaterialData {
        public bool unlocked;           // 해금 여부
        public int count;               // 보유량
    }
    public PlayerSubMaterialData[] subMaterialsP = new PlayerSubMaterialData[2];  // 0:pearl, 1:sparkle
    
    // 보유 실 리스트
    public List<YarnData> yarns = new List<YarnData>();

    // 생성자 - 배열 초기화
    public PlayerData()
    {
        dyesP = new PlayerDyeData[5];
        for(int i = 0; i < 5; i++)
        {
            dyesP[i] = new PlayerDyeData();
        }

        subMaterialsP = new PlayerSubMaterialData[2];
        for(int i = 0; i < 2; i++)
        {
            subMaterialsP[i] = new PlayerSubMaterialData();
        }
    }
}
