using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class useful for collisions that don't need to know what class/script collided with something. Override OnTriggered in child classes
/// </summary>
public class ITriggerable : MonoBehaviour
{
    public virtual void OnTriggered(GameObject entered)
    {

    }

    public virtual void OnCollision(GameObject collided)
    {

    }

    /*
    // Call like this:
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<ITriggerable>(out ITriggerable triggerable) == true)
            triggerable.OnTriggered(this.gameObject);
    }
    private void OnCollisionEnter(Collider other)
    {
        if (other.TryGetComponent<ITriggerable>(out ITriggerable triggerable) == true)
            triggerable.OnTriggered(this.gameObject);
    }
    */
}
