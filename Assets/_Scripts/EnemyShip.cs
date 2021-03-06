﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/*
 * This script handles different types of ships including their health, behavior, movement
 * Attached to Enemy Ship prefabs
 */

public enum ShipType { FourPoint, Seeker, Grunt };
// FourPoint - this ship has six fire points which fire outward
// Seeker - this ship will aim towards the player
// Grunt - this is a weaker ship

public class EnemyShip : MonoBehaviour {
    public static int score;

    [Header("Ship Movement")]
    public float flyInTime = 3.0f; // Speed the ship will fly in at
    public float speed = 10f;
    public float chanceToChangeDirections = 0.01f;
    public float leftRightEdge = 60f;
    public float upDownEdge = 20f;
    public float rotateSpeed = 0.5f;
    public float rotateSpeedGrunt = 0.25f;

    [Header("Type of Ship")]
    public ShipType type;  // Holds the type of ship 

    [Header("Floating Text")]
    public GameObject floatingTextParent;

    [Header("Death Animation/Sound")]
    public GameObject deathAnimation;

    [Header("Private")]
    private Vector3 startPos;
    private float startTime; // Start time from when ship is spawned
    private bool startMoving = false;
    private Quaternion rotation; // track rotation of ship
    
    void Awake () {
        // Set start time and position
        startTime = Time.time;
        
        startPos = transform.position;

        if (type == ShipType.FourPoint)
        {
            if (StartScreen.easyDifficulty)
                Health = 75;
            else
                Health = 150;

            Invoke("ShipMovement", 4);

        }
        else if(type == ShipType.Seeker)
        {
            if (StartScreen.easyDifficulty)
                Health = 20;
            else
                Health = 40;

            Invoke("ShipMovement", 4);
        }
        else if (type == ShipType.Grunt)
        {
            if (StartScreen.easyDifficulty)
                Health = 15;
            else
                Health = 20;

            RandVector();
            Invoke("ShipMovement", 4);
        }
    }

    void Update () {

        if (Time.time < 6f + startTime)
        {
            // Track time for Lerp
            float fracComplete = (Time.time - startTime) / flyInTime;
    
            // if it spawned top
            if (startPos.z >= 85)
                transform.position = Vector3.Lerp(startPos,
                    new Vector3(startPos.x, startPos.y, 30), fracComplete);
            // if it spawned left
            else if (startPos.x <= -90)
                transform.position = Vector3.Lerp(startPos,
                    new Vector3(-45, startPos.y, startPos.z), fracComplete);
            // if it spawned right
            else if (startPos.x >= 90)
                transform.position = Vector3.Lerp(startPos,
                    new Vector3(45, startPos.y, startPos.z), fracComplete);
            // if it spawned top
            else if (startPos.z >= -85)
                transform.position = Vector3.Lerp(startPos,
                    new Vector3(startPos.x, startPos.y, -20), fracComplete);

        }
        else
        {
            // Allow the ship to start moving when done spawning
            if (startMoving) ShipMovement();  
        }              

        // Track the ship's health
        if (gameObject != null && Health < 0)
        {
            if(deathAnimation != null)
            {
                Instantiate(deathAnimation, transform.position, Quaternion.identity);
            }

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
        if(collision.gameObject.name == "PlayerProjectile(Clone)")
        {
            Health -= 1;    // Projectile damage
            GameObject floatingText = Instantiate(floatingTextParent);
            floatingText.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform, false);
            floatingText.transform.position = Camera.main.WorldToScreenPoint(collision.transform.position);
        }
    }

    void ShipMovement()
    {
        switch(type){
            // FourPoint and Seeker ship will move randomly
            case ShipType.FourPoint:
            case ShipType.Seeker:   
                // activate the firepoints
                foreach (Transform child in transform)
                    child.gameObject.SetActive(true);

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
                break;
            case ShipType.Grunt:
                // enable laser
                foreach (Transform child in transform)
                    child.gameObject.SetActive(true);
                break;
            default:
                break;
        }
    }

    void ShipBehavior()
    {
        GameObject playerShip = GameObject.Find("Ship");
        Vector3 psPos = playerShip.transform.position;

        switch (type)
        {
            case ShipType.FourPoint:
                // this ship will fire from four points   
                transform.Rotate(new Vector3(0, rotateSpeed, 0));
                foreach (Transform child in transform)
                    if(child.name != "fourpointship") child.transform.Rotate(new Vector3(0, rotateSpeed, 0));
                break;
            case ShipType.Seeker:
                // this ship will face the player
                RotateToPlayer(gameObject, psPos);
                break;
            case ShipType.Grunt:
                // they will spin will firing a continuous laser
                transform.Rotate(new Vector3(0, rotateSpeedGrunt, 0));
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

    public Vector3 RandVector()
    {
        // Choose random x & z values
        float rx = Random.value * 80;
        float rz = Random.value * 50;

        Vector3 randomVector = new Vector3(rx * leftRightEdge, -2, rz * upDownEdge);

        return randomVector;
    }

    public void DestroyDeathAudioSource()
    {

    }

    public int Health { get; set; }
}
