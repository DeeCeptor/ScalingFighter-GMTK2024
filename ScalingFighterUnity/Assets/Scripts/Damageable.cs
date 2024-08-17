using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : ITriggerable
{
    public float Health = 3f;
    public float MaxHealth = 3f;
    public string TagThatHurtsUs;


    public void AlterHealth(float amount)
    {
        Health = Mathf.Clamp(Health + amount, 0f, MaxHealth);
        Debug.Log(this.transform.name + " took damage: " + amount + " remaining health " + Health, this.gameObject);
        if (Health <= 0f)
        {
            Debug.Log(this.transform.name + " died", this.gameObject);
            Destroy(this.gameObject);
        }
    }
    public override void OnCollision(GameObject collided)
    {
        base.OnCollision(collided);
        if (collided.CompareTag(TagThatHurtsUs))
        {
            AlterHealth(-1f);
        }
    }
    void Awake()
    {
        MaxHealth = Health;
    }
}
