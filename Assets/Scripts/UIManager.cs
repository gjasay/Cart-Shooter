using MagicPigGames;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _scoreText;
    private ProgressBar _progressBar;
    // Start is called before the first frame update
    void Start()
    {
        _progressBar = GameObject.Find("Progress Bar").GetComponent<ProgressBar>();

        _progressBar.SetProgress(1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void UpdateScore(int score)
    {
        _scoreText.text = "Score: " + score.ToString();
    }

    public void UpdateHealth(int health)
    {
        Debug.Log(health);
        float progress = health / 100f;
        Debug.Log(progress);
        _progressBar.SetProgress(progress);
    }
}
