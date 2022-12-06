using System;
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
    public int GoalCount => _goalCounter;

    public PlayedActionCardsDisplay PlayedActionCards;

    private int _totalDistractionCount = 0;
    public int TotalDistractionCount => _totalDistractionCount;

    private int _totalExpressionCount = 0;
    public int TotalExpressionCount => _totalExpressionCount;

    private int _totalProcessingCount = 0;
    public int TotalProcessingCount => _totalProcessingCount;

    private int _totalReappraisalCount = 0;
    public int TotalReappraisalCount => _totalReappraisalCount;

    // For Win Tracker
    private bool _durationWinStatus;
    public bool DurationWinStatus => _durationWinStatus;

    /* ------------------------------ Player Timer ------------------------------ */
    public float PlayerTimer = 30;
    public bool StopTimer = false;
    public bool PlayerSkipped = false;

    // ! Legacy timer
    // // [SerializeField] private TMP_Text _timerText;
    // // [SerializeField] private Image _timerFill;

    /* -------------------------------- Win Lose -------------------------------- */
    private bool _isWon;
    private string _winLoseStatus;
    private string _gameOverTip = null;
    [SerializeField] private GameSetDisplay gameSetDisplay;

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

        // ! Legacy Hide timer
        // // _timerText.text = null;
        // // _timerFill.fillAmount = 0f;

        // Set Win Tracker Status
        if (NPC.RangeWinDuration > 0)
        {
            _durationWinStatus = false;
        }
        else
        {
            _durationWinStatus = true;
        }
    }

    void Update(){
        // Subtract from timer
        if(!StopTimer && _playerTurn != -1){
            PlayerTimer -= Time.deltaTime;

            //! Legacy timer
            // // _timerText.text = (Math.Ceiling(PlayerTimer)).ToString();
            // // _timerFill.fillAmount = (float)(Math.Ceiling(PlayerTimer)) / 30;
        }

        // If timer runs out
        if(PlayerTimer <= 0 && !StopTimer){
            StopTimer = true;
            StartCoroutine(SkipPlayer());
        }
    }

    /* -------------------------------------------------------------------------- */
    /*                              Custom Functions                              */
    /* -------------------------------------------------------------------------- */
    /* ------------------------------- Skip Player ------------------------------ */
    public IEnumerator SkipPlayer(){
        // No need na
        // // WinLoseLabel.gameObject.SetActive(true);
        // // WinLoseLabelText.text = "Ran out of time! Your turn was skipped";

        PlayerSkipped = true;

        yield return new WaitForSeconds(3f);

        StopTimer = false;
        PlayerSkipped = false;
        // // WinLoseLabel.gameObject.SetActive(false);

        NextPlayer();
    }

    /* ----------------------- Goes to next player's turn ----------------------- */
    public void NextPlayer(){
        if(_playerTurn == NumberOfPlayers){
            StartCoroutine(NextRound());
        }else{
            _playerTurn++;
            PlayerTimer = 30;
        }
    }

    /* ------------------------- Executes on next round ------------------------- */
    public IEnumerator NextRound(){
        StopTimer = true; // Stop the timer for now because there is a 3 second delay from switching to next round
        yield return new WaitForSeconds(3f);
        StopTimer = false; // switching to next round
        _winLoseStatus = CheckWinLoseStatus();
        if(_winLoseStatus == "continue"){
            _round++;
            _playerTurn = -1;

            // ! Legacy Hide timer
            // // _timerText.text = null;
            // // _timerFill.fillAmount = 0f;
        }else{
            StopTimer = true;
            gameSetDisplay.SetGameStatus(_isWon, _winLoseStatus, _gameOverTip);
            gameSetDisplay.gameObject.SetActive(true);
        }
    }

    /* ------------- Checks if NPC wins or loses or continue playing ------------ */
    private string CheckWinLoseStatus(){
        /* ------------------------- Overload and Underload ------------------------- */
        if(PlayedActionCards.IsAnyLoad){
            _isWon = false;

            // Tip message
            if (PlayedActionCards.LoadLevelType == LevelType.Joy)
            {
                _gameOverTip = "Tip: Joy is wonderful, but don't lose sight of other experiences in life.";
            }
            else if (PlayedActionCards.LoadLevelType == LevelType.Sadness)
            {
                _gameOverTip = "Tip: Sadness comes from love, but try not to dwell on it either.";
            }
            else if (PlayedActionCards.LoadLevelType == LevelType.Fear)
            {
                _gameOverTip = "Tip: Fear keeps you safe, but neither cowardice nor recklessness is a virtue.";
            }
            else if (PlayedActionCards.LoadLevelType == LevelType.Anger)
            {
                _gameOverTip = "Tip: Anger lets you speak out, but don't let it control you.";
            }

            return "Oh no! At least one emotion was under or overloaded!";
        }

        if(NPC.EnergyLvl <= -10){
            _isWon = false;
            return "Oh no! You have overexhausted " + NPC.CardName + "!";
        }

        /* ------------------- Count total number of played cards ------------------- */
        _totalDistractionCount += PlayedActionCards.DistractionCount;
        _totalExpressionCount += PlayedActionCards.ExpressionCount;
        _totalProcessingCount += PlayedActionCards.ProcessingCount;
        _totalReappraisalCount += PlayedActionCards.ReappraisalCount;

        /* ------------------------- Card Per Round Counter ------------------------- */
        if(PlayedActionCards.DistractionCount > NPC.MaxDistractionPerRound)
        {
            _isWon = false;
            return "You relied on Distraction too much! It's important to learn the others too.";
        }
        if (PlayedActionCards.ExpressionCount > NPC.MaxExpressionPerRound)
        {
            _isWon = false;
            return "You relied on Expression too much! It's important to learn the others too.";
        }
        if (PlayedActionCards.ProcessingCount > NPC.MaxProcessingPerRound)
        {
            _isWon = false;
            return "You relied on Processing too much! It's important to learn the others too.";
        }
        if (PlayedActionCards.ReappraisalCount > NPC.MaxReappraisalPerRound)
        {
            _isWon = false;
            return "You relied on Reappraisal too much! It's important to learn the others too.";
        }

        /* ------------------------------- Card Total ------------------------------- */
        if(NPC.MinDistractionTotal <= _totalDistractionCount &&
        NPC.MinExpressionTotal <= _totalExpressionCount &&
        NPC.MinProcessingTotal <= _totalProcessingCount &&
        NPC.MinReappraisalTotal <= _totalReappraisalCount){
            if(NPC.MinDistractionTotal != 0 && NPC.MinExpressionTotal != 0 && NPC.MinProcessingTotal != 0 && NPC.MinReappraisalTotal != 0){
                _isWon = true;
                return "You've learned to use a certain strategy more! Keep it up!";
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
            _durationWinStatus = true;
            _isWon = true;
            return "You've kept " + NPC.CardName + "'s emotions at healthy levels! Congratulations!";
        }

        // Emotion Range Lose Checker
        if(NPC.RangeLoseDuration == -1){
            // Do nothing
        }else if(_goalCounter * -1 == NPC.RangeLoseDuration){
            _durationWinStatus = false;
            _isWon = false;
            return "It's hard to balance emotions. Don't be afraid to try again!";
        }

        return "continue";
    }
}