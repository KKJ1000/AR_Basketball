using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

//플레이어의 공던지는 기회와 관련된 로직
public class ChanceManager : MonoBehaviour
{
    public static ChanceManager Instance { get; private set; }
    [SerializeField] private GameObject gameOverPanel;  //게임오버 시 활성화될 게임오버 패널 오브젝트
    [SerializeField] private Text highScoreText;
    [SerializeField] private Text currentScoreText;
    [SerializeField] private Image[] ballIcons; //5개의 농구공 아이콘
    [SerializeField] private Button restartButton;
    [SerializeField] private Button quitButton;

    private int highScore = 0; //플레이어의 최고 점수
    public int currentChance = 5;    //플레이어의 공 던지기 최대 횟수
   
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        restartButton.onClick.AddListener(OnClickRestartButton);
        quitButton.onClick.AddListener(OnClickQuitButton);
        gameOverPanel.SetActive(false);
        UpdateBallIcons();
    }

    public void UseChance() //기회 사용
    {
        currentChance--;
        UpdateBallIcons();

        if (currentChance <= 0)
        {
            GameOver();
        }
    }

    private void UpdateBallIcons()
    {
        for (int i = 0; i < ballIcons.Length; i++)
        {
            ballIcons[i].enabled = i < currentChance;
        }
    }

    public void GameOver()
    {
        gameOverPanel.SetActive(true);

        int currentScore = Goal.Instance.score; //현재 획득 점수
        currentScoreText.text = $"현재 점수 : {currentScore}"; //현재 점수 표시

        //최고 점수 갱신 
        int savedHighScore = PlayerPrefs.GetInt("HighScore", 0); //저장된 최고 점수 불러오기 (기본값 0)
        if (currentScore > savedHighScore)
        {
            savedHighScore = currentScore; //최고 점수 갱신
            PlayerPrefs.SetInt("HighScore", savedHighScore); //새로운 최고 점수 저장
            PlayerPrefs.Save(); //저장
        }

        highScore = savedHighScore;
        highScoreText.text = $"최고 점수 : {highScore}";

        StopAllCoroutines();
    }

    private void OnClickRestartButton()
    {
        // ARSession 초기화
        ARSession arSession = FindObjectOfType<ARSession>();
        if (arSession != null)
        {
            arSession.Reset(); //ARSession 초기화
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); //현재씬 불러오기
    }

    private void OnClickQuitButton()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

}
