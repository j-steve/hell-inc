using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WordGameController : MonoBehaviour
{
    private const string DEMO_CONVERSATION = "The quick brown fox \"JUMPY\" jumps over the lazy dog \"DOGGO\"! This is some random words, dude. omg can you believe how many-words are here? there are like a thousand words; I think.";
    private static CombatModifiers DEFAULT_COMBAT_MODIFIERS = new CombatModifiers(1, 1, 1, 1, 1, 1, 1, 1, 1, 1);

    /** How long to wait after spawning the final word before ending the game, in seconds. */
    private const float DELAY_AFTER_LAST_WORD_SECONDS = 5;

    public event Action OnGameWon;
    public event Action OnGameLost;

    [SerializeField] WordSpawnerController wordSpawner;
    [SerializeField] ShooterController shooter;
    [SerializeField] GoalController goal;
    [SerializeField] WordAttackUi ui;

    CombatModifiers combatModifiers;
    float playerHealth = 1;
    float gameVictoryTime = 0;

    public void Start()
    {
        wordSpawner.OnWordSpawningComplete += delegate () {
            gameVictoryTime = Time.time + DELAY_AFTER_LAST_WORD_SECONDS;
        };
        goal.OnHit += delegate () {
            playerHealth -= 0.01f * (float)combatModifiers.HealthLoss;
            ui.UpdateHealthBar(playerHealth);
            if (playerHealth <= 0) {
                LoseGame();
            }
        };
    }

    public void Initialize(string conversationMessage, CombatModifiers combatModifiers)
    {
        this.combatModifiers = combatModifiers;
        gameObject.SetActive(true);
        wordSpawner.Initialize(conversationMessage, combatModifiers);
        shooter.Initialize(combatModifiers);
        gameVictoryTime = 0;
        playerHealth = 1;
        Time.timeScale = 1;
    }

    private void Update()
    {
        if (gameVictoryTime != 0 && Time.time >= gameVictoryTime) {
            gameObject.SetActive(false);
            OnGameWon();
        } 
        // Debug commands to insta-win or lose for testing purposes.
        if (Input.GetKey("w") && Input.GetKey("i")) {
            gameObject.SetActive(false);
            OnGameWon();
        }
        if (Input.GetKey("l") && Input.GetKey("o")) {
            LoseGame();
        }
    }

    private void LoseGame()
    {
        gameObject.SetActive(false);
        OnGameLost();
    }

}
