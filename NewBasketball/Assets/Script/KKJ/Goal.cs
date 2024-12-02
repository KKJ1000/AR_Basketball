using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Goal : MonoBehaviour
{
    public static Goal Instance { get; private set; }

    [SerializeField] private Text scoreText;
    [SerializeField] private AudioClip goalAudioClip; //�� ����� Ŭ��

    private AudioSource audioSource; //����� �ҽ� ������Ʈ

    public int score = 0;
    public bool isGoal { get; private set; } //�� ���� �÷���

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
            isGoal = true; // �� ����
            score++;
            scoreText.text = $"Score: {score}";
            audioSource.Play();
        }
    }

    public void ResetGoal() //�÷��� �ʱ�ȭ �Լ�
    {
        isGoal = false;
    }
}
