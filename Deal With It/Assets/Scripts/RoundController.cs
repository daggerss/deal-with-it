using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundController : MonoBehaviour
{
    private int _round = 0;
    public int Round
    {
        get
        {
            return _round;
        }
        set
        {
            _round = value;
        }
    }

    // Initial is -1
    // -1 is when event cards are drawn
    private int _playerTurn = -1;
    public int PlayerTurn
    {
        get
        {
            return _playerTurn;
        }
        set
        {
            _playerTurn = value;
        }
    }

    public int NumberOfPlayers;

    /* ----------------------------------- NPC ---------------------------------- */
    public NPC NPC;

    /* ------------------------------ Goal Counter ------------------------------ */
    private int _goalCounter = 0;

    // Start is called before the first frame update
    void Start()
    {
        NumberOfPlayers = GameObject.FindGameObjectsWithTag("PlayerTag").Length;

        NPCDisplay npcDisplay = (NPCDisplay)GameObject.FindGameObjectWithTag("NPC").GetComponent(typeof(NPCDisplay));
        npc = npcDisplay.npc;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /* -------------------------------------------------------------------------- */
    /*                              Custom Functions                              */
    /* -------------------------------------------------------------------------- */
    /* ----------------------- Goes to next player's turn ----------------------- */
    public void NextPlayer(){
        if(_playerTurn == NumberOfPlayers){
            StartCoroutine(NextRound());
        }else{
            _playerTurn++;
        }
    }

    /* ------------------------- Executes on next round ------------------------- */
    public IEnumerator NextRound(){
        yield return new WaitForSeconds(3f);
        _round++;
        _playerTurn = -1;
    }

    // Checks if NPC wins or loses or continue playing
    private string WinLoseStatus(){
        switch(npc.CardName.ToUpper()){
            /* ---------------------------------- Knot ---------------------------------- */
            case "KNOT":
                if((npc.JoyLvl >= 6 && npc.JoyLvl <= 8) &&
                (npc.SadnessLvl >= 6 && npc.SadnessLvl <= 8) &&
                (npc.FearLvl < 6) &&
                (npc.AngerLvl < 6)){
                    _goalCounter++;
                }else{
                    _goalCounter = 0;
                }

                if(_goalCounter == 7){
                    return "win";
                }
                break;
            default:
                break;
        }
    }
}