using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Action Card", menuName = "Deal With It/Cards/Action Card", order = 2)]
public class Action : Card
{
    /* ------------------------- Energy & Emotion Values ------------------------ */
    // The energy and emotion levels are capped accordingly.

    // Energy
    [SerializeField] private int _energyConstVal;
    private int _energyVal;

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

    // Constants - basis for range, no getter/setter
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

    /* --------------------------- Card Values Change --------------------------- */

    // Directions
    // dir = 0: up
    // dir = 1: down
    // dir = 2: equal
    public int EnergyValChangeDir => CheckChangeDir(_energyOriginalVal, _energyVal);
    public int JoyValChangeDir => CheckChangeDir(_joyOriginalVal, _joyVal);
    public int SadnessValChangeDir => CheckChangeDir(_sadnessOriginalVal, _sadnessVal);
    public int FearValChangeDir => CheckChangeDir(_fearOriginalVal, _fearVal);
    public int AngerValChangeDir => CheckChangeDir(_angerOriginalVal, _angerVal);

    // Did value change
    private bool _energyValChanged;
    private bool _joyValChanged;
    private bool _sadnessValChanged;
    private bool _fearValChanged;
    private bool _angerValChanged;

    // Is canceled out
    public bool EnergyValCanceled {get; set;}
    public bool JoyValCanceled {get; set;}
    public bool SadnessValCanceled {get; set;}
    public bool FearValCanceled {get; set;}
    public bool AngerValCanceled {get; set;}

    /* ------------------------- Effect Tooltips Content ------------------------ */
    // NPC Traits
    public string EnergyTraitEffectText {get; set;}
    public string JoyTraitEffectText {get; set;}
    public string SadnessTraitEffectText {get; set;}
    public string FearTraitEffectText {get; set;}
    public string AngerTraitEffectText {get; set;}

    // In Order Combos
    public string PrevIOStrategyText {get; set;}
    public string NextIOStrategyText {get; set;}
    public string EnergyInOrderEffectText {get; set;}
    public string JoyInOrderEffectText {get; set;}
    public string SadnessInOrderEffectText {get; set;}
    public string FearInOrderEffectText {get; set;}
    public string AngerInOrderEffectText {get; set;}

    // At Least Combos
    public string ALStrategyText {get; set;}
    public string EnergyAtLeastEffectText {get; set;}
    public string JoyAtLeastEffectText {get; set;}
    public string SadnessAtLeastEffectText {get; set;}
    public string FearAtLeastEffectText {get; set;}
    public string AngerAtLeastEffectText {get; set;}

    /* ----------------------------- Custom Methods ----------------------------- */

    // Set random emotion values
    public void SetRandomEmotions()
    {
        EnergyVal = _energyConstVal;
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

        EnergyValCanceled = false;
        JoyValCanceled = false;
        SadnessValCanceled = false;
        FearValCanceled = false;
        AngerValCanceled = false;
    }

    // Determine value change direction
    private int CheckChangeDir(int original, int current)
    {
        if (original < current)
        {
            return 0;
        }
        else if (original > current)
        {
            return 1;
        }
        else
        {
            return 2;
        }
    }

    // Update bools for val changed
    public void UpdateValueChanges()
    {
        _energyValChanged = (EnergyValChangeDir < 2);
        _joyValChanged = (JoyValChangeDir < 2);
        _sadnessValChanged = (SadnessValChangeDir < 2);
        _fearValChanged = (FearValChangeDir < 2);
        _angerValChanged = (AngerValChangeDir < 2);
    }

    // Update val changed given level type
    public void UpdateValueChanges(LevelType levelType)
    {
        if (levelType == LevelType.Energy)
        {
            _energyValChanged = (EnergyValChangeDir < 2);
        }
        else if (levelType == LevelType.Joy)
        {
            _joyValChanged = (JoyValChangeDir < 2);
        }
        else if (levelType == LevelType.Sadness)
        {
            _sadnessValChanged = (SadnessValChangeDir < 2);
        }
        else if (levelType == LevelType.Fear)
        {
            _fearValChanged = (FearValChangeDir < 2);
        }
        else if (levelType == LevelType.Anger)
        {
            _angerValChanged = (AngerValChangeDir < 2);
        }
    }

    // Update canceled out
    public void UpdateValueCanceled()
    {
        EnergyValCanceled = (_energyValChanged && EnergyOriginalVal == EnergyVal);
        JoyValCanceled = (_joyValChanged && JoyOriginalVal == JoyVal);
        SadnessValCanceled = (_sadnessValChanged && SadnessOriginalVal == SadnessVal);
        FearValCanceled = (_fearValChanged && FearOriginalVal == FearVal);
        AngerValCanceled = (_angerValChanged && AngerOriginalVal == AngerVal);
    }

    // Update canceled out given level type
    public void UpdateValueCanceled(LevelType levelType)
    {
        if (levelType == LevelType.Energy)
        {
            EnergyValCanceled = (_energyValChanged && EnergyOriginalVal == EnergyVal);
        }
        else if (levelType == LevelType.Joy)
        {
            JoyValCanceled = (_joyValChanged && JoyOriginalVal == JoyVal);
        }
        else if (levelType == LevelType.Sadness)
        {
            SadnessValCanceled = (_sadnessValChanged && SadnessOriginalVal == SadnessVal);
        }
        else if (levelType == LevelType.Fear)
        {
            FearValCanceled = (_fearValChanged && FearOriginalVal == FearVal);
        }
        else if (levelType == LevelType.Anger)
        {
            AngerValCanceled = (_angerValChanged && AngerOriginalVal == AngerVal);
        }
    }
}

/* ------------------------------- Action Type ------------------------------ */
public enum ActionType {None, Distraction, Expression, Processing, Reappraisal};