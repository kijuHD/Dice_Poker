using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreCounter : MonoBehaviour
{
    

    private int[] SumUpResults(int[] results)
    {
        int[] counts = new int[] { 0, 0, 0, 0, 0, 0 };

        for(int i=0; i < results.Length; ++i)
        {
            counts[results[i] - 1]++;
        }        

        return counts;
    }

    public string GetHandName(int score)
    {
        switch (score)
        {
            case 0:
                return "Nothing";
            case 1:
                return "One pair";
            case 2:
                return "Two pair";
            case 3:
                return "Three of a kind";
            case 4:
                return "Full house";
            case 8:
                return "Four of a kind";
            case 10:
                return "Five of a kind";
            default:
                return "Error -hand name-";
        }
    }

    public int CountScore(int[] results)
    {
        /*
        Highest

        Five of a kind = 10
        Four of a kind = 8
        Full house (Three of a kind and a pair) = 4
        Three of a kind = 3
        Two pair = 2
        One pair = 1

        Lowest
        */

        int[] counts = SumUpResults(results);
        int score = 0;

        for(int i = 0; i < counts.Length; ++i)
        {      

            switch (counts[i])
            {                
                case 2:
                    score += 1;
                    break;
                case 3:
                    score += 3;
                    break;
                case 4:
                    score += 8;
                    break;
                case 5:
                    score += 10;
                    break;
                default:
                    break;
            }
        }
        return score;
    }

    public bool CheckPlayerRoundWin(int playerScore,int enemyScore)
    {
        if (playerScore > enemyScore)
            return true;

        return false;
    }

}
