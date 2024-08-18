using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouseRaycaster : MonoBehaviour
{

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100))
        {
            Debug.Log("Hit " + hit.transform.name, hit.transform.gameObject);
        }
    }
}
