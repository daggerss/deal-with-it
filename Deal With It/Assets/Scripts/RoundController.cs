using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundController : MonoBehaviour
{
    public NPCDisplay npcDisplay;
    
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

    private int _numberOfPlayers;

    // Start is called before the first frame update
    void Start()
    {
        _numberOfPlayers = GameObject.FindGameObjectsWithTag("PlayerTag").Length;

        // Initializing NPC
        npcDisplay = (NPCDisplay)GameObject.FindGameObjectWithTag("NPC").GetComponent(typeof(NPCDisplay));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /* ---------------------------- Custom Functions ---------------------------- */
    // Goes to next player's turn
    public void NextPlayer(){
        _playerTurn++;
        if(_playerTurn == _numberOfPlayers){
            NextRound();
            _playerTurn = -1;
        }
    }

    // Executes on next round
    public void NextRound(){
        if(npcDisplay.npc.EnergyLvl < 20)
        {
            npcDisplay.npc.EnergyLvl = 20;
        }
        _round++;
    }
}
