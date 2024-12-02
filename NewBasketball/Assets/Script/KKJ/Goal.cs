using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Goal : MonoBehaviour
{
    public static Goal Instance { get; private set; }

    [SerializeField] private Text scoreText;
    [SerializeField] private AudioClip goalAudioClip; //골 오디오 클립

    private AudioSource audioSource; //오디오 소스 컴포넌트

    public int score = 0;
    public bool isGoal { get; private set; } //골 여부 플래그

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
        scoreText.text = $"Score : {score}";
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = goalAudioClip;
        isGoal = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BALL"))
        {
            isGoal = true; // 골 성공
            score++;
            scoreText.text = $"Score: {score}";
            audioSource.Play();
        }
    }

    public void ResetGoal() //플래그 초기화 함수
    {
        isGoal = false;
    }
}
