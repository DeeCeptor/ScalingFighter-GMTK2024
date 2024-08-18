using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCollisionTriggerable : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.TryGetComponent<ITriggerable>(out ITriggerable triggerable) == true)
            triggerable.OnCollision(this.gameObject, other.GetContact(0).point);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.TryGetComponent<ITriggerable>(out ITriggerable triggerable) == true)
            triggerable.OnTriggered(this.gameObject, other.ClosestPoint(this.transform.position));
    }


    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.TryGetComponent<ITriggerable>(out ITriggerable triggerable) == true)
            triggerable.OnCollision(this.gameObject, other.GetContact(0).point);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.TryGetComponent<ITriggerable>(out ITriggerable triggerable) == true)
            triggerable.OnTriggered(this.gameObject, other.ClosestPoint(this.transform.position));
    }
}
