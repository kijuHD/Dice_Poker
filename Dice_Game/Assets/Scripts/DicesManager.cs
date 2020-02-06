using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DicesManager : MonoBehaviour
{
    bool needToGetResult = false;
    bool isPlayer;
    bool enemyRoll = false;
    bool getScore = true;
    bool stage3Startup = true;

    public int[] results;
    public int score;
    public string handName;

    public List<GameObject> dices;
    public GameObject dicePrefab;

    public List<Transform> diceSpawners;

    public Transform board;

    public GameManager gameManager;

    private DicesManipulator manipulator;
    private ScoreCounter scoreCounter;
    private EnemyAI enemyAI;

    private void Awake()
    {
        manipulator = gameObject.GetComponent<DicesManipulator>();
        enemyAI = gameObject.GetComponent<EnemyAI>();
        scoreCounter = gameObject.GetComponent<ScoreCounter>();

        if (gameObject.name == "Player")
            isPlayer = true;
        else
            isPlayer = false;

        manipulator.isPlayer = this.isPlayer;

        score = -1;
    }

    // Start is called before the first frame update
    void Start()
    {
        SpawnDices();

        InitializeResultsTab();

        if(isPlayer)
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
                if (isPlayer
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
                        gameManager.SetStageAndChangeUIMessages(2);
                        UnselectAllDices();                        
                    }
                }
                break;
            case 2:
                if (!enemyRoll
                    && !isPlayer)
                {
                    StartCoroutine(Wait());
                    SelectAllDices();
                    RollDices();
                    //manipulator.SetIsKineticForSelectedDices(ref dices, true);
                    enemyRoll = true;
                }

                //Debug.Log("Enemy's Turn: isPlayer: "+isPlayer+" needToGetResult: "+needToGetResult);

                if (!isPlayer
                    && needToGetResult)
                {

                    Debug.Log("Checking stage 2..");
                    GetResult();
                    //sprawdzam rowniez ilosc wynikow w list result
                    needToGetResult = CheckNeedToGetResults();
                    Debug.Log("Need to get ENEMY result: " + needToGetResult);
                    if (!needToGetResult)
                    {
                        manipulator.PlaceSelectedDicesOnBoardAndRotate(ref dices, results, diceSpawners);
                        gameManager.SetStageAndChangeUIMessages(3);
                        UnselectAllDices();
                        enemyRoll = false;
                    }
                }
                break;
            case 3:
                if (stage3Startup
                    && isPlayer)
                {
                    Debug.Log("stage 3 startUp");
                    manipulator.SetInteractableForAllDices(ref dices, true);
                    stage3Startup=!stage3Startup;
                }
                //Debug.Log("players Reroll: isPlayer: " + isPlayer + " needToGetResult: " + needToGetResult);
                if (isPlayer
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
                        gameManager.SetStageAndChangeUIMessages(4);
                        UnselectAllDices();
                        manipulator.SetInteractableForAllDices(ref dices, false);
                        ShowFinalResults();
                        
                    }
                }
                break;
            case 4:
               // Debug.Log("players Reroll: isPlayer: " + isPlayer + " enemyRoll: " + enemyRoll);
                if (!isPlayer 
                    && !enemyRoll)
                {
                    Debug.Log("Enemy is selecting dices");
                    enemyAI.SelectRandomDices(ref dices);
                    enemyRoll = true;

                    manipulator.PlaceSelectedDicesFromObjectAndRandomRotate(ref dices, board.position);
                    manipulator.SetIsKineticForSelectedDices(ref dices, false);
                    manipulator.AddForceToSelectedDices(ref dices, 15f);
                    manipulator.AddRandomTorqueToSelectedDices(ref dices);

                    needToGetResult = true;
                    Debug.Log("Need to get result: " + needToGetResult);

                    if (gameManager.GetStage() == 3 || gameManager.GetStage() == 4)
                        ResetResultForSelectedDices();
                    //RollDices();
                }
                Debug.Log("players Reroll: isPlayer: " + isPlayer + " needToGetResult: " + needToGetResult);
                if (!isPlayer
                    && needToGetResult)
                {

                    Debug.Log("Checking stage 4..");
                    GetResult();
                    //sprawdzam rowniez ilosc wynikow w list result
                    needToGetResult = CheckNeedToGetResults();
                    Debug.Log("Need to get ENEMY result: " + needToGetResult);
                    if (!needToGetResult)
                    {
                        manipulator.PlaceSelectedDicesOnBoardAndRotate(ref dices, results, diceSpawners);
                        gameManager.SetStageAndChangeUIMessages(5);
                        UnselectAllDices();
                        enemyRoll = false;
                    }
                }
                break;
            case 5:
                if (getScore)
                {
                    score = scoreCounter.CountScore(results);
                    handName = scoreCounter.GetHandName(score);
                    
                    if (isPlayer)
                    {
                        Debug.Log("PLAYER Score: " + score + " Hand name: " + handName);
                        gameManager.playerScore = score;
                        gameManager.playerHand = handName;
                    }
                    else
                    {
                        Debug.Log("Enemy Score: " + score + " Hand name: " + handName);
                        gameManager.enemyScore = score;
                        gameManager.enemyHand = handName;
                    }                    
                    if (score > -1)
                    {
                        getScore = false;
                    }                        
                }                
                break;
            default:
                break;
        }            
    }

    public void Reset()
    {
        needToGetResult = false;
        enemyRoll = false;
        getScore = true;
        stage3Startup = true;

        score = -1;
        handName = "";

        ResetResultForAllDices();

        if (isPlayer)
            SelectAllDices();
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(5);
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

        if (gameManager.GetStage() == 3 || gameManager.GetStage() == 4)
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

    private void ResetResultForAllDices()
    {
        for (int i = 0; i < dices.Count; ++i)
        {
            GameObject dice = dices[i];
            D6 diceScript = dice.GetComponent<D6>();
                results[i] = 0;
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
