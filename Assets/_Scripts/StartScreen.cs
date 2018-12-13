using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartScreen : MonoBehaviour
{

    public Button m_StartButton, m_HowButton, m_EnableTestingButton;
    public Image isNotChecked;
    public Image isChecked;

    public static bool testingEnabled;

    void Awake()
    {
        Button startBtn = m_StartButton.GetComponent<Button>();
        Button howBtn = m_HowButton.GetComponent<Button>();
        Button enableBtn = m_EnableTestingButton.GetComponent<Button>();

        startBtn.onClick.AddListener(startGameOnClick);
        howBtn.onClick.AddListener(howToPlayOnClick);
        enableBtn.onClick.AddListener(EnableTestingOnClick);

        isChecked.enabled = false; // make test mode disabled by default
    }

    void startGameOnClick()
    {
        SceneManager.LoadScene("Level_1");
    }
    void howToPlayOnClick()
    {
        SceneManager.LoadScene("Tutorial");
    }
    void EnableTestingOnClick()
    {
        if (isChecked.enabled)
        {
            isNotChecked.enabled = true;
            isChecked.enabled = false;

            testingEnabled = false;
        }
        else
        {
            isNotChecked.enabled = false;
            isChecked.enabled = true;

            testingEnabled = true;
        }
        
    }
}
