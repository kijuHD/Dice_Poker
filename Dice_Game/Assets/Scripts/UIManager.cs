using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text messageText;
    public Text resultText;
    public Text playerHandNameText;
    public Text enemyHandNameText;
    public Text playerRoundScoreText;
    public Text enemyRoundScoreText;

    public Button playAgainButton;
    public Text playAgainButtonText;

    public GameObject resultPanel;


    public void ShowMessage(int stage)
    {
        switch (stage)
        {
            case 0:
                messageText.text = "END";
                break;
            case 1:
                messageText.text = "Roll the dices";
                break;
            case 2:
                messageText.text = "Enemy's turn";
                break;
            case 3:
                messageText.text = "Select dices you want to reroll";
                break;
            case 4:
                messageText.text = "Enemy is rerolling";
                break;
            case 5:
                messageText.text = "RESULTS";
                break;
            default:
                break;
        }
    }

    private void SetHandNamesForResultPanel(string playerHand, string enemyHand)
    {
        playerHandNameText.text = playerHand;
        enemyHandNameText.text = enemyHand;    
    }
    private void SetResultForResultPanel(int stage,bool isPlayerWin)
    {
        if (stage == 6)
        {
            if (isPlayerWin)
                resultText.text = "You win this round!";
            else
                resultText.text = "You lose this round!";
        }
        else if (stage == 7)
        {
            if (isPlayerWin)
                resultText.text = "You win this game!";
            else
                resultText.text = "You lose this game!";
        }
    }

    public void ChangePlayAgainButtonActionAndText(UnityEngine.Events.UnityAction call,string txt)
    {
        ChangePlayAgainButtonText(txt);
        ChangeOnClickActionPlayAgainButton(call);
    }

    private void ChangePlayAgainButtonText(string txt)
    {
        playAgainButtonText.text = txt;
    }
    private void ChangeOnClickActionPlayAgainButton(UnityEngine.Events.UnityAction call)
    {
        playAgainButton.onClick.RemoveAllListeners();
        playAgainButton.onClick.AddListener(call);
    }


    public void SetRoundScore(int playerScore, int enemyScore)
    {
        string ps = playerScore.ToString();
        SetPlayerRoundScore(ps);

        string es = enemyScore.ToString();
        SetEnemyRoundScore(es);
    }

    private void SetPlayerRoundScore(string score)
    {
        playerRoundScoreText.text = score;
    }
    private void SetEnemyRoundScore(string score)
    {
        enemyRoundScoreText.text = score;
    }

}
