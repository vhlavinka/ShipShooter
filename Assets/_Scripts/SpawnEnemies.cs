using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour {

    // Different types of ships
    [Header("Types of Ships")]
    public GameObject fourPointShip;
    public GameObject seekerShip;
    public GameObject gruntShip;

    [Header("Spawn other objects")]
    public GameObject healthCloud; // allows player to regain health upon collision
    public float healthCloudProbability = 0.05f;

    // Time to spawn between ships, changes depending on last ship called
    private float spawnTime = 10f;
    private ShipType lastCalled;

    [Header("Random positions for other objects")]
    private Vector3 randCloudPos;
    private float randHealthCloud;


    private float gameStartTime;

    void Start () {
        Invoke("SpawnShip", 5f); // first ship spawned
        gameStartTime = Time.time;

        Invoke("SpawnHealthCloud", 10f); // spawn health cloud after one minute
    }
	
	void Update () {
        
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
}
