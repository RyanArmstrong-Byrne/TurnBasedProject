using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stats : MonoBehaviour
{
    public float maxSTRStat = 99, maxMAGStat = 99, maxDEFStat = 99, currentSTRStat = 5, currentMAGStat = 5, currentDEFStat = 5;

    public Text STRText;
    public Text MAGText;
    public Text DEFText;
    public Image STRBAR;
    public Image MAGBAR;
    public Image DEFBAR;
    public int SkillPoints = 5;
    public Text SkillPointsText;

    // If the text value changes the image fill amount changes as well
    void textToSTRStat()
    {
        STRText.text = currentSTRStat.ToString();
    }
    void textToMAGStat()
    {
        MAGText.text = currentMAGStat.ToString();
    }
    void textToDEFStat()
    {
        DEFText.text = currentDEFStat.ToString();
    }
    void UpdateSTRUI()
    {
        STRBAR.fillAmount = Mathf.Clamp01(currentSTRStat / maxSTRStat);
    }
    void UpdateMAGUI()
    {
        MAGBAR.fillAmount = Mathf.Clamp01(currentMAGStat / maxMAGStat);
    }
    void UpdateDEFUI()
    {
            DEFBAR.fillAmount = Mathf.Clamp01(currentDEFStat / maxDEFStat);
    }

    private void Update()
    {
        UpdateSTRUI();
        textToSTRStat();
        UpdateMAGUI();
        textToMAGStat();
        UpdateDEFUI();
        textToDEFStat();

    }

    public void AddToSTRStats()
    {
        currentSTRStat++;
        SkillPoints--;
        UpdateSTRUI();
        Debug.Log("STR+!");
    }
    public void AddToMAGStats()
    {
        currentMAGStat++;
        SkillPoints--;
        UpdateMAGUI();
        Debug.Log("MAG+!");
    }
    public void AddToDEFStats()
    {
        currentDEFStat++;
        SkillPoints--;
        UpdateDEFUI();
        Debug.Log("DEF+!");
    }
    void skillpointToText()
    {
        SkillPointsText.text = $"SkillPoints: {SkillPoints}";
    }
}
    




