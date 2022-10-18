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

    /* -------------------------------------------------------------------------- */
    /*                                   Methods                                  */
    /* -------------------------------------------------------------------------- */
    /* ------------------------- Combo checking methods ------------------------- */
    public int ProjectComboEffect(LevelType levelType, int effectValue, ActionType actionType){
        // Get current slot
        // * Used this because the previous player might play no card
        CurrentSlot = Array.IndexOf(PlayedActionCards, null);

        /* -------------------------------- At least -------------------------------- */
        // Count each number of action types
        // Count number of cards in the previous played cards
        foreach(Action card in PlayedActionCards){
            ActionType cardActionType = card.CardActionType;
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

        // Add current card
        if(actionType == ActionType.Distraction){
            _distractionCount++;
        }else if(actionType == ActionType.Expression){
            _expressionCount++;
        }else if(actionType == ActionType.Processing){
            _processingCount++;
        }else if(actionType == ActionType.Reappraisal){
            _reappraisalCount++;
        }

        // Check at least combos
        if(_distractionCount == 3){
            // addend
        }else if(_expressionCount == 4){
            // addend
        }else if(_processingCount == 3){
            // addend
        }else if(_reappraisalCount == 3){
            // addend
        }

        /* -------------------------------- In order -------------------------------- */
        if(CurrentSlot != 0){ // * Not the first player because CurrentSlot -1 might have an error
            ActionType previousActionCardType = playedActionCards[CurrentSlot - 1].CardActionType;
            if(previousActionCardType == ActionType.Distraction){
                // nested ifs
            }else if(previousActionCardType == ActionType.Expression){
                // nested ifs
            }else if(previousActionCardType == ActionType.Processing){
                // nested ifs
            }else if(previousActionCardType == ActionType.Reappraisal){
                // nested ifs
            }
        }
    }
}
