using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour {

    public float speed = 10f;
    public float chanceToChangeDirections = 0.1f;
    public float leftRightEdge = 90f;
    public float upDownEdge = 20f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 pos = transform.position;
        pos.x += speed * Time.deltaTime;
        pos.z += speed * Time.deltaTime;
        transform.position = pos;

        // Ensure ship does not move off screen
        if(pos.x < -leftRightEdge)
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


}
