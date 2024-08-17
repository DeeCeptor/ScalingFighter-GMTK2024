using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


/// <summary>
/// Healthbar showing player health. Hold click to regrow/heal
/// </summary>
public class Healthbar : MonoBehaviour
{
    public TextMeshProUGUI HealthText;
    float prevPlayerHealth;

    void Update()
    {
        // If player health changed, change scale
        if (Player.Instance.PlayerDamage.Health != prevPlayerHealth)
        {
            this.transform.localScale = Player.Instance.PlayerDamage.Health * Vector3.one / 100f;
        }
        HealthText.text = "" + (int)(this.transform.localScale.x * 100f);
        prevPlayerHealth = Player.Instance.PlayerDamage.Health;
    }
    /// <summary>
    /// Set player health based on current scale
    /// </summary>
    public void PlayerClickedHealthbar()
    {
        Player.Instance.PlayerDamage.Health = (this.transform.localScale.x * 100f);
    }

    public static Healthbar Instance;
    void Awake()
    {
        Instance = this;
    }
}
