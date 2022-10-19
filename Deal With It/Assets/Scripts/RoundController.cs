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

    private int _numberOfPlayers;

    // Start is called before the first frame update
    void Start()
    {
        _numberOfPlayers = GameObject.FindGameObjectsWithTag("PlayerTag").Length;
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
        if(_playerTurn == _numberOfPlayers - 1){
            StartCoroutine(NextRound());
        }else{
            _playerTurn++;
        }
    }

    /* ------------------------- Executes on next round ------------------------- */
    public IEnumerator NextRound(){
        //! Add delay to not move to next round right away
        yield return new WaitForSeconds(3f);
        _round++;
        _playerTurn = -1;
    }
}
