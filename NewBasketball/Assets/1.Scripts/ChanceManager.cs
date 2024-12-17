using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

//�÷��̾��� �������� ��ȸ�� ���õ� ����
public class ChanceManager : MonoBehaviour
{
    public static ChanceManager Instance { get; private set; }
    [SerializeField] private GameObject gameOverPanel;  //���ӿ��� �� Ȱ��ȭ�� ���ӿ��� �г� ������Ʈ
    [SerializeField] private Text highScoreText;
    [SerializeField] private Text currentScoreText;
    [SerializeField] private Image[] ballIcons; //5���� �󱸰� ������
    [SerializeField] private Button restartButton;
    [SerializeField] private Button quitButton;

    private int highScore = 0; //�÷��̾��� �ְ� ����
    public int currentChance = 5;    //�÷��̾��� �� ������ �ִ� Ƚ��
   
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

    public void UseChance() //��ȸ ���
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

        int currentScore = Goal.Instance.score; //���� ȹ�� ����
        currentScoreText.text = $"���� ���� : {currentScore}"; //���� ���� ǥ��

        //�ְ� ���� ���� 
        int savedHighScore = PlayerPrefs.GetInt("HighScore", 0); //����� �ְ� ���� �ҷ����� (�⺻�� 0)
        if (currentScore > savedHighScore)
        {
            savedHighScore = currentScore; //�ְ� ���� ����
            PlayerPrefs.SetInt("HighScore", savedHighScore); //���ο� �ְ� ���� ����
            PlayerPrefs.Save(); //����
        }

        highScore = savedHighScore;
        highScoreText.text = $"�ְ� ���� : {highScore}";

        StopAllCoroutines();
    }

    private void OnClickRestartButton()
    {
        // ARSession �ʱ�ȭ
        ARSession arSession = FindObjectOfType<ARSession>();
        if (arSession != null)
        {
            arSession.Reset(); //ARSession �ʱ�ȭ
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); //����� �ҷ�����
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
