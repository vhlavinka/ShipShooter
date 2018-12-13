using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *  Needs to be placed on each firepoint on enemy ship prefab
 */ 

public class EnemyShoot : MonoBehaviour
{
    [Header("Enemy Ship components")]
    public GameObject firePoint;
    public List<GameObject> vfx = new List<GameObject>();
    public EnemyShip enemyShip;
    
    [Header("Ignore this layer")]
    public static int layerMask;

    private GameObject effectToSpawn;
    private float timeToFire = 0;
    private LineRenderer lr;
    private Player playerShip;


    void Start()
    {
        layerMask = 1 << 9; // ignore everything except player

        if (enemyShip.type == ShipType.Grunt)
            lr = GetComponent<LineRenderer>();
        else
            effectToSpawn = vfx[0];

        playerShip = GameObject.Find("Ship").GetComponent<Player>();       
    }

    void Update()
    {
        switch (enemyShip.type)
        {
            case ShipType.Grunt:
                lr.SetPosition(0, transform.position);
                RaycastHit hit;
                print(playerShip.playerHealth);
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
                {
                    if (hit.collider && !playerShip.forceField.activeSelf)
                    {
                        // prevent the ray front extending past collider
                        lr.SetPosition(1, hit.point);
                        
                        // update playerHealth
                        playerShip.playerHealth -= 0.33f;
                        playerShip.updateHealthBar();
                    }
                }
                else
                {
                    lr.SetPosition(1, transform.forward * 5000);
                }

                break;
            case ShipType.FourPoint:
            case ShipType.Seeker:
                if (Time.time >= timeToFire)
                {
                    timeToFire = Time.time + 1 / effectToSpawn.GetComponent<ProjectileMove>().fireRate;
                    SpawnVFX();
                }
                break;
            default:
                break;
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
                if(enemyShip.type == ShipType.FourPoint )
                {
                    vfx.transform.localRotation = transform.localRotation;
                }
            }
        }
        else
        {
            Debug.Log("No fire point");
        }
    }

}
