using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class DoorController : MonoBehaviour
{
    private RectTransform rectTransform;
    private void Start()
    {
        UpdateDoorState();
    }

    private void UpdateDoorState()
    {
        bool hasAnyGem = false;
        var gems = GameManager.Instance.PlayerManager.PlayerData.gemsP;

        for(int i = 0; i < gems.Length; i++)
        {
            if(gems[i])
            {
                hasAnyGem = true;
                break;
            }
        }

        // 젬이 있으면 비활성화, 없으면 활성화
        gameObject.SetActive(!hasAnyGem);
    }
}
