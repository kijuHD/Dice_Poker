using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DicesManager : MonoBehaviour
{
    bool needToGetResult = false;
    public List<int>  savedResults;
    public int[] results;

    public List<GameObject> dices;

    public GameObject dicePrefab;
    public List<Transform> diceSpawners;

    public Transform board;

    public GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        SpawnDices();
        results = new int[dices.Count];
        for(int i = 0; i < results.Length; ++i)
        {
            results[i]=0;
        }
        SelectAllDices();
    }

    // Update is called once per frame
    void Update()
    {

        switch (gameManager.GetStage())
        {
            case 0:
                break;
            case 1:
                if (gameObject.name == "Player"
                    && needToGetResult)
                {
                    Debug.Log("Checking stage 1..");
                    GetResult();
                    //sprawdzam rowniez ilosc wynikow w list result
                    needToGetResult=CheckNeedToGetResults();
                    Debug.Log("Need to get result: " + needToGetResult);
                    if (!needToGetResult)
                    {
                        PlaceSelectedDicesOnBoard();
                        //SaveResults();
                        gameManager.SetStage(3);
                        UnselectAllDices();
                        SetInteractableForAllDices(true);
                    }
                }
                break;
            case 2:
                break;
            case 3:
                if (gameObject.name == "Player"
                   && needToGetResult)
                {
                    Debug.Log("Checking stage 3..");
                    GetResult();
                    //sprawdzam rowniez ilosc wynikow w list result
                    needToGetResult = CheckNeedToGetResults();
                    if (!needToGetResult)
                    {
                        PlaceSelectedDicesOnBoard();
                        //SaveResults();
                        gameManager.SetStage(3);
                        UnselectAllDices();
                        SetInteractableForAllDices(true);
                        ShowFinalResults();
                        //for test only
                        /*for(int i = 0; i < savedResults.Count; i++)
                        {
                            Debug.Log("Saved Results: " + savedResults[i]);
                        }
                        */
                    }
                }
                break;
            case 4:
                break;
            case 5:
                break;
            default:
                break;
        }

       
            


       // if (CheckIfRollEnded())
            
    }

    private void SpawnDices()
    {
        foreach (Transform spawner in diceSpawners)
        {
            GameObject dice;
            dice = Instantiate(dicePrefab, spawner.position, spawner.rotation);            
            dices.Add(dice);
        }        
    }
    public void RollDices()
    {
        
        PlaceSelectedDicesAboveBoard();
        SetIsKineticForAllSelectedDices(false);
         AddForceToSelectedDices();
          AddTorqueToSelectedDices();
        needToGetResult = true;
        Debug.Log("Need to get result: " + needToGetResult);

        if (gameManager.GetStage() == 3)
        {
            ResetResultForSelectedDices();
        }
    }

    private void PlaceSelectedDicesAboveBoard()
    {
        int count = 1;
        float x = -0.25f;
        float y = 0.5f;
        float z = 1f;

        foreach (GameObject dice in dices)
        {
            D6 diceScript = dice.GetComponent<D6>();
            if (diceScript.IsSelected())
            {
                if (count % 2 == 0)
                {
                    x += 0.25f;
                    diceScript.MoveAboveObject(new Vector3(x, y, z), board.position);
                }
                else
                    diceScript.MoveAboveObject(new Vector3(x, y + 0.5f, z), board.position);

                diceScript.RandomRotate();
                count++;
            }            
        }
    }

    private void SetIsKineticForAllSelectedDices(bool var)
    {
        foreach (GameObject dice in dices)
        {
            D6 diceScript = dice.GetComponent<D6>();
            if (diceScript.IsSelected())
            {
                Rigidbody rigidbody = dice.GetComponent<Rigidbody>();

                rigidbody.isKinematic = var;
            }            
        }
    }

    private void AddForceToSelectedDices()
    {
        foreach(GameObject dice in dices)
        {
            D6 diceScript = dice.GetComponent<D6>();
            if (diceScript.IsSelected())
            {
                if (gameObject.name == "Player")
                    diceScript.AddForce(-15);

                else
                    diceScript.AddForce(15);
            }            
        }
    }

    private void AddTorqueToSelectedDices()
    {
        foreach (GameObject dice in dices)
        {
            D6 diceScript = dice.GetComponent<D6>();
            if (diceScript.IsSelected())
            {
                diceScript.AddRandomTorque();
            }
            
        }
    }

    private void GetResult()
    {
        for(int i = 0; i < dices.Count; ++i)
        {
            GameObject dice = dices[i];

            Rigidbody rigidbody = dice.GetComponent<Rigidbody>();

            if (rigidbody.IsSleeping())
            {
                D6 diceScript = dice.GetComponent<D6>();
                Transform diceTransform = dice.GetComponent<Transform>();
                int result = diceScript.GetResult(Vector3.up, 45f);

                if (results[i] <= 0)
                {
                    results[i] = result;

                    Debug.Log("RESULT is: " + result);
                    rigidbody.isKinematic = true;
                }                
            }
        }       
    }

    private void PlaceSelectedDicesOnBoard()
    {
        int res = 0;
        for (int i =0; i<dices.Count;++i)
        {            
            D6 diceScript = dices[i].GetComponent<D6>();

            if (diceScript.IsSelected())
            {
                Transform transform = dices[i].GetComponent<Transform>();
                //Do zmiany
                // transform.rotation = diceSpawners[i].rotation;
                //diceScript.Rotate(diceSpawners[i].rotation);


                diceScript.MoveTo(diceSpawners[i].position);

                transform.rotation = new Quaternion(0, 0, 0, 1);

                diceScript.RotateToShowResult(results[i], diceSpawners[i].rotation);
                res++;
            }       
                     
        }        
    }

    private bool CheckNeedToGetResults()
    {
      /*  int count = 0;
        foreach(GameObject dice in dices)
        {
            D6 diceScript = dice.GetComponent<D6>();
            if (diceScript.IsSelected())
                count++;
        }

      //  Debug.Log("Result count: " + results.Count + " dices count: " + count);

        if (results.Count == dices.Count && needToGetResult)
            needToGetResult = false;*/

        for(int i = 0; i < results.Length; ++i)
        {
            if (results[i] <= 0)
                return true;
        }
        return false;
    }

    //dev only - przenosze results do saveresults
    private void SaveResults()
    {
        foreach(int result in results)
        {
            savedResults.Add(result);
        }
        //results.Clear();
    }

    private void SetInteractableForAllDices(bool var)
    {
        foreach(GameObject dice in dices)
        {
            D6 diceScript = dice.GetComponent<D6>();
            diceScript.interactable = var;
        }
    }

    private void SelectAllDices()
    {
        foreach(GameObject dice in dices)
        {
            D6 diceScript = dice.GetComponent<D6>();
            diceScript.Select();
        }
    }
    private void UnselectAllDices()
    {
        foreach (GameObject dice in dices)
        {
            D6 diceScript = dice.GetComponent<D6>();
            diceScript.UnSelect();
        }
    }

    private void ResetResultForSelectedDices()
    {
        for(int i = 0; i < dices.Count; ++i)
        {
            GameObject dice = dices[i];
            D6 diceScript = dice.GetComponent<D6>();
            if (diceScript.IsSelected())
            {
                results[i] = 0;
            }
        }
    }

    private void ShowFinalResults()
    {
        Debug.Log("Final results: " + results[0] + "," + results[1] + "," + results[2] + "," + results[3] + "," + results[4]);
    }
}
