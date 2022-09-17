using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : ScriptableObject
{
    /* -------------------------------- Card Name ------------------------------- */
    [SerializeField]
    private string _CardName;
    public string CardName => _CardName;

    /* ---------------------------- Card Description ---------------------------- */
    [SerializeField] 
    [TextArea(1, 3)]
    private string _CardDescription;
    public string CardDescription => _CardDescription;

    /* ------------------------------- Test things ------------------------------ */
    // public Card(string CardName, string CardDescription){
    //     _CardName = CardName;
    //     _CardDescription = CardDescription;
    // }

    public void Info(){
        Debug.Log("Card Name: " + _CardName);
        Debug.Log("Card Description: " + _CardDescription);
    }
}
