using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Goal : MonoBehaviour
{
    AudioSource audioSource; //오디오 소스 컴포넌트
    [SerializeField] private AudioClip goalAudioClip; //골 오디오 클립
    private Text scoreText;
    private int score; //점수


    void Start()
    {
        scoreText = FindObjectOfType<Text>();
        scoreText.text = $"Score : {score}";
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = goalAudioClip;
    }

    void OnTriggerEnter(Collider other)
    {
        other.CompareTag("BALL");
        score++;
        scoreText.text = $"Score : {score}";
        audioSource.Play();
    }

}
