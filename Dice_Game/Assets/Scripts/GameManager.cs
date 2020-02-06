using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject resultPanel;

    public Text messageText;
    public Text resultText;
    public Text playerHandNameText;
    public Text enemyHandNameText;
    public Text playerRoundScoreText;
    public Text enemyRoundScoreText;

    public Button playAgainButton;
    public Text playAgainButtonText;

    public DicesManager player, enemy;
    
    public GameScoreCounter gameScoreCounter;
    ScoreCounter scoreCounter;
    UIManager uiManager;

    public int playerScore, enemyScore;
    public string playerHand, enemyHand;



    //0-setup 1-player roll 2-enemy roll 3-player reroll 4-enemy reroll 5-results 6-end
    private int stage;

    private void Awake()
    {
        playerScore = -1;
        enemyScore = -1;

        scoreCounter = gameObject.GetComponent<ScoreCounter>();
        uiManager = gameObject.GetComponent<UIManager>();

        resultPanel.SetActive(false);

        SetStageAndChangeUIMessages(1);
    }

    // Update is called once per frame
    void Update()
    {
        if (stage == 5 && playerScore > -1 && enemyScore > -1)
        {
            Debug.Log("Stage 6");
            Debug.Log("GM - Player score: " + playerScore + " Enemy score: " + enemyScore);

            if (scoreCounter.CheckPlayerRoundWin(playerScore, enemyScore))
                gameScoreCounter.IncreasePlayerRoundsWon();
            else
                gameScoreCounter.IncreaseEnemyRoundsWon();

            if (!gameScoreCounter.CheckEndOfGame())
            {
                SetStageAndChangeUIMessages(6);                
            }
            else
            {
                SetStageAndChangeUIMessages(7);           
                uiManager.ChangePlayAgainButtonActionAndText(RestartGame,"Next game");
            }

            uiManager.SetRoundScore(playerScore, enemyScore);
            gameScoreCounter.IncreaseRoundNumber();    
        }
                   
    }

    private void Reset()
    {
        playerScore = -1;
        enemyScore = -1;

        playerHand = "";
        enemyHand = "";

        resultPanel.SetActive(false);      
    }

    public void SetStageAndChangeUIMessages(int stage)
    {
        this.stage = stage;
        uiManager.ShowMessage(stage);
    }

    public int GetStage()
    {
        return stage;
    } 
    

    public void NextRound()
    {
        player.Reset();
        enemy.Reset();
        Reset();
        SetStageAndChangeUIMessages(1);
    }
    public void RestartGame()
    {
        gameScoreCounter.RestartGameScore();
        player.Reset();
        enemy.Reset();
        Reset();

        SetStageAndChangeUIMessages(1);
        uiManager.ChangePlayAgainButtonActionAndText(NextRound, "Next round");
    }
}
