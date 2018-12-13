using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TutorialButtons : MonoBehaviour {

    public Button m_MainMenuButton, m_AgainButton;

    void Awake()
    {
        Button mainMenuBtn = m_MainMenuButton.GetComponent<Button>();
        Button againBtn = m_AgainButton.GetComponent<Button>();


        mainMenuBtn.onClick.AddListener(startGameOnClick);
        againBtn.onClick.AddListener(replayGameOnClick);

    }

    void startGameOnClick()
    {
        SceneManager.LoadScene("MainMenu");
    }

    void replayGameOnClick()
    {
        SceneManager.LoadScene("Tutorial");
    }
}
