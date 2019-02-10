using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAnimation : MonoBehaviour {

    private float timer = 0;

	void Update () {
        timer += Time.deltaTime;

        if (timer >= 4f)
        {
            Destroy(gameObject);
        }

    }
}
