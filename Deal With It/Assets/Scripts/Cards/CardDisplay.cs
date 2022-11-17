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
                                        ActionType actionType, string rationale, int addend)
    {
        // Compose only when has content
        if (addend != 0)
        {
            // NPC Traits
            if (tooltipType == TooltipType.Trait)
            {
                // NPC x Events
                if (actionType == ActionType.None)
                {
                    return rationale + ", so " + levelType.ToString() + " events are " +
                           VerbalizeExtraEffect(levelType, addend);
                }
                // NPC x Strategy
                else
                {
                    if (levelType == LevelType.Energy)
                    {
                        return rationale + ", so " + actionType.ToString() + " " +
                               VerbalizeExtraEffect(levelType, addend);
                    }
                    
                    return rationale + ", so " + actionType.ToString() + " is " +
                           VerbalizeExtraEffect(levelType, addend);
                }
            }
        }

        return null;
    }
}
