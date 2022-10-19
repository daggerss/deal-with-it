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

    /* ---------------------------------- Count --------------------------------- */
    private int _distractionCount = 0;
    private int _expressionCount = 0;
    private int _processingCount = 0;
    private int _reappraisalCount = 0;

    /* --------------------------- Effect Value Totals -------------------------- */
    public int TotalEnergyVal = 0;
    public int TotalJoyVal = 0;
    public int TotalSadnessVal = 0;
    public int TotalFearVal = 0;
    public int TotalAngerVal = 0;

    /* ---------------------------- Round Controller ---------------------------- */
    private RoundController RoundController;
    private int _currentTurn = -2;

    /* ----------------------------------- NPC ---------------------------------- */
    public NPCDisplay NPCDisplay;
    public NPC NPC;

    /* ---------------------------------- Misc ---------------------------------- */
    private bool _effectsApplied = false;

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
    }

    /* --------------------- Update is called once per frame -------------------- */
    void Update()
    {
        // If turn changed
        if(RoundController.PlayerTurn != _currentTurn){
            _currentTurn = RoundController.PlayerTurn;

            // At the start of the next round
            if(_currentTurn == -1){
                // Reset Counts
                _distractionCount = 0;
                _expressionCount = 0;
                _processingCount = 0;
                _reappraisalCount = 0;

                // Reset _effectsApplied
                _effectsApplied = false;

                // Clear PlayedActionCards and all properties
                for(int i = 0; i < PlayedActionCards.Length; i++){
                    // Remove card
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

                // Reset Current Slot
                CurrentSlot = 0;

            // Apply PlayedActionCards
            }else if(_currentTurn == RoundController.NumberOfPlayers && !_effectsApplied){
                _effectsApplied = true;

                GetTotalValues();
                NPCDisplay.ApplyEffect(LevelType.Energy, TotalEnergyVal, ActionType.None);
                NPCDisplay.ApplyEffect(LevelType.Joy, TotalJoyVal, ActionType.None);
                NPCDisplay.ApplyEffect(LevelType.Sadness, TotalSadnessVal, ActionType.None);
                NPCDisplay.ApplyEffect(LevelType.Fear, TotalFearVal, ActionType.None);
                NPCDisplay.ApplyEffect(LevelType.Anger, TotalAngerVal, ActionType.None);

                // Go next turn
                RoundController.NextPlayer();
                Debug.Log("Effects Applied!");
            }
        }
    }

    /* ------------------------- Combo checking methods ------------------------- */
    public int ProjectComboEffect(LevelType levelType, int effectValue, ActionType actionType){
        /* ----------------------------- All strategies ----------------------------- */
        //! LEGACY All strategies :(
        // // if(_distractionCount >= 1 && _expressionCount >= 1 && _processingCount >= 1 && _reappraisalCount >= 1){
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
        /* -------------------- 3 Distraction Cards in one round -------------------- */
        //* Checked
        if(_distractionCount >= 3){
            // Additional +1 energy to all non distraction cards

            // Project on the cards already played
            if(levelType == LevelType.Energy){ // To make sure this only runs once or else the value will keep changing
                for(int i = 0; i < PlayedActionCards.Length; i++){
                    Action playedActionCard = PlayedActionCards[i];
                    if(playedActionCard == null){
                        break;
                    }else if(playedActionCard.CardActionType != ActionType.Distraction){
                        Debug.Log("Hello?");
                        playedActionCard.EnergyVal -= 1;
                    }
                }
            }

            // Project on the selected card
            if(actionType != ActionType.Distraction && levelType == LevelType.Energy){
                addend = -1;
            }

        /* --------------------- 4 Expression cards in one round -------------------- */
        // * checked
        }else if(_expressionCount >= 4){
            // Flips values of expression card (+1 becomes -1)

            //Project on the cards already played
            if(levelType == LevelType.Energy){ // To make sure this only runs once or else the value will keep changing
                for(int i = 0; i < PlayedActionCards.Length; i++){
                    Action playedActionCard = PlayedActionCards[i];
                    if(playedActionCard == null){
                        break;
                    }else if(playedActionCard.CardActionType == ActionType.Expression){
                        playedActionCard.EnergyVal *= -1;
                        playedActionCard.JoyVal *= -1;
                        playedActionCard.SadnessVal *= -1;
                        playedActionCard.FearVal *= -1;
                        playedActionCard.AngerVal *= -1;
                    }
                }
            }

            // Project on the selected card
            if(levelType != LevelType.Energy){ 
                flip = -1;
            }

        /* --------------------- 3 Processing Cards in one round -------------------- */
        // * Checked
        }else if(_processingCount >= 3){
            // Increases negative emotion effects by 2

            // Project on the cards already played
            if(levelType == LevelType.Energy){ // To make sure this only runs once or else the value will keep changing
                for(int i = 0; i < PlayedActionCards.Length; i++){
                    Action playedActionCard = PlayedActionCards[i];
                    if(playedActionCard == null){
                        break;
                    }else if(playedActionCard.CardActionType == ActionType.Processing){
                        playedActionCard.SadnessVal += 2;
                        playedActionCard.FearVal += 2;
                        playedActionCard.AngerVal += 2;
                    }
                }
            }

            // Project on the selected card
            if(levelType != LevelType.Energy && levelType != LevelType.Joy){
                addend = 2; //? Sadness -2 becomes Sadness -0
            }
        
        /* -------------------- 3 Reappraisal Cards in one round -------------------- */
        }else if(_reappraisalCount >= 3){
            // Decreases efficacy of the reappraisal cards by 1

            // Project on the cards already played
            if(levelType == LevelType.Energy){ // To make sure this only runs once or else the value will keep changing
                for(int i = 0; i < PlayedActionCards.Length; i++){
                    Action playedActionCard = PlayedActionCards[i];
                    if(playedActionCard == null){
                        break;
                    }else if(playedActionCard.CardActionType == ActionType.Reappraisal){
                        playedActionCard.JoyVal += AddExtraEffect(playedActionCard.JoyVal, -1);
                        playedActionCard.SadnessVal += AddExtraEffect(playedActionCard.SadnessVal, -1);
                        playedActionCard.FearVal += AddExtraEffect(playedActionCard.FearVal, -1);
                        playedActionCard.AngerVal += AddExtraEffect(playedActionCard.AngerVal, -1);
                    }
                }
            }


            if(levelType != LevelType.Energy){
                addend = AddExtraEffect(effectValue, -1);
            }
        }

        /* -------------------------------- In order -------------------------------- */
        //* Not the first player because CurrentSlot -1 might have an error
        //! Is energy not affected by any of these?
        //! We are returning the actual value not the added value SEE: Expression - Expression effect
        if(CurrentSlot != 0){
            ActionType previousActionCardType = PlayedActionCards[CurrentSlot - 1].CardActionType;
            // Previous card = Distraction
            if(previousActionCardType == ActionType.Distraction){
                // nested ifs

            // Previous card = Expression
            }else if(previousActionCardType == ActionType.Expression){
                //nested ifs

            // Previous card = Processing
            }else if(previousActionCardType == ActionType.Processing){
                // nested ifs

            // Previous card = Reappraisal
            }else if(previousActionCardType == ActionType.Reappraisal){
                // nested ifs
            }
        }

        return (effectValue + addend) * flip;
    }

    /* ------------------------------- Select Card ------------------------------ */
    public void AddCurrentCard(ActionType cardActionType){
        CountCard(cardActionType, 1);
    }

    /* ------------------------------ Deselect Card ----------------------------- */
    public void RemoveCurrentCard(ActionType cardActionType){
        CountCard(cardActionType, -1);
    }

    /* ------------------------ Count the number of cards ----------------------- */
    private void CountCard(ActionType cardActionType, int increment){
        if(cardActionType == ActionType.Distraction){
            _distractionCount += increment;
        }else if(cardActionType == ActionType.Expression){
            _expressionCount += increment;
        }else if(cardActionType == ActionType.Processing){
            _processingCount += increment;
        }else if(cardActionType == ActionType.Reappraisal){
            _reappraisalCount += increment;
        }
    }

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

            // Increment count
            Debug.Log("d" + _distractionCount + " e " + _expressionCount + " p " + _processingCount + " r " + _reappraisalCount);
            CurrentSlot++;
        }
    }

    /* -------------------------- Revert all cards back ------------------------- */
    public void RevertAll(){
        for(int i = 0; i < PlayedActionCards.Length; i++){
            if(PlayedActionCards[i] != null){
                PlayedActionCards[i].EnergyVal = _energyOriginalVals[i];
                PlayedActionCards[i].JoyVal = _joyOriginalVals[i];
                PlayedActionCards[i].SadnessVal = _sadnessOriginalVals[i];
                PlayedActionCards[i].FearVal = _fearOriginalVals[i];
                PlayedActionCards[i].AngerVal = _angerOriginalVals[i];
            }else{
                break;
            }
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
}
