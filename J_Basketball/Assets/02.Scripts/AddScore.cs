using JetBrains.Annotations;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AddScore : MonoBehaviour
{
    int score; //������ ����� ����
    public Text scoreText; //������ ����� �ؽ�Ʈ
    public Text statusText; //�� ���� ���� ���� ��� �ؽ�Ʈ
    [SerializeField]
    private Button ballButton; //�󱸰� Ŭ��

    public GameOverUI gameOverUI;

    private bool isPlaying = false; //���� ������ ������ Ȯ�� �÷���

    // Start is called before the first frame update
    void Start()
    {
        ballButton.onClick.AddListener(OnClickBasketball);
        scoreText.gameObject.SetActive(true);
        statusText.gameObject.SetActive(false);
        scoreText.text = $"{score}";
        statusText.text = "�����߽��ϴ�\n�ٽ� Ŭ���ϼ���!";
    }

    private void OnClickBasketball()
    {
        if (!isPlaying)
        {
            isPlaying = true;
            ballButton.interactable = false; //��ư ��Ȱ��ȭ
            int index = Random.Range(0, 3);  // 0,1,2 �� �ϳ��� ���� �� ����
            if (index == 0 || index == 1)
            {
                score += 1;
                isPlaying = true;
                UpdateUI();
                if (score == 15)
                {
                    gameOverUI.ActiveGameOverUI();
                    gameOverUI.gameOverScore.text = $"Score : {score}";
                }
            }
            else if (index == 2)
            {
                isPlaying = true;
                StartCoroutine(ActiveText());
            }
        }
    }

    IEnumerator ActiveText()
    {
        statusText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        statusText.gameObject.SetActive(false);
        isPlaying = false;
        ballButton.interactable = true;
    }

    private void UpdateUI()
    {
        scoreText.text = $"{score}";
        isPlaying = false;
        ballButton.interactable = true;
    }
}
