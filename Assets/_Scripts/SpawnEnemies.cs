using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SpawnEnemies : MonoBehaviour {

    // Different types of ships
    [Header("Types of Ships")]
    public GameObject fourPointShip;
    public GameObject seekerShip;
    public GameObject gruntShip;

    [Header("Spawn other objects")]
    public GameObject healthCloud; // allows player to regain health upon collision
    public float healthCloudProbability = 0.05f;
    public Text RoundWin;
    public Text tipText;
    public Image tipWindow;

    [Header("Used in SpawnShip(), testing only")] 
    private float spawnTime = 10f;
    private ShipType lastCalled;

    [Header("Random positions for other objects")]
    private Vector3 randCloudPos;
    private float randHealthCloud;
    private int sequence = 0;
    private Vector3 topRight = new Vector3(25, -2, 85);
    private Vector3 topLeft = new Vector3(-25, -2, 85);
    private float gameStartTime;
    private bool beginChecking = false;
    private int tipSequence;
    private float tipTime = 2f;

    [Header("Current Scene")]
    private int sceneSequence;

    [Header("Use GameObject.Find on these")]
    private GameObject btnAgain;
    private GameObject txtBonusPoints_GO;
    private Text txtBonusPoints;

    void Start () {
        // Get current scene
        Scene scene = SceneManager.GetActiveScene();
        if (scene.name == "Tutorial")
        {
            // Welcome Player
            tipSequence = 0;
            tipText.text = "Welcome to ship shooter!";
            Invoke("TipTextUpdate", tipTime);

            // Set scene to #0
            sceneSequence = 0;

            // Begin tutorial
            Invoke("Tutorial", 5f);
            btnAgain = GameObject.Find("Again"); // Restart button for tutorial
            btnAgain.SetActive(false);

            // Bonus points text for end of each level
            txtBonusPoints_GO = GameObject.Find("BonusPointsText"); 
            txtBonusPoints = txtBonusPoints_GO.GetComponent<Text>();
            txtBonusPoints_GO.SetActive(false);
        }
        else if (scene.name == "Level_1")
        {
            // Set scene to #1
            sceneSequence = 1;

            // Begin level
            Invoke("LevelOneTest", 5f); 
        }
        else if (scene.name == "Level_2")
        {
            // Set scene to #1
            sceneSequence = 2;

            // Begin level
            Invoke("LevelTwo", 5f);
        }

        // Track time
        gameStartTime = Time.time;

        if(sceneSequence!=0)
            Invoke("SpawnHealthCloud", 10f); // begin to spawn health clouds (health packs)
    }
	
	void Update () {
        // Begin to check for win after final wave
        if (beginChecking) CheckForWin();
    }

    // ================== SpawnShip() for TESTING ONLY =================== //
    void SpawnShip()
    {
        // Create new ship GO
        GameObject spShip;  

        // Chose a random fly in point
        float randFlyInPoint = Random.value; // choose random x value for fly in point
        float leftOrRight = Random.value; // 50% change it will be negative or positive of x-axis
        Vector3 flyInPointV; // this vector holds the point for the ship to lerp to

        if (leftOrRight > .50f)
        {
            flyInPointV = new Vector3(randFlyInPoint * 50, -2, 85);
        }
        else
        {
            flyInPointV = new Vector3(randFlyInPoint * 50, -2, 85);
        }


        // Choose to spawn a random ship
        float randShip = Random.value;

        if (randShip >= .66)
        {
            // Instantiate the ship
            spShip = Instantiate(fourPointShip, flyInPointV, Quaternion.identity);
            lastCalled = ShipType.FourPoint;
        }
        else if (randShip >= .33)
        {
            // Instantiate the ship
            spShip = Instantiate(seekerShip, flyInPointV, Quaternion.identity);
            lastCalled = ShipType.Seeker;
        }
        else
        {
            // Instantiate the ship
            spShip = Instantiate(gruntShip, flyInPointV, Quaternion.identity);
            lastCalled = ShipType.Grunt;
        }

        // continue spawning ships, change spawn time depending upon last ship that went out
        switch (lastCalled)
        {
            case ShipType.FourPoint:
                spawnTime = 10f;
                break;
            case ShipType.Seeker:
                spawnTime = 8f;
                break;
            case ShipType.Grunt:
                spawnTime = 1f;
                break;
        }

        Invoke("SpawnShip", spawnTime);
    }

    // Health clouds will randomly spawn
    void SpawnHealthCloud()
    {
        randHealthCloud = Random.value;
        float leftOrRight = Random.value;

        if(leftOrRight <= 0.50f)
        {
            randCloudPos = new Vector3(Random.value * 50, -10, Random.value * 50);
        }
        else
        {
            randCloudPos = new Vector3(Random.value * -50, -10, Random.value * -50);
        }
        
        Instantiate(healthCloud, randCloudPos, Quaternion.identity);

        Invoke("SpawnHealthCloud", Random.value*60);
    }

    // For the tutorial
    void Tutorial()
    {
        // track which sequence of events we are on
        sequence++;

        // update the tip text box
        Invoke("TipTextUpdate", tipTime);
        
        // Create new ship GO
        GameObject spShip;

        // Chose a random fly in point
        float randFlyInPoint = Random.value; // choose random x value for fly in point
        float leftOrRight = Random.value; // 50% change it will be negative or positive of x-axis
        Vector3 flyInPointV; // this vector holds the point for the ship to lerp to

        if (leftOrRight > .50f)
        {
            flyInPointV = new Vector3(randFlyInPoint * 50, -2, 85);
        }
        else
        {
            flyInPointV = new Vector3(randFlyInPoint * 50, -2, 85);
        }

        int countSeeker = 0; // number of seekers per wave
        int countFP = 0;    // number of four point ships per wave
        int countGrunt = 0;
        switch (sequence)
        {
            case 1:
                for (int i = 2; i > countSeeker; countSeeker++)
                {
                    spShip = Instantiate(seekerShip, flyInPoint(), Quaternion.identity);
                }
                Invoke("Tutorial", 10f);
                break;
            case 2:
                for (int i = 1; i > countFP; countFP++)
                {
                    spShip = Instantiate(fourPointShip, flyInPoint(), Quaternion.identity);
                }
                Invoke("Tutorial", 15f);
                break;
            case 3:
                for (int i = 4; i > countGrunt; countGrunt++)
                {
                    spShip = Instantiate(gruntShip, new Vector3(-30, -2, 85), Quaternion.identity);

                    spShip = Instantiate(gruntShip, new Vector3(30, -2, 85), Quaternion.identity);

                    spShip = Instantiate(gruntShip, new Vector3(0, -2, -85), Quaternion.identity);

                }
                Invoke("Tutorial", 8f);
                Invoke("SpawnHealthCloud", 6f);
                break;
            case 4:
                CheckForWin();
                break;
            default:
                break;
        }
    }

    // First Level
    void LevelOneTest()
    {
        sequence++;

        // Create new ship GO
        GameObject spShip;

        // Chose a random fly in point
        float randFlyInPoint = Random.value; // choose random x value for fly in point
        float leftOrRight = Random.value; // 50% change it will be negative or positive of x-axis
        Vector3 flyInPointV; // this vector holds the point for the ship to lerp to

        if (leftOrRight > .50f)
        {
            flyInPointV = new Vector3(randFlyInPoint * 50, -2, 85);
        }
        else
        {
            flyInPointV = new Vector3(randFlyInPoint * 50, -2, 85);
        }

        int countSeeker = 0; // number of seekers per wave
        int countFP = 0;    // number of four point ships per wave
        int countGrunt = 0;    // number of four point ships per wave

        switch (sequence)
        {
            case 1:
                TipTextUpdate();
                for (int i = 4; i > countSeeker; countSeeker++)
                {
                    spShip = Instantiate(seekerShip, flyInPoint(), Quaternion.identity);

                }

                Invoke("LevelOneTest", 10f);
                break;
            case 2:
                TipTextUpdate();
                for (int i = 1; i > countFP; countFP++)
                {
                    spShip = Instantiate(fourPointShip, flyInPoint(), Quaternion.identity);
                }
                Invoke("LevelOneTest", 15f);
                break;
            case 3:
                TipTextUpdate();
                for (int i = 1; i > countFP; countFP++)
                {
                    spShip = Instantiate(fourPointShip, flyInPoint(), Quaternion.identity);
                }
                for (int i = 2; i > countSeeker; countSeeker++)
                {
                    spShip = Instantiate(seekerShip, flyInPoint(), Quaternion.identity);
                }
                Invoke("LevelOneTest", 20f);
                break;
            case 4:
                TipTextUpdate();
                for (int i = 2; i > countFP; countFP++)
                {
                    spShip = Instantiate(fourPointShip, flyInPoint(), Quaternion.identity);
                }
                Invoke("LevelOneTest", 20f);
                break;
            case 5:
                TipTextUpdate();
                for (int i = 1; i > countFP; countFP++)
                {
                    spShip = Instantiate(fourPointShip, flyInPoint(), Quaternion.identity);
                }
                for (int i = 2; i > countSeeker; countSeeker++)
                {
                    spShip = Instantiate(seekerShip, flyInPoint(), Quaternion.identity);
                }
                Invoke("LevelOneTest", 20f);
                break;
            case 6:
                TipTextUpdate(); // Update wave count UI

                int shiftX = 0;
                for (int i = 2; i > countGrunt; countGrunt++)
                {
                    if (countGrunt == 0)
                        shiftX = 15;
                    else if (countGrunt == 1)
                        shiftX = 30;
                    else if (countGrunt == 2)
                        shiftX = 45;
                    else 
                        shiftX = 60;
                      
                    spShip = Instantiate(gruntShip, new Vector3(-shiftX, -2, 85), Quaternion.identity);

                    spShip = Instantiate(gruntShip, new Vector3(shiftX, -2, 85), Quaternion.identity);

                    spShip = Instantiate(gruntShip, new Vector3(-shiftX, -2, -85), Quaternion.identity);

                    spShip = Instantiate(gruntShip, new Vector3(shiftX, -2, -85), Quaternion.identity);
                }

                Invoke("LevelOneTest", 15f);
                break;
            case 7:
                CheckForWin();
                break;
            default:
                break;
        }
    }

    // Second Level
    void LevelTwo()
    {
        sequence++;

        // Create new ship GO
        GameObject spShip;

        // Chose a random fly in point
        float randFlyInPoint = Random.value; // choose random x value for fly in point
        float leftOrRight = Random.value; // 50% change it will be negative or positive of x-axis
        Vector3 flyInPointV; // this vector holds the point for the ship to lerp to

        if (leftOrRight > .50f)
        {
            flyInPointV = new Vector3(randFlyInPoint * 50, -2, 85);
        }
        else
        {
            flyInPointV = new Vector3(randFlyInPoint * 50, -2, 85);
        }

        int countSeeker = 0; // number of seekers per wave
        int countFP = 0;    // number of four point ships per wave
        int countGrunt = 0;    // number of four point ships per wave

        switch (sequence)
        {
            case 1:
                TipTextUpdate();
                
                // 2 FP
                for (int i = 2; i > countFP; countFP++)
                {
                    spShip = Instantiate(fourPointShip, flyInPoint(), Quaternion.identity);
                }

                Invoke("LevelTwo", 20f);
                break;
            case 2:
                TipTextUpdate();

                // 1 grunt
                spShip = Instantiate(gruntShip, new Vector3(0, -2, 85), Quaternion.identity);

                // 1 FP
                spShip = Instantiate(fourPointShip, flyInPoint(), Quaternion.identity);

                Invoke("LevelTwo", 15f);
                break;
            case 3:
                TipTextUpdate();

                // 6 seekers
                for (int i = 6; i > countSeeker; countSeeker++)
                {
                    spShip = Instantiate(seekerShip, flyInPoint(), Quaternion.identity);
                }

                Invoke("LevelTwo", 20f);
                break;
            case 4:
                TipTextUpdate();

                // 2 grunts
                spShip = Instantiate(gruntShip, new Vector3(0, -2, 85), Quaternion.identity);
                spShip = Instantiate(gruntShip, new Vector3(0, -2, -85), Quaternion.identity);

                // 3 seekers
                for (int i = 3; i > countSeeker; countSeeker++)
                {
                    spShip = Instantiate(seekerShip, flyInPoint(), Quaternion.identity);
                }

                Invoke("LevelTwo", 15f);
                break;
            case 5:
                TipTextUpdate();

                // 2 grunts
                spShip = Instantiate(gruntShip, new Vector3(0, -2, 85), Quaternion.identity);
                spShip = Instantiate(gruntShip, new Vector3(0, -2, -85), Quaternion.identity);

                // 1 FP
                spShip = Instantiate(fourPointShip, flyInPoint(), Quaternion.identity);

                Invoke("LevelTwo", 20f);
                break;
            case 6:
                TipTextUpdate(); 
                
                int shiftX = 0;

                if (countGrunt == 0)
                    shiftX = 15;
                else if (countGrunt == 1)
                    shiftX = 30;
                else if (countGrunt == 2)
                    shiftX = 45;
                else
                    shiftX = 60;

                // 4 grunts
                spShip = Instantiate(gruntShip, new Vector3(-shiftX, -2, 85), Quaternion.identity);

                spShip = Instantiate(gruntShip, new Vector3(shiftX, -2, 85), Quaternion.identity);

                spShip = Instantiate(gruntShip, new Vector3(-shiftX, -2, -85), Quaternion.identity);

                spShip = Instantiate(gruntShip, new Vector3(shiftX, -2, -85), Quaternion.identity);

                // 1 FP
                spShip = Instantiate(fourPointShip, flyInPoint(), Quaternion.identity);

                Invoke("LevelTwo", 15f);
                break;
            case 7:
                CheckForWin();
                break;
            default:
                break;
        }
    }

    // Generate random fly in points of enemy ships
    public Vector3 flyInPoint()
    {
        // Chose a random fly in point
        float randFlyInPoint = Random.value; // choose random x value for fly in point
        float randX = Random.value; // 50% change it will be negative or positive of x-axis
        float randZ = Random.value;
        Vector3 flyInPointV; // this vector holds the point for the ship to lerp to

        // randomly negative or positive
        float nOrP = Random.value;

        // Top
        if (randFlyInPoint >= .66f)
        {           
            if (nOrP >= .50f)
                flyInPointV = new Vector3(randX * 50, -2, 85);
            else
                flyInPointV = new Vector3(randX * -50, -2, 85);
        }
        // Left
        else if (randFlyInPoint >= .33f)
        {
            if (nOrP >= .50f)
                flyInPointV = new Vector3(-95, -2, randZ * 30);
            else
                flyInPointV = new Vector3(-95, -2, randZ * -30);
        }
        // Right
        else 
        {
            if (nOrP >= .50f)
                flyInPointV = new Vector3(95, -2, randZ * 30);
            else
                flyInPointV = new Vector3(95, -2, randZ * -30);
        }

        return flyInPointV;
    }

    // Checks if player has won round so they can move on to next level
    public void CheckForWin()
    {
        beginChecking = true; // Sets to true once called from case 7

        if (GameObject.Find("Seeker(Clone)") ==  null 
            && GameObject.Find("FourPointShip(Clone)") == null 
            && GameObject.Find("Grunt(Clone)") == null)
        {
            RoundWin.enabled = true;
            EnemyShip.score += Player.bonusScore; // add bonus points
            Invoke("NextRound", 3);

            if (btnAgain != null) btnAgain.SetActive(true); // tutorial only

            if (txtBonusPoints != null) // levels only
            {
                txtBonusPoints_GO.SetActive(true);
                txtBonusPoints.text = Player.bonusScore.ToString();
            }
        }
    }

    // Updates the text box in tutorial and throughout levels for wave count
    public void TipTextUpdate()
    {
        if(sceneSequence == 0)
        {
            tipSequence++;

            switch (tipSequence)
            {
                case 1:
                    tipText.text = "To the right is your health bar. When you are hit by enemy lasers, you take damage.";
                    tipTime = 5f;
                    break;
                case 2:
                    tipText.text = "Press the left mouse button down to attack.";
                    tipTime = 6f;
                    Invoke("TipTextUpdate", tipTime);
                    break;
                case 3:
                    tipText.text = "This is a stronger ship.\n" +
                        "You cannot destroy it's attacks, so get ready to dodge!";
                    tipTime = 3f;
                    break;
                case 4:
                    tipText.text = "Press SPACEBAR to to gain a force field which makes you invincible and move faster.";
                    break;
                case 5:
                    tipText.text = "SPACEBAR is especially helpful against these lasers!";
                    break;
                case 6:
                    tipText.text = "Pick up a health pack to regain HP.";
                    Invoke("TipTextUpdate", 4f);
                    break;
                case 7:
                    tipText.text = "There's the basics. Press RESTART to do this tutorial again or go BACK to the Main Menu.";
                    break;
                default:
                    break;
            }
        }

        if(sceneSequence > 0)
        {
            tipSequence++;
            tipText.text = "WAVE: " + tipSequence;
        }
    }

    // Move on to next round
    void NextRound()
    {
        if(sceneSequence == 2)
            SceneManager.LoadScene("GameOver");
        else
            SceneManager.LoadScene("Level_"+ (sceneSequence+1).ToString());
   
    }
}
