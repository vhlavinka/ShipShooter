using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartScreen : MonoBehaviour
{

    public Button m_StartButton, m_HowButton, m_EnableTestingButton, m_ChangeDifficulty;
    public Image isNotChecked;
    public Image isChecked;
    public Text txtDifficulty;
    public static bool testingEnabled;
    public static bool easyDifficulty;

    void Awake()
    {
        Button startBtn = m_StartButton.GetComponent<Button>();
        Button howBtn = m_HowButton.GetComponent<Button>();
        Button enableBtn = m_EnableTestingButton.GetComponent<Button>();
        Button changeDifficulty = m_ChangeDifficulty.GetComponent<Button>();

        startBtn.onClick.AddListener(startGameOnClick);
        howBtn.onClick.AddListener(howToPlayOnClick);
        enableBtn.onClick.AddListener(EnableTestingOnClick);
        changeDifficulty.onClick.AddListener(changeDifficultyOnClick);

        isChecked.enabled = false; // make test mode disabled by default

        easyDifficulty = true; // easy difficulty by default
        txtDifficulty.text = "Beginner";
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

    void changeDifficultyOnClick()
    {
        if (easyDifficulty)
        {
            easyDifficulty = false;
            txtDifficulty.text = "Experienced";
        }
        else
        {
            easyDifficulty = true;
            txtDifficulty.text = "Beginner";
        }
    }
}
