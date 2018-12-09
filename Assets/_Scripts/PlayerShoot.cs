using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour {
    [Header("Player firing point")]
    public GameObject firePoint;
    public Player player;

    [Header("Add 1 to the list and set projectile")]
    public List<GameObject> vfx = new List<GameObject>();

    [Header("Force Field")]
    public GameObject forceField;

    [Header("Set Audio Clip")]
    public AudioClip shootSound;

    [Header("Audio Settings")]
    private AudioSource source;

    [Header("Set Dynamically")]
    private GameObject effectToSpawn;
    private float timeToFire = 0;

    void Awake()
    {
        // set audio source
        source = GetComponent<AudioSource>();
    }

    void Start () {
        effectToSpawn = vfx[0];
    }
	
	void Update () {
        if (Input.GetMouseButton(0) && Time.time >= timeToFire)
        {
            if(gameObject.name=="FirepointRight" && !forceField.activeSelf) source.PlayOneShot(shootSound, 1.0f);
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
