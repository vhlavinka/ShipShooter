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

    // Time to spawn between ships, changes depending on last ship called
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

    void Start () {
        // Get current scene
        Scene scene = SceneManager.GetActiveScene();
        if (scene.name == "Tutorial")
        {
            // Welcome Player
            tipSequence = 0;
            tipText.text = "Welcome to ship shooter!\n This tutorial will teach you the basics.";
            Invoke("TipTextUpdate", tipTime);

            // set scene to #0
            sceneSequence = 0;

            // begin tutorial
            Invoke("Tutorial", 5f);
        }
        else if (scene.name == "LevelOne")
        {
            // set scene to #1
            sceneSequence = 1;

            // begin level
            Invoke("LevelOneTest", 5f); 
        }
        
        // track time
        gameStartTime = Time.time;

        if(sceneSequence!=0)
            Invoke("SpawnHealthCloud", 10f); // begin to spawn health clouds
    }
	
	void Update () {
        if (beginChecking) CheckForWin();
    }

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

    void Tutorial()
    {
        sequence++;

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
        switch (sequence)
        {
            case 1:
                print("Wave 1");
                for (int i = 3; i >= countSeeker; countSeeker++)
                {
                    spShip = Instantiate(seekerShip, flyInPoint(), Quaternion.identity);
                    countSeeker++;
                }
                Invoke("Tutorial", 10f);
                break;
            case 2:
                print("Wave 2");
                for (int i = 1; i >= countFP; countFP++)
                {
                    spShip = Instantiate(fourPointShip, flyInPoint(), Quaternion.identity);
                    countFP++;
                }
                Invoke("Tutorial", 15f);
                break;
            case 3:
                print("Wave 3");
                for (int i = 1; i >= countSeeker; countSeeker++)
                {
                    spShip = Instantiate(seekerShip, flyInPoint(), Quaternion.identity);
                    countFP++;
                }
                Invoke("Tutorial", 12f);
                Invoke("SpawnHealthCloud", 6f);
                break;
            case 4:
                print("Wave 4");
                Invoke("SpawnHealthCloud", 1f);
                for (int i = 2; i >= countFP; countFP++)
                {
                    spShip = Instantiate(fourPointShip, flyInPoint(), Quaternion.identity);
                    countFP++;
                }
                Invoke("Tutorial", 7f);
                break;
            case 5:
                CheckForWin();
                break;
            default:
                break;
        }
    }

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
        switch (sequence)
        {
            case 1:
                print("Wave 1");
                TipTextUpdate();
                for (int i = 4; i >= countSeeker; countSeeker++)
                {
                    spShip = Instantiate(seekerShip, flyInPoint(), Quaternion.identity);
                    countSeeker++;
                }
                Invoke("LevelOneTest", 10f);
                break;
            case 2:
                print("Wave 2");
                TipTextUpdate();
                for (int i = 1; i >= countFP; countFP++)
                {
                    spShip = Instantiate(fourPointShip, flyInPoint(), Quaternion.identity);
                    countFP++;
                }
                Invoke("LevelOneTest", 15f);
                break;
            case 3:
                print("Wave 3");
                TipTextUpdate();
                for (int i = 1; i >= countFP; countFP++)
                {
                    spShip = Instantiate(fourPointShip, flyInPoint(), Quaternion.identity);
                    countFP++;
                }
                for (int i = 2; i >= countSeeker; countSeeker++)
                {
                    spShip = Instantiate(seekerShip, flyInPoint(), Quaternion.identity);
                    countFP++;
                }
                Invoke("LevelOneTest", 20f);
                break;
            case 4:
                print("Wave 4");
                TipTextUpdate();
                for (int i = 2; i >= countFP; countFP++)
                {
                    spShip = Instantiate(fourPointShip, flyInPoint(), Quaternion.identity);
                    countFP++;
                }
                Invoke("LevelOneTest", 20f);
                break;
            case 5:
                print("Wave 5");
                TipTextUpdate();
                for (int i = 1; i >= countFP; countFP++)
                {
                    spShip = Instantiate(fourPointShip, flyInPoint(), Quaternion.identity);
                    countFP++;
                }
                for (int i = 2; i >= countSeeker; countSeeker++)
                {
                    spShip = Instantiate(seekerShip, flyInPoint(), Quaternion.identity);
                    countFP++;
                }
                Invoke("LevelOneTest", 30f);
                break;
            case 6:
                print("Wave 6");
                TipTextUpdate();
                for (int i = 2; i >= countFP; countFP++)
                {
                    spShip = Instantiate(fourPointShip, flyInPoint(), Quaternion.identity);
                    countFP++;
                }
                for (int i = 2; i >= countSeeker; countSeeker++)
                {
                    spShip = Instantiate(seekerShip, flyInPoint(), Quaternion.identity);
                    countFP++;
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

    public void CheckForWin()
    {
        beginChecking = true; // Sets to true once called from case 7

        GameObject btn = GameObject.Find("Again");
        if(btn != null) btn.SetActive(true);

        if (GameObject.Find("Seeker(Clone)") ==  null && GameObject.Find("FourPointShip(Clone)") == null)
        {
            RoundWin.enabled = true;
            print("round complete");
        }
    }

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
                    tipText.text = "These are seeker ships.\n" +
                        "You can destroy their lasers with your own.";
                    tipTime = 6f;
                    Invoke("TipTextUpdate", tipTime);
                    break;
                case 3:
                    tipText.text = "Press the left mouse button down to attack.";
                    tipTime = 3f;
                    break;
                case 4:
                    tipText.text = "This is a stronger ship.\n"+
                        "You cannot destroy it's attacks, so get ready to dodge!";
                    break;
                case 5:
                    tipText.text = "Press SPACEBAR to to become invincible and move faster for a short period of time.";
                    break;
                case 6:
                    tipText.text = "If you take damage, be sure to pick up a health pack to recover!";
                    break;
                case 7:
                    tipText.text = "That's all there is to it!";
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
}
