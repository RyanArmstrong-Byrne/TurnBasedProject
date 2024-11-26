using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrineMenu : MonoBehaviour
{
    public Collider trigger;
    public GameObject menu;
    private void Start()
    {
        menu.SetActive(false);
        Debug.Log("Menu False");
    }
    public void shrineMenu()
    {
        menu.SetActive(true);
    }

    public void OnTriggerEnter(Collider Player)
    {
        Debug.Log("trigger active");
        shrineMenu();
    }

    public void ExitMenu()
    {
        menu.SetActive(false);
    }



}
