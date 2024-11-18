using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuCtrl : MonoBehaviour
{

    [SerializeField] private Button startBtn;   //시작버튼
    [SerializeField] private Button settingBtn; //설정버튼
    [SerializeField] private Button quitBtn;    //나가기버튼

    public Text descText;      //임시텍스트

    void Start()
    {
        startBtn.onClick.AddListener(OnClickStartButton);
        settingBtn.onClick.AddListener(OnClickSettingButton);
        quitBtn.onClick.AddListener(OnClickQuitButton);
    }

    private void OnClickStartButton()
    {
        SceneManager.LoadScene("InGame");
    }

    private void OnClickSettingButton()
    {
        Debug.Log("설정 창이 열렸습니다.");
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
