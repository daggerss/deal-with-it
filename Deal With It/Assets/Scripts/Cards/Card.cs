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

    /* ----------------------------- Custom Methods ----------------------------- */
    // Set random value based on constant
    public int RandomizeValue(int value)
    {
        if (value == 0)
        {
            return value;
        }
        // Range: 1, 2
        else if (value == 1)
        {
            return Random.Range(1, 3);
        }
        // Range: -2, -1
        else if (value == -1)
        {
            return Random.Range(-2, 0);
        }

        // Range: x-1, x, x+1
        return Random.Range(value-1, value+2);
    }
}

/* ------------------------------- Level Types ------------------------------ */
public enum LevelType {Energy, Joy, Sadness, Fear, Anger};