using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button startBtn;   // 시작 버튼
    [SerializeField] private Button settingBtn; // 설정 버튼
    [SerializeField] private Button quitBtn;    // 종료 버튼
    public Text descText;  // 임시 텍스트

    void Start()
    {
        startBtn.onClick.AddListener(OnClickStartButton);
        settingBtn.onClick.AddListener(OnClickSettingButton);
        quitBtn.onClick.AddListener(OnClickQuitButton);
    }

    private void OnClickStartButton()
    {
        SceneManager.LoadScene(1);
    }

    private void OnClickSettingButton()
    {
        StartCoroutine(OnClickedSet());
    }

    IEnumerator OnClickedSet()
    {
        descText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        descText.gameObject.SetActive(false);
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
