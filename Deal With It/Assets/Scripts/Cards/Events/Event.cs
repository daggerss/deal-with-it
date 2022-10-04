using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Event Card", menuName = "Deal With It/Cards/Event Card", order = 0)]
public class Event : Card
{
    // The energy and emotion levels are capped accordingly.
    // Serialization is causing issues, but it's not necessary since emotions
    // are set within a range for each game.
    [SerializeField] private int _energyVal;
    [SerializeField] private int _joyVal;
    [SerializeField] private int _sadnessVal;
    [SerializeField] private int _fearVal;
    [SerializeField] private int _angerVal;
    [SerializeField] private int _extraEventCards;

    public int EnergyVal
    {
        get
        {
            return _energyVal;
        }
        set
        {
            _energyVal = Mathf.Clamp(value, -20, 20);
        }
    }

    public int JoyVal
    {
        get
        {
            return _joyVal;
        }
        set
        {
            _joyVal = Mathf.Clamp(value, -13, 13);
        }
    }

    public int SadnessVal
    {
        get
        {
            return _sadnessVal;
        }
        set
        {
            _sadnessVal = Mathf.Clamp(value, -13, 13);
        }
    }

    public int FearVal
    {
        get
        {
            return _fearVal;
        }
        set
        {
            _fearVal = Mathf.Clamp(value, -13, 13);
        }
    }

    public int AngerVal
    {
        get
        {
            return _angerVal;
        }
        set
        {
            _angerVal = Mathf.Clamp(value, -13, 13);
        }
    }

    public int ExtraEventCards
    {
        get
        {
            return _extraEventCards;
        }
        set
        {
            _extraEventCards = value;
        }
    }
}
