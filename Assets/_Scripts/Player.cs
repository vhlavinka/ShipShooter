using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public float speed = 30f; // speed of ship movement
    public GameObject healthBar;

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
        print(playerHealth);
        playerHealth -= 10;

        // update health bar
        healthBar.transform.localScale -= new Vector3(0.1f, 0, 0);

        // make certain projectiles do more damage
    }
}

