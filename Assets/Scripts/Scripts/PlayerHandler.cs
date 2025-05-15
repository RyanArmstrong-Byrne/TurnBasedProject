using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UI;

public class PlayerHandler : MonoBehaviour
{
    
    public static PlayerHandler instance;
    [SerializeField] private StatDisplay statDisplay = new StatDisplay();

    public float maxhealthREF;
    public float healthREF;
    public float strengthREF;
    public float magicREF;
    public float defenceREF;
    public float potionsREF;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null && instance != this)
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        maxhealthREF = statDisplay.maxHealth;
        healthREF = statDisplay.health;
        strengthREF = statDisplay.strength;
        magicREF = statDisplay.magic;
        defenceREF = statDisplay.defence;
        potionsREF = statDisplay.potions;
    }
}


[System.Serializable]
public class StatDisplay
{
    [SerializeField]
    public float health = 100;
    public float maxHealth = 100;
    public float strength = 5;
    public float magic = 0;
    public float potions = 0;
    public int defence = 5;
}