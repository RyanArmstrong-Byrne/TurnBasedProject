using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyMovement : MonoBehaviour
{
    #region Variables
    // Why: These variables track where the object should move and how it should rotate, without changing its position and rotation until later.
    //targetPosition: This Vector3 will store the target position of the object.
    [SerializeField] Vector3 targetPosition;
    //targetRotation: This Vector3 will store the target rotation in Euler angles (pitch, yaw, roll).
    [SerializeField] Vector3 targetRotation;
    //public Text actionPointDisplay;
    public Text enemyTurnText;
    public float unit = 4;
    public int actionsInTurn = 3;
    public bool canAction;
    public float turnTimer = 2;
    public EnemyDetect detect;
    //public Combat combat;
    public PlayerSystem playerSystem;
    #endregion
    #region Unity Event Functions
    //Why: LateUpdate is used here because you may want to set the final position and rotation of an object after all other logic and physics calculations have been made. This ensures smooth, predictable movement and rotation.
    //LateUpdate: This Unity method is called once per frame after all Update methods have been executed. It is typically used to adjust things like camera position or object transformations, after all other logic has been processed.
    private void Start()
    {
        detect = this.gameObject.GetComponent<EnemyDetect>();
        UpdateActionPoints(0);
    }
    private void LateUpdate()
    {
        //transform.position = targetPosition;: This sets the position of the GameObject to the targetPosition. The transform.position property represents the object's position in world space.
        transform.position = targetPosition;
        //transform.rotation = Quaternion.Euler(targetRotation);: This sets the GameObject�s rotation based on the targetRotation vector. Quaternion.Euler() converts the Euler angles (pitch, yaw, roll) from targetRotation into a Quaternion, which Unity uses for 3D rotations.
        transform.rotation = Quaternion.Euler(targetRotation);
        this.transform.position = new Vector3(transform.position.x, 1, transform.position.z);
    }
    #endregion

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.state == GameStates.EnemyTurn)
        {
            detect.ForwardRay();
            detect.BackwardRay();
            detect.RightRay();
            detect.LeftRay();
            //when entering turn make sure we tell user
            if (enemyTurnText.text != "Enemy Turn")
            {
                actionsInTurn = 3;
                enemyTurnText.text = "Enemy Turn";
                Debug.Log("Enemy can move now!!!");
                canAction = true;
                turnTimer = 2;
            }
            if (canAction)
            {


                Debug.Log("Do action");
                if (detect.ForwardRay() || detect.BackwardRay() || detect.RightRay() || detect.LeftRay())
                {
                    Debug.Log("Player Detected");

                    //is player infront? else turn to fight
                    if (detect.ForwardRay())
                    {
                        //Attack
                        if (detect.playerDist > 5)
                        {
                            if (actionsInTurn > 0)
                            {
                                Forward();
                            }
                            //else
                            //{
                            //    combat.enemyAttackValue = 10;
                            //    combat.EnemyAttack(combat.enemyAttackValue);
                            //}
                        }
                        //else
                        //{
                        //    combat.enemyAttackValue = 20;
                        //    combat.EnemyAttack(combat.enemyAttackValue);
                        //}
                    }
                    else if (detect.RightRay())
                    {
                        TurnRight();
                    }
                    else if (detect.LeftRay())
                    {
                        TurnLeft();
                    }
                    else if (detect.BackwardRay())
                    {
                        TurnAround();
                    }
                }
                else
                {
                    Debug.Log("No player detected");

                    if (detect.front == null || (detect.front != null && !detect.front.CompareTag("Player") && !detect.front.CompareTag("walls")))
                    {
                        Debug.Log("Can move forward");
                        if (actionsInTurn > 0)
                        {
                            Forward();
                        }
                    }
                    else if (detect.right == null || (detect.right != null && !detect.right.CompareTag("Player") && !detect.right.CompareTag("walls")))
                    {
                        if (actionsInTurn > 0)
                        {
                            TurnRight();
                        }
                    }
                    else if (detect.left == null || (detect.left != null && !detect.left.CompareTag("Player") && !detect.left.CompareTag("walls")))
                    {
                        if (actionsInTurn > 0)
                        {
                            TurnLeft();
                        }
                    }
                    else if (detect.back == null || (detect.back != null && !detect.back.CompareTag("Player") && !detect.back.CompareTag("walls")))
                    {
                        if (actionsInTurn > 1)
                        {
                            TurnAround();
                        }
                    }
                }
                canAction = false;
                return;
            }
            //if cant action do timer sa we can
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
        else
        {
            if (enemyTurnText.text != "Player Turn")
            {
                enemyTurnText.text = "Player Turn";
                Debug.Log("Back to Player Turn");
                playerSystem.StartPlayerTurn();

            }
        }
    }

    void UpdateActionPoints(int value)
    {
        actionsInTurn -= value;
        if (actionsInTurn == 0)
        {
            Debug.Log("Action point is now zero");
            // Change to the enemy's turn...
            GameManager.instance.state = GameStates.PlayerTurn;
        }
        //actionPointDisplay.text = $"Action Points: {actionsInTurn}";
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
            //targetRotation -= Vector3.up * 90f;: Vector3.up represents the unit vector (0, 1, 0), which is the Y-axis. Multiplying it by 90 degrees gives a vector that represents a 90-degree rotation around the Y-axis. By subtracting this from targetRotation, you are rotating the object 90 degrees counter-clockwise.
            targetRotation -= Vector3.up * 90f;
            UpdateActionPoints(1);
        }
    }
    void TurnRight()
    {
        if (actionsInTurn > 0)
        {

            //targetRotation += Vector3.up * 90f;: Here, you add 90 degrees to the targetRotation vector. This causes the object to rotate clockwise around the Y-axis.
            targetRotation += Vector3.up * 90f;
            UpdateActionPoints(1);
        }
    }
    void TurnAround()
    {
        if (actionsInTurn > 1)
        {

            //targetRotation -= Vector3.up * 180f;: By subtracting 180 degrees from the targetRotation, you rotate the object 180 degrees around the Y-axis.
            targetRotation -= Vector3.up * 180f;
            UpdateActionPoints(2);
        }
    }

}