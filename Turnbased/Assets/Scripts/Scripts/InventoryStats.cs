using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryStats : MonoBehaviour
{
    public GameObject inventory;
    public Text STRText;
    public Text MAGText;
    public Text DEFText;
    public Image STRBAR;
    public Image MAGBAR;
    public Image DEFBAR;

    public float maxSTRStat = 99, maxMAGStat = 99, maxDEFStat = 99, currentSTRStat, currentMAGStat, currentDEFStat;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (inventory.activeInHierarchy)
            {
                inventory.SetActive(false);
            }
            else
            {
                inventory.SetActive(true);
                GetStats();
            }
        }
    }

   
    public void GetStats()
    {
        currentSTRStat = PlayerHandler.instance.strengthREF;
        currentMAGStat = PlayerHandler.instance.magicREF;
        currentDEFStat = PlayerHandler.instance.defenceREF;
        UpdateSTRUI();
        UpdateMAGUI();
        UpdateDEFUI();
    }

    void UpdateSTRUI()
    {
        STRText.text = currentSTRStat.ToString();
        STRBAR.fillAmount = Mathf.Clamp01(currentSTRStat / maxSTRStat);
    }
    void UpdateMAGUI()
    {
        MAGText.text = currentMAGStat.ToString();
        MAGBAR.fillAmount = Mathf.Clamp01(currentMAGStat / maxMAGStat);
    }
    void UpdateDEFUI()
    {
        DEFText.text = currentDEFStat.ToString();
        DEFBAR.fillAmount = Mathf.Clamp01(currentDEFStat / maxDEFStat);
    }
}