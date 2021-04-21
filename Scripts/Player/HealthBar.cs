using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public float currentHealth;
    private float maxHealth = 100f;
    private Image healthBar;

    CharacterLocomotion playerScript;

    // Start is called before the first frame update
    void Start()
    {
        healthBar = GetComponent<Image>();
        playerScript = FindObjectOfType<CharacterLocomotion>();
    }

    // Update is called once per frame
    void Update()
    {
        currentHealth = playerScript.health;
        healthBar.fillAmount = currentHealth / maxHealth;
    }
}
