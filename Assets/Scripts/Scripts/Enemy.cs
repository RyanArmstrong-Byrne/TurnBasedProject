using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{

    public int EnemnyActionPoints = 3;
    public Text turnDisplay;
    public Text actionPointDisplay;
    bool _isMoving;
    bool _changedTurn;

    // Similar Movement and detection 

    // if the AI detects a wall rotate

    // has a set amount of movement like player

    // if movement points run out switch it to player turn with full points on player movement.
    private void Update()
    {
        if (GameManager.instance.state == GameStates.EnemyTurn)
        {
            if (!_changedTurn)
            {
                _changedTurn = true;
                UpdateActionPoints(0);
            }

            if (!_isMoving)
            {
                StartCoroutine(EnemyTurn());
            }
            //UpdateActionPoints(1);
        }
    }


    IEnumerator EnemyTurn()
    {
        yield return new WaitForSeconds(.5f);
        if (!_isMoving)
        {
            _isMoving = true;
            UpdateActionPoints(1);
        }
        yield return new WaitForSeconds(2f);
        _isMoving = false;
    }

    void UpdateActionPoints(int value)
    {
        EnemnyActionPoints -= value;
        Debug.Log("Enemy - Removeed action point");
        actionPointDisplay.text = $"Action Points: {EnemnyActionPoints}";


        if (EnemnyActionPoints == 0)
        {
            turnDisplay.text = "Player Turn";
            GameManager.instance.state = GameStates.PlayerTurn;
            EnemnyActionPoints = 3;
            _changedTurn = false;
        }
    }
}
