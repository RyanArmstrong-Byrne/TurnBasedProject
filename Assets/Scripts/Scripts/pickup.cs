using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickup : MonoBehaviour
{
    public GameObject inGameItem;
    public GameObject itemInInv;
    public GameObject itemImage;
    public AudioSource pickupSFX;
    public bool potions = false;

    public AudioSource PotionSFX;

    private void Start()
    {
        inGameItem.SetActive(true);
        itemInInv.SetActive(false);
    }
    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            itemInInv.SetActive(true);
            inGameItem.SetActive(false);
            itemImage.SetActive(true);
            pickupSFX.Play();
            if (potions)
            {
                PlayerHandler.instance.potionsREF += 1;
                Debug.Log("This is a potion...");
                PotionSFX.Play();
            }
        }
    }
}
