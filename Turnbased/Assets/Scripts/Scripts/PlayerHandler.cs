using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHandler : MonoBehaviour
{
    [SerializeField] public Stat health, strength, magic, defence;

}


[System.Serializable]
public struct Stat
{
    public int currentValue;
    public float maxValue;
}

[System.Serializable]
public struct StatDisplay
{
    [SerializeField]
    public Stat stat;
    public string displayName;
    public Text displayText;
    public Image displayBar;
}