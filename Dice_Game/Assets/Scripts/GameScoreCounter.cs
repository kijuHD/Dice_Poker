using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScoreCounter : MonoBehaviour
{
    int roundNumber, maxRoundNumber, playerRoundsWon, enemyRoundsWon;

    private void Awake()
    {
        RestartGameScore();
    }

    public void IncreasePlayerRoundsWon()
    {
        playerRoundsWon++;
    }
    public void IncreaseEnemyRoundsWon()
    {
        enemyRoundsWon++;
    }
    public void IncreaseRoundNumber()
    {
        roundNumber++;
    }
    public bool CheckEndOfGame()
    {
        if (roundNumber == maxRoundNumber 
            || playerRoundsWon == 2
            || enemyRoundsWon == 2)
        {
            return true;
        }            
        return false;
    }
    public int GetPlayerRoundsWonNumber()
    {
        return playerRoundsWon;
    }
    public int GetEnemyRoundsWonNumber()
    {
        return enemyRoundsWon;
    }
    public void RestartGameScore()
    {
        roundNumber = 1;
        maxRoundNumber = 3;
        playerRoundsWon = 0;
        enemyRoundsWon = 0;
    }
}
