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
    [SerializeField] PlayerHandler _playerHandler;
    [SerializeField] EnemyHandler _enemyHandler;
    [SerializeField] GameObject _playerObject;
    [SerializeField] GameObject _enemyObject;
    [SerializeField] PlayerSystem _playerSystem;
    [SerializeField] EnemySystem _enemySystem;
    [Header("Player Elements")]
    [SerializeField] private Image _battlePlayerHealthBar;
    [SerializeField] private Text _battlePlayerTextHealthBar;
    [SerializeField] private Text _playerStrengthValue, _playerMagicValue, _playerDefenceValue, _playerPotionsValue;
    [Header("Enemy Elements")]
    [SerializeField] private Image _battleEnemyHealthBar;
    [SerializeField] private Text _battleEnemyTextHealthBar;
    [SerializeField] private Text _enemyAttackValue, _enemyMagicValue, _enemyDefenceValue;

    private float playerattack;

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
        UpdateElements();
        GameManager.instance.state = GameStates.Battle;
        StartCoroutine(EnemyTurn());

        battleCamera.SetActive(true);
        playerCamera.SetActive(false);
        battleButton.SetActive(false);
        _battleHUD.SetActive(true);
        _movementHUD.SetActive(false);


    }
    // Start Battle Button
    public void PlayerBattleStart()
    {
        UpdateElements();
        GameManager.instance.state = GameStates.Battle;
        StartCoroutine(PlayerTurn(0f));

        battleCamera.SetActive(true);
        playerCamera.SetActive(false);
        battleButton.SetActive(false);
        _battleHUD.SetActive(true);
        _movementHUD.SetActive(false);

    }

    private void UpdateElements()
    {
        // Player Elements
        if (_playerSystem == null)
        {
            Debug.Log("update start....");
            _playerObject = GameObject.FindWithTag("Player");
            _playerSystem = _playerObject.GetComponent<PlayerSystem>();
            _playerHandler = PlayerHandler.instance;

            _enemyObject = GameObject.FindWithTag("Enemy");
            _enemySystem = _enemyObject.GetComponent<EnemySystem>();
            _enemyHandler = EnemyHandler.instance;
            Debug.Log("update end....");
        }

        _battlePlayerHealthBar.fillAmount = _playerHandler.healthREF / 100;
        _battlePlayerTextHealthBar.text = _playerHandler.healthREF.ToString();
        _playerStrengthValue.text = $"Strength: {_playerHandler.strengthREF}";
        _playerMagicValue.text = $"Magic: {_playerHandler.magicREF}";
        _playerDefenceValue.text = $"Defence: {_playerHandler.defenceREF}";
        _playerPotionsValue.text = $"Potions: {_playerHandler.potionsREF}";

        // Enemy Elements
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
            AttackEnemy();
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
            if (_playerHandler.healthREF < _playerHandler.maxhealthREF)
            {
                if (_playerHandler.potionsREF > 0)
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

    #region Player Battle Fuctions
    private void HealPlayer(int healingAmount)
    {
        // Increase health by X amount
        _playerHandler.healthREF += healingAmount;
        if (_playerHandler.healthREF > _playerHandler.maxhealthREF)
        {
            _playerHandler.healthREF = _playerHandler.maxhealthREF;
        }
        // Decrease the potion count by one
        _playerHandler.potionsREF -= 1;
        UpdateElements();
    }

    private void AttackEnemy()
    {
        // Check how far the enemy is from the player
        // To get range or melee
        if (_playerSystem.enemyDist > 6)
        {
            //ranged
            Debug.Log($"Range attack... {_playerHandler.magicREF}");
            playerattack = _playerHandler.magicREF;
            Debug.Log($"player Attack: {playerattack}");
        }
        else
        {
            //melee
            Debug.Log($"Melee attack...{_playerHandler.strengthREF}");
            playerattack = _playerHandler.strengthREF;
            Debug.Log($"player Attack: {playerattack}");
        }

        // Player attack value (minus) Enemy defence value
        //e.g. 30 - 25 = 5 (true attack damage)
        playerattack = playerattack - _enemyHandler.defenceREF;
        Debug.Log($"player Attack: {playerattack}");
        // update UI & edit enemy current health
        if (playerattack < 0)
        {
            playerattack = 0;
        }
        _enemyHandler.healthREF = _enemyHandler.healthREF - playerattack;
        string _attackText = $"Player Did {playerattack} damage against enemy";
        _battleText.text = _attackText;
        UpdateElements();
    }

    // Enemy attacking player
    private void BlockEnemyAttack()
    {
        // get the attack value from the enemy
        // Get the defence value from the player
        // then half the value (this is the true attackvalue)
        // Then deal damage to the player and update UI
        
    }

    #endregion

    #region Enemy Battle Function

    private void AttackPlayer()
    {

    }

    private void BlockPlayerAttack()
    {

    }
    #endregion

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