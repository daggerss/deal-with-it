using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Action Card", menuName = "Deal With It/Cards/Action Card", order = 2)]
public class Action : Card
{
    /* ------------------------- Energy & Emotion Values ------------------------ */
    // The energy and emotion levels are capped accordingly.

    // Energy
    [SerializeField] private int _energyVal;

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

    // Emotion constants - basis for range, no getter/setter
    [SerializeField] private int _joyConstVal;
    [SerializeField] private int _sadnessConstVal;
    [SerializeField] private int _fearConstVal;
    [SerializeField] private int _angerConstVal;

    // Emotion random values - set during gameplay
    private int _joyVal;
    private int _sadnessVal;
    private int _fearVal;
    private int _angerVal;

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

    /* ----------------------------- Custom Methods ----------------------------- */

    // Set random emotion values
    public void SetRandomEmotions()
    {
        JoyVal = RandomizeValue(_joyConstVal);
        SadnessVal = RandomizeValue(_sadnessConstVal);
        FearVal = RandomizeValue(_fearConstVal);
        AngerVal = RandomizeValue(_angerConstVal);
    }

    // Set random value based on constant
    private int RandomizeValue(int value)
    {
        if (value == 0)
        {
            return value;
        }
        // Range: 1, 2, 3
        else if (value == 1)
        {
            return Random.Range(1, 4);
        }
        // Range: -3, -2, -1
        else if (value == -1)
        {
            return Random.Range(-3, 0);
        }

        // Range: x-1, x, x+1
        return Random.Range(value-1, value+2);
    }
}

/* ------------------------------- Action Type ------------------------------ */
public enum ActionType {None, Distraction, Expression, Processing, Reappraisal};