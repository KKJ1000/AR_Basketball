using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Goal : MonoBehaviour
{
    private Text scoreText;
    private int score; //Á¡¼ö

    void Start()
    {
        scoreText = FindObjectOfType<Text>();
        scoreText.text = $"Score : {score}";
    }

    void OnTriggerEnter(Collider other)
    {
        other.CompareTag("BALL");
        score++;
        scoreText.text = $"Score : {score}";
    }

}
