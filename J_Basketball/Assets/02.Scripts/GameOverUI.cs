using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private Button mainMenuBtn;
    public Text gameOverScore; //게임 오버 시 최종 점수 스코어 출력

    void Start()
    {
        mainMenuBtn.onClick.AddListener(GoToMainScene);
        gameOverUI.SetActive(false);    
    }

    public void ActiveGameOverUI()
    {
        gameOverUI.SetActive(true);
    }

    private void GoToMainScene()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
