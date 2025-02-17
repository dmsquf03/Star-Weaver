using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class SceneChanger : MonoBehaviour
{
    [SerializeField] private Button workPlaceButton;    // 작업실 버튼
    [SerializeField] private Button returnButton;       // 메인 돌아가기 버튼

    private void Start()
    {
        UpdateButtonStates();
    }

    private void UpdateButtonStates()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        
        // 각 버튼이 존재할 때만 interactable 설정
        if (workPlaceButton != null)
        {
            workPlaceButton.interactable = (currentScene != "WorkPlace");
        }
        
        if (returnButton != null)
        {
            returnButton.interactable = true; // 메인씬 아닐때만 존재, 항상 활성화
        }
    }
    public void mainSceneChange()
    {
      SceneManager.LoadScene("Main");
    }

    public void workPlaceSceneChange()
    {
      SceneManager.LoadScene("WorkPlace");
    }
}
