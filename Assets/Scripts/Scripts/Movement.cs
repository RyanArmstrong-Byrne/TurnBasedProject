using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Movement : MonoBehaviour
{
    #region variables
    [SerializeField] UnityEngine.Vector3 targetPosition;
    [SerializeField] float unit = 4;
    [SerializeField] GameObject player;
    [SerializeField] UnityEngine.Vector3 targetRotation;
    public int actionsInTurn = 3;
    public Text actionPointDisplay;
    public Text turnDisplay;
    public bool _hitWall;
    public bool _hitEnemyRange;
    public bool _hitEnemyMelee;
    public  GameObject battleButton;

    #endregion
    private void Start()
    {
        UpdateActionPoints(0);
        turnDisplay.text = "Player Turn";
        battleButton.SetActive(false);

    }
    private void LateUpdate()
    {
        transform.rotation = UnityEngine.Quaternion.Euler(targetRotation);
    }

    #region Move Forward Direction
    // press W key move a space forward
    public void PlayerMovementForward()
    {
        if (actionsInTurn > 0 && !_hitWall)
        //if(actionsInTurn > 0)
        {
            targetPosition += player.transform.forward * unit;
            player.transform.position = targetPosition;
            UpdateActionPoints(1);
            Debug.Log("Moved Forward");
        }

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
        if (actionsInTurn > 0)
        {
            //player.transform.rotation
            targetRotation -= UnityEngine.Vector3.up * 90;
            player.transform.rotation = transform.rotation;
            Debug.Log("Moved Left");
            UpdateActionPoints(1);
        }

    }

    #endregion

    #region Move Right
    //press D key rotate right
    // rotate 90
    // minus 1 action point

    public void PlayerRotateRight()
    {
        if (actionsInTurn > 0)
        {
            //player.transform.rotation
            targetRotation += UnityEngine.Vector3.up * 90;
            player.transform.rotation = transform.rotation;
            UpdateActionPoints(1);
            Debug.Log("Moved Right");
        }

    }

    #endregion

    private void Update()
    {
        if (GameManager.instance.state == GameStates.PlayerTurn)
        {
            Ray ranged = new Ray(transform.position, transform.forward);
            RaycastHit hitInfo;
            if (Physics.Raycast(ranged, out hitInfo, 8))
            {
                if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                {
                    Debug.Log("I have hit the enemy...");
                    _hitEnemyRange = true;
                    _hitEnemyMelee = false;
                    _hitWall = false;
                }
            }
            else
            {
                _hitEnemyRange = false;
            }

            if (Physics.Raycast(ranged, out hitInfo, 4))
            {
                if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("walls"))
                {
                    Debug.Log("I have hit a wall...");
                    _hitWall = true;
                    _hitEnemyRange = false;
                    _hitEnemyMelee = false;
                }
                else if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                {
                    Debug.Log("I have hit the enemy...");
                    _hitEnemyMelee = true;
                    _hitEnemyRange = false;
                    _hitWall = false;
                }
            }
            else
            {
                _hitWall = false;
                _hitEnemyMelee = false;
            }

            ToBattleState();
        }

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
    void UpdateActionPoints(int value)
    {
        actionsInTurn -= value;
        actionPointDisplay.text = $"Action Points: {actionsInTurn}";
        if (actionsInTurn == 0)
        {
            GameManager.instance.state = GameStates.EnemyTurn;
            turnDisplay.text = "Enemy Turn";
        }

    }

    public void ToBattleState()
    {
        if (_hitEnemyRange == true)
        {
            battleButton.SetActive(true);
        }
        else if (_hitEnemyMelee == true)
        {
            battleButton.SetActive(true);
        }
        else { }
        {
            Debug.Log("something broken");
        }

    }
}
