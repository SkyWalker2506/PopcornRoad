using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Health")]
    [SerializeField]
    float maxHealth;
    float currentHealth;
    float CurrentHealth
    {
        get
        {
             return currentHealth;
        }
        set
        {
            currentHealth = Mathf.Max(0,Mathf.Min(value, maxHealth));
            UpdatePlayerColor();
            if (currentHealth==0)
            {
                PlayerIsDead();
            }
        }
    }
    float healthPercentage
    {
        get
        {
            return CurrentHealth / maxHealth;
        }
    }
    [SerializeField]
    float regenerationSpeed;

    public bool isPlayerHealthFull
    {
        get
        {
            return maxHealth == CurrentHealth;
        }
    }

    public static Action PlayerIsDead;

    [Header("Color")]
    Material material;
    [SerializeField]
    Color coldestColor = Color.blue;
    [SerializeField]
    Color hotestColor = Color.red;

    void Awake()
    {
        material = GetComponent<MeshRenderer>().material;
    }

    private void OnEnable()
    {
        Ice.HitToPlayer += UpdatePlayerHealth;
    }

    private void OnDisable()
    {
        Ice.HitToPlayer -= UpdatePlayerHealth;
    }

    private void FixedUpdate()
    {
        if(!isPlayerHealthFull)
            UpdatePlayerHealth(regenerationSpeed * Time.deltaTime);
    }

    public void InitializePlayerHealth()
    {
        CurrentHealth = maxHealth;
    }

    void UpdatePlayerHealth(float value)
    {
        CurrentHealth += value;
    }

    void UpdatePlayerColor()
    {
        material.color = coldestColor * (1 - healthPercentage) + hotestColor * healthPercentage;
    }

}
