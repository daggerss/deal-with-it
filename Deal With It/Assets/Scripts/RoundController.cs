using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

    /* ----------------------------- Win Lose Label ----------------------------- */
    public Button WinLoseLabel;
    public TMP_Text WinLoseLabelText;

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

        // Initialize WinLoseLabel
        WinLoseLabel.gameObject.SetActive(false);
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
        }else{
            WinLoseLabel.gameObject.SetActive(true);
            WinLoseLabelText.text = winLoseStatus;
        }
    }

    /* ------------- Checks if NPC wins or loses or continue playing ------------ */
    private string WinLoseStatus(){
        /* ------------------------- Overload and Underload ------------------------- */
        if(PlayedActionCards.OverloadUnderload){
            return "One or more of the emotions has overloaded or underloaded! You lose!";
        }

        if(NPC.EnergyLvl <= -10){
            return "You have overexhausted " + NPC.CardName + "! You lose!";
        }

        /* ------------------- Count total number of played cards ------------------- */
        _totalDistractionCount += PlayedActionCards.DistractionCount;
        _totalExpressionCount += PlayedActionCards.ExpressionCount;
        _totalProcessingCount += PlayedActionCards.ProcessingCount;
        _totalReappraisalCount += PlayedActionCards.ReappraisalCount;

        /* ------------------------- Card Per Round Counter ------------------------- */
        if(PlayedActionCards.DistractionCount > NPC.MaxDistractionPerRound ||
        PlayedActionCards.ExpressionCount > NPC.MaxExpressionPerRound ||
        PlayedActionCards.ProcessingCount > NPC.MaxProcessingPerRound ||
        PlayedActionCards.ReappraisalCount > NPC.MaxReappraisalPerRound){
            return "You used too many of a certain action type per round! You lose!";
        }

        /* ------------------------------- Card Total ------------------------------- */
        if(NPC.MinDistractionTotal < _totalDistractionCount &&
        NPC.MinExpressionTotal < _totalExpressionCount &&
        NPC.MinProcessingTotal < _totalProcessingCount &&
        NPC.MinReappraisalTotal < _totalReappraisalCount){
            if(NPC.MinDistractionTotal != 0 && NPC.MinExpressionTotal != 0 && NPC.MinProcessingTotal != 0 && NPC.MinReappraisalTotal != 0){
                return "You reached the goal of certain action card type played! You win!";
            }
        }

        /* ----------------- Checks is Emotions are within NPC range ---------------- */
        if(NPC.JoyRange.x <= NPC.JoyLvl && NPC.JoyLvl <= NPC.JoyRange.y &&
        NPC.SadnessRange.x <= NPC.SadnessLvl && NPC.SadnessLvl <= NPC.SadnessRange.y &&
        NPC.FearRange.x <= NPC.FearLvl && NPC.FearLvl <= NPC.FearRange.y &&
        NPC.AngerRange.x <= NPC.AngerLvl && NPC.AngerLvl <= NPC.AngerRange.y){
            // If goal counter is negative or 0 set goal counter to 1
            if(_goalCounter <= 0){
                _goalCounter = 1;

            // If goal counter is positive and not equal to 0 increment goal counter by 1
            }else{
                _goalCounter++;
            }
        }else{

            // If goal counter is positive or 0 set goal counter to -1
            if(_goalCounter >= 0){
                _goalCounter = -1;

            // If goal counter is negative and not 0 decrement goal counter by 1
            }else{
                _goalCounter--;
            }
        }

        // Emotion Range Win Checker
        if(NPC.RangeWinDuration == -1){
            // Do nothing
        }else if(_goalCounter == NPC.RangeWinDuration){
            return "You have successfully kept the emotions within the correct range! You win!";
        }

        // Emotion Range Lose Checker
        if(NPC.RangeLoseDuration == -1){
            // Do nothing
        }else if(_goalCounter * -1 == NPC.RangeLoseDuration){
            return "You failed to keep the emotions within the right range! You lose!";
        }

        return "continue";
    }
}