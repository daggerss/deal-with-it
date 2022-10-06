using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NPCDisplay : CardDisplay
{
    public NPC npc;

    /* ---------------------------- Card Information ---------------------------- */
    public TMP_Text cardTypeText;
    public TMP_Text nameText;
    public TMP_Text descriptionText;

    public Image illustrationImage;

    public TMP_Text energyText;
    public TMP_Text joyText;
    public TMP_Text sadnessText;
    public TMP_Text fearText;
    public TMP_Text angerText;

    public TMP_Text energyEffectText;
    public TMP_Text joyEffectText;
    public TMP_Text sadnessEffectText;
    public TMP_Text fearEffectText;
    public TMP_Text angerEffectText;

    /* --------------------------------- Bar UI --------------------------------- */
    public LevelBar EnergyFrontBar;
    public LevelBar EnergyBackBar;

    public LevelBar JoyFrontBar;
    public LevelBar JoyBackBar;

    public LevelBar SadnessFrontBar;
    public LevelBar SadnessBackBar;

    public LevelBar FearFrontBar;
    public LevelBar FearBackBar;

    public LevelBar AngerFrontBar;
    public LevelBar AngerBackBar;

    private float _lerpTimer;
    private float _chipSpeed = 0.5f;
    private float _percentComplete;

    /* --------------------------------- Methods -------------------------------- */
    // Start is called before the first frame update
    void Start()
    {
        // Set energy level
        npc.EnergyLvl = 20;

        // Set energy bar
        EnergyFrontBar.SetMaxValue(20);
        EnergyFrontBar.SetValue(20);

        EnergyBackBar.SetMaxValue(20);
        EnergyBackBar.SetValue(20);

        // Set random starting emotion levels
        if (npc.CardName == "Knot")
        {
            npc.JoyLvl = Random.Range(6, 9);
            npc.SadnessLvl = Random.Range(4, 7);
            npc.FearLvl = Random.Range(9, 12);
            npc.AngerLvl = Random.Range(9, 12);
        }
        else if (npc.CardName == "Pickles")
        {
            npc.JoyLvl = Random.Range(4, 7);
            npc.SadnessLvl = Random.Range(9, 12);
            npc.FearLvl = Random.Range(9, 12);
            npc.AngerLvl = Random.Range(9, 12);
        }
        else if (npc.CardName == "Sniffles")
        {
            npc.JoyLvl = Random.Range(9, 12);
            npc.SadnessLvl = Random.Range(9, 12);
            npc.FearLvl = Random.Range(9, 12);
            npc.AngerLvl = Random.Range(9, 12);
        }
        else
        {
            npc.JoyLvl = Random.Range(6, 9);
            npc.SadnessLvl = Random.Range(6, 9);
            npc.FearLvl = Random.Range(6, 9);
            npc.AngerLvl = Random.Range(6, 9);
        }

        // Set emotions front bar
        JoyFrontBar.SetMaxValue(13);
        SadnessFrontBar.SetMaxValue(13);
        FearFrontBar.SetMaxValue(13);
        AngerFrontBar.SetMaxValue(13);

        JoyFrontBar.SetValue(npc.JoyLvl);
        SadnessFrontBar.SetValue(npc.SadnessLvl);
        FearFrontBar.SetValue(npc.FearLvl);
        AngerFrontBar.SetValue(npc.AngerLvl);

        // Set emotions back bar
        JoyBackBar.SetMaxValue(13);
        SadnessBackBar.SetMaxValue(13);
        FearBackBar.SetMaxValue(13);
        AngerBackBar.SetMaxValue(13);

        JoyBackBar.SetValue(npc.JoyLvl);
        SadnessBackBar.SetValue(npc.SadnessLvl);
        FearBackBar.SetValue(npc.FearLvl);
        AngerBackBar.SetValue(npc.AngerLvl);

        // Set text UI
        cardTypeText.text = npc.GetType().Name;
        nameText.text = npc.CardName;
        descriptionText.text = npc.CardDescription;

        illustrationImage.sprite = npc.Illustration;

        energyText.text = npc.EnergyLvl.ToString();
        joyText.text = npc.JoyLvl.ToString();
        sadnessText.text = npc.SadnessLvl.ToString();
        fearText.text = npc.FearLvl.ToString();
        angerText.text = npc.AngerLvl.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateLevelBars();
    }

    /* ----------------------------- Custom Methods ----------------------------- */
    // Apply energy and emotion effects on bars
    public void UpdateLevelBars()
    {
        // Update energy
        if (EnergyBackBar.Value > npc.EnergyLvl)
        {
            _lerpTimer += Time.deltaTime;
            _percentComplete = Mathf.Pow(_lerpTimer / _chipSpeed, 2);
            EnergyFrontBar.SetValue(npc.EnergyLvl);
            EnergyBackBar.SetValue(Mathf.Lerp(EnergyBackBar.Value, npc.EnergyLvl, _percentComplete));
        }

        if (EnergyFrontBar.Value < npc.EnergyLvl)
        {
            _lerpTimer += Time.deltaTime;
            _percentComplete = Mathf.Pow(_lerpTimer / _chipSpeed, 2);
            EnergyBackBar.SetValue(npc.EnergyLvl);
            EnergyFrontBar.SetValue(Mathf.Lerp(EnergyFrontBar.Value, npc.EnergyLvl, _percentComplete));
        }

        // Update joy
        if (JoyBackBar.Value > npc.JoyLvl)
        {
            _lerpTimer += Time.deltaTime;
            _percentComplete = Mathf.Pow(_lerpTimer / _chipSpeed, 2);
            JoyFrontBar.SetValue(npc.JoyLvl);
            JoyBackBar.SetValue(Mathf.Lerp(JoyBackBar.Value, npc.JoyLvl, _percentComplete));
        }

        if (JoyFrontBar.Value < npc.JoyLvl)
        {
            _lerpTimer += Time.deltaTime;
            _percentComplete = Mathf.Pow(_lerpTimer / _chipSpeed, 2);
            JoyBackBar.SetValue(npc.JoyLvl);
            JoyFrontBar.SetValue(Mathf.Lerp(JoyFrontBar.Value, npc.JoyLvl, _percentComplete));
        }

        // Update sadness
        if (SadnessBackBar.Value > npc.SadnessLvl)
        {
            _lerpTimer += Time.deltaTime;
            _percentComplete = Mathf.Pow(_lerpTimer / _chipSpeed, 2);
            SadnessFrontBar.SetValue(npc.SadnessLvl);
            SadnessBackBar.SetValue(Mathf.Lerp(SadnessBackBar.Value, npc.SadnessLvl, _percentComplete));
        }

        if (SadnessFrontBar.Value < npc.SadnessLvl)
        {
            _lerpTimer += Time.deltaTime;
            _percentComplete = Mathf.Pow(_lerpTimer / _chipSpeed, 2);
            SadnessBackBar.SetValue(npc.SadnessLvl);
            SadnessFrontBar.SetValue(Mathf.Lerp(SadnessFrontBar.Value, npc.SadnessLvl, _percentComplete));
        }

        // Update fear
        if (FearBackBar.Value > npc.FearLvl)
        {
            _lerpTimer += Time.deltaTime;
            _percentComplete = Mathf.Pow(_lerpTimer / _chipSpeed, 2);
            FearFrontBar.SetValue(npc.FearLvl);
            FearBackBar.SetValue(Mathf.Lerp(FearBackBar.Value, npc.FearLvl, _percentComplete));
        }

        if (FearFrontBar.Value < npc.FearLvl)
        {
            _lerpTimer += Time.deltaTime;
            _percentComplete = Mathf.Pow(_lerpTimer / _chipSpeed, 2);
            FearBackBar.SetValue(npc.FearLvl);
            FearFrontBar.SetValue(Mathf.Lerp(FearFrontBar.Value, npc.FearLvl, _percentComplete));
        }

        // Update anger
        if (AngerBackBar.Value > npc.AngerLvl)
        {
            _lerpTimer += Time.deltaTime;
            _percentComplete = Mathf.Pow(_lerpTimer / _chipSpeed, 2);
            AngerFrontBar.SetValue(npc.AngerLvl);
            AngerBackBar.SetValue(Mathf.Lerp(AngerBackBar.Value, npc.AngerLvl, _percentComplete));
        }

        if (AngerFrontBar.Value < npc.AngerLvl)
        {
            _lerpTimer += Time.deltaTime;
            _percentComplete = Mathf.Pow(_lerpTimer / _chipSpeed, 2);
            AngerBackBar.SetValue(npc.AngerLvl);
            AngerFrontBar.SetValue(Mathf.Lerp(AngerFrontBar.Value, npc.AngerLvl, _percentComplete));
        }
    }

    // Apply energy or emotion effects on NPC + text
    public void ApplyEffect(LevelType levelType, int effectValue, ActionType actionType)
    {
        int addend = 0;

        // Energy
        if (levelType == LevelType.Energy)
        {
            // NPC x Strategy
            // Distraction
            if (actionType == ActionType.Distraction)
            {
                addend += AddExtraEffect(effectValue, npc.DistractionEnergyAddend);
            }
            // Expression
            else if (actionType == ActionType.Expression)
            {
                addend += AddExtraEffect(effectValue, npc.ExpressionEnergyAddend);
            }
            // Processing
            else if (actionType == ActionType.Processing)
            {
                addend += AddExtraEffect(effectValue, npc.ProcessingEnergyAddend);
            }
            // Reappraisal
            else if (actionType == ActionType.Reappraisal)
            {
                addend += AddExtraEffect(effectValue, npc.ReappraisalEnergyAddend);
            }

            npc.EnergyLvl += effectValue + addend;

            energyText.text = npc.EnergyLvl.ToString();
            energyEffectText.text = FormatText(effectValue + addend);
        }

        // Emotion
        else
        {
            if (effectValue != 0)
            {
                // NPC x Strategy
                // Distraction
                if (actionType == ActionType.Distraction)
                {
                    addend += AddExtraEffect(effectValue, npc.DistractionEmotionAddend);
                }
                // Expression
                else if (actionType == ActionType.Expression)
                {
                    addend += AddExtraEffect(effectValue, npc.ExpressionEmotionAddend);
                }
                // Processing
                else if (actionType == ActionType.Processing)
                {
                    addend += AddExtraEffect(effectValue, npc.ProcessingEmotionAddend);
                }
                // Reappraisal
                else if (actionType == ActionType.Reappraisal)
                {
                    addend += AddExtraEffect(effectValue, npc.ReappraisalEmotionAddend);
                }

                // Per emotion
                // Joy
                if (levelType == LevelType.Joy)
                {
                    // NPC x Events
                    if (actionType == ActionType.None)
                    {
                        addend += AddExtraEffect(effectValue, npc.JoyAddend);
                    }

                    npc.JoyLvl += effectValue + addend;

                    joyText.text = npc.JoyLvl.ToString();
                    joyEffectText.text = FormatText(effectValue + addend);
                }
                // Sadness
                else if (levelType == LevelType.Sadness)
                {
                    // NPC x Events
                    if (actionType == ActionType.None)
                    {
                        addend += AddExtraEffect(effectValue, npc.SadnessAddend);
                    }

                    npc.SadnessLvl += effectValue + addend;

                    sadnessText.text = npc.SadnessLvl.ToString();
                    sadnessEffectText.text = FormatText(effectValue + addend);
                }
                // Fear
                else if (levelType == LevelType.Fear)
                {
                    // NPC x Events
                    if (actionType == ActionType.None)
                    {
                        addend += AddExtraEffect(effectValue, npc.FearAddend);
                    }

                    npc.FearLvl += effectValue + addend;

                    fearText.text = npc.FearLvl.ToString();
                    fearEffectText.text = FormatText(effectValue + addend);
                }
                // Anger
                else if (levelType == LevelType.Anger)
                {
                    // NPC x Events
                    if (actionType == ActionType.None)
                    {
                        addend += AddExtraEffect(effectValue, npc.AngerAddend);
                    }

                    npc.AngerLvl += effectValue + addend;

                    angerText.text = npc.AngerLvl.ToString();
                    angerEffectText.text = FormatText(effectValue + addend);
                }
            }
        }

        _lerpTimer = 0f;
    }

    // Adds or subtracts the addend according to energy/emotion value
    private int AddExtraEffect(int original, int addend)
    {
        if (original > 0)
        {
            return addend;
        }
        else if (original < 0)
        {
            return -addend;
        }

        return 0;
    }
}
