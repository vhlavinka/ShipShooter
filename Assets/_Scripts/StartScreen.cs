using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartScreen : MonoBehaviour
{

    public Button m_StartButton, m_HowButton;

    void Awake()
    {
        Button startBtn = m_StartButton.GetComponent<Button>();
        Button howBtn = m_HowButton.GetComponent<Button>();

        startBtn.onClick.AddListener(startGameOnClick);
        howBtn.onClick.AddListener(howToPlayOnClick);
    }

    void startGameOnClick()
    {
        SceneManager.LoadScene("LevelOne");
    }
    void howToPlayOnClick()
    {
        SceneManager.LoadScene("Tutorial");
    }
}
