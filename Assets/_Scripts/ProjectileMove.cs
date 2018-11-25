using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMove : MonoBehaviour {

    public float speed;
    public float fireRate;

	void Update () {
        if (speed != 0)
        {
            transform.position += transform.forward * (speed * Time.deltaTime);
        }
        else
        {
            Debug.Log("Speed not set");
        }
		
	}

    // Delete projectiles upon collision with object or outer boundary walls
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("COLLISION WITH: " + collision.gameObject.name);
        speed = 0;
        Destroy(gameObject); // added a "Player" layer so projectiles won't accidently be destroyed       
    }
}
