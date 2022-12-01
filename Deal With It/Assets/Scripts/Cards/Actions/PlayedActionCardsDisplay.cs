using System; // For Array Controllers
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayedActionCardsDisplay : CardDisplay
{
    /* -------------------------------------------------------------------------- */
    /*                                  Variables                                 */
    /* -------------------------------------------------------------------------- */
    /* ---------------------------- PlayedActionCards --------------------------- */
    public Action[] PlayedActionCards = new Action[5];
    public Button[] PlayedActionCardsButton = new Button[5];
    public int CurrentSlot = 0;

    // Original values
    private int[] _energyOriginalVals = new int[5];
    private int[] _joyOriginalVals = new int[5];
    private int[] _sadnessOriginalVals = new int[5];
    private int[] _fearOriginalVals = new int[5];
    private int[] _angerOriginalVals = new int[5];

    // Canceled
    private bool[] _energyOriginalCanceled = new bool[5];
    private bool[] _joyOriginalCanceled = new bool[5];
    private bool[] _sadnessOriginalCanceled = new bool[5];
    private bool[] _fearOriginalCanceled = new bool[5];
    private bool[] _angerOriginalCanceled = new bool[5];

    /* ---------------------------------- Count --------------------------------- */
    public int DistractionCount = 0;
    public int ExpressionCount = 0;
    public int ProcessingCount = 0;
    public int ReappraisalCount = 0;

    /* --------------------------- Effect Value Totals -------------------------- */
    public int TotalEnergyVal = 0;
    public int TotalJoyVal = 0;
    public int TotalSadnessVal = 0;
    public int TotalFearVal = 0;
    public int TotalAngerVal = 0;
    public bool OverloadUnderload = false;

    /* ---------------------------- Projected Values ---------------------------- */
    private int _energyProjectedVal = 0;
    private int _joyProjectedVal = 0;
    private int _sadnessProjectedVal = 0;
    private int _fearProjectedVal = 0;
    private int _angerProjectedVal = 0;

    /* ---------------------------- Round Controller ---------------------------- */
    private RoundController RoundController;
    private int _currentTurn = -2;

    /* ----------------------------------- NPC ---------------------------------- */
    public NPCDisplay NPCDisplay;
    public NPC NPC;

    /* -------------------------------- Tooltips -------------------------------- */
    // In Order Combos
    private string _prevIOStrategyText;
    public string PrevIOStrategyText => _prevIOStrategyText;

    private string _nextIOStrategyText;
    public string NextIOStrategyText => _nextIOStrategyText;

    private string _inOrderEffectText;
    public string InOrderEffectText => _inOrderEffectText;

    // At Least Combos
    private string _ALStrategyText;
    public string ALStrategyText => _ALStrategyText;

    private string _atLeastEffectText;
    public string AtLeastEffectText => _atLeastEffectText;

    /* ---------------------------------- Misc ---------------------------------- */
    private bool _effectsApplied = false;
    public ActionCardDeck ActionCardDeck;

    /* -------------------------------------------------------------------------- */
    /*                                   Methods                                  */
    /* -------------------------------------------------------------------------- */
    /* -------------- Start is called before the first frame update ------------- */
    void Start()
    {
        // Initialize RoundController
        RoundController = (RoundController)GameObject.FindGameObjectWithTag("Round Controller").GetComponent(typeof(RoundController));

        // Initializing NPC
        NPCDisplay = (NPCDisplay)GameObject.FindGameObjectWithTag("NPC").GetComponent(typeof(NPCDisplay));
        NPC = NPCDisplay.npc;

        // Initializing Action Card Deck
        ActionCardDeck = (ActionCardDeck)GameObject.FindGameObjectWithTag("Action Card Deck").GetComponent(typeof(ActionCardDeck));
    }

    /* --------------------- Update is called once per frame -------------------- */
    void Update()
    {
        // Project effects on NPC level bars
        StartCoroutine(ProjectOnNPC());

        // If turn changed
        if(RoundController.PlayerTurn != _currentTurn){
            _currentTurn = RoundController.PlayerTurn;

            // At the start of the next round
            if(_currentTurn == -1){
                // Reset Counts
                DistractionCount = 0;
                ExpressionCount = 0;
                ProcessingCount = 0;
                ReappraisalCount = 0;

                // Reset _effectsApplied
                _effectsApplied = false;

                // Clear PlayedActionCards and all properties
                ClearCards();

                // Reset Current Slot
                CurrentSlot = 0;

            // Apply PlayedActionCards
            }else if(_currentTurn == RoundController.NumberOfPlayers && !_effectsApplied){
                _effectsApplied = true;

                // Reset projected effects UI
                NPCDisplay.ResetProjectedUI();

                GetTotalValues();

                // Check for overload or under load
                NPC npc = NPCDisplay.npc;
                if((npc.JoyLvl + TotalJoyVal) < 0 || (npc.JoyLvl + TotalJoyVal) > 13 ||
                (npc.SadnessLvl + TotalSadnessVal) < 0 || (npc.SadnessLvl + TotalSadnessVal) > 13 ||
                (npc.FearLvl + TotalFearVal) < 0 || (npc.FearLvl + TotalFearVal) > 13 ||
                (npc.AngerLvl + TotalAngerVal) < 0 || (npc.AngerLvl + TotalAngerVal) > 13){
                    OverloadUnderload = true;
                }

                NPCDisplay.ApplyEffect(LevelType.Energy, TotalEnergyVal);
                NPCDisplay.ApplyEffect(LevelType.Joy, TotalJoyVal);
                NPCDisplay.ApplyEffect(LevelType.Sadness, TotalSadnessVal);
                NPCDisplay.ApplyEffect(LevelType.Fear, TotalFearVal);
                NPCDisplay.ApplyEffect(LevelType.Anger, TotalAngerVal);

                // Revert cards
                RevertAll();
                ClearCards();

                // Go next turn
                RoundController.NextPlayer();
                Debug.Log("Effects Applied!");
            }
        }
    }

    /* ------------------------- Combo checking methods ------------------------- */
    public int ProjectComboEffect(LevelType levelType, int effectValue, ActionType actionType){
        /* ------------------------ Compose tooltip arguments ----------------------- */
        string rationale = "";
        int baseAddend = 0;
        int threshold = 0;
        string fullTooltip = ""; // For previously played cards
        string atLeastStrat = "";

        /* ----------------------------- All strategies ----------------------------- */
        //! LEGACY All strategies :(
        // // if(DistractionCount >= 1 && ExpressionCount >= 1 && ProcessingCount >= 1 && ReappraisalCount >= 1){
        // //     // All emotions move by 1 towards 7
        // //     if(levelType == LevelType.Joy){
        // //         return MoveTowards(NPC.JoyLvl, 7);
        // //     }else if(levelType == LevelType.Sadness){
        // //         return MoveTowards(NPC.SadnessLvl, 7);
        // //     }else if(levelType == LevelType.Fear){
        // //         return MoveTowards(NPC.FearLvl, 7);
        // //     }else if(levelType == LevelType.Anger){
        // //         return MoveTowards(NPC.AngerLvl, 7);
        // //     }
        // // }

        /* -------------------------------- At least -------------------------------- */
        // Check at least combos
        int addend = 0;
        int flip = 1;
        int count = 1;
        /* -------------------- 3 Distraction Cards in one round -------------------- */
        //* Checked
        if(DistractionCount >= 3){
            // Additional +1 energy to all non distraction cards

            // Tooltip content
            atLeastStrat = "Distraction";
            rationale = "keeps you from addressing the issue";
            threshold = 3;
            fullTooltip = ComposeTooltipContent(TooltipType.AtLeastCombo, levelType,
                                               actionType, rationale, 1, threshold);

            // Project on the cards already played
            if(levelType == LevelType.Energy && DistractionCount == 3){ // To make sure this only runs once or else the value will keep changing
                for(int i = 0; i < PlayedActionCards.Length; i++){
                    Action playedActionCard = PlayedActionCards[i];
                    if(playedActionCard == null || count > 3){
                        break;
                    }else if(playedActionCard.CardActionType != ActionType.Distraction && count <= 3){
                        playedActionCard.UpdateValueChanges(LevelType.Energy);
                        playedActionCard.EnergyVal -= 1;
                        playedActionCard.UpdateValueCanceled(LevelType.Energy);

                        // Tooltip content
                        playedActionCard.ALStrategyText = atLeastStrat;
                        playedActionCard.EnergyAtLeastEffectText = fullTooltip;
                    }else{
                        count++;
                    }
                }
            }

            // Project on the selected card
            if(actionType != ActionType.Distraction && levelType == LevelType.Energy){
                addend = -1;
            }

        /* --------------------- 4 Expression cards in one round -------------------- */
        // * checked
        }else if(ExpressionCount >= 4){
            // Flips emotion values of expression card (+1 becomes -1)

            // Tooltip content
            atLeastStrat = "Expression";
            rationale = "amplifies the experience instead";
            threshold = 4;
            fullTooltip = ComposeTooltipContent(TooltipType.AtLeastCombo, levelType,
                                               actionType, rationale, 1, threshold);

            //Project on the cards already played
            if(levelType == LevelType.Energy && ExpressionCount == 4){ // To make sure this only runs once or else the value will keep changing
                for(int i = 0; i < PlayedActionCards.Length; i++){
                    Action playedActionCard = PlayedActionCards[i];
                    if(playedActionCard == null){
                        break;
                    }else if(playedActionCard.CardActionType == ActionType.Expression && count < 4){
                        playedActionCard.JoyVal *= -1;
                        playedActionCard.SadnessVal *= -1;
                        playedActionCard.FearVal *= -1;
                        playedActionCard.AngerVal *= -1;
                        count++;

                        // Tooltip content
                        playedActionCard.ALStrategyText = atLeastStrat;
                        playedActionCard.JoyAtLeastEffectText = fullTooltip;
                        playedActionCard.SadnessAtLeastEffectText = fullTooltip;
                        playedActionCard.FearAtLeastEffectText = fullTooltip;
                        playedActionCard.AngerAtLeastEffectText = fullTooltip;
                    }
                }
            }

            // Project on the selected card
            if(levelType != LevelType.Energy && actionType == ActionType.Expression){
                flip = -1;
            }

        /* --------------------- 3 Processing Cards in one round -------------------- */
        // * Checked
        }else if(ProcessingCount >= 3){
            // Decreases the efficacy of the Processing cards’ negative emotion effects by 2

            // Tooltip content
            atLeastStrat = "Processing";
            rationale = "becomes brooding";
            threshold = 3;
            fullTooltip = ComposeTooltipContent(TooltipType.AtLeastCombo, levelType,
                                               actionType, rationale, 1, threshold);

            // Project on the cards already played
            if(levelType == LevelType.Energy && ProcessingCount == 3){ // To make sure this only runs once or else the value will keep changing
                for(int i = 0; i < PlayedActionCards.Length; i++){
                    Action playedActionCard = PlayedActionCards[i];
                    if(playedActionCard == null){
                        break;
                    }else if(playedActionCard.CardActionType == ActionType.Processing && count < 3){
                        playedActionCard.UpdateValueChanges(LevelType.Sadness);
                        playedActionCard.UpdateValueChanges(LevelType.Fear);
                        playedActionCard.UpdateValueChanges(LevelType.Anger);

                        playedActionCard.SadnessVal += AddExtraEffect(playedActionCard.SadnessOriginalVal, -2);
                        playedActionCard.FearVal += AddExtraEffect(playedActionCard.FearOriginalVal, -2);
                        playedActionCard.AngerVal += AddExtraEffect(playedActionCard.AngerOriginalVal, -2);

                        playedActionCard.UpdateValueCanceled(LevelType.Sadness);
                        playedActionCard.UpdateValueCanceled(LevelType.Fear);
                        playedActionCard.UpdateValueCanceled(LevelType.Anger);

                        // Tooltip content
                        playedActionCard.ALStrategyText = atLeastStrat;
                        playedActionCard.SadnessAtLeastEffectText = fullTooltip;
                        playedActionCard.FearAtLeastEffectText = fullTooltip;
                        playedActionCard.AngerAtLeastEffectText = fullTooltip;
                    }
                }
            }

            // Project on the selected card
            if(levelType != LevelType.Energy && levelType != LevelType.Joy && actionType == ActionType.Processing){
                addend = AddExtraEffect(effectValue, -2);
            }

        /* -------------------- 3 Reappraisal Cards in one round -------------------- */
        }else if(ReappraisalCount >= 3){
            // Decreases efficacy of the reappraisal cards by 1

            // Tooltip content
            atLeastStrat = "Reappraisal";
            rationale = "distorts your perspective";
            threshold = 3;
            fullTooltip = ComposeTooltipContent(TooltipType.AtLeastCombo, levelType,
                                               actionType, rationale, 1, threshold);

            // Project on the cards already played
            if(levelType == LevelType.Energy && ReappraisalCount == 3){ // To make sure this only runs once or else the value will keep changing
                for(int i = 0; i < PlayedActionCards.Length; i++){
                    Action playedActionCard = PlayedActionCards[i];
                    if(playedActionCard == null){
                        break;
                    }else if(playedActionCard.CardActionType == ActionType.Reappraisal && count < 3){
                        playedActionCard.UpdateValueChanges(LevelType.Joy);
                        playedActionCard.UpdateValueChanges(LevelType.Sadness);
                        playedActionCard.UpdateValueChanges(LevelType.Fear);
                        playedActionCard.UpdateValueChanges(LevelType.Anger);

                        playedActionCard.JoyVal += AddExtraEffect(playedActionCard.JoyOriginalVal, -1);
                        playedActionCard.SadnessVal += AddExtraEffect(playedActionCard.SadnessOriginalVal, -1);
                        playedActionCard.FearVal += AddExtraEffect(playedActionCard.FearOriginalVal, -1);
                        playedActionCard.AngerVal += AddExtraEffect(playedActionCard.AngerOriginalVal, -1);

                        playedActionCard.UpdateValueCanceled(LevelType.Joy);
                        playedActionCard.UpdateValueCanceled(LevelType.Sadness);
                        playedActionCard.UpdateValueCanceled(LevelType.Fear);
                        playedActionCard.UpdateValueCanceled(LevelType.Anger);

                        // Tooltip content
                        playedActionCard.ALStrategyText = atLeastStrat;
                        playedActionCard.JoyAtLeastEffectText = fullTooltip;
                        playedActionCard.SadnessAtLeastEffectText = fullTooltip;
                        playedActionCard.FearAtLeastEffectText = fullTooltip;
                        playedActionCard.AngerAtLeastEffectText = fullTooltip;
                    }
                }
            }

            // Project on the selected card
            if(levelType != LevelType.Energy && actionType == ActionType.Reappraisal){
                addend = AddExtraEffect(effectValue, -1);
            }
        }

        // At least tooltip content
        _ALStrategyText = atLeastStrat;
        // * Addend does nothing here, but zero returns null
        _atLeastEffectText = ComposeTooltipContent(TooltipType.AtLeastCombo,
                                                   levelType, actionType,
                                                   rationale, 1, threshold);

        /* -------------------------------- In order -------------------------------- */
        //* Not the first player because CurrentSlot -1 might have an error
        if(CurrentSlot != 0){
            ActionType previousActionCardType = PlayedActionCards[CurrentSlot - 1].CardActionType;
            // Previous card = Distraction
            if(previousActionCardType == ActionType.Distraction){
                // Tooltip content
                _prevIOStrategyText = "Distraction";

                // Distraction -> Processing
                // Decreases the efficacy of the Processing card by 1
                if(actionType == ActionType.Processing)
                {
                    if(levelType != LevelType.Energy)
                    {
                        addend += AddExtraEffect(effectValue, -1);

                        // Tooltip content
                        _nextIOStrategyText = "Processing";
                        rationale = "Distraction distances you from the emotional experience";
                        baseAddend = -1;
                    }
                }

                // Distraction -> Reappraisal
                // Increases the efficacy of the Reappraisal card by 1
                else if(actionType == ActionType.Reappraisal)
                {
                    if(levelType != LevelType.Energy)
                    {
                        addend += AddExtraEffect(effectValue, 1);

                        // Tooltip content
                        _nextIOStrategyText = "Reappraisal";
                        rationale = "Taking time to cool down allows you to think more clearly later on";
                        baseAddend = 1;
                    }
                }

            // Previous card = Expression
            }else if(previousActionCardType == ActionType.Expression){
                // Tooltip content
                _prevIOStrategyText = "Expression";

                // Expression -> Processing
                // Increases the efficacy of the Processing card by 1
                if(actionType == ActionType.Processing)
                {
                    if(levelType != LevelType.Energy)
                    {
                        addend += AddExtraEffect(effectValue, 1);

                        // Tooltip content
                        _nextIOStrategyText = "Processing";
                        rationale = "Expressing yourself can give you more insights";
                        baseAddend = 1;
                    }
                }

                // Expression -> Reappraisal
                // Decreases the energy of the Reappraisal card by 1
                else if(actionType == ActionType.Reappraisal)
                {
                    if(levelType == LevelType.Energy)
                    {
                        addend += AddExtraEffect(effectValue, -1);

                        // Tooltip content
                        _nextIOStrategyText = "Reappraisal";
                        rationale = "Expressing yourself can open up reflexive discussions";
                        baseAddend = -1;
                    }
                }

            // Previous card = Processing
            }else if(previousActionCardType == ActionType.Processing){
                // Tooltip content
                _prevIOStrategyText = "Processing";

                // Processing -> Expression
                // Increases the efficacy of the Expression card by 1
                if(actionType == ActionType.Expression)
                {
                    if(levelType != LevelType.Energy)
                    {
                        addend += AddExtraEffect(effectValue, 1);

                        // Tooltip content
                        _nextIOStrategyText = "Expression";
                        rationale = "Processing gives you full awareness of the experience";
                        baseAddend = 1;
                    }
                }

                // Processing -> Reappraisal
                // Increases the efficacy of the Reappraisal card by 2
                // But requires an additional +1 energy
                else if(actionType == ActionType.Reappraisal)
                {
                    if(levelType == LevelType.Energy)
                    {
                        addend += AddExtraEffect(effectValue, 1);
                    }

                    else
                    {
                        addend += AddExtraEffect(effectValue, 2);
                    }

                    // Tooltip content
                    _nextIOStrategyText = "Reappraisal";
                    rationale = "Processing gives you a view of the entire picture, but following it with Reappraisal is quite taxing";
                    baseAddend = -13; // Designation for special tooltip text
                }
            }
        }

        // In order tooltip content
        _inOrderEffectText = ComposeTooltipContent(TooltipType.InOrderCombo,
                                                   levelType, actionType,
                                                   rationale, baseAddend);

        // Tooltips are null if nothing happened
        if (addend == 0)
        {
            _inOrderEffectText = null;
        }

        if (addend == 0 && flip == 1)
        {
            _atLeastEffectText = null;
        }

        return (effectValue + addend) * flip;
    }

    /* ------------------------------- Select Card ------------------------------ */
    public void AddActionType(ActionType cardActionType){
        CountActionType(cardActionType, 1);
    }

    /* ------------------------------ Deselect Card ----------------------------- */
    public void RemoveActionType(ActionType cardActionType){
        CountActionType(cardActionType, -1);
    }

    /* ------------------------ Count the number of cards ----------------------- */
    private void CountActionType(ActionType cardActionType, int increment){
        if(cardActionType == ActionType.Distraction){
            DistractionCount += increment;
        }else if(cardActionType == ActionType.Expression){
            ExpressionCount += increment;
        }else if(cardActionType == ActionType.Processing){
            ProcessingCount += increment;
        }else if(cardActionType == ActionType.Reappraisal){
            ReappraisalCount += increment;
        }
    }

    // ! Legacy
    /* --------------------------- Move towards number -------------------------- */
    private int MoveTowards(int npcLevel, int reference){
        // Effect greater than reference point
        if(npcLevel > reference){
            return -1;
        // Effect less than reference point
        }else if(npcLevel < reference){
            return 1;
        // Effect is equal to reference point
        }else{
            return 0;
        }
    }

    /* -------------------- Add card to the PlayedActionCards ------------------- */
    public void AddPlayedActionCard(Action actionCard){
        if(actionCard != null){
            // Put card in PlayedActionCards
            PlayedActionCards[CurrentSlot] = actionCard;

            // Show card
            PlayedActionCardsButton[CurrentSlot].gameObject.SetActive(true);

            // Save original values
            _energyOriginalVals[CurrentSlot] = actionCard.EnergyVal;
            _joyOriginalVals[CurrentSlot] = actionCard.JoyVal;
            _sadnessOriginalVals[CurrentSlot] = actionCard.SadnessVal;
            _fearOriginalVals[CurrentSlot] = actionCard.FearVal;
            _angerOriginalVals[CurrentSlot] = actionCard.AngerVal;

            // Save canceled
            _energyOriginalCanceled[CurrentSlot] = actionCard.EnergyValCanceled;
            _joyOriginalCanceled[CurrentSlot] = actionCard.JoyValCanceled;
            _sadnessOriginalCanceled[CurrentSlot] = actionCard.SadnessValCanceled;
            _fearOriginalCanceled[CurrentSlot] = actionCard.FearValCanceled;
            _angerOriginalCanceled[CurrentSlot] = actionCard.AngerValCanceled;

            // If an at least combo is triggered, the original values of the affected cards will change
            ChangeOriginalValue(actionCard.CardActionType);

            CurrentSlot++;
        }
    }
    
    /* -------------------- Remove Projected card from the PlayedActionCards ------------------- */
    public void RemoveActionCard(Action actionCard){
        if(actionCard != null){
            // Remove card in PlayedActionCards
            PlayedActionCards[CurrentSlot - 1] = null;

            // Show card
            PlayedActionCardsButton[CurrentSlot - 1].gameObject.SetActive(false);

            CurrentSlot--;
        }
    }

    /* -------------------------- Change Original Value ------------------------- */
    private void ChangeOriginalValue(ActionType cardActionType){
        if((cardActionType == ActionType.Distraction && DistractionCount == 3) ||
        (cardActionType == ActionType.Expression && ExpressionCount == 4) ||
        (cardActionType == ActionType.Processing && ProcessingCount == 3) ||
        (cardActionType == ActionType.Reappraisal && ProcessingCount == 3)){
            for(int i = 0; i < PlayedActionCards.Length; i++){
                Action playedActionCard = PlayedActionCards[i];
                if(playedActionCard == null){
                    break;
                }else{
                    // Save original values
                    _energyOriginalVals[i] = PlayedActionCards[i].EnergyVal;
                    _joyOriginalVals[i] = PlayedActionCards[i].JoyVal;
                    _sadnessOriginalVals[i] = PlayedActionCards[i].SadnessVal;
                    _fearOriginalVals[i] = PlayedActionCards[i].FearVal;
                    _angerOriginalVals[i] = PlayedActionCards[i].AngerVal;

                    // Save canceled
                    _energyOriginalCanceled[i] = PlayedActionCards[i].EnergyValCanceled;
                    _joyOriginalCanceled[i] = PlayedActionCards[i].JoyValCanceled;
                    _sadnessOriginalCanceled[i] = PlayedActionCards[i].SadnessValCanceled;
                    _fearOriginalCanceled[i] = PlayedActionCards[i].FearValCanceled;
                    _angerOriginalCanceled[i] = PlayedActionCards[i].AngerValCanceled;
                }
            }
        }
    }

    /* -------------------------- Revert all cards back ------------------------- */
    public void RevertAll(){
        // Revert card values
        for(int i = 0; i < PlayedActionCards.Length; i++){
            if(PlayedActionCards[i] != null){
                PlayedActionCards[i].EnergyVal = _energyOriginalVals[i];
                PlayedActionCards[i].JoyVal = _joyOriginalVals[i];
                PlayedActionCards[i].SadnessVal = _sadnessOriginalVals[i];
                PlayedActionCards[i].FearVal = _fearOriginalVals[i];
                PlayedActionCards[i].AngerVal = _angerOriginalVals[i];

                PlayedActionCards[i].EnergyValCanceled = _energyOriginalCanceled[i];
                PlayedActionCards[i].JoyValCanceled = _joyOriginalCanceled[i];
                PlayedActionCards[i].SadnessValCanceled = _sadnessOriginalCanceled[i];
                PlayedActionCards[i].FearValCanceled = _fearOriginalCanceled[i];
                PlayedActionCards[i].AngerValCanceled = _angerOriginalCanceled[i];
            }else{
                break;
            }
        }

        // Reset projected values
        _energyProjectedVal = NPC.EnergyLvl;
        _joyProjectedVal = NPC.JoyLvl;
        _sadnessProjectedVal = NPC.SadnessLvl;
        _fearProjectedVal = NPC.FearLvl;
        _angerProjectedVal = NPC.AngerLvl;
    }

    /* ---------------------- Clear all played action cards --------------------- */
    private void ClearCards()
    {
        for(int i = 0; i < PlayedActionCards.Length; i++){
            // Remove card
            if(PlayedActionCards[i] != null){
                ActionCardDeck.PutCardBack(PlayedActionCards[i]);
            }
            PlayedActionCards[i] = null;

            // Hide card
            PlayedActionCardsButton[i].gameObject.SetActive(false);

            // Reset original
            _energyOriginalVals[i] = 0;
            _joyOriginalVals[i] = 0;
            _sadnessOriginalVals[i] = 0;
            _fearOriginalVals[i] = 0;
            _angerOriginalVals[i] = 0;
        }
    }

    /* ---------------- Get Total Values of all PlayedActionCards --------------- */
    private void GetTotalValues(){
        //Reset Total
        TotalEnergyVal = 0;
        TotalJoyVal = 0;
        TotalSadnessVal = 0;
        TotalFearVal = 0;
        TotalAngerVal = 0;

        for(int i = 0; i < PlayedActionCards.Length; i++){
            Action playedActionCard = PlayedActionCards[i];
            if(playedActionCard == null){
                break;
            }else{
                TotalEnergyVal += playedActionCard.EnergyVal;
                TotalJoyVal += playedActionCard.JoyVal;
                TotalSadnessVal += playedActionCard.SadnessVal;
                TotalFearVal += playedActionCard.FearVal;
                TotalAngerVal += playedActionCard.AngerVal;
            }
        }
    }

    /* -------------------------------------------------------------------------- */
    /*                                 Coroutines                                 */
    /* -------------------------------------------------------------------------- */
    /* ----------------------- Activate project effect UI ----------------------- */
    IEnumerator ProjectOnNPC()
    {
        // Make checks when players are playing
        yield return new WaitUntil(() => _currentTurn >= 0);
        yield return new WaitForSeconds(1f);

        GetTotalValues();

        // Computed projected totals
        _energyProjectedVal = NPC.EnergyLvl + TotalEnergyVal;
        _joyProjectedVal = NPC.JoyLvl + TotalJoyVal;
        _sadnessProjectedVal = NPC.SadnessLvl + TotalSadnessVal;
        _fearProjectedVal = NPC.FearLvl + TotalFearVal;
        _angerProjectedVal = NPC.AngerLvl + TotalAngerVal;

        // Project
        yield return new WaitUntil(() => (_energyProjectedVal != NPC.EnergyLvl) ||
                                         (_joyProjectedVal != NPC.JoyLvl) ||
                                         (_sadnessProjectedVal != NPC.SadnessLvl) ||
                                         (_fearProjectedVal != NPC.FearLvl) ||
                                         (_angerProjectedVal != NPC.AngerLvl));
        yield return new WaitUntil(() => _currentTurn >= 1);

        NPCDisplay.ProjectEffectUI(LevelType.Energy, _energyProjectedVal);
        NPCDisplay.ProjectEffectUI(LevelType.Joy, _joyProjectedVal);
        NPCDisplay.ProjectEffectUI(LevelType.Sadness, _sadnessProjectedVal);
        NPCDisplay.ProjectEffectUI(LevelType.Fear, _fearProjectedVal);
        NPCDisplay.ProjectEffectUI(LevelType.Anger, _angerProjectedVal);
    }
}
