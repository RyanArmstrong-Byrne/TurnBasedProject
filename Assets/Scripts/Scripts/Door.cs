using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Door : MonoBehaviour
{
    public GameObject keyINV_sprite;
    public GameObject keyINV;
    public GameObject DoorOBJ;
    public bool isOpen;
    public bool nearDoor;
    [SerializeField] Text _interactionText;


    private void Start()
    {
        isOpen = false;
        DoorOBJ = this.transform.gameObject;
        DoorOBJ.SetActive(true);
        keyINV.SetActive(false);

        _interactionText = GameObject.Find("InteractionText").GetComponent<Text>();
        _interactionText.enabled = false;
    }


    void Update()
    {
        if (GameManager.instance.state == GameStates.PlayerTurn)
        {
            if (nearDoor)
            {
                if (!_interactionText.enabled)
                {
                    _interactionText.enabled = true;
                }

                if (Input.GetButtonUp("Interaction"))
                {
                    Debug.Log("Interaction");
                    if (keyINV.activeInHierarchy)
                    {
                        nearDoor = false; 
                        keyINV_sprite.SetActive(false);
                        _interactionText.enabled = false;
                        isOpen = !isOpen;
                        DoorOBJ.SetActive(false);
                    }
                    else
                    {
                        Debug.Log("Door Locked");
                        StartCoroutine(InteractionTextChange("Door Locked - Need key."));
                    }
                }
            }
        }
    }

    public void HideText()
    {
        _interactionText.enabled = false;
    }

    IEnumerator InteractionTextChange(string textDisplay)
    {
        _interactionText.text = textDisplay;
        yield return new WaitForSeconds(3f);
        _interactionText.text = "Press E to interact";
    }
}