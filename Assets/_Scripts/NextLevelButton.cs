using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NextLevelButton : MonoBehaviour {

    public Button m_NextButton;
    public bool isTestingEnabled;

    void Start()
    {
        isTestingEnabled = StartScreen.testingEnabled;

        if (isTestingEnabled)
        {
            m_NextButton.interactable = true;
        }
        else
        {
            m_NextButton.interactable = false;
        }

        if(SpawnEnemies.sceneSequence == 4)
        {
            m_NextButton.interactable = false;
        }

        Button nextBtn = m_NextButton.GetComponent<Button>();

        nextBtn.onClick.AddListener(nextSceneOnClick);
    }

    void nextSceneOnClick()
    {
        SceneManager.LoadScene("Level_"+(SpawnEnemies.sceneSequence+1).ToString());
    }

}
