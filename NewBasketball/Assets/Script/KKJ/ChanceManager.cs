using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//�÷��̾��� �������� ��ȸ�� ���õ� ����
public class ChanceManager : MonoBehaviour
{
    public static ChanceManager Instance { get; private set; }
    [SerializeField] private GameObject gameOverPanel;  //���ӿ��� �� Ȱ��ȭ�� ���ӿ��� �г� ������Ʈ
    [SerializeField] private Text maxScoreText;
    [SerializeField] private Image[] ballIcons; //5���� �󱸰� ������
    [SerializeField] private Button restartButton;
    [SerializeField] private Button quitButton;

    private int maxScore = 0; //�÷��̾��� �ְ� ����
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
        maxScore = Goal.Instance.score;
        maxScoreText.text = $"Max Score: {maxScore}";
        StopAllCoroutines();
    }

    private void OnClickRestartButton()
    {
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
