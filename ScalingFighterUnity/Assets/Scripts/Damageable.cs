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


    public void TakeHit(Vector3 position)
    {
        AlterHealth(-20f);
        GameObject obj = (GameObject)Instantiate(AssetHolder.Instance.DamageAnimation, position, Quaternion.identity);
    }
    public override void OnTriggered(GameObject collided, Vector2 position)
    {
        base.OnTriggered(collided, position);
        if (collided.CompareTag(TagThatHurtsUs))
        {
            TakeHit(position);
        }
    }
    public override void OnCollision(GameObject collided, Vector2 position)
    {
        base.OnCollision(collided, position);
        if (collided.CompareTag(TagThatHurtsUs))
        {
            TakeHit(position);
        }
    }
    void Awake()
    {
        MaxHealth = Health;
    }
}
