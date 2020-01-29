using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject resultPanel;
    public Text messageText;



    //0-setup 1-player roll 2-enemy roll 3-player reroll 4-enemy reroll 5-results 6-end
    private int stage;

    private void Awake()
    {
        resultPanel.SetActive(false);
        SetStage(1);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetStage(int stage)
    {
        this.stage = stage;
        ShowMessage(stage);
    }

    public int GetStage()
    {
        return stage;
    }

    private void ShowMessage(int stage)
    {
        switch (stage)
        {            
            case 1:
                messageText.text = "Roll the dices";
                //Messages[0].gameObject.SetActive(true);

                break;
            case 2:
                messageText.text = "Enemy's turn";
                // Messages[2].SetActive(true);

                break;
            case 3:
                messageText.text = "Select dices you want to reroll";
                /// Messages[1].SetActive(true);

                break;
        }
    }

}
