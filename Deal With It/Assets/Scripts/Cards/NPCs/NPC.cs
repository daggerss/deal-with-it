using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NPC Card", menuName = "Deal With It/Cards/NPC Card", order = 0)]
public class NPC : ScriptableObject
{
    [field: SerializeField]
    public string CardName {get; private set;}

    [field: SerializeField]
    [field: TextArea(1,3)]
    public string CardDescription {get; private set;}

    [SerializeField]
    [TextArea(1,5)]
    List<string> _cardGoals = new List<string>();

    [field: SerializeField]
    public Sprite Illustration {get; private set;}

    // The energy and emotion levels are capped accordingly.
    // Serialization is causing issues, but it's not necessary since emotions
    // are set within a range for each game.
    private int _energyLvl = 20;
    private int _joyLvl;
    private int _sadnessLvl;
    private int _fearLvl;
    private int _angerLvl;

    public int EnergyLvl
    {
        get
        {
            return _energyLvl;
        }
        set
        {
            _energyLvl = Mathf.Clamp(value, 0, 20);
        }
    }

    public int JoyLvl
    {
        get
        {
            return _joyLvl;
        }
        set
        {
            _joyLvl = Mathf.Clamp(value, 0, 13);
        }
    }

    public int SadnessLvl
    {
        get
        {
            return _sadnessLvl;
        }
        set
        {
            _sadnessLvl = Mathf.Clamp(value, 0, 13);
        }
    }

    public int FearLvl
    {
        get
        {
            return _fearLvl;
        }
        set
        {
            _fearLvl = Mathf.Clamp(value, 0, 13);
        }
    }

    public int AngerLvl
    {
        get
        {
            return _angerLvl;
        }
        set
        {
            _angerLvl = Mathf.Clamp(value, 0, 13);
        }
    }
}
