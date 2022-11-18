using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NPC Card", menuName = "Deal With It/Cards/NPC Card", order = 0)]
public class NPC : Card
{
    /* ---------------------- Card Goals aka Win Conditions --------------------- */
    [SerializeField]
    [TextArea(1,5)]
    List<string> _cardGoals = new List<string>();
    public List<string> CardGoals => _cardGoals;

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

    /* ------------------------------ Trait Effects ----------------------------- */

    // [GUIDE]
    // Addend must be positive (+) when effects "increased", "more effective"
    // Addend must be negative (-) when effects "diminished"
    // If card energy/emotion value is positive (+), add the addend
    // If card energy/emotion value is negative (-), subtract the addend

    // Event Addends
    [SerializeField]
    private int _joyAddend;
    public int JoyAddend => _joyAddend;

    [SerializeField]
    private int _sadnessAddend;
    public int SadnessAddend => _sadnessAddend;
    
    [SerializeField]
    private int _fearAddend;
    public int FearAddend => _fearAddend;

    [SerializeField]
    private int _angerAddend;
    public int AngerAddend => _angerAddend;

    // Strategy Emotion Addends
    [SerializeField]
    private int _distractionEmotionAddend;
    public int DistractionEmotionAddend => _distractionEmotionAddend;

    [SerializeField]
    private int _expressionEmotionAddend;
    public int ExpressionEmotionAddend => _expressionEmotionAddend;
    
    [SerializeField]
    private int _processingEmotionAddend;
    public int ProcessingEmotionAddend => _processingEmotionAddend;

    [SerializeField]
    private int _reappraisalEmotionAddend;
    public int ReappraisalEmotionAddend => _reappraisalEmotionAddend;

    // Strategy Energy Addends
    [SerializeField]
    private int _distractionEnergyAddend;
    public int DistractionEnergyAddend => _distractionEnergyAddend;

    [SerializeField]
    private int _expressionEnergyAddend;
    public int ExpressionEnergyAddend => _expressionEnergyAddend;
    
    [SerializeField]
    private int _processingEnergyAddend;
    public int ProcessingEnergyAddend => _processingEnergyAddend;

    [SerializeField]
    private int _reappraisalEnergyAddend;
    public int ReappraisalEnergyAddend => _reappraisalEnergyAddend;

    /* ------------------------- Win Condition Variables ------------------------ */
    // Emotion Ranges
    [SerializeField]
    private Vector2 _joyRange;
    public Vector2 JoyRange => _joyRange;

    [SerializeField]
    private Vector2 _sadnessRange;
    public Vector2 SadnessRange => _sadnessRange;

    [SerializeField]
    private Vector2 _fearRange;
    public Vector2 FearRange => _fearRange;

    [SerializeField]
    private Vector2 _angerRange;
    public Vector2 AngerRange => _angerRange;

    [SerializeField]
    private int _rangeWinDuration;
    public int RangeWinDuration => _rangeWinDuration;

    [SerializeField]
    private int _rangeLoseDuration;
    public int RangeLoseDuration => _rangeLoseDuration;

    // Action Cards Played
    [SerializeField]
    private int _maxDistractionPerRound;
    public int MaxDistractionPerRound => _maxDistractionPerRound;

    [SerializeField]
    private int _maxExpressionPerRound;
    public int MaxExpressionPerRound => _maxExpressionPerRound;

    [SerializeField]
    private int _maxProcessingPerRound;
    public int MaxProcessingPerRound => _maxProcessingPerRound;

    [SerializeField]
    private int _maxReappraisalPerRound;
    public int MaxReappraisalPerRound => _maxReappraisalPerRound;

    [SerializeField]
    private int _minDistractionTotal;
    public int MinDistractionTotal => _minDistractionTotal;

    [SerializeField]
    private int _minExpressionTotal;
    public int MinExpressionTotal => _minExpressionTotal;

    [SerializeField]
    private int _minProcessingTotal;
    public int MinProcessingTotal => _minProcessingTotal;

    [SerializeField]
    private int _minReappraisalTotal;
    public int MinReappraisalTotal => _minReappraisalTotal;
}
