using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    void Start () {
        Invoke("LevelOneTest", 5f); // first ship spawned
        gameStartTime = Time.time;

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
            randCloudPos = new Vector3(Random.value * 50, -2, Random.value * 50);
        }
        else
        {
            randCloudPos = new Vector3(Random.value * -50, -2, Random.value * -50);
        }
        
        Instantiate(healthCloud, randCloudPos, Quaternion.identity);

        Invoke("SpawnHealthCloud", Random.value*60);
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
                for (int i = 4; i >= countSeeker; countSeeker++)
                {
                    spShip = Instantiate(seekerShip, flyInPoint(), Quaternion.identity);
                    countSeeker++;
                }
                Invoke("LevelOneTest", 10f);
                break;
            case 2:
                print("Wave 2");
                for (int i = 1; i >= countFP; countFP++)
                {
                    spShip = Instantiate(fourPointShip, flyInPoint(), Quaternion.identity);
                    countFP++;
                }
                Invoke("LevelOneTest", 15f);
                break;
            case 3:
                print("Wave 3");
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
                for (int i = 2; i >= countFP; countFP++)
                {
                    spShip = Instantiate(fourPointShip, flyInPoint(), Quaternion.identity);
                    countFP++;
                }
                Invoke("LevelOneTest", 20f);
                break;
            case 5:
                print("Wave 5");
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

        if (GameObject.Find("Seeker(Clone)") ==  null && GameObject.Find("FourPointShip(Clone)") == null)
        {
            RoundWin.enabled = true;
        }

    }
}
