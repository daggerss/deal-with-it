using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Action Card", menuName = "Deal With It/Cards/Action Card", order = 0)]
public class Action : Card
{
    /* ------------------------- Energy & Emotion Values ------------------------ */
    // The energy and emotion levels are capped accordingly.
    // Serialization is causing issues, but it's not necessary since emotions
    // are set within a range for each game.
    [SerializeField] private int _energyVal;
    [SerializeField] private int _joyVal;
    [SerializeField] private int _sadnessVal;
    [SerializeField] private int _fearVal;
    [SerializeField] private int _angerVal;

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

    /* ---------------------------- Card Action Type ---------------------------- */
    [SerializeField]
    private ActionType _actionType;
    public ActionType CardActionType => _actionType;
}

/* ------------------------------- Action Type ------------------------------ */
public enum ActionType {None, Distraction, Expression, Processing, Reappraisal};