using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public GameObject pausemenu;
    // Update is called once per frame
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (pausemenu.activeInHierarchy)
            {
                pausemenu.SetActive(false);
            }
            else
            {

                pausemenu.SetActive(true);

            }
        }
    }
}
