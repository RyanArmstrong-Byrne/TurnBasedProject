using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UI;

public class EnemyHandler : MonoBehaviour
{
    
    public static EnemyHandler instance;
    [SerializeField] private EnemyStatDisplay enemyStatDisplay = new EnemyStatDisplay();

    public float healthREF;
    public float strengthREF;
    public float magicREF;
    public float defenceREF;

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
        healthREF = enemyStatDisplay.enemyHealth;
        strengthREF = enemyStatDisplay.enemyStrength;
        magicREF = enemyStatDisplay.enemyMagic;
        defenceREF = enemyStatDisplay.enemyDefence;
    }
}


[System.Serializable]
public class EnemyStatDisplay
{
    [SerializeField]
    public float enemyHealth = 100;
    public float enemyStrength = 25;
    public float enemyMagic = 10;
    public int enemyDefence = 15;
}