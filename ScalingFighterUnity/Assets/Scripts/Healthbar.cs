using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


/// <summary>
/// Healthbar showing player health. Communal. ANY player character taking a hit lowers this
/// </summary>
public class Healthbar : MonoBehaviour
{
    public TextMeshProUGUI HealthText;
    float prevPlayerHealth;
    /// <summary>
    /// Enable at low health
    /// </summary>
    public GameObject LowHealthAlert;

    void Update()
    {
        HealthText.text = "" + (int)(this.transform.localScale.x * 100f);
        if (this.transform.localScale.x < 0.05f)
        {
            // Game over!
            FightManager.Instance.GameOver();
        }
        LowHealthAlert.SetActive(this.transform.localScale.x < 0.4f);
    }
    /// <summary>
    /// Set player health based on current scale
    /// </summary>
    public void TakeHit()
    {
        this.transform.localScale -= Vector3.one * 0.1f;
    }

    public static Healthbar Instance;
    void Awake()
    {
        Instance = this;
    }
}
