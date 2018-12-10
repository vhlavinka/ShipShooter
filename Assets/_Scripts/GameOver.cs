﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class GameOver : MonoBehaviour
{
    public Button m_StartButton, m_QuitButton;
    public Text m_score;


    void Start()
    {
        Button startBtn = m_StartButton.GetComponent<Button>();
        Button quitBtn = m_QuitButton.GetComponent<Button>();

        startBtn.onClick.AddListener(startGameOnClick);
        quitBtn.onClick.AddListener(quitGameOnClick); 

    }

    void startGameOnClick()
    {
        SceneManager.LoadScene("LevelOne");
    }

    void quitGameOnClick()
    {
        SceneManager.LoadScene("MainMenu");
    }
}