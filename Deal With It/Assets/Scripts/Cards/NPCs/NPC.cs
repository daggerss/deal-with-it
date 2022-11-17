using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NPC Card", menuName = "Deal With It/Cards/NPC Card", order = 0)]
public class NPC : Card
{
    /* ---------------------- Card Goals aka Win Conditions --------------------- */
    [SerializeField]
    [TextArea(1,3)]
    private string _cardGoals;
    public string CardGoals => _cardGoals;

    /* --------------------- Event & Strategy Effects (Copy) -------------------- */
    [SerializeField]
    [TextArea(1,3)]
    private string _eventEffects;
    public string EventEffects => _eventEffects;

    [SerializeField]
    [TextArea(1,3)]
    private string _strategyEffects;
    public string StrategyEffects => _strategyEffects;

    /* ------------------------- Energy & Emotion Levels ------------------------ */
    // The energy and emotion levels are capped accordingly.
    // Serialization is causing issues, but it's not necessary since emotions are
    // set within a range for each game.
    private int _energyLvl = 20;
    private int _joyLvl = 7;
    private int _sadnessLvl = 7;
    private int _fearLvl = 7;
    private int _angerLvl = 7;

    public int EnergyLvl
    {
        get
        {
            return _energyLvl;
        }
        set
        {
            _energyLvl = Mathf.Clamp(value, -50, 50);
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

    /* ---------------------------- Starting Emotions --------------------------- */
    [SerializeField]
    private int _joyStartingMin;
    public int JoyStartingMin => _joyStartingMin;
    [SerializeField]
    private int _joyStartingMax;
    public int JoyStartingMax => _joyStartingMax;

    [SerializeField]
    private int _sadnessStartingMin;
    public int SadnessStartingMin => _sadnessStartingMin;
    [SerializeField]
    private int _sadnessStartingMax;
    public int SadnessStartingMax => _sadnessStartingMax;

    [SerializeField]
    private int _fearStartingMin;
    public int FearStartingMin => _fearStartingMin;
    [SerializeField]
    private int _fearStartingMax;
    public int FearStartingMax => _fearStartingMax;

    [SerializeField]
    private int _angerStartingMin;
    public int AngerStartingMin => _angerStartingMin;
    [SerializeField]
    private int _angerStartingMax;
    public int AngerStartingMax => _angerStartingMax;

    /* ------------------------------ Trait Effects ----------------------------- */

    // [GUIDE]
    // Addend must be positive (+) when effects "increased", "more effective"
    // Addend must be negative (-) when effects "diminished"
    // If card energy/emotion value is positive (+), add the addend
    // If card energy/emotion value is negative (-), subtract the addend

    // EVENT ADDENDS
    // Joy
    [SerializeField]
    private int _joyAddend;
    public int JoyAddend => _joyAddend;

    [SerializeField]
    [TextArea(1,3)]
    private string _joyRationale;
    public string JoyRationale => _joyRationale;

    // Sadness
    [SerializeField]
    private int _sadnessAddend;
    public int SadnessAddend => _sadnessAddend;

    [SerializeField]
    [TextArea(1,3)]
    private string _sadnessRationale;
    public string SadnessRationale => _sadnessRationale;

    // Fear
    [SerializeField]
    private int _fearAddend;
    public int FearAddend => _fearAddend;

    [SerializeField]
    [TextArea(1,3)]
    private string _fearRationale;
    public string FearRationale => _fearRationale;

    // Anger
    [SerializeField]
    private int _angerAddend;
    public int AngerAddend => _angerAddend;

    [SerializeField]
    [TextArea(1,3)]
    private string _angerRationale;
    public string AngerRationale => _angerRationale;

    // STRATEGY EMOTION ADDENDS
    // Distraction
    [SerializeField]
    private int _distractionEmotionAddend;
    public int DistractionEmotionAddend => _distractionEmotionAddend;

    [SerializeField]
    [TextArea(1,3)]
    private string _distractionEmotionRationale;
    public string DistractionEmotionRationale => _distractionEmotionRationale;

    // Expression
    [SerializeField]
    private int _expressionEmotionAddend;
    public int ExpressionEmotionAddend => _expressionEmotionAddend;

    [SerializeField]
    [TextArea(1,3)]
    private string _expressionEmotionRationale;
    public string ExpressionEmotionRationale => _expressionEmotionRationale;

    // Processing
    [SerializeField]
    private int _processingEmotionAddend;
    public int ProcessingEmotionAddend => _processingEmotionAddend;

    [SerializeField]
    [TextArea(1,3)]
    private string _processingEmotionRationale;
    public string ProcessingEmotionRationale => _processingEmotionRationale;

    // Reappraisal
    [SerializeField]
    private int _reappraisalEmotionAddend;
    public int ReappraisalEmotionAddend => _reappraisalEmotionAddend;

    [SerializeField]
    [TextArea(1,3)]
    private string _reappraisalEmotionRationale;
    public string ReappraisalEmotionRationale => _reappraisalEmotionRationale;

    // STRATEGY ENERGY ADDENDS
    // Distraction
    [SerializeField]
    private int _distractionEnergyAddend;
    public int DistractionEnergyAddend => _distractionEnergyAddend;

    [SerializeField]
    [TextArea(1,3)]
    private string _distractionEnergyRationale;
    public string DistractionEnergyRationale => _distractionEnergyRationale;

    // Expression
    [SerializeField]
    private int _expressionEnergyAddend;
    public int ExpressionEnergyAddend => _expressionEnergyAddend;

    [SerializeField]
    [TextArea(1,3)]
    private string _expressionEnergyRationale;
    public string ExpressionEnergyRationale => _expressionEnergyRationale;

    // Processing
    [SerializeField]
    private int _processingEnergyAddend;
    public int ProcessingEnergyAddend => _processingEnergyAddend;

    [SerializeField]
    [TextArea(1,3)]
    private string _processingEnergyRationale;
    public string ProcessingEnergyRationale => _processingEnergyRationale;

    // Reappraisal
    [SerializeField]
    private int _reappraisalEnergyAddend;
    public int ReappraisalEnergyAddend => _reappraisalEnergyAddend;

    [SerializeField]
    [TextArea(1,3)]
    private string _reappraisalEnergyRationale;
    public string ReappraisalEnergyRationale => _reappraisalEnergyRationale;
}
