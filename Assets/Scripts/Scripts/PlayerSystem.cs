using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSystem : MonoBehaviour
{
    #region variables
    [SerializeField] Vector3 targetPosition;
    [SerializeField] float unit = 4;
    [SerializeField] GameObject player;
    [SerializeField] Vector3 targetRotation;
    public int actionsInTurn = 3;
    public Text actionPointDisplay;
    public Text turnDisplay;
    [SerializeField] private bool _hitWallFront;
    public bool hitDoorFront ;
    [SerializeField] private bool _hitEnemyRange;
    [SerializeField] private bool _hitEnemyMelee;
    public  GameObject battleButton;
    //bool _changedTurn;
    Door door;
    public EnemySystem enemySystem;

    [SerializeField] GameObject Left_detect, Right_detect;

    [Header("Player Detection Variables")]

    public float detectRange = 8f;
    public float detectMelee = 4f;
    public Vector3 raycastForward = Vector3.forward;
    public Vector3 raycastLeft = Vector3.left;
    public Vector3 raycastRight = Vector3.right;

    [Header("Objects that the enemy sees (hits)")]
    public GameObject front;
    public GameObject left;
    public GameObject right;
    public float enemyDist;

    #endregion
    private void Start()
    {
        //UpdateActionPoints(0);
        //turnDisplay.text = "Player Turn";
        //battleButton.SetActive(false);

        StartPlayerTurn();

    }
    private void LateUpdate()
    {
        transform.rotation = Quaternion.Euler(targetRotation);
    }

    public void StartPlayerTurn()
    {
        actionsInTurn = 3;
        UpdateActionPoints(0);
        GameManager.instance.state = GameStates.PlayerTurn;
        turnDisplay.text = "Player Turn";
    }


    #region Move Forward Direction
    // press W key move a space forward
    public void PlayerMovementForward()
    {
        if (actionsInTurn > 0 && !_hitWallFront && !hitDoorFront )
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
            targetRotation -= Vector3.up * 90;
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
        #region Old
        //if (GameManager.instance.state == GameStates.PlayerTurn)
        //{
        //if (!_changedTurn)
        //{
        //    _changedTurn = true;
        //    UpdateActionPoints(0);
        //}

        //Ray ranged = new Ray(transform.position, transform.forward);
        //RaycastHit hitInfo;
        //if (Physics.Raycast(ranged, out hitInfo, 8))
        //{
        //    if (!_hitEnemyMelee)
        //    {
        //        if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        //        {
        //            Debug.Log("Range - I have hit the enemy...");
        //            _hitEnemyRange = true;
        //            _hitEnemyMelee = false;
        //            _hitWall = false;
        //            battleButton.SetActive(true);
        //        }
        //    }
        //}
        //else
        //{
        //    _hitEnemyRange = false;
        //}

        //if (Physics.Raycast(ranged, out hitInfo, 4))
        //{
        //    if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("walls"))
        //    {
        //        Debug.Log("I have hit a wall...");
        //        _hitWall = true;
        //        _hitEnemyRange = false;
        //        _hitEnemyMelee = false;
        //    }
        //    else if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        //    {
        //        Debug.Log("Melee - I have hit the enemy...");
        //        _hitEnemyMelee = true;
        //        _hitEnemyRange = false;
        //        _hitWall = false;
        //        battleButton.SetActive(true);
        //    }
        //    else if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Door"))
        //    {
        //        Debug.Log("Door - Show pop up text here...");
        //        door = hitInfo.transform.gameObject.GetComponent<Door>();
        //        door.nearDoor = true;
        //    }
        //}
        //else
        //{
        //    _hitWall = false;
        //    _hitEnemyMelee = false;
        //    if (door !=null)
        //    {
        //        door.nearDoor = false;
        //        door.HideText();
        //    }
        //}
        #endregion

        if (GameManager.instance.state == GameStates.PlayerTurn)
        {
            ForwardRay();
            LeftRay();
            RightRay();
            
            ToBattleState();
            
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

            if (ForwardRay() || LeftRay() || RightRay())
            {
                if (ForwardRay())
                {
                    //if (enemyDist > 5)
                    //{
                    //    if (actionsInTurn > 0)
                    //    {

                    //    }
                    //}
                    Left_detect.SetActive(false);
                    Right_detect.SetActive(false);


                    Debug.Log("Enemy Forward");
                }
                else if (RightRay())
                {
                    Debug.Log("Enemy right side");
                    Left_detect.SetActive(false);
                    Right_detect.SetActive(true);
                }
                else if (LeftRay())
                {
                    Debug.Log("Enemy left side");
                    Right_detect.SetActive(false);
                    Left_detect.SetActive(true);
                }
            }
            else
            {
                if (Right_detect.activeInHierarchy)
                {
                    Right_detect.SetActive(false);
                }
                if (Left_detect.activeInHierarchy)
                {
                    Left_detect.SetActive(false); 
                }

                Debug.Log("No enemy detected");

                if (front == null || (front != null && !front.CompareTag("Enemy") && !front.CompareTag("walls")))
                {
                    _hitWallFront = false;
                }
            }
        }
    }

    //if (Physics.Raycast(ranged, out hitInfo, 4))
    //{
    //    if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("walls"))
    //    {
    //        Debug.Log("I have hit a wall...");
    //        _hitWall = true;
    //        _hitEnemyRange = false;
    //        _hitEnemyMelee = false;
    //    }
    //    else if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Enemy"))
    //    {
    //        Debug.Log("Melee - I have hit the enemy...");
    //        _hitEnemyMelee = true;
    //        _hitEnemyRange = false;
    //        _hitWall = false;
    //        battleButton.SetActive(true);
    //    }
    //    else if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Door"))
    //    {
    //        Debug.Log("Door - Show pop up text here...");
    //        door = hitInfo.transform.gameObject.GetComponent<Door>();
    //        door.nearDoor = true;
    //    }
    //}
    //else
    //{
    //    _hitWall = false;
    //    _hitEnemyMelee = false;
    //    if (door !=null)
    //    {
    //        door.nearDoor = false;
    //        door.HideText();
    //    }
    //}

    public bool ForwardRay()
    {
        // Cast a ray from the enemy's position
        Vector3 rayPosition = new Vector3(transform.position.x, 1, transform.position.z);

        Ray Forward = new Ray(rayPosition, transform.TransformDirection(raycastForward));
        RaycastHit hit;
        Debug.DrawRay(rayPosition, transform.TransformDirection(raycastForward) * detectMelee, Color.blue);
        // Perform the raycast
        if (Physics.Raycast(Forward, out hit, detectMelee))
        {
            front = hit.collider.gameObject;

            if (hit.collider.CompareTag("Door"))
            {
                Debug.Log("Door - Show pop up text here...");
                door = hit.transform.gameObject.GetComponent<Door>();
                door.nearDoor = true;
                hitDoorFront  = true;
            }
            else
            {
                hitDoorFront  = false;
            }

            // Check if the object hit is tagged as "walls"
            if (hit.collider.CompareTag("walls"))
            {
                //Debug.Log("Forward Wall detected");
                _hitWallFront = true;
            }
            else
            {
                _hitWallFront = false;
            }
        }
        else
        {
            front = null;
        }

        // Perform the raycast
        if (Physics.Raycast(Forward, out hit, detectRange))
        {
            // Check if the object hit is tagged as "Enemy"
            if (hit.collider.CompareTag("Enemy"))
            {
                Debug.DrawRay(rayPosition, transform.TransformDirection(raycastForward) * detectRange, Color.blue);

                enemyDist = hit.distance;
                hitDoorFront  = false;
                _hitWallFront = false;
                //Debug.Log("Enemy detected: Can Attack");
                return true;
            }
            else
            {
                enemyDist = 0;
                return false;
            }
        }
        else
        {
            enemyDist = 0;
            return false;
        }
    }


    public bool LeftRay()
    {
        // Cast a ray from the enemy's position
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
            // Check if the object hit is tagged as "Enemy"
            if (hit.collider.CompareTag("Enemy"))
            {
                Debug.DrawRay(rayPosition, transform.TransformDirection(raycastLeft) * detectRange, Color.yellow);

                //Debug.Log("Enemy detected Left");
                return true;
                // This will activate the left threat
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
        // Cast a ray from the enemy's position
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
            // Check if the object hit is tagged as "Enemy"
            if (hit.collider.CompareTag("Enemy"))
            {
                Debug.DrawRay(rayPosition, transform.TransformDirection(raycastRight) * detectRange, Color.red);

                //Debug.Log("Enemy detected Right");
                return true;
                // This will activate the Right threat
            }
            else
            {

                return false;
            }
        }
        return false;
    }


    void UpdateActionPoints(int value)
    {
        actionsInTurn -= value;
        actionPointDisplay.text = $"Action Points: {actionsInTurn}";
        if (actionsInTurn == 0)
        {
            GameManager.instance.state = GameStates.EnemyTurn;
            actionsInTurn = 3;
            enemySystem.StartEnemyTurn();
            //_changedTurn = false;
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
            //Debug.Log("battlebutton not active");
            battleButton.SetActive(false);
        }

    }
}
