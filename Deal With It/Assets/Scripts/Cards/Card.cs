using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : ScriptableObject
{
    /* -------------------------------- Card Name ------------------------------- */
    [SerializeField]
    private string _cardName;
    public string CardName => _cardName;

    /* ---------------------------- Card Description ---------------------------- */
    [SerializeField] 
    [TextArea(1, 3)]
    private string _cardDescription;
    public string CardDescription => _cardDescription;

    /* ------------------------------- Card Sprite ------------------------------ */
    [field: SerializeField]
    public Sprite Illustration {get; private set;}
}
