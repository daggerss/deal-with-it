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

    // Show or hide image
    public bool ShowImage(int value){
        string EmotionPoints = FormatText(value);

        if(EmotionPoints == null){
            return false;
        }else{
            return true;
        }
    }
    
    // Show or hide arrows
    // dir = 0: arrow up
    // dir = 1: arrow down
    public bool ShouldShowArrow(int dir, int original, int current)
    {
        if ((dir == 0 && original < current) ||
            (dir == 1 && original > current))
        {
            return true;
        }

        return false;
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
}
