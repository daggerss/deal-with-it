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
    public PlayedActionCardsDisplay PlayedActionCards;
    private int _totalDistractionCount = 0;
    private int _totalExpressionCount = 0;
    private int _totalProcessingCount = 0;
    private int _totalReappraisalCount = 0;

    /* ------------------------------- Skip Player ------------------------------ */
    public IEnumerator Skip; 
    public bool CountdownActive = false;

    /* -------------------------------------------------------------------------- */
    /*                                  Functions                                 */
    /* -------------------------------------------------------------------------- */
    // Start is called before the first frame update
    void Start()
    {
        // Initialize NumberOfPlayers
        NumberOfPlayers = GameObject.FindGameObjectsWithTag("PlayerTag").Length;

        // Initialize NPC
        NPCDisplay NPCDisplay = (NPCDisplay)GameObject.FindGameObjectWithTag("NPC").GetComponent(typeof(NPCDisplay));
        NPC = NPCDisplay.npc;

        // Initialize PlayedActionCards
        PlayedActionCards = (PlayedActionCardsDisplay)GameObject.FindGameObjectWithTag("Played Action Cards Display").GetComponent(typeof(PlayedActionCardsDisplay));

        // Initialize _skipPlayer
        Skip = SkipPlayer(10f);
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
        CountdownActive = false;
        if(_playerTurn == NumberOfPlayers){
            StartCoroutine(NextRound());
        }else{
            _playerTurn++;
        }
    }

    /* ------------------------ Skip player if over time ------------------------ */
    public IEnumerator SkipPlayer(float seconds){
        Debug.Log("Coroutine started");
        CountdownActive = true;
        int elapsedTime = 0;
        while(elapsedTime < seconds){
            yield return new WaitForSeconds(1f);
            elapsedTime++;
        }

        if(elapsedTime >= seconds){
            Debug.Log("Player skipped");
            NextPlayer();
        }else{
            Debug.Log("Player Not Skipped");
        }

        CountdownActive = false;
    }

    /* ------------------------- Executes on next round ------------------------- */
    public IEnumerator NextRound(){
        yield return new WaitForSeconds(3f);
        string winLoseStatus = WinLoseStatus();
        Debug.Log(_goalCounter);
        if(winLoseStatus == "continue"){
            _round++;
            _playerTurn = -1;
        }else if(winLoseStatus == "win"){
            Debug.Log("You win!");
        }else{
            Debug.Log("You lose!");
        }
    }

    // Checks if NPC wins or loses or continue playing
    private string WinLoseStatus(){
        // Count total number of played cards
        for(int i = 0; i < PlayedActionCards.PlayedActionCards.Length; i++){
            if(PlayedActionCards.PlayedActionCards[i] != null){
                ActionType actionType = PlayedActionCards.PlayedActionCards[i].CardActionType;
                if(actionType == ActionType.Distraction){
                    _totalDistractionCount++;
                }else if(actionType == ActionType.Expression){
                    _totalExpressionCount++;
                }else if(actionType == ActionType.Processing){
                    _totalProcessingCount++;
                }else if(actionType == ActionType.Reappraisal){
                    _totalReappraisalCount++;
                }
            }else{
                break;
            }
        }

        switch(NPC.CardName.ToUpper()){
            /* ---------------------------------- Knot ---------------------------------- */
            case "KNOT":
                if((NPC.JoyLvl >= 6 && NPC.JoyLvl <= 8) &&
                (NPC.SadnessLvl >= 6 && NPC.SadnessLvl <= 8) &&
                (NPC.FearLvl < 6) &&
                (NPC.AngerLvl < 6)){ // Joy and Sadness neutral and Fear and Anger below 6 for 7 rounds
                    _goalCounter++;
                }else{ // Goal counter reset when neither is set
                    _goalCounter = 0;
                }

                if(PlayedActionCards.ExpressionCount > 2){
                    _goalCounter = -1;
                }

                if(_goalCounter == 7){
                    return "win";
                }

                break;

            case "PICKLES":
                if(NPC.JoyLvl > 10 ||
                NPC.SadnessLvl > 10 ||
                NPC.FearLvl > 10 ||
                NPC.AngerLvl > 10){
                    _goalCounter = -1;
                }

                // TODO Add playing more than 10 expression cards in the whole game as win

                if(_goalCounter == 1){
                    return "win";
                }

                break;

            case "SNIFFLES":
                if((NPC.JoyLvl >= 6 && NPC.JoyLvl <= 8) &&
                (NPC.SadnessLvl >= 6 && NPC.SadnessLvl <= 8) &&
                (NPC.FearLvl < 6) &&
                (NPC.AngerLvl < 6)){ // Joy and Sadness neutral and Fear and Anger below 6 for 7 rounds
                    _goalCounter++;
                }else{ // Goal counter reset when neither is set
                    _goalCounter = 0;
                }

                if(_goalCounter == 1){
                    return "win";
                }

                break;

            default:
                return "continue";
        }

        if(NPC.JoyLvl + PlayedActionCards.TotalJoyVal > 13 ||
        NPC.SadnessLvl + PlayedActionCards.TotalSadnessVal > 13 ||
        NPC.FearLvl + PlayedActionCards.TotalFearVal > 13 ||
        NPC.AngerLvl + PlayedActionCards.TotalAngerVal > 13){
            _goalCounter = -1;
        }

        if(_goalCounter < 0){
            return "lose";
        }else{
            return "continue";
        }
    }
}