using System; // For Array Controllers
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayedActionCardsDisplay : CardDisplay
{
    /* -------------------------------------------------------------------------- */
    /*                                  Variables                                 */
    /* -------------------------------------------------------------------------- */
    /* ---------------------------- PlayedActionCards --------------------------- */
    public Action[] PlayedActionCards = new Action[5];
    public int CurrentSlot = 0;

    /* ---------------------------------- Count --------------------------------- */
    private int _distractionCount = 0;
    private int _expressionCount = 0;
    private int _processingCount = 0;
    private int _reappraisalCount = 0;

    /* ---------------------------- Round Controller ---------------------------- */
    private RoundController RoundController;
    private int _currentTurn = -1;

    /* ----------------------------------- NPC ---------------------------------- */
    public NPC NPC;

    /* -------------------------------------------------------------------------- */
    /*                                   Methods                                  */
    /* -------------------------------------------------------------------------- */
    /* -------------- Start is called before the first frame update ------------- */
    void Start()
    {
        // Initialize RoundController
        RoundController = (RoundController)GameObject.FindGameObjectWithTag("Round Controller").GetComponent(typeof(RoundController));

        // Initializing NPC
        NPC = ((NPCDisplay)GameObject.FindGameObjectWithTag("NPC").GetComponent(typeof(NPCDisplay))).npc;
    }

    /* --------------------- Update is called once per frame -------------------- */
    void Update()
    {
        // If turn changed
        if(RoundController.PlayerTurn != _currentTurn){
            _currentTurn = RoundController.PlayerTurn;

            // If turn is -1 (Event Card turn (new round)) then reset count
            if(_currentTurn == -1){
                _distractionCount = 0;
                _expressionCount = 0;
                _processingCount = 0;
                _reappraisalCount = 0;
            }

            // Recount previous cards
            foreach(Action card in PlayedActionCards){
                CountCard(card.CardActionType);
                //TODO delete if method works
                // // ActionType cardActionType = card.CardActionType;
                // // if(cardActionType == ActionType.Distraction){
                // //     _distractionCount++;
                // // }else if(cardActionType == ActionType.Expression){
                // //     _expressionCount++;
                // // }else if(cardActionType == ActionType.Processing){
                // //     _processingCount++;
                // // }else if(cardActionType == ActionType.Reappraisal){
                // //     _reappraisalCount++;
                // // }
            }
        }
    }

    /* ------------------------- Combo checking methods ------------------------- */
    public int ProjectComboEffect(LevelType levelType, int effectValue, ActionType actionType){
        // Get current empty slot
        CurrentSlot = Array.IndexOf(PlayedActionCards, null);

        // Add current card
        CountCard(actionType);
        //TODO delete if method works
        // // if(actionType == ActionType.Distraction){
        // //     _distractionCount++;
        // // }else if(actionType == ActionType.Expression){
        // //     _expressionCount++;
        // // }else if(actionType == ActionType.Processing){
        // //     _processingCount++;
        // // }else if(actionType == ActionType.Reappraisal){
        // //     _reappraisalCount++;
        // // }

        /* ----------------------------- All strategies ----------------------------- */
        //? Is this supposed to be for the NPC emotionLevel I'm not sure
        if(_distractionCount >= 1 && _expressionCount >= 1 && _processingCount >= 1 && _reappraisalCount >= 1){
            // All emotions move by 1 towards 7
            if(levelType == LevelType.Joy){
                return MoveTowards(NPC.JoyLvl, 7);
            }else if(levelType == LevelType.Sadness){
                return MoveTowards(NPC.SadnessLvl, 7);
            }else if(levelType == LevelType.Fear){
                return MoveTowards(NPC.FearLvl, 7);
            }else if(levelType == LevelType.Anger){
                return MoveTowards(NPC.AngerLvl, 7);
            }
        }

        /* -------------------------------- At least -------------------------------- */
        // Check at least combos
        int addend = 0;
        if(_distractionCount == 3){
            // Additional +1 energy
            if(levelType == LevelType.Energy) {
                addend = 1;
            }
        }else if(_expressionCount == 4){
            // Flips values of card (+1 becomes -1)
            //* I'll return this because if this moves on to the In order then the whole thing gets messed up
            if(levelType != LevelType.Energy){ 
                return effectValue * -1; 
            }else{ 
                return effectValue;
            }
        }else if(_processingCount == 3){
            // Increases negative emotion effects by 2
            //? Should this be => Sadness -2 becomes Sadness -4 or => Sadness -2 becomes Sadness -0 
            if(levelType != LevelType.Energy && levelType != LevelType.Joy){
                // // addend = AddExtraEffect(effectValue, 2); //? Sadness -2  becomes Sadness -4
                addend = 2; //? Sadness -2 becomes Sadness -0
            }
        }else if(_reappraisalCount == 3){
            // Decreases efficacy of the cards by 1
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

        return effectValue;
    }

    /* ------------------------ Count the number of cards ----------------------- */
    private void CountCard(ActionType cardActionType){
        if(cardActionType == ActionType.Distraction){
            _distractionCount++;
        }else if(cardActionType == ActionType.Expression){
            _expressionCount++;
        }else if(cardActionType == ActionType.Processing){
            _processingCount++;
        }else if(cardActionType == ActionType.Reappraisal){
            _reappraisalCount++;
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
}
