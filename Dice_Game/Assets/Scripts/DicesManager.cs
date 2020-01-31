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

    private DicesManipulator manipulator;

    private void Awake()
    {
        manipulator = gameObject.GetComponent<DicesManipulator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        SpawnDices();

        InitializeResultsTab();

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
                        manipulator.PlaceSelectedDicesOnBoardAndRotate(ref dices, results, diceSpawners);
                        //SaveResults();
                        gameManager.SetStage(3);
                        UnselectAllDices();
                        manipulator.SetInteractableForAllDices(ref dices, true);
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
                        manipulator.PlaceSelectedDicesOnBoardAndRotate(ref dices, results, diceSpawners);
                        //SaveResults();
                        gameManager.SetStage(3);
                        UnselectAllDices();
                        manipulator.SetInteractableForAllDices(ref dices, true);
                        ShowFinalResults();
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
    }

    //On roll button click
    public void RollDices()
    {
        manipulator.PlaceSelectedDicesFromObjectAndRandomRotate(ref dices, board.position);
        manipulator.SetIsKineticForSelectedDices(ref dices, false);
        manipulator.AddForceToSelectedDices(ref dices, 15f);
        manipulator.AddRandomTorqueToSelectedDices(ref dices);

        needToGetResult = true;
        Debug.Log("Need to get result: " + needToGetResult);

        if (gameManager.GetStage() == 3)
            ResetResultForSelectedDices();
    }

    #region On Game's Start Methods
    private void SpawnDices()
    {
        foreach (Transform spawner in diceSpawners)
        {
            GameObject dice;
            dice = Instantiate(dicePrefab, spawner.position, spawner.rotation);            
            dices.Add(dice);
        }        
    }

    private void InitializeResultsTab()
    {
        results = new int[dices.Count];

        for (int i = 0; i < results.Length; ++i)
        {
            results[i] = 0;
        }
    }

    #endregion  

    #region Select Methods
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

    #endregion

    #region Result Methods
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
    private void GetResult()
    {
        for (int i = 0; i < dices.Count; ++i)
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

    //Dev only
    private void ShowFinalResults()
    {
        Debug.Log("Final results: " + results[0] + "," + results[1] + "," + results[2] + "," + results[3] + "," + results[4]);
    }
    private bool CheckNeedToGetResults()
    {
        for (int i = 0; i < results.Length; ++i)
        {
            if (results[i] <= 0)
                return true;
        }
        return false;
    }

    #endregion
}
