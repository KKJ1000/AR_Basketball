using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button startBtn;   // ���� ��ư
    [SerializeField] private Button settingBtn; // ���� ��ư
    [SerializeField] private Button quitBtn;    // ���� ��ư
    public Text descText;  // �ӽ� �ؽ�Ʈ

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
