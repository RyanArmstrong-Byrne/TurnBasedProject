using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemySystem : MonoBehaviour
{
    #region variables

    [Header("Enemy Movement Variables")]
    [SerializeField] Vector3 targetPosition;
    [SerializeField] Vector3 targetRotation;
    public Text enemyTurnText;
    public Text actionPointDisplay;
    public float unit = 4;
    public int actionsInTurn = 3;
    public bool canAction;
    public float turnTimer = 2;
    //public EnemyDetect detect;
    public PlayerSystem playerSystem;

    [Header("Enemy Detection Variables")]

    public float detectRange = 8f;
    public float detectMelee = 4f;
    public Vector3 raycastForward = Vector3.forward;
    public Vector3 raycastLeft = Vector3.left;
    public Vector3 raycastRight = Vector3.right;
    //Can Attack
    public bool canAttack = false;

    [Header("Objects that the enemy sees (hits)")]
    public GameObject front;
    public GameObject left;
    public GameObject right;
    public float playerDist;

    #endregion

    #region Enemy Movement
    public void StartEnemyTurn()
    {
        actionsInTurn = 3;
        UpdateActionPoints(0);
        GameManager.instance.state = GameStates.EnemyTurn;
        enemyTurnText.text = "Enemy Turn";

        StartCoroutine(delayStart());
    }

    IEnumerator delayStart()
    {
        yield return new WaitForSeconds(1);
        canAction = true;
        turnTimer = 2;
    }

    IEnumerator delayBattleStart()
    {
        yield return new WaitForSeconds(1f);
        BattleManager.instance.EnemyBattleStart();
    }

    private void LateUpdate()
    {
        transform.position = targetPosition;
        transform.rotation = Quaternion.Euler(targetRotation);
        this.transform.position = new Vector3(transform.position.x, 0, transform.position.z);
    }


    void Update()
    {
        if (GameManager.instance.state == GameStates.EnemyTurn)
        {
            ForwardRay();
            RightRay();
            LeftRay();

            if (canAction)
            {


                Debug.Log("Do action");
                if (ForwardRay() || RightRay() || LeftRay())
                {
                    Debug.Log("Player Detected");
                    Debug.Log("Need to create the BattleSystem for the Enemy");


                    if (ForwardRay())
                    {
                        if (actionsInTurn >= 2)
                        {
                            if (playerDist > 5)
                            {
                                // 50/50 change of range or melee
                                int attacknumber = Random.Range(1, 3);
                                // If it is range start battle
                                if (attacknumber == 1)
                                {
                                    Debug.Log("ranged attack");
                                    BattleManager.instance.EnemyBattleStart();
                                }
                                // else walk forward and then start battle 
                                else // attacknumber = 2
                                {
                                    Debug.Log("melee attack");
                                    Forward();
                                    StartCoroutine(delayBattleStart());
                                }
                            }
                            else
                            {
                                BattleManager.instance.EnemyBattleStart();
                            }

                        }
                        // if (playerDist > 5)
                        //     {
                        //         if (actionsInTurn > 0)
                        //         {
                        //             Forward();
                        //         }
                        //     }
                    }
                    else if (RightRay())
                    {
                        TurnRight();
                    }
                    else if (LeftRay())
                    {
                        TurnLeft();
                    }
                }
                else
                {
                    Debug.Log("No player detected");

                    if (front == null || (front != null && !front.CompareTag("Player") && !front.CompareTag("walls")))
                    {
                        Debug.Log("Can move forward");
                        if (actionsInTurn > 0)
                        {
                            Forward();
                        }
                    }
                    else if (right == null || (right != null && !right.CompareTag("Player") && !right.CompareTag("walls")))
                    {
                        if (actionsInTurn > 0)
                        {
                            TurnRight();
                        }
                    }
                    else if (left == null || (left != null && !left.CompareTag("Player") && !left.CompareTag("walls")))
                    {
                        if (actionsInTurn > 0)
                        {
                            TurnLeft();
                        }
                    }
                }
                canAction = false;
                return;
            }
            if (!canAction)
            {
                if (turnTimer > 0)
                {
                    turnTimer -= Time.deltaTime;
                    if (turnTimer <= 0)
                    {
                        turnTimer = 2;
                        canAction = true;
                    }
                }
            }
        }
    }

    void UpdateActionPoints(int value)
    {
        actionsInTurn -= value;
        actionPointDisplay.text = $"Action Points: {actionsInTurn}";

        if (actionsInTurn == 0)
        {
            Debug.Log("Action point is now zero");
            GameManager.instance.state = GameStates.PlayerTurn;
            actionsInTurn = 3;
            playerSystem.StartPlayerTurn();
        }
    }

    void Forward()
    {
        if (actionsInTurn > 0)
        {
            Debug.Log("Enemy moved Forward");
            targetPosition += transform.forward * unit;
            UpdateActionPoints(1);
        }
    }
    void TurnLeft()
    {
        if (actionsInTurn > 0)
        {
            targetRotation -= Vector3.up * 90f;
            UpdateActionPoints(1);
        }
    }
    void TurnRight()
    {
        if (actionsInTurn > 0)
        {
            targetRotation += Vector3.up * 90f;
            UpdateActionPoints(1);
        }
    }

    #endregion


    #region EnemyDetect
    public bool ForwardRay()
    {
        // Cast a ray from the player's position
        Vector3 rayPosition = new Vector3(transform.position.x, 1, transform.position.z);

        Ray Forward = new Ray(rayPosition, transform.TransformDirection(raycastForward));
        RaycastHit hit;
        Debug.DrawRay(rayPosition, transform.TransformDirection(raycastForward) * detectMelee, Color.blue);
        // Perform the raycast
        if (Physics.Raycast(Forward, out hit, detectMelee))
        {
            front = hit.collider.gameObject;
        }
        else
        {
            front = null;
        }

        // Perform the raycast
        if (Physics.Raycast(Forward, out hit, detectRange))
        {
            // Check if the object hit is tagged as "Player"
            if (hit.collider.CompareTag("Player"))
            {
                Debug.DrawRay(rayPosition, transform.TransformDirection(raycastForward) * detectRange, Color.blue);

                playerDist = hit.distance;
                Debug.Log("Player detected: Can Attack");
                return true;
            }
            else
            {
                playerDist = 0;
                return false;
            }
        }
        else
        {
            playerDist = 0;
            return false;
        }
    }


    public bool LeftRay()
    {
        // Cast a ray from the player's position
        Vector3 rayPosition = new Vector3(transform.position.x, 1, transform.position.z);

        Ray Left = new Ray(rayPosition, transform.TransformDirection(raycastLeft));
        RaycastHit hit;
        Debug.DrawRay(rayPosition, transform.TransformDirection(raycastLeft) * detectMelee, Color.yellow);
        // Perform the raycast
        if (Physics.Raycast(Left, out hit, detectMelee))
        {
            left = hit.collider.gameObject;
        }
        else
        {
            left = null;
        }
        // Perform the raycast
        if (Physics.Raycast(Left, out hit, detectRange))
        {
            // Check if the object hit is tagged as "Player"
            if (hit.collider.CompareTag("Player"))
            {
                Debug.DrawRay(rayPosition, transform.TransformDirection(raycastLeft) * detectRange, Color.yellow);

                Debug.Log("Player detected Left");
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }

    public bool RightRay()
    {
        // Cast a ray from the player's position
        Vector3 rayPosition = new Vector3(transform.position.x, 1, transform.position.z);

        Ray Right = new Ray(rayPosition, transform.TransformDirection(raycastRight));
        RaycastHit hit;
        Debug.DrawRay(rayPosition, transform.TransformDirection(raycastRight) * detectMelee, Color.red);
        // Perform the raycast
        if (Physics.Raycast(Right, out hit, detectMelee))
        {
            right = hit.collider.gameObject;
        }
        else
        {
            right = null;
        }
        // Perform the raycast
        if (Physics.Raycast(Right, out hit, detectRange))
        {
            // Check if the object hit is tagged as "Player"
            if (hit.collider.CompareTag("Player"))
            {
                Debug.DrawRay(rayPosition, transform.TransformDirection(raycastRight) * detectRange, Color.red);

                Debug.Log("Player detected Right");
                return true;
            }
            else
            {

                return false;
            }
        }
        return false;
    }
    #endregion

    public void FacePlayer()
    {
        //if the player is behind
        if (!ForwardRay() && !RightRay() && !LeftRay())
        {
            targetRotation += Vector3.up * 180f;
        }
        //else if player is on the left
        else if (LeftRay())
        {
            targetRotation -= Vector3.up * 90f;
        }
        //else if player is on the right
        else if (RightRay())
        {
            targetRotation += Vector3.up * 90f;
        }
    }
}
