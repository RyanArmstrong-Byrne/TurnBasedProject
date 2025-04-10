using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{

    public int EnemnyActionPoints = 3;
    public Text turnDisplay;
    public Text actionPointDisplay;
    [SerializeField] bool _isMoving;
    bool _changedTurn;
    [SerializeField] GameObject enemy;
    [SerializeField] float Unit = 4;
    [SerializeField] Vector3 targetPosition;
    [SerializeField] Vector3 targetRotation;

    public bool _hitWallFront = false, _hitWallLeft = false, _hitWallRight = false;
    public bool _hitPlayerRange;
    public bool _hitPlayerMelee;



    private void Awake()
    {
        enemy = this.gameObject;
        this.gameObject.transform.position = targetPosition;
    }

    // Similar Movement and detection 

    // if the AI detects a wall rotate

    // has a set amount of movement like player

    // if movement points run out switch it to player turn with full points on player movement.
    private void Update()
    {
        if (GameManager.instance.state == GameStates.PlayerTurn)
        {
            EnemnyActionPoints = 3;
            _changedTurn = false;
        }

        if (GameManager.instance.state == GameStates.EnemyTurn)
        {
            if (!_changedTurn)
            {
                _changedTurn = true;
                //UpdateActionPoints(0);

                //if (EnemnyActionPoints == 0)
                //{
                //    //turnDisplay.text = "Player Turn";
                //    //GameManager.instance.state = GameStates.PlayerTurn;
                //    EnemnyActionPoints = 3;
                //    //_changedTurn = false;
                //}
            }

            if (!_isMoving)
            {
                #region Forward Raycast
                Ray rangedFront;
                rangedFront = new Ray(transform.position, transform.forward);
                //rangedFront = _frontCam.GetComponent<Camera>().ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
                RaycastHit hitInfoFront;
                if (Physics.Raycast(rangedFront, out hitInfoFront, 4))
                {
                    Debug.Log($"Front Wall = {hitInfoFront.transform.name}");


                    Debug.DrawRay(rangedFront.origin, transform.forward * 4, Color.green);

                    if (hitInfoFront.transform.gameObject.layer == LayerMask.NameToLayer("walls"))
                    {
                        Debug.Log("Wall Front");
                        _hitWallFront = true;
                        _hitPlayerRange = false;
                        _hitPlayerMelee = false;
                    }
                    else if (hitInfoFront.transform.gameObject.layer == LayerMask.NameToLayer("Player"))
                    {
                        Debug.Log("Melee Front");
                        _hitPlayerMelee = true;
                        _hitPlayerRange = false;
                        _hitWallFront = false;
                    }
                }
                else
                {
                    _hitWallFront = false;
                    _hitPlayerMelee = false;
                }


                //if (Physics.Raycast(rangedFront, out hitInfoFront, 8))
                //{
                //    Debug.DrawRay(rangedFront.origin, transform.forward * 8, Color.yellow);
                //    if (!_hitPlayerMelee)
                //    {
                //        if (hitInfoFront.transform.gameObject.layer == LayerMask.NameToLayer("Player"))
                //        {
                //            Debug.Log("Range Front");
                //            _hitPlayerRange = true;
                //            _hitPlayerMelee = false;
                //            _hitWallFront = false;
                //        }
                //    }
                //}
                //else
                //{
                //    _hitPlayerRange = false;
                //}
                #endregion

                #region Right Raycast

                Ray rangedRight;
                rangedRight = new Ray(transform.position, transform.right);
                RaycastHit hitInfoRight;
                Debug.DrawRay(rangedRight.origin, transform.right * 8, Color.red); // Right side
                if (Physics.Raycast(rangedRight, out hitInfoRight, 8))
                {
                    Debug.Log($"Right Wall = {hitInfoRight.transform.name}");

                    if (hitInfoRight.transform.gameObject.layer == LayerMask.NameToLayer("walls"))
                    {
                        Debug.Log("Wall Right");
                        _hitWallRight = true;
                        _hitPlayerRange = false;
                        _hitPlayerMelee = false;
                    }

                    if (hitInfoRight.transform.gameObject.layer == LayerMask.NameToLayer("Player"))
                    {
                        Debug.Log("Range Right");
                        _hitPlayerRange = true;
                        _hitPlayerMelee = false;
                        _hitWallRight = false;
                    }
                }
                else
                {
                    _hitPlayerRange = false;
                    _hitWallRight = false;
                }

                #endregion


                #region Left Raycast
                Ray rangedLeft;
                rangedLeft = new Ray(transform.position, -transform.right);
                RaycastHit hitInfoLeft;
                Debug.DrawRay(rangedLeft.origin, -transform.right * 8, Color.red); // Right side
                if (Physics.Raycast(rangedLeft, out hitInfoLeft, 8))
                {
                    Debug.Log($"Left Wall = {hitInfoLeft.transform.name}");

                    if (hitInfoLeft.transform.gameObject.layer == LayerMask.NameToLayer("walls"))
                    {
                        Debug.Log("Wall Left");
                        _hitWallLeft = true;
                        _hitPlayerRange = false;
                        _hitPlayerMelee = false;
                    }

                    if (hitInfoLeft.transform.gameObject.layer == LayerMask.NameToLayer("Player"))
                    {
                        Debug.Log("Range Left");
                        _hitPlayerRange = true;
                        _hitPlayerMelee = false;
                        _hitWallLeft = false;
                    }
                }
                else
                {
                    _hitPlayerRange = false;
                    _hitWallLeft = false;
                }
                #endregion

                StartCoroutine(EnemyTurn());
            }
            //UpdateActionPoints(1);
        }
    }

    void EnemyForward()
    {
        if (EnemnyActionPoints > 0)
        {
            if (_hitWallFront)
            {
                if (_hitWallLeft == false)
                {
                    // rotate left
                    targetRotation = Vector3.up * -90;
                    enemy.transform.rotation = transform.rotation;
                    transform.rotation = Quaternion.Euler(targetRotation);

                    Debug.Log("Moved Left");
                    //Debug.Log(EnemnyActionPoints);
                    UpdateActionPoints(1);
                    //Debug.Log(EnemnyActionPoints);
                    //_isMoving = false;
                }
                else if (_hitWallRight == false)
                {
                    // rotate right
                    targetRotation = Vector3.up * +90;
                    enemy.transform.rotation = transform.rotation;
                    transform.rotation = Quaternion.Euler(targetRotation);

                    Debug.Log("Moved Right");
                    UpdateActionPoints(1);
                    //_isMoving = false;
                }
                else
                {
                    Debug.LogWarning("ENEMY - SHOUDLN'T SHOW UP");
                }
            }
            else
            {
                _isMoving = false;
                targetPosition += enemy.transform.forward * Unit;
                enemy.transform.position = targetPosition;
                UpdateActionPoints(1);
                Debug.Log("Enemy moved forward");
            }
        }
    }



    IEnumerator EnemyTurn()
    {
        yield return new WaitForSeconds(.5f);
        if (!_isMoving)
        {
            _isMoving = true;
            EnemyForward();
        }
        //yield return new WaitForSeconds(2f);
        //_isMoving = false;
    }

    void UpdateActionPoints(int value)
    {
        EnemnyActionPoints -= value;
        Debug.Log("Enemy - Removed action point");
        actionPointDisplay.text = $"Action Points: {EnemnyActionPoints}";


        if (EnemnyActionPoints == 0)
        {
            turnDisplay.text = "Player Turn";
            GameManager.instance.state = GameStates.PlayerTurn;
        }
    }
}
