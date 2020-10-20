using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParameters : MonoBehaviour
{
    public float CurrentHealth = 100f, MaxHealth = 100f;

    MenuManager menu;

    private void Start()
    {
        if (CurrentHealth > MaxHealth) CurrentHealth = MaxHealth;
        menu = FindObjectOfType<MenuManager>();
        menu.ChangeHealth(CurrentHealth);
    }

    public void TakeDamage(float Damage)
    {
        CurrentHealth -= Damage;
        menu.ChangeHealth(CurrentHealth);
        // if (CurrentHealth < 0) defeat
    }
}
