using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

public class ShrineMenu : MonoBehaviour
{
    public GameObject menu;
    //public StatDisplay health, strength, magic, defence;
    public float maxSTRStat = 99, maxMAGStat = 99, maxDEFStat = 99, currentSTRStat, currentMAGStat, currentDEFStat;
    [SerializeField] int _upgradeValue = 5;
    public Text STRText;
    public Text MAGText;
    public Text DEFText;
    public Image STRBAR;
    public Image MAGBAR;
    public Image DEFBAR;
    public int SkillPoints = 5;
    public Text SkillPointsText;
    [SerializeField] bool exit = false;
    [SerializeField] bool inside = false;

    public AudioSource ShrineSFX;
    public AudioSource StatUpSFX;

    private void Start()
    {
        menu.SetActive(false);
        Debug.Log("Menu False");
        skillpointToText(0);
    }
    public void shrineMenu()
    {
        inside = true;
        StatsUpdateCurrent();
        menu.SetActive(true);
        
        Debug.Log("Menu true");
    }

    public void StatsUpdateCurrent()
    {
        currentSTRStat = PlayerHandler.instance.strengthREF;
        currentMAGStat = PlayerHandler.instance.magicREF;
        currentDEFStat = PlayerHandler.instance.defenceREF;
        UpdateSTRUI();
        UpdateMAGUI();
        UpdateDEFUI();
    }

    public void StatsUpdateREF()
    {
        PlayerHandler.instance.strengthREF = currentSTRStat;
        PlayerHandler.instance.magicREF = currentMAGStat;
        PlayerHandler.instance.defenceREF = currentDEFStat;
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (!exit)
            {
                if (!inside)
                {
                    Debug.Log("trigger active");
                    shrineMenu();
                }
            }
            
        }
    }

    public void ExitMenu()
    {
        StatsUpdateREF();   
        ShrineSFX.Play();
        inside = false;
        exit = true;
        menu.SetActive(false);
    }

    

    // If the text value changes the image fill amount changes as well
    void UpdateSTRUI()
    {
        STRText.text = ($" STR: {currentSTRStat}");
        STRBAR.fillAmount = Mathf.Clamp01(currentSTRStat / maxSTRStat);
    }
    void UpdateMAGUI()
    {
        MAGText.text = ($" MAG: {currentMAGStat}");
        MAGBAR.fillAmount = Mathf.Clamp01(currentMAGStat / maxMAGStat);
    }
    void UpdateDEFUI()
    {
        DEFText.text = ($" DEF: {currentDEFStat}");
        DEFBAR.fillAmount = Mathf.Clamp01(currentDEFStat / maxDEFStat);
    }

    public void AddToSTRStats()
    {
        if (SkillPoints > 0)
        {
            currentSTRStat += _upgradeValue;
            skillpointToText(1);
            UpdateSTRUI();
            Debug.Log("STR+!");
            StatUpSFX.Play();
        }
      
    }
    public void AddToMAGStats()
    {
        if (SkillPoints > 0)
        {
            currentMAGStat += _upgradeValue;
            skillpointToText(1);
            UpdateMAGUI();
            Debug.Log("MAG+!");
            StatUpSFX.Play();
        }
      
    }
    public void AddToDEFStats()
    {
        if (SkillPoints > 0)
        {
            currentDEFStat += _upgradeValue;
            skillpointToText(1);
            UpdateDEFUI();
            Debug.Log("DEF+!");
            StatUpSFX.Play();
        }
    }
    void skillpointToText(int value)
    {
        SkillPoints -= value;
        SkillPointsText.text = $"SkillPoints: {SkillPoints}";
    }

}
