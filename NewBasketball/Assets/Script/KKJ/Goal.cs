using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Goal : MonoBehaviour
{
    AudioSource audioSource; //����� �ҽ� ������Ʈ
    [SerializeField] private AudioClip goalAudioClip; //�� ����� Ŭ��
    private Text scoreText;
    private int score; //����


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
