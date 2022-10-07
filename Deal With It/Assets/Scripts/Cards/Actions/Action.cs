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

    // Saved values for switching between original and projected
    private int _energyOriginalVal;
    public int EnergyOriginalVal => _energyOriginalVal;

    private int _joyOriginalVal;
    public int JoyOriginalVal => _joyOriginalVal;

    private int _sadnessOriginalVal;
    public int SadnessOriginalVal => _sadnessOriginalVal;

    private int _fearOriginalVal;
    public int FearOriginalVal => _fearOriginalVal;

    private int _angerOriginalVal;
    public int AngerOriginalVal => _angerOriginalVal;

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
        
        // Save
        _energyOriginalVal = EnergyVal;
        _joyOriginalVal = JoyVal;
        _sadnessOriginalVal = SadnessVal;
        _fearOriginalVal = FearVal;
        _angerOriginalVal = AngerVal;
    }

    // Revert to the original set
    public void Revert()
    {
        EnergyVal = _energyOriginalVal;
        JoyVal = _joyOriginalVal;
        SadnessVal = _sadnessOriginalVal;
        FearVal = _fearOriginalVal;
        AngerVal = _angerOriginalVal;
    }
}

/* ------------------------------- Action Type ------------------------------ */
public enum ActionType {None, Distraction, Expression, Processing, Reappraisal};