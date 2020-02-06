using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DicesManipulator : MonoBehaviour
{
    public bool isPlayer;
    public void SetIsKineticForSelectedDices(ref List<GameObject> dices, bool var)
    {
        foreach (GameObject dice in dices)
        {
            Dice script = dice.GetComponent<Dice>();
            if (script.IsSelected())
            {
                Rigidbody rigidbody = dice.GetComponent<Rigidbody>();

                rigidbody.isKinematic = var;
            }
        }
    }
    public void SetInteractableForAllDices(ref List<GameObject> dices, bool var)
    {
        foreach (GameObject dice in dices)
        {
            Dice script = dice.GetComponent<Dice>();
            script.interactable = var;
        }
    }
    public void AddForceToSelectedDices(ref List<GameObject> dices, float force)
    {
        foreach (GameObject dice in dices)
        {
            Dice script = dice.GetComponent<Dice>();
            if (script.IsSelected())
            {
                if (isPlayer)
                    script.AddForce(-force);

                else
                    script.AddForce(force);
            }
        }
    }
    public void AddRandomTorqueToSelectedDices(ref List<GameObject> dices)
    {
        foreach (GameObject dice in dices)
        {
            Dice script = dice.GetComponent<Dice>();
            if (script.IsSelected())
            {
                script.AddRandomTorque();
            }
        }
    }
    public void PlaceSelectedDicesOnBoardAndRotate(ref List<GameObject> dices, int[] results, List<Transform> diceSpawners)
    {
        int res = 0;
        for (int i = 0; i < dices.Count; ++i)
        {
            Dice script = dices[i].GetComponent<Dice>();

            if (script.IsSelected())
            {
                Transform transform = dices[i].GetComponent<Transform>();

                script.MoveTo(diceSpawners[i].position);

                transform.rotation = new Quaternion(0, 0, 0, 1);

                script.RotateToShowResult(results[i], diceSpawners[i].rotation);
                res++;
            }
        }
    }
    public void PlaceSelectedDicesFromObjectAndRandomRotate(ref List<GameObject> dices, Vector3 objectPosition)
    {
        //only for players dices
        //later change with enemy added
        float x = 0, y = 0, z = 0;

        if (isPlayer)
        {
            x = -0.25f;
            y = 0.5f;
            z = 1f;
        }
        else
        {
            x = -0.5f;
            y = 0.5f;
            z = -1.25f;
        }

        for (int i = 1; i <= dices.Count; ++i)
        {
            GameObject dice = dices[i - 1];
            Dice script = dice.GetComponent<Dice>();
            if (script.IsSelected())
            {
                if (i % 2 == 0)
                {
                    x += 0.25f;
                    script.MoveFromObject(new Vector3(x, y, z), objectPosition);
                }
                else
                    script.MoveFromObject(new Vector3(x, y + 0.5f, z), objectPosition);

                script.RandomRotate();
            }
        }
    }
}
