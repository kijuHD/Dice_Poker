using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public void SelectRandomDices(ref List<GameObject> dices)
    {
        List<int> selectedDices = new List<int>();
        selectedDices.Add(-1);

        // int numOfDices = Random.Range(0, dices.Count + 1);
        int numOfDices = 5;

        Debug.Log("Enemy wants to reroll " + numOfDices + " dices");

        int count = 0;

        for (int i = 0; i < numOfDices; ++i)
        {
             int diceNum=-1;
            /* do
             {
                 diceNum = Random.Range(0, dices.Count);
                 count++;
             } while ( !selectedDices.Contains(diceNum) || count<1000);*/

            diceNum = Random.Range(0, dices.Count);

            Dice script = dices[diceNum].GetComponent<Dice>();
            script.Select();
            Debug.Log("Enemy selected Dice number: " + diceNum);
            selectedDices.Add(diceNum);
        }

        selectedDices.RemoveAt(0);
    }


    private bool CheckIfSelectedBefore(int num, List<int>selectedDices)
    {
        if (selectedDices.Count > 0)
        {
            foreach (int selected in selectedDices)
            {
                if (selected == num)
                    return true;
            }
        }        
        return false;
    }

}
