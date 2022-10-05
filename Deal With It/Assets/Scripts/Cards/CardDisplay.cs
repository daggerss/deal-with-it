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
}
