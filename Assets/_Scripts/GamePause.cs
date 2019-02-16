using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GamePause : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private bool isPaused;
    [SerializeField] private GameObject[] allObjects;

    private void Start()
    {
        isPaused = false;

        allObjects = FindObjectsOfType<GameObject>();
        foreach (GameObject go in allObjects)
            if (go.activeInHierarchy)
                print(go + " is an active object");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused)
            {
                PauseGame();
                print("paused game");
            }
            else 
            {
                ContinueGame();
                print("continue game");
            }
        }
    }
    private void PauseGame()
    {
        Time.timeScale = 0; // Freeze game
        AudioListener.pause = true; // Stop Music
        pausePanel.SetActive(true); // Enable pause menu
        isPaused = true; // Set paused status to true

        foreach (GameObject go in allObjects)
        {
            if (go.activeInHierarchy && go.GetComponent<Rigidbody>() != null)
            {
                go.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            }
        }
    }
    private void ContinueGame()
    {
        Time.timeScale = 1;
        AudioListener.pause = false;
        pausePanel.SetActive(false);
        isPaused = false;

        foreach (GameObject go in allObjects)
        {
            if (go.activeInHierarchy && go.GetComponent<Rigidbody>() != null)
            {
                go.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY;
            }
        }
    }
}