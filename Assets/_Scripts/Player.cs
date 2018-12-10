using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Player : MonoBehaviour {

    [Header("Player Stats")]
    public float speed = 30f; // speed of ship movement
    public GameObject healthBar;

    [Header("Utilities")]
    public GameObject forceField;  // absorbs enemy projectiles when activated

    [Header("Score Text")]
    public Text scoreText;

    [Header("Sounds")]
    public AudioClip playerHitSound;
    public AudioClip gainHealthSound;

    [Header("Player movement and health")]
    private Quaternion rotation;
    private int playerHealth = 100;

    [Header("Audio Settings")]
    private AudioSource source;

    void Start()
    {
        // set audio source
        source = GetComponent<AudioSource>();
    }

    void Update()
    {
        // Speed-dodge when spacebar is pressed
        if (Input.GetKeyDown("space"))
        {
            speed = 70f;
            Invoke("RevertSpeedBoost", 0.5f);
            forceField.SetActive(true);
        }
            
    }

    void FixedUpdate () {
        // Movement Controls
        Vector3 pos = transform.position;

        if (Input.GetKey("w") && pos.z < 45)
        {
            pos.z += speed * Time.deltaTime;
        }
        if (Input.GetKey("s") && pos.z > -45)
        {
            pos.z -= speed * Time.deltaTime;
        }
        if (Input.GetKey("d") && pos.x < 85)
        {
            pos.x += speed * Time.deltaTime;
        }
        if (Input.GetKey("a") && pos.x > -85)
        {
            pos.x -= speed * Time.deltaTime;
        }
        transform.position = pos;

        // Make the player face the mouse
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = new Vector3(mousePos.x,
            transform.position.y,
            mousePos.z);
        rotation = Quaternion.LookRotation(direction - transform.position);
        transform.rotation = rotation;

        scoreText.text = "SCORE: " + EnemyShip.score.ToString();
    }

    // put speed back to normal
    void RevertSpeedBoost()
    {
        speed = 30f;
        forceField.SetActive(false);
    }

    public Quaternion GetRotation()
    {
        return rotation;
    }

    // if passed through health cloud, add hp back
    void OnTriggerEnter(Collider collision)
    {
        Debug.Log("Before: " + playerHealth);
        
        if (collision.name == "HealthCloud(Clone)" && playerHealth < 100)
        {
            source.PlayOneShot(gainHealthSound, 1.0f);
            playerHealth += 10;
            Destroy(collision.gameObject);
            Debug.Log("After: " + playerHealth);

            // update health bar
            healthBar.transform.localScale = new Vector3(playerHealth * Mathf.Pow(10, -2),
                        healthBar.transform.localScale.y,
                        healthBar.transform.localScale.z);
        }
    }

    // take hp away when hit with enemy projectiles
    void OnCollisionEnter(Collision collision)
    {
        // update player health
        if (!forceField.activeInHierarchy)
        {
            source.PlayOneShot(playerHitSound, 1.0f);
            playerHealth -= 10;
        }
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

