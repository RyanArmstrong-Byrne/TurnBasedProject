using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UI;

public class PlayerHandler : MonoBehaviour
{
    
    public static PlayerHandler instance;
    [SerializeField] private StatDisplay statDisplay = new StatDisplay();

    public float healthREF;
    public float strengthREF;
    public float magicREF;
    public float defenceREF;
    public float PotionsREF;

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
        healthREF = statDisplay.health;
        strengthREF = statDisplay.strength;
        magicREF = statDisplay.magic;
        defenceREF = statDisplay.defence;
        PotionsREF = statDisplay.potions;
    }
}


[System.Serializable]
public class StatDisplay
{
    [SerializeField]
    public float health = 100;
    public float strength = 3;
    public float magic = 2;
    public float potions = 0;
    public int defence = 5;
    public string displayName;
}