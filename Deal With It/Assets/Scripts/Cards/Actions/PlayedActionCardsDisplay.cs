using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayedActionCardsDisplay : CardDisplay
{
    public Action[] PlayedActionCards = new Action[5];

    /* ------------------------- Combo checking methods ------------------------- */
    public int ProjectComboEffect(int currentTurn, LevelType levelType, int effectValue, ActionType actionType){
        /* -------------------------------- At least -------------------------------- */
        // Count each number of action types
        int distractionCount = 0;
        int expressionCount = 0;
        int processingCount = 0;
        int reappraisalCount = 0;

        foreach(Action card in PlayedActionCards){
            ActionType cardActionType = card.CardActionType;
            if(cardActionType == ActionType.Distraction){
                distractionCount++;
            }else if(cardActionType == ActionType.Expression){
                expressionCount++;
            }else if(cardActionType == ActionType.Processing){
                processingCount++;
            }else if(cardActionType == ActionType.Reappraisal){
                reappraisalCount++;
            }
        }

        // Add current card
        if(actionType == ActionType.Distraction){
            distractionCount++;
        }else if(actionType == ActionType.Expression){
            expressionCount++;
        }else if(actionType == ActionType.Processing){
            processingCount++;
        }else if(actionType == ActionType.Reappraisal){
            reappraisalCount++;
        }

        // Check at least combos
        if(distractionCount >= 3){
            // return
        }else if(expressionCount >= 4){
            // return
        }else if(processingCount >= 3){
            // return
        }else if(reappraisalCount >= 3){
            // return
        }

        // In order
        ActionType previousActionCardType = PlayedActionCards[currentTurn - 1].CardActionType;
        if(previousActionCardType == ActionType.Distraction){
            // nested ifs
        }else if(previousActionCardType == ActionType.Expression){
            // nested ifs
        }else if(previousActionCardType == ActionType.Processing){
            // nested ifs
        }else if(previousActionCardType == ActionType.Reappraisal){
            // nested ifs
        }

        return 0;
    }
}
