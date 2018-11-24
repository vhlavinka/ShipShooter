using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour {
    public GameObject firePoint;
    public List<GameObject> vfx = new List<GameObject>();
    public Player player;

    private GameObject effectToSpawn;
    private float timeToFire = 0;


	void Start () {
        effectToSpawn = vfx[0];
	}
	
	void Update () {
        if (Input.GetMouseButton(0) && Time.time >= timeToFire)
        {
            timeToFire = Time.time + 1 / effectToSpawn.GetComponent<ProjectileMove>().fireRate;
            SpawnVFX();
        }
	}

    void SpawnVFX()
    {
        GameObject vfx;

        if(firePoint != null)
        {
            // Create the projectile
            vfx = Instantiate(effectToSpawn, firePoint.transform.position, Quaternion.identity);     
            // Point projectile in same direction as mouse
            if (player != null)
            {
                vfx.transform.localRotation = player.GetRotation();       
            }
        }
        else
        {
            Debug.Log("No fire point");
        }
    }

}
