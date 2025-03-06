using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battle : MonoBehaviour
{
    public GameObject battleCamera;
    public GameObject playerCamera;
    public GameObject battleButton;
    public void battleStart()
    {
        GameManager.instance.state = GameStates.Battle;
        battleCamera.SetActive(true);
        playerCamera.SetActive(false);
        battleButton.SetActive(false);
    }
}
