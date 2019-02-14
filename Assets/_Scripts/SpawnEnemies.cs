using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SpawnEnemies : MonoBehaviour {

    [Header("Track current scene with int")]
    static public int sceneSequence;
    public Text txtBonusPoints;

    // Different types of ships
    [Header("Types of Ships")]
    public GameObject fourPointShip;
    public GameObject seekerShip;
    public GameObject gruntShip;

    [Header("Spawn other objects")]
    public GameObject healthCloud; // allows player to regain health upon collision
    public float healthCloudProbability = 0.05f;
    public Text txtRoundWin;
    public Text tipText;
    public Text txtLevel;

    [Header("Used in SpawnShip(), testing only")] 
    private float spawnTime = 10f;
    private ShipType lastCalled;

    [Header("Random positions for other objects")]
    private Vector3 randCloudPos;
    private float randHealthCloud;
    private int sequence = 0;
    private bool beginChecking = false;
    private int tipSequence;
    private float tipTime = 2f;

    [Header("Again Button")]
    private GameObject btnAgain;

    static public int scoreAtStartofRound;

    private float addBonus = 0;
    private float endOfRoundTimer = 0;
    private int endOfRoundBonus = 0;
    private int endOfRoundPlayerScore = 0;
    

    void Start () {

        scoreAtStartofRound = EnemyShip.score;

        // Bonus points text for end of each level
        if (txtBonusPoints != null) txtBonusPoints.text = "";

        txtRoundWin.enabled = false;

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

        }
        else if (scene.name == "Level_1")
        {
            // Set scene to #1
            sceneSequence = 1;

            // Begin level
            Invoke("LevelOne", 5f); 
        }
        else if (scene.name == "Level_2")
        {
            // Set scene to #1
            sceneSequence = 2;

            // Begin level
            Invoke("LevelTwo", 5f);
        }
        else if (scene.name == "Level_3")
        {
            // Set scene to #1
            sceneSequence = 3;

            // Begin level
            Invoke("LevelThree", 5f);
        }
        else if (scene.name == "Level_4")
        {
            // Set scene to #1
            sceneSequence = 4;

            // Begin level
            Invoke("LevelFour", 5f);
        }

        if (sceneSequence!=0)
            Invoke("SpawnHealthCloud", 10f); // begin to spawn health clouds (health packs)

        txtLevel.text = "Level " + sceneSequence;
    }
	
	void Update () {
        // Begin to check for win after final wave
        if (beginChecking) CheckForWin();

    }

    void FixedUpdate()
    {
        if (txtRoundWin.enabled)
        {
            endOfRoundTimer += Time.deltaTime;        

            if (endOfRoundTimer >= 3f)
            {
                EnemyShip.score = endOfRoundPlayerScore + endOfRoundBonus;
                txtBonusPoints.text = "Bonus Points : 0";
            }
            else if (endOfRoundTimer >= 1f)
            {
                // Give the illusion of the score being added incrementally
                addBonus = Player.bonusScore / 180; // Next round is invoked in 3 secs
                EnemyShip.score += (int)System.Math.Floor(addBonus); // add bonus points
                Player.bonusScore -= (int)System.Math.Floor(addBonus);
                txtBonusPoints.text = "Bonus Points: " + Player.bonusScore.ToString();
            }
        }
    }

    // ================== SpawnShip() for TESTING ONLY =================== //
    void SpawnShip()
    {
        // Create new ship GO
        GameObject spShip;  

        // Chose a random fly in point
        float randFlyInPoint = Random.value; // choose random x value for fly in point
        float leftOrRight = Random.value; // 50% change it will be negative or positive of x-axis

        // Choose to spawn a random ship
        float randShip = Random.value;

        if (randShip >= .66)
        {
            // Instantiate the ship
            spShip = Instantiate(fourPointShip, flyInPoint(), Quaternion.identity);
            lastCalled = ShipType.FourPoint;
        }
        else if (randShip >= .33)
        {
            // Instantiate the ship
            spShip = Instantiate(seekerShip, flyInPoint(), Quaternion.identity);
            lastCalled = ShipType.Seeker;
        }
        else
        {
            // Instantiate the ship
            spShip = Instantiate(gruntShip, flyInPoint(), Quaternion.identity);
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

        Invoke("SpawnHealthCloud", Random.value*60); // spawn at random times, maximum a minute
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
    void LevelOne()
    {
        sequence++;

        // Create new ship GO
        GameObject spShip;

        // Chose a random fly in point
        float randFlyInPoint = Random.value; // choose random x value for fly in point
        float leftOrRight = Random.value; // 50% change it will be negative or positive of x-axis

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

                Invoke("LevelOne", 10f);
                break;
            case 2:
                TipTextUpdate();
                for (int i = 1; i > countFP; countFP++)
                {
                    spShip = Instantiate(fourPointShip, flyInPoint(), Quaternion.identity);
                }
                Invoke("LevelOne", 15f);
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
                Invoke("LevelOne", 20f);
                break;
            case 4:
                TipTextUpdate();
                for (int i = 2; i > countFP; countFP++)
                {
                    spShip = Instantiate(fourPointShip, flyInPoint(), Quaternion.identity);
                }
                Invoke("LevelOne", 20f);
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
                Invoke("LevelOne", 20f);
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

                Invoke("LevelOne", 15f);
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

    // Third Level
    void LevelThree()
    {
        sequence++;

        // Create new ship GO
        GameObject spShip;

        // Chose a random fly in point
        float randFlyInPoint = Random.value; // choose random x value for fly in point
        float leftOrRight = Random.value; // 50% change it will be negative or positive of x-axis

        int countSeeker = 0; // number of seekers per wave
        int countFP = 0;    // number of four point ships per wave
        int countGrunt = 0;    // number of four point ships per wave

        switch (sequence)
        {
            case 1:
                TipTextUpdate();

                // 1 FP
                spShip = Instantiate(fourPointShip, flyInPoint(), Quaternion.identity);

                // 2 grunts
                spShip = Instantiate(gruntShip, new Vector3(0, -2, 85), Quaternion.identity);
                spShip = Instantiate(gruntShip, new Vector3(15, -2, 85), Quaternion.Euler(0, 90, 0));

                Invoke("LevelThree", 20f);
                break;
            case 2:
                TipTextUpdate();

                // 4 grunts
                spShip = Instantiate(gruntShip, new Vector3(0, -2, 85), Quaternion.identity);
                spShip = Instantiate(gruntShip, new Vector3(0, -2, -85), Quaternion.Euler(0, 180, 0));
                spShip = Instantiate(gruntShip, new Vector3(-95, -2, 0), Quaternion.Euler(0, 180, 0));
                spShip = Instantiate(gruntShip, new Vector3(95, -2, 0), Quaternion.Euler(0, 180, 0));

                // 1 FP
                spShip = Instantiate(fourPointShip, flyInPoint(), Quaternion.identity);

                Invoke("LevelThree", 15f);
                break;
            case 3:
                TipTextUpdate();

                // 3 seekers
                for (int i = 3; i > countSeeker; countSeeker++)
                {
                    spShip = Instantiate(seekerShip, flyInPoint(), Quaternion.identity);
                }

                Invoke("LevelThree", 20f);
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

                Invoke("LevelThree", 15f);
                break;
            case 5:
                TipTextUpdate();

                // 2 grunts
                spShip = Instantiate(gruntShip, new Vector3(0, -2, 85), Quaternion.identity);
                spShip = Instantiate(gruntShip, new Vector3(0, -2, -85), Quaternion.identity);

                // 2 FP
                spShip = Instantiate(fourPointShip, flyInPoint(), Quaternion.identity);
                spShip = Instantiate(fourPointShip, flyInPoint(), Quaternion.identity);

                Invoke("LevelThree", 20f);
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

                Invoke("LevelThree", 15f);
                break;
            case 7:
                CheckForWin();
                break;
            default:
                break;
        }
    }

    // Fourth Level
    void LevelFour()
    {
        sequence++;

        // Create new ship GO
        GameObject spShip;

        // Create new ship obj
        EnemyShip spShipObj;

        // Chose a random fly in point
        float randFlyInPoint = Random.value; // choose random x value for fly in point
        float leftOrRight = Random.value; // 50% change it will be negative or positive of x-axis

        int countSeeker = 0; // number of seekers per wave
        int countFP = 0;    // number of four point ships per wave
        int countGrunt = 0;    // number of four point ships per wave

        switch (sequence)
        {
            case 1:
                TipTextUpdate();

                // 5 seekers
                for (int i = 5; i > countSeeker; countSeeker++)
                {
                    spShip = Instantiate(seekerShip, flyInPoint(), Quaternion.identity);
                }

                Invoke("LevelFour", 20f);
                break;
            case 2:
                TipTextUpdate();
                // 2 grunts
                spShip = Instantiate(gruntShip, new Vector3(0, -2, 85), Quaternion.identity);
                spShip = Instantiate(gruntShip, new Vector3(0, -2, -85), Quaternion.identity);

                // 3 seekers
                for (int i = 3; i > countSeeker; countSeeker++)
                {
                    spShip = Instantiate(seekerShip, flyInPoint(), Quaternion.identity);
                }

                Invoke("LevelFour", 15f);
                break;
            case 3:
                TipTextUpdate();

                // 6 grunts 
                spShip = Instantiate(gruntShip, new Vector3(0, -2, 85), Quaternion.Euler(0, 60, 0));
                spShipObj = spShip.GetComponent<EnemyShip>();
                spShipObj.rotateSpeedGrunt = .25f;

                spShip = Instantiate(gruntShip, new Vector3(0, -2, -85), Quaternion.Euler(0, 120, 0));
                spShipObj = spShip.GetComponent<EnemyShip>();
                spShipObj.rotateSpeedGrunt = .25f;

                spShip = Instantiate(gruntShip, new Vector3(-95, -2, 20), Quaternion.Euler(0, 180, 0));
                spShipObj = spShip.GetComponent<EnemyShip>();
                spShipObj.rotateSpeedGrunt = .25f;

                spShip = Instantiate(gruntShip, new Vector3(95, -2, -20), Quaternion.Euler(0, 240, 0));
                spShipObj = spShip.GetComponent<EnemyShip>();
                spShipObj.rotateSpeedGrunt = .25f;

                spShip = Instantiate(gruntShip, new Vector3(-95, -2, 40), Quaternion.Euler(0, 300, 0));
                spShipObj = spShip.GetComponent<EnemyShip>();
                spShipObj.rotateSpeedGrunt = .25f;

                spShip = Instantiate(gruntShip, new Vector3(95, -2, -40), Quaternion.Euler(0, 360, 0));
                spShipObj = spShip.GetComponent<EnemyShip>();
                spShipObj.rotateSpeedGrunt = .25f;

                Invoke("LevelFour", 8f);
                break;
            case 4:
                TipTextUpdate();

                // 5 seekers
                for (int i = 5; i > countSeeker; countSeeker++)
                {
                    spShip = Instantiate(seekerShip, flyInPoint(), Quaternion.identity);
                }

                Invoke("LevelFour", 20f);
                break;
            case 5:
                TipTextUpdate();

                // 6 grunts top  
                spShip = Instantiate(gruntShip, new Vector3(0, -2, 85), Quaternion.Euler(0, 60, 0));
                spShipObj = spShip.GetComponent<EnemyShip>();
                spShipObj.rotateSpeedGrunt = .25f;

                spShip = Instantiate(gruntShip, new Vector3(0, -2.10f, 85), Quaternion.Euler(0, 120, 0));
                spShipObj = spShip.GetComponent<EnemyShip>();
                spShipObj.rotateSpeedGrunt = .25f;

                spShip = Instantiate(gruntShip, new Vector3(0, -2.20f, 85), Quaternion.Euler(0, 180, 0));
                spShipObj = spShip.GetComponent<EnemyShip>();
                spShipObj.rotateSpeedGrunt = .25f;

                spShip = Instantiate(gruntShip, new Vector3(0, -2.30f, 85), Quaternion.Euler(0, 240, 0));
                spShipObj = spShip.GetComponent<EnemyShip>();
                spShipObj.rotateSpeedGrunt = .25f;

                spShip = Instantiate(gruntShip, new Vector3(0, -2.30f, 85), Quaternion.Euler(0, 300, 0));
                spShipObj = spShip.GetComponent<EnemyShip>();
                spShipObj.rotateSpeedGrunt = .25f;

                spShip = Instantiate(gruntShip, new Vector3(0, -2.50f, 85), Quaternion.Euler(0, 360, 0));
                spShipObj = spShip.GetComponent<EnemyShip>();
                spShipObj.rotateSpeedGrunt = .25f;


                // 6 grunts bottom
                spShip = Instantiate(gruntShip, new Vector3(0, -2, -85), Quaternion.Euler(0, 60, 0));
                spShipObj = spShip.GetComponent<EnemyShip>();
                spShipObj.rotateSpeedGrunt = .25f;

                spShip = Instantiate(gruntShip, new Vector3(0, -2.10f, -85), Quaternion.Euler(0, 120, 0));
                spShipObj = spShip.GetComponent<EnemyShip>();
                spShipObj.rotateSpeedGrunt = .25f;

                spShip = Instantiate(gruntShip, new Vector3(0, -2.20f, -85), Quaternion.Euler(0, 180, 0));
                spShipObj = spShip.GetComponent<EnemyShip>();
                spShipObj.rotateSpeedGrunt = .25f;

                spShip = Instantiate(gruntShip, new Vector3(0, -2.30f, -85), Quaternion.Euler(0, 240, 0));
                spShipObj = spShip.GetComponent<EnemyShip>();
                spShipObj.rotateSpeedGrunt = .25f;

                spShip = Instantiate(gruntShip, new Vector3(0, -2.30f, -85), Quaternion.Euler(0, 300, 0));
                spShipObj = spShip.GetComponent<EnemyShip>();
                spShipObj.rotateSpeedGrunt = .25f;

                spShip = Instantiate(gruntShip, new Vector3(0, -2.50f, -85), Quaternion.Euler(0, 360, 0));
                spShipObj = spShip.GetComponent<EnemyShip>();
                spShipObj.rotateSpeedGrunt = .25f;

                Invoke("LevelFour", 10f);
                break;
            case 6:
                TipTextUpdate();

                // 4 grunts, one in each corner
                spShip = Instantiate(gruntShip, new Vector3(50, -2, 85), Quaternion.identity);
                spShipObj = spShip.GetComponent<EnemyShip>();
                spShipObj.rotateSpeedGrunt = .25f;

                spShip = Instantiate(gruntShip, new Vector3(-50, -2, 85), Quaternion.identity);
                spShipObj = spShip.GetComponent<EnemyShip>();
                spShipObj.rotateSpeedGrunt = .25f;

                spShip = Instantiate(gruntShip, new Vector3(50, -2, -85), Quaternion.identity);
                spShipObj = spShip.GetComponent<EnemyShip>();
                spShipObj.rotateSpeedGrunt = .25f;

                spShip = Instantiate(gruntShip, new Vector3(-50, -2, -85), Quaternion.identity);
                spShipObj = spShip.GetComponent<EnemyShip>();
                spShipObj.rotateSpeedGrunt = .25f;

                // 1 FP
                spShip = Instantiate(fourPointShip, flyInPoint(), Quaternion.identity);

                // 1 FP
                spShip = Instantiate(fourPointShip, flyInPoint(), Quaternion.identity);

                Invoke("LevelFour", 15f);
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
            print("End of round");
            txtRoundWin.enabled = true;
            endOfRoundBonus = Player.bonusScore;
            endOfRoundPlayerScore = EnemyShip.score;
            beginChecking = false;  // stop calling CheckForWin() in Update
            //EnemyShip.score += Player.bonusScore; // add bonus points
            Invoke("NextRound", 5f);

            if (btnAgain != null) btnAgain.SetActive(true); // tutorial only

            if (txtBonusPoints != null) // levels only
            {
                txtBonusPoints.text = "Bonus Points: " + Player.bonusScore.ToString();
            }
        }
    }

    // Updates the text box in tutorial and throughout levels for wave count
    public void TipTextUpdate()
    {
        if(sceneSequence == 0) // For the tutorial
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

        if(sceneSequence > 0) // For wave count
        {
            tipSequence++;
            tipText.text = "WAVE: " + tipSequence;
        }
    }

    // Move on to next round
    void NextRound()
    {
        txtBonusPoints.enabled = false;
        if (sceneSequence == 4)
            SceneManager.LoadScene("GameOver");
        else if (sceneSequence != 0)
            SceneManager.LoadScene("Level_"+ (sceneSequence+1).ToString());
   
    }

}
