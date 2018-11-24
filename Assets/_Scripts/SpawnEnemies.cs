using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour {

    // Different types of ships
    [Header("Types of Ships")]
    public GameObject sixPointShip;
    public GameObject seekerShip;
    public GameObject gruntShip;

    // Time to spawn between ships, changes depending on last ship called
    private float spawnTime = 10f;
    private ShipType lastCalled;

    private float gameStartTime;
    

    // set points where ships fly in from 
    private Vector3 flyInPointOne = new Vector3(-50, -2, 85);  // from left
    private Vector3 flyInPointTwo = new Vector3(0, -2, 85);    // from middle
    private Vector3 flyInPointThree = new Vector3(50, -2, 85); // from right

    void Start () {
        Invoke("SpawnShip", 5); // first ship spawned
        gameStartTime = Time.time;
    }
	
	void Update () {
        
	}

    void SpawnShip()
    {
        // Create new ship GO
        GameObject spShip;  

        // Chose a random fly in point
        float randFlyInPoint = Random.value;
        Vector3 flyInPointV;

        if (randFlyInPoint >= .66)
        {
            flyInPointV = flyInPointOne;
        }
        else if (randFlyInPoint >= .33)
        {
            flyInPointV = flyInPointTwo;
        }
        else
        {
            flyInPointV = flyInPointThree;
        }

        // Choose to spawn a random ship
        float randShip = Random.value;

        if (randShip >= .66)
        {
            // Instantiate the ship
            spShip = Instantiate(sixPointShip, flyInPointV, Quaternion.identity);
            lastCalled = ShipType.SixPoint;
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
            case ShipType.SixPoint:
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
    
}
