using UnityEngine;
using System.Collections.Generic;

// 플레이어 데이터 관리
// - WoolCollector가 얘를 통해 wool 생산 관련 데이터 접근

public class PlayerManager
{
    private PlayerData playerData;

    public int Wool => playerData.wool;
    public int CurrentClicks => playerData.currentClicks;
    public int Money => playerData.money;

    public PlayerManager(bool useTestData = false)
    {
        // 초기 생성 시
        if (!LoadPlayerData())  // 저장된 데이터가 없다면
        {
            playerData = new PlayerData();  // 새로운 데이터 생성 (이때 money는 0으로 시작)
            
            #if UNITY_EDITOR    // 에디터 내에서 설정에 따라 초기 데이터 설정
            if (useTestData)
            {
                InitializeTestData();
            }
            #endif

            SavePlayerData();  // 초기 데이터 저장
        }
    }

    
    // 테스트 데이터
    #if UNITY_EDITOR
    private void InitializeTestData()
    {
        // 재화
        playerData.money = 50000;

        // 젬
        playerData.gemsP[0] = true;
        playerData.gemsP[1] = true;

        // 염색약
        playerData.dyesP[0] = new PlayerData.PlayerDyeData { unlocked = true, count = 5 }; // 빨강
        playerData.dyesP[1] = new PlayerData.PlayerDyeData { unlocked = true, count = 3 }; // 노랑
        
        // 부재료
        playerData.subMaterialsP[0] = new PlayerData.PlayerSubMaterialData { unlocked = true, count = 10 }; // 진주
        playerData.subMaterialsP[1] = new PlayerData.PlayerSubMaterialData { unlocked = true, count = 7 };  // 반짝이

        // 실 데이터
        playerData.yarns.Add(new YarnData 
        { 
            name = "빨간색 기본실",
            count = 3,
            mainType = "기본",
            color = Color.red
        });

        playerData.yarns.Add(new YarnData 
        { 
            name = "파란색 푹신실",
            count = 2,
            mainType = "푹신",
            color = Color.blue
        });
    }
    #endif
    private bool LoadPlayerData()
    {
        string data = PlayerPrefs.GetString("PlayerData", "");
        if (string.IsNullOrEmpty(data))
        {
            return false;  // 저장된 데이터가 없음
        }

        try
        {
            playerData = JsonUtility.FromJson<PlayerData>(data);
            return true;
        }
        catch
        {
            return false;
        }
    }

    private void SavePlayerData()
    {
        string data = JsonUtility.ToJson(playerData);
        PlayerPrefs.SetString("PlayerData", data);
        PlayerPrefs.Save();
    }

    // 돈 수정 메서드 (벌기, 쓰기)
    public void AddMoney(int amount)
    {
        long tempSum = (long)playerData.money + amount;
        if (tempSum >= PlayerData.MAX_MONEY)
        {
            playerData.money = PlayerData.MAX_MONEY;
        }
        else
        {
            playerData.money += amount;
        }
        SavePlayerData();
    }

    public bool TrySpendMoney(int amount)
    {
        if (playerData.money >= amount)
        {
            playerData.money -= amount;
            SavePlayerData();
            return true;
        }
        return false;
    }

    // 양털 수정 메서드 (사용 추가 해야?)
    public void AddWool()
    {
        playerData.wool++;
    }

    public void AddClick()
    {
        playerData.currentClicks++;
    }

    public void ResetClicks()
    {
        playerData.currentClicks = 0;
    }
}
