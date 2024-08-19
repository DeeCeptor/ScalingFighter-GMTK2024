using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Damageable : ITriggerable
{
    public float Health = 3f;
    public float MaxHealth = 3f;
    public string TagThatHurtsUs;
    public TextMeshPro HealthText;
    public bool Dead = false;
    public Animator Anims;

    public void AlterHealth(float amount)
    {
        Health = Mathf.Clamp(Health + amount, 0f, MaxHealth);
        Debug.Log(this.transform.name + " took damage: " + amount + " remaining health " + Health, this.gameObject);
        if (Health <= 0f)
        {
            Die();
        }
    }
    public void Die()
    {
        Debug.Log(this.transform.name + " died", this.gameObject);
        FightManager.Instance.AddToScore(FightManager.ScorePerEnemy);
        this.Dead = true;
        Destroy(this.gameObject);
    }

    private void Update()
    {
        if (HealthText != null)
            HealthText.text = "" + Health.ToString("0.0");
    }


    /// <summary>
    /// Deal damage based on SCALE of enemy
    /// </summary>
    /// <param name="position"></param>
    /// <param name="from"></param>
    public virtual void TakeHit(Vector3 position, GameObject from)
    {
        if (Anims != null)
        {
            Anims.SetBool("IsSlapped", true);
        }
        float damage = -20f;
        if (from != null)
        {
            // Check scale of enemy to influence damage
            Damageable unitHittingUs = from.GetComponentInParent<Damageable>();
            if (unitHittingUs != null)
            {
                damage = -Mathf.Abs(unitHittingUs.transform.localScale.x);
                Debug.Log("TakeHit from scaled object " + unitHittingUs.transform.name + " " + damage, unitHittingUs.gameObject);
            }
        }
        AlterHealth(damage);
        GameObject obj = (GameObject)Instantiate(AssetHolder.Instance.DamageAnimation, position, Quaternion.identity);
    }
    public override void OnTriggered(GameObject collided, Vector3 position)
    {
        base.OnTriggered(collided, position);
        if (collided.CompareTag(TagThatHurtsUs))
        {
            TakeHit(position, collided);
        }
    }
    public override void OnCollision(GameObject collided, Vector3 position)
    {
        base.OnCollision(collided, position);
        if (collided.CompareTag(TagThatHurtsUs))
        {
            TakeHit(position, collided);
        }
    }
    void Awake()
    {
        MaxHealth = Health;
    }

    private void OnEnable()
    {
        FightManager.Instance.RegisterTarget(this.gameObject);
    }
    private void OnDisable()
    {
        FightManager.Instance.RemoveTarget(this.gameObject);
    }
    private void OnDestroy()
    {
        FightManager.Instance.RemoveTarget(this.gameObject);
    }
}
