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
    public Quaternion rotation;
    public float playerHealth = 100;

    [Header("Audio Settings")]
    private AudioSource source;

    private bool playerDied;

    void Start()
    {
        // set audio source
        source = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (!playerDied)
        {
            // Speed-dodge when spacebar is pressed
            if (Input.GetKeyDown("space"))
            {
                speed = 70f;
                Invoke("RevertSpeedBoost", 0.5f);
                forceField.SetActive(true);
            }
        }
    }

    void FixedUpdate () {
        if (!playerDied)
        {
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
        }

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

            updateHealthBar();
        }
    }

    // take hp away when hit with enemy projectiles
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision);
        
        // update player health
        if (!forceField.activeInHierarchy)
        {
            source.PlayOneShot(playerHitSound, 1.0f);
           
            if (playerHealth < 10 && playerHealth > 0)
                playerHealth -= playerHealth;   // prevent from subtracting 10 health if player health is already < 10
            else
                playerHealth -= 10;
        }

        updateHealthBar();

    }

    // update the health bar, also used in EnemyShoot for grunt laser
    public void updateHealthBar()
    {
        if(playerHealth >= 0)
        {
            // update health bar
            healthBar.transform.localScale = new Vector3(playerHealth * Mathf.Pow(10, -2),
                        healthBar.transform.localScale.y,
                        healthBar.transform.localScale.z);
        }
        else
        {
            playerDied = true;
            Invoke("LoadGameOver", 2f);
        }
    }

    void LoadGameOver()
    {
        SceneManager.LoadScene("GameOver");
    }
}

