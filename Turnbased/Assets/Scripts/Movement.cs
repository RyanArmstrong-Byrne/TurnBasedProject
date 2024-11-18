using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Movement : MonoBehaviour
{
    #region variables
    [SerializeField] Vector3 targetPosition;
    [SerializeField] float unit = 8;
    [SerializeField] GameObject player;
    #endregion
    #region Move Forward Direction
    // press W key move a space forward
    public void PlayerMovementForward()
    {
        targetPosition += player.transform.forward * unit;
        player.transform.position = targetPosition;
        Debug.Log("Moved Forward");
    }
    // move 8s forward
    // minus 1 action point
    #endregion

    #region Move Left
    //press A key rotate left
    // rotate -90
    // minus 1 action point
    public void PlayerRotateLeft()
    {
        //player.transform.rotation
        Debug.Log("Moved Left");
    }

    #endregion

    #region Move Right
    //press D key rotate right
    // rotate 90
    // minus 1 action point
    public void PlayerRotateRight()
    {

        Debug.Log("Moved Right");
    }

    #endregion

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            PlayerMovementForward();
        }

        if (Input.GetKeyDown(KeyCode.A)) 
        {
            PlayerRotateLeft();
        }
    
        if (Input.GetKeyDown(KeyCode.D)) 
        {
            PlayerRotateRight();
        }
    }
}
