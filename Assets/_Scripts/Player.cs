using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Player : MonoBehaviour {

    [Header("Player Stats")]
    public float speed = 30f; // speed of ship movement
    public GameObject healthBar; // UI for player health
    static public int bonusScore; // bonus for not taking damage

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
        bonusScore = 1000;
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

    // if passed through health pack, add hp back
    void OnTriggerEnter(Collider collision)
    {      
        if (collision.name == "HealthCloud(Clone)" && playerHealth < 100) // was originally a cloud, now it's a "pack"
        {
            // play the gain health sound
            source.PlayOneShot(gainHealthSound, 1.0f);

            // destroy health pack
            Destroy(collision.gameObject);

            // don't overheal past maximum health
            if (playerHealth > 90 && playerHealth < 100)
                playerHealth += (100 - playerHealth);
            else
                playerHealth += 10;

            // update health bar to reflect changes
            updateHealthBar();
        }
    }

    // take hp away when hit with enemy projectiles
    void OnCollisionEnter(Collision collision)
    {      
        // update player health
        if (!forceField.activeInHierarchy)
        {
            // sound of player being hit
            source.PlayOneShot(playerHitSound, 1.0f);

            // prevent from subtracting 10 health if player health is already < 10
            if (playerHealth < 10 && playerHealth > 0)
                playerHealth -= playerHealth;
            else
                playerHealth -= 10;

            bonusScore -= 100;
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
        EnemyShip.score = SpawnEnemies.scoreAtStartofRound; // revert score
        SceneManager.LoadScene("GameOver");
    }
}

