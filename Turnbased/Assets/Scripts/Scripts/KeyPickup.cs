using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    public GameObject KeyInGame;
    public GameObject KeyOutGame;

    private void Start()
    {
        KeyInGame.SetActive(true);
        KeyOutGame.SetActive(false);
    }
    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            KeyOutGame.SetActive(true);
            KeyInGame.SetActive(false);
        }
    }
}
