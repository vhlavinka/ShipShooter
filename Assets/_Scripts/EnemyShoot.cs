using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *  Needs to be placed on each firepoint on enemy ship prefab
 */ 

public class EnemyShoot : MonoBehaviour
{
    public GameObject firePoint;
    public List<GameObject> vfx = new List<GameObject>();
    public EnemyShip enemyShip;

    private GameObject effectToSpawn;
    private float timeToFire = 0;


    void Start()
    {
        effectToSpawn = vfx[0];
    }

    void Update()
    {
        if (Time.time >= timeToFire)
        {
            timeToFire = Time.time + 1 / effectToSpawn.GetComponent<ProjectileMove>().fireRate;
            SpawnVFX();
        }
    }

    void SpawnVFX()
    {
        GameObject vfx;

        if (firePoint != null)
        {
            // Create the projectile
            vfx = Instantiate(effectToSpawn, firePoint.transform.position, Quaternion.identity);
            // Point projectile in same direction as mouse
            if (enemyShip != null)
            {
                vfx.transform.localRotation = enemyShip.GetRotation();
                if(enemyShip.type == ShipType.SixPoint )
                {
                    vfx.transform.localRotation = Quaternion.identity;
                }
            }
        }
        else
        {
            Debug.Log("No fire point");
        }
    }

}
