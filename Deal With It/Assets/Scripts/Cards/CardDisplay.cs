using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDisplay : MonoBehaviour
{
    // Returns null if value is 0 (for printing values of energy etc.)
    public string FormatText(int value){
        if(value == 0){
            return null;
        }else if(value > 0){
            return "+" + value.ToString();
        }else{
            return value.ToString();
        }
    }

    // Returns null if value is 0 and didn't change
    public string FormatText(int value, int original){
        if(value == 0 && value == original){
            return null;
        }else if(value > 0){
            return "+" + value.ToString();
        }else{
            return value.ToString();
        }
    }

    // Show or hide image
    public bool ShowImage(int value){
        string EmotionPoints = FormatText(value);

        if(EmotionPoints == null){
            return false;
        }else{
            return true;
        }
    }

    // Show or hide image
    public bool ShowImage(int value, int original){
        string EmotionPoints = FormatText(value, original);

        if(EmotionPoints == null){
            return false;
        }else{
            return true;
        }
    }

    // Adds or subtracts the addend according to energy/emotion value
    public int AddExtraEffect(int original, int addend)
    {
        if (original > 0)
        {
            return addend;
        }
        else if (original < 0)
        {
            return -addend;
        }

        return 0;
    }

    // Produce copy for extra effects
    private string VerbalizeExtraEffect(LevelType levelType, int addend)
    {
        if (addend > 0)
        {
            if (levelType == LevelType.Energy)
            {
                return "costs +" + addend.ToString() + " more energy.";
            }
            else
            {
                return "more effective by " + addend.ToString() + ".";
            }
        }
        else if (addend < 0)
        {
            if (levelType == LevelType.Energy)
            {
                return "costs " + addend.ToString() + " less energy.";
            }
            else
            {
                return "less effective by " + addend.ToString() + ".";
            }
        }

        return null;
    }

    // Construct tooltip content
    public string ComposeTooltipContent(TooltipType tooltipType, LevelType levelType,
                                        ActionType actionType, string rationale, int addend,
                                        int atLeastThreshold = 0)
    {
        // Compose only when has content
        if (addend != 0)
        {
            string firstClause = rationale + ", so ";

            // NPC Traits
            if (tooltipType == TooltipType.Trait)
            {
                // NPC x Events
                if (actionType == ActionType.None)
                {
                    return firstClause + levelType.ToString() + " events are " +
                           VerbalizeExtraEffect(levelType, addend);
                }
                // NPC x Strategy
                else
                {
                    if (levelType == LevelType.Energy)
                    {
                        return firstClause + actionType.ToString() + " " +
                               VerbalizeExtraEffect(levelType, addend);
                    }
                    
                    return firstClause + actionType.ToString() + " is " +
                           VerbalizeExtraEffect(levelType, addend);
                }
            }

            // In Order Combos
            else if (tooltipType == TooltipType.InOrderCombo)
            {
                // Processing -> Reappraisal is special
                if (actionType == ActionType.Reappraisal && addend == -13)
                {
                    return firstClause + actionType.ToString() + " is more effective by 2 but costs +1 more energy.";
                }

                if (levelType == LevelType.Energy)
                {
                    return firstClause + actionType.ToString() + " " +
                           VerbalizeExtraEffect(levelType, addend);
                }
                    
                return firstClause + actionType.ToString() + " is " +
                       VerbalizeExtraEffect(levelType, addend);
            }

            // At Least Combos
            else if (tooltipType == TooltipType.AtLeastCombo && atLeastThreshold != 0)
            {
                firstClause = "Too much " + actionType.ToString() + " " + rationale +
                              "! At least " + atLeastThreshold.ToString() + " " +
                              actionType.ToString() + " cards ";
                
                // Distraction -> Distraction
                if (actionType == ActionType.Distraction)
                {
                    return firstClause + "causes all other strategies to cost +1 more energy.";
                }
                // Expression -> Expression
                else if (actionType == ActionType.Expression)
                {
                    return firstClause + "inverts their emotional effects.";
                }
                // Processing -> Processing
                else if (actionType == ActionType.Processing)
                {
                    return firstClause + "makes their negative emotions less effective by 2.";
                }
                // Reappraisal -> Reappraisal
                else if (actionType == ActionType.Reappraisal)
                {
                    return firstClause + "makes them less effective by 1.";
                }
            }
        }

        return null;
    }
}
