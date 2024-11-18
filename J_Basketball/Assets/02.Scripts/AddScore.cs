using JetBrains.Annotations;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AddScore : MonoBehaviour
{
    int score; //점수를 기록할 변수
    public Text scoreText; //점수를 출력할 텍스트
    public Text statusText; //골 성공 실패 여부 출력 텍스트
    [SerializeField]
    private Button ballButton; //농구공 클릭

    public GameOverUI gameOverUI;

    private bool isPlaying = false; //공을 던지는 중인지 확인 플래그

    // Start is called before the first frame update
    void Start()
    {
        ballButton.onClick.AddListener(OnClickBasketball);
        scoreText.gameObject.SetActive(true);
        statusText.gameObject.SetActive(false);
        scoreText.text = $"{score}";
        statusText.text = "실패했습니다\n다시 클릭하세요!";
    }

    private void OnClickBasketball()
    {
        if (!isPlaying)
        {
            isPlaying = true;
            ballButton.interactable = false; //버튼 비활성화
            int index = Random.Range(0, 3);  // 0,1,2 중 하나의 랜덤 값 생성
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
