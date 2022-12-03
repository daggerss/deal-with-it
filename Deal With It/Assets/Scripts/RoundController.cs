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
    public PlayedActionCardsDisplay PlayedActionCards;
    private int _totalDistractionCount = 0;
    private int _totalExpressionCount = 0;
    private int _totalProcessingCount = 0;
    private int _totalReappraisalCount = 0;

    /* ------------------------------ Player Timer ------------------------------ */
    public float PlayerTimer = 30;
    public bool StopTimer = false;
    [SerializeField] private TMP_Text _timerText;
    [SerializeField] private Image _timerFill;

    /* ----------------------------- Win Lose Label ----------------------------- */
    public Button WinLoseLabel;
    public TMP_Text WinLoseLabelText;

    /* ------------------------- Win Condition Tracker  ------------------------- */
    public GameObject WinTracker;

    [SerializeField] private TMP_Text conditionVars;
    [SerializeField] private TMP_Text counter;
    [SerializeField] private GameObject emotionRangeCheckmark;
    [SerializeField] private GameObject joyCheckmark;
    [SerializeField] private GameObject sadCheckmark;
    [SerializeField] private GameObject fearCheckmark;
    [SerializeField] private GameObject angerCheckmark;
    [SerializeField] private TMP_Text joyTracker;
    [SerializeField] private TMP_Text sadTracker;
    [SerializeField] private TMP_Text fearTracker;
    [SerializeField] private TMP_Text angerTracker;
    bool joy = false;
    bool sadness = false;
    bool anger = false;
    bool fear = false;

    [SerializeField] private GameObject expressionPerRound;
    [SerializeField] private TMP_Text expressionMax;
    [SerializeField] private TMP_Text expressionUsed;
    [SerializeField] private GameObject maxExpressionCheckmark;

    [SerializeField] private GameObject expressionTotal;
    [SerializeField] private TMP_Text expressionMin;
    [SerializeField] private TMP_Text expressionUsedTotal;
    [SerializeField] private GameObject minExpressionCheckmark;

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

        // Initialize WinLoseLabel
        WinLoseLabel.gameObject.SetActive(false);

        // Hide timer
        _timerText.text = null;
        _timerFill.fillAmount = 0f;

        WinLoseTracker();
    }

    void Update(){
        // Subtract from timer
        if(!StopTimer && _playerTurn != -1){
            PlayerTimer -= Time.deltaTime;
            _timerText.text = (Math.Ceiling(PlayerTimer)).ToString();
            _timerFill.fillAmount = (float)(Math.Ceiling(PlayerTimer)) / 30;
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
        WinLoseLabel.gameObject.SetActive(true);
        WinLoseLabelText.text = "Ran out of time! Your turn was skipped";

        yield return new WaitForSeconds(3f);

        StopTimer = false;
        WinLoseLabel.gameObject.SetActive(false);

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
        WinLoseTracker();
        StopTimer = true; // Stop the timer for now because there is a 3 second delay from switching to next round
        yield return new WaitForSeconds(3f);
        StopTimer = false; // switching to next round
        string winLoseStatus = WinLoseStatus();
        if(winLoseStatus == "continue"){
            _round++;
            _playerTurn = -1;

            // Hide timer
            _timerText.text = null;
            _timerFill.fillAmount = 0f;
        }else{
            StopTimer = true;
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
    /* ------------------- Adjusts Win Tracker Based on NPC ------------------ */
    private string WinLoseTracker(){
        if(NPC.RangeWinDuration > 0){
            conditionVars.text = NPC.RangeWinDuration.ToString();
            counter.text = "(" + _goalCounter + "/" + NPC.RangeWinDuration.ToString() + ")";


            joyTracker.text = NPC.JoyRange.x.ToString() + "-" + NPC.JoyRange.y.ToString();
            sadTracker.text = NPC.SadnessRange.x.ToString() + "-" + NPC.SadnessRange.y.ToString();
            fearTracker.text = NPC.FearRange.x.ToString() + "-" + NPC.FearRange.y.ToString();
            angerTracker.text = NPC.AngerRange.x.ToString() + "-" + NPC.AngerRange.y.ToString();

            if (NPC.JoyLvl < NPC.JoyRange.y && NPC.JoyLvl > NPC.JoyRange.x){
                joyCheckmark.GetComponent<Image>().color = new Color32(0,171,109,255);
                joy =  true;
            }
            else {
                joyCheckmark.GetComponent<Image>().color = new Color32(255,255,255,255);
                joy = false;
            }

            if (NPC.SadnessLvl < NPC.SadnessRange.y && NPC.SadnessLvl > NPC.SadnessRange.x){
                sadCheckmark.GetComponent<Image>().color = new Color32(0,171,109,255);
                sadness =  true;
            }
            else {
                sadCheckmark.GetComponent<Image>().color = new Color32(255,255,255,255);
                sadness = false;
            }
            if (NPC.AngerLvl < NPC.AngerRange.y && NPC.AngerLvl > NPC.AngerRange.x){
                angerCheckmark.GetComponent<Image>().color = new Color32(0,171,109,255);
                anger =  true;
            }
            else {
                angerCheckmark.GetComponent<Image>().color = new Color32(255,255,255,255);
                anger = false;
            }
            if (NPC.FearLvl < NPC.FearRange.y && NPC.FearLvl > NPC.FearRange.x){
                fearCheckmark.GetComponent<Image>().color = new Color32(0,171,109,255);
                fear =  true;
            }
            else {
                fearCheckmark.GetComponent<Image>().color = new Color32(255,255,255,255);
                fear = false;
            }
            if (joy == true && sadness == true && anger == true && fear == true){
                emotionRangeCheckmark.GetComponent<Image>().color = new Color32(0,171,109,255);
            }
            else {
                emotionRangeCheckmark.GetComponent<Image>().color = new Color32(255,255,255,255);
            }

        }
        if (NPC.MaxExpressionPerRound < 5){
            expressionPerRound.gameObject.SetActive(true);

            expressionMax.text = NPC.MaxExpressionPerRound.ToString() + " Expression ";
            expressionUsed.text = "(" + PlayedActionCards.ExpressionCount.ToString() + "/" + NPC.MaxExpressionPerRound.ToString();

            if (PlayedActionCards.ExpressionCount < NPC.MaxExpressionPerRound){
                maxExpressionCheckmark.GetComponent<Image>().color = new Color32(0,171,109,255);
            }
            else {
                maxExpressionCheckmark.GetComponent<Image>().color = new Color32(255,255,255,255);
            }
        }
        if (NPC.MinExpressionTotal > 0){
            expressionTotal.gameObject.SetActive(true);

            expressionMin.text = NPC.MinExpressionTotal.ToString() + " Expression ";
            expressionUsedTotal.text = "(" + _totalExpressionCount.ToString() + "/" + NPC.MaxExpressionPerRound.ToString();
            if (_totalExpressionCount < NPC.MinExpressionTotal){
                minExpressionCheckmark.GetComponent<Image>().color = new Color32(0,171,109,255);
            }
            else {
                minExpressionCheckmark.GetComponent<Image>().color = new Color32(255,255,255,255);
            }
        }

        return "";
    }
}