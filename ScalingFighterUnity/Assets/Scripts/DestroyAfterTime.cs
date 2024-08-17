using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    public float seconds_to_live = 1f;

    void Update()
    {
        seconds_to_live -= Time.deltaTime;
        if (seconds_to_live <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
