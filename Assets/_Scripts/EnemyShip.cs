using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This script handles different types of ships including their health, behavior, movement
 * Attached to Enemy Ship prefabs
 */

public enum ShipType { SixPoint, Seeker, Grunt };
// SixPoint - this ship has six fire points which fire outward
// Seeker - this ship will aim towards the player
// Grunt - this is a weaker ship

public class EnemyShip : MonoBehaviour {
    public static int score;

    [Header("Ship Movement")]
    public float flyInTime = 3.0f; // Speed the ship will fly in at
    public float speed = 10f;
    public float chanceToChangeDirections = 0.01f;
    public float leftRightEdge = 90f;
    public float upDownEdge = 20f;

    private Vector3 startPos;
    private ShipType type;  // Holds the type of ship 
    private float startTime; // Start time from when ship is spawned
    private bool startMoving = false;
    private Quaternion rotation; // track rotation of ship

    void Awake () {
        // Set start time and position
        startTime = Time.time;
        startPos = transform.position;

        if (gameObject.name == "Target(Clone)")
        {
            type = ShipType.SixPoint;
            Health = 75;
        }
        else if(gameObject.name == "Seeker(Clone)")
        {
            type = ShipType.Seeker;
            Health = 40;
            
        }
        else if (gameObject.name == "Grunt(Clone)")
        {
            type = ShipType.Grunt;
            Health = 15;
        }

        Invoke("ShipMovement", 6);

    }
	
	void Update () {
        //Debug.Log(Time.time);

        if(Time.time < 6f + startTime)
        {
            // Make ship fly in from top
            float fracComplete = (Time.time - startTime) / flyInTime;

            transform.position = Vector3.Lerp(startPos,
                new Vector3(startPos.x, startPos.y, 30), fracComplete);
        }
        else
        {
            // Allow the ship to start moving when done spawning
            if (startMoving) ShipMovement();
        }              

        // Track the ship's health
        if (gameObject != null && Health < 0)
        {
            Destroy(gameObject);
            score += 100;
        }

        // Controls how each ship behaves
        ShipBehavior();
    }

    void FixedUpdate()
    {
        if (Random.value < chanceToChangeDirections)
        {
            speed *= -1; // Change direction
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
        if(collision.gameObject.name == "PlayerProjectile(Clone)")
        {
            Health -= 1;    // Projectile damage
            //Debug.Log(Health);
        }
    }

    void ShipMovement()
    {
        startMoving = true; // make sure ship is allowed to start moving
  
        Vector3 pos = transform.position;
        pos.x += speed * Time.deltaTime;
        pos.z += speed * Time.deltaTime;
        transform.position = pos;

        // Ensure ship does not move off screen
        if (pos.x < -leftRightEdge)
        {
            speed = Mathf.Abs(speed);
        }
        else if (pos.x > leftRightEdge)
        {
            speed = -Mathf.Abs(speed);
        }

        if (pos.z < -upDownEdge)
        {
            speed = Mathf.Abs(speed);
        }
        else if (pos.z > upDownEdge)
        {
            speed = -Mathf.Abs(speed);
        }
    }

    void ShipBehavior()
    {
        GameObject playerShip = GameObject.Find("Ship");
        Vector3 psPos = playerShip.transform.position;

        switch (type)
        {
            case ShipType.SixPoint:
                // this ship will fire from six points
                break;
            case ShipType.Seeker:
                // this ship will face the player
                RotateToPlayer(gameObject, psPos);
                break;
            case ShipType.Grunt:
                RotateToPlayer(gameObject, psPos);
                break;
            default:
                break;
        }
    }

    void RotateToPlayer(GameObject obj, Vector3 destination) // obj = enemyship, destination= player vector
    {
        Vector3 direction = destination - obj.transform.position;
        rotation = Quaternion.LookRotation(direction);
        obj.transform.localRotation = Quaternion.Lerp(obj.transform.rotation, rotation, 1);
    }

    public Quaternion GetRotation()
    {
        return rotation;
    }

    public int Health { get; set; }
}
