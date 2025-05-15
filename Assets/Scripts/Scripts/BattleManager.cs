using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    public static BattleManager instance;
    public BattleStates state = BattleStates.NotInBattle;

    public GameObject battleCamera;
    public GameObject playerCamera;
    public GameObject battleButton;

    public bool playerTurn = false;
    public bool enemyTurn = false;
    public bool isPlayerDead = false;

    [Header("Battle Elements")]
    [SerializeField] private GameObject _movementHUD;
    [SerializeField] private GameObject _battleHUD;
    [SerializeField] private Text _battleText;
    [Header("Player Elements")]
    [SerializeField] private Image _battlePlayerHealthBar;
    [SerializeField] private Text _battlePlayerTextHealthBar;
    [SerializeField] private Text _playerStrengthValue, _playerMagicValue, _playerDefenceValue, _playerPotionsValue;
    [Header("Enemy Elements")]
    [SerializeField] private Image _battleEnemyHealthBar;
    [SerializeField] private Text _battleEnemyTextHealthBar;
    [SerializeField] private Text _enemyAttackValue, _enemyMagicValue, _enemyDefenceValue;

    [Header("Bool Checks")]
    [SerializeField] private bool _hasPressedAction = false;
    [SerializeField] private bool _startFromEnemy = false;
    [SerializeField] private bool _isBlocked = false;
    [SerializeField] private int _healAmount = 15;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null && instance != this)
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        battleButton.SetActive(false);
        battleCamera.SetActive(false);
        _movementHUD.SetActive(true);
        _battleHUD.SetActive(false);

        // Define Player bools
        _isBlocked = false;
    }

    public void EnemyBattleStart()
    {
        _startFromEnemy = true;
        GameManager.instance.state = GameStates.Battle;
        UIElements();
        StartCoroutine(EnemyTurn());

        battleCamera.SetActive(true);
        playerCamera.SetActive(false);
        battleButton.SetActive(false);
        _battleHUD.SetActive(true);
        _movementHUD.SetActive(false);


    }
    public void PlayerBattleStart()
    {
        GameManager.instance.state = GameStates.Battle;
        UIElements();
        StartCoroutine(PlayerTurn(0f));

        battleCamera.SetActive(true);
        playerCamera.SetActive(false);
        battleButton.SetActive(false);
        _battleHUD.SetActive(true);
        _movementHUD.SetActive(false);

    }

    private void UIElements()
    {
        // Player Elements
        PlayerHandler _playerHandler = PlayerHandler.instance;
        _battlePlayerHealthBar.fillAmount = _playerHandler.healthREF / 100;
        _battlePlayerTextHealthBar.text = _playerHandler.healthREF.ToString();
        _playerStrengthValue.text = $"Strength: {_playerHandler.strengthREF}";
        _playerMagicValue.text = $"Magic: {_playerHandler.magicREF}";
        _playerDefenceValue.text = $"Defence: {_playerHandler.defenceREF}";
        _playerPotionsValue.text = $"Potions: {_playerHandler.potionsREF}";

        // Enemy Elements
        EnemyHandler _enemyHandler = EnemyHandler.instance;
        _battleEnemyHealthBar.fillAmount = _enemyHandler.healthREF / 100;
        _battleEnemyTextHealthBar.text = _enemyHandler.healthREF.ToString();
        _enemyAttackValue.text = $"Strength: {_enemyHandler.strengthREF}";
        _enemyMagicValue.text = $"Magic: {_enemyHandler.magicREF}";
        _enemyDefenceValue.text = $"Defence: {_enemyHandler.defenceREF}";
    }

    #region Enemy Turn System
    IEnumerator EnemyTurn()
    {
        if (!_startFromEnemy)
        {
            yield return new WaitForSeconds(2f);
        }
        else
        {
            _startFromEnemy = false;
            state = BattleStates.EnemyTurn;
            _battleText.text = $"Enemy Turn...";
            enemyTurn = true;
            playerTurn = false;
            Debug.Log("Is now on enemy turn function");
            yield return new WaitForSeconds(1f);
        }
        _battleText.text = $"Enemy is making a choice...";
        Debug.Log("Enemy will do actions here... Wait time will be defined beforehand");
        yield return new WaitForSeconds(2f);
        Debug.Log("Enemy Turn is now over");
        StartCoroutine(PlayerTurn(0f));
    }
    #endregion

    #region 
    // private void PlayerTurn()
    IEnumerator PlayerTurn(float delay)

    {
        yield return new WaitForSeconds(delay);
        state = BattleStates.PlayerTurn;
        string _text = "Player Turn...";
        playerTurn = true;
        enemyTurn = false;
        _hasPressedAction = false;
        Debug.Log(_text);
        _battleText.text = _text;
        if (_isBlocked)
        {
            _isBlocked = false;
        }
    }
    #endregion


    #region Player Battle Buttons
    // Block Button
    public void BlockActionButton()
    {
        if (!_hasPressedAction)
        {
            _hasPressedAction = true;
            string _blockText = "Player steadys themself...";
            Debug.Log(_blockText);
            _battleText.text = _blockText;
            _isBlocked = true;
            // Play block anim
            StartCoroutine(EnemyTurn());
        }
        else
        {
            string _blockText = "find sound effect for can't press button";
            Debug.Log(_blockText);
        }
    }

    // Attack Button
    public void AttackActionButton()
    {
        if (!_hasPressedAction)
        {
            _hasPressedAction = true;
            string _attackText = "Player Attacks Enemy...";
            Debug.Log(_attackText);
            _battleText.text = _attackText;
            StartCoroutine(EnemyTurn());
        }
        else
        {
            string _blockText = "find sound effect for can't press button";
            Debug.Log(_blockText);
        }

    }
    // Heal Button
    public void HealActionButton()
    {
        if (!_hasPressedAction)
        {
            _hasPressedAction = true;
            if (PlayerHandler.instance.healthREF < PlayerHandler.instance.maxhealthREF)
            {
                if (PlayerHandler.instance.potionsREF > 0)
                {
                    HealPlayer(_healAmount);
                    string _healText = "Player uses a potion...";
                    Debug.Log(_healText);
                    _battleText.text = _healText;
                    StartCoroutine(EnemyTurn());
                }
                else
                {
                    string _healText = "Player doesn't have any potions...";
                    Debug.Log(_healText);
                    _battleText.text = _healText;
                    StartCoroutine(PlayerTurn(2f));
                }
            }
            else
            {
                string _healText = "Player has max health...";
                Debug.Log(_healText);
                _battleText.text = _healText;
                StartCoroutine(PlayerTurn(2f));
            }
        }
        else
        {
            string _blockText = "find sound effect for can't press button";
            Debug.Log(_blockText);
        }
    }
    #endregion

    private void HealPlayer(int healingAmount)
    {
        // Increase health by X amount
        PlayerHandler.instance.healthREF += healingAmount;
        if (PlayerHandler.instance.healthREF > PlayerHandler.instance.maxhealthREF)
        {
            PlayerHandler.instance.healthREF = PlayerHandler.instance.maxhealthREF;
        }
        // Decrease the potion count by one
        PlayerHandler.instance.potionsREF -= 1;
        UIElements();
    }

    public void OnEndGame() // Used only in the GameWonSequence
    {
        state = BattleStates.EndGame;
    }

    public void OnDeath()
    {
        isPlayerDead = true;
        state = BattleStates.Death;
    }

    private void DeathSequence()
    {
        OnDeath();
        OnEndGame();
    }

    private void GameWonSequence(bool restartLevel)
    {
        OnEndGame();

        if (restartLevel)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }


    public void TriggerWinState(bool restartLevel)
    {
        GameWonSequence(restartLevel);
    }
    public void TriggerLoseState()
    {
        DeathSequence();
    }
}

public enum BattleStates
{
    NotInBattle, PlayerTurn, EnemyTurn, Pause, Menu, Death, EndGame
}