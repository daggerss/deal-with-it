using System; //Cloning
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Event Card", menuName = "Deal With It/Cards/Event Card", order = 1)]
public class Event : Card
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

    /* -------------------------- Additional Mechanics -------------------------- */
    [field: SerializeField]
    public int ExtraEventCards {get; private set;}

    [SerializeField]
    private bool _randomize;
    public bool Randomize => _randomize;

    /* ----------------------------- Custom Methods ----------------------------- */
    public void RandomVariation(){
        int[] values = {_joyVal, _sadnessVal, _fearVal, _angerVal};

        // Choose one of the values that isn't 0
        int rng = UnityEngine.Random.Range(0, values.Length);
        while(values[rng] == 0){
            rng = UnityEngine.Random.Range(0, values.Length);
        }

        // Set all other values to 0
        for(int i = 0; i < values.Length; i++){
            if(i != rng){
                values[i] = 0;
            }
        }

        // Set the actual values to the new values of the random variation
        _joyVal = values[0];
        _sadnessVal = values[1];
        _fearVal = values[2];
        _angerVal = values[3];
    }

    // Returns a clone of this object
    public object Clone(){
        return this.MemberwiseClone();
    }

    // Set random emotion values
    public void SetRandomEmotions()
    {
        JoyVal = RandomizeValue(_joyConstVal);
        SadnessVal = RandomizeValue(_sadnessConstVal);
        FearVal = RandomizeValue(_fearConstVal);
        AngerVal = RandomizeValue(_angerConstVal);
    }

    // Save original values
    public void SaveValues()
    {
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
