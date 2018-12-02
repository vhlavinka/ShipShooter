using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Player : MonoBehaviour {

    [Header("Player Stats")]
    public float speed = 30f; // speed of ship movement
    public GameObject healthBar;

    [Header("Score Text")]
    public Text scoreText;

    //private Rigidbody rb;
    private Quaternion rotation;
    private int playerHealth = 100;


	void Start () {
        //rb = GetComponent<Rigidbody>();
	}
	
	void FixedUpdate () {
        // Movement Controls
        Vector3 pos = transform.position;

        if (Input.GetKey("w"))
        {
            pos.z += speed * Time.deltaTime;
        }
        if (Input.GetKey("s"))
        {
            pos.z -= speed * Time.deltaTime;
        }
        if (Input.GetKey("d"))
        {
            pos.x += speed * Time.deltaTime;
        }
        if (Input.GetKey("a"))
        {
            pos.x -= speed * Time.deltaTime;
        }
        transform.position = pos;

        // Make it face the mouse
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector3 direction = new Vector3(mousePos.x,
            transform.position.y,
            mousePos.z);
        rotation = Quaternion.LookRotation(direction - transform.position);
        transform.rotation = rotation;

        // Speed-dodge when spacebar is pressed
        if (Input.GetKeyDown("space"))
        {
            speed = 70f;
            Invoke("RevertSpeedBoost", 0.5f);
        }

        scoreText.text = "SCORE: " + EnemyShip.score.ToString();
    }

    // put speed back to normal
    void RevertSpeedBoost()
    {
        speed = 30f;
    }

    public Quaternion GetRotation()
    {
        return rotation;
    }
    
    // take hp away when hit with enemy projectiles
    void OnCollisionEnter(Collision collision)
    {
        // update player health
        playerHealth -= 10;

        // end game if health bar = 0 
        if (playerHealth == 0)
        {
            SceneManager.LoadScene("GameOver");
        }

        // update health bar
        healthBar.transform.localScale = new Vector3(playerHealth * Mathf.Pow(10, -2),
                    healthBar.transform.localScale.y,
                    healthBar.transform.localScale.z);

    }
}

