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

    /* ------------------------------- Effects UI ------------------------------- */
    [SerializeField] private CanvasGroup effectTexts;
    public TMP_Text energyEffectText;
    public TMP_Text joyEffectText;
    public TMP_Text sadnessEffectText;
    public TMP_Text fearEffectText;
    public TMP_Text angerEffectText;

    private float _fadeSpeed = 0.00025f;

    /* -------------------------------- Tooltips -------------------------------- */
    private string _traitEffectText;
    public string TraitEffectText => _traitEffectText;

    /* --------------------------------- Bar UI --------------------------------- */
    public Image EnergyBarGlow;
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
    private float _chipSpeed = 1f;
    private float _percentComplete;

    /* --------------------------- Project Effects UI --------------------------- */
    public TMP_Text energyProjectText;
    public TMP_Text joyProjectText;
    public TMP_Text sadnessProjectText;
    public TMP_Text fearProjectText;
    public TMP_Text angerProjectText;

    public Image energySlash;
    public Image joySlash;
    public Image sadnessSlash;
    public Image fearSlash;
    public Image angerSlash;

    public LevelBar energyProjectUp;
    public LevelBar joyProjectUp;
    public LevelBar sadnessProjectUp;
    public LevelBar fearProjectUp;
    public LevelBar angerProjectUp;

    public LevelBar energyProjectDown;
    public LevelBar joyProjectDown;
    public LevelBar sadnessProjectDown;
    public LevelBar fearProjectDown;
    public LevelBar angerProjectDown;

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
        npc.JoyLvl = Random.Range(npc.JoyStartingMin, npc.JoyStartingMax+1);
        npc.SadnessLvl = Random.Range(npc.SadnessStartingMin, npc.SadnessStartingMax+1);
        npc.FearLvl = Random.Range(npc.FearStartingMin, npc.FearStartingMax+1);
        npc.AngerLvl = Random.Range(npc.AngerStartingMin, npc.AngerStartingMax+1);

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

        // Set project bars
        energyProjectUp.SetMaxValue(20);
        joyProjectUp.SetMaxValue(13);
        sadnessProjectUp.SetMaxValue(13);
        fearProjectUp.SetMaxValue(13);
        angerProjectUp.SetMaxValue(13);

        energyProjectDown.SetMaxValue(20);
        joyProjectDown.SetMaxValue(13);
        sadnessProjectDown.SetMaxValue(13);
        fearProjectDown.SetMaxValue(13);
        angerProjectDown.SetMaxValue(13);

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

        // Coroutines
        StartCoroutine(UpdateLevelBars());
        StartCoroutine(FadeOutEffects());
        StartCoroutine(ShowEnergySignal());
        StartCoroutine(HideEnergySignal());
        StartCoroutine(ChangeExpression());
    }

    /* ----------------------------- Custom Methods ----------------------------- */
    // Apply energy or emotion effects on NPC + text
    public void ApplyEffect(LevelType levelType, int effectValue)
    {
        // Energy
        if (levelType == LevelType.Energy)
        {
            npc.EnergyLvl += effectValue;

            energyText.text = npc.EnergyLvl.ToString();
            energyEffectText.text = FormatText(effectValue);
        }

        // Emotion
        else
        {
            if (effectValue != 0)
            {
                // Joy
                if (levelType == LevelType.Joy)
                {
                    npc.JoyLvl += effectValue;

                    joyText.text = npc.JoyLvl.ToString();
                    joyEffectText.text = FormatText(effectValue);
                }
                // Sadness
                else if (levelType == LevelType.Sadness)
                {
                    npc.SadnessLvl += effectValue;

                    sadnessText.text = npc.SadnessLvl.ToString();
                    sadnessEffectText.text = FormatText(effectValue);
                }
                // Fear
                else if (levelType == LevelType.Fear)
                {
                    npc.FearLvl += effectValue;

                    fearText.text = npc.FearLvl.ToString();
                    fearEffectText.text = FormatText(effectValue);
                }
                // Anger
                else if (levelType == LevelType.Anger)
                {
                    npc.AngerLvl += effectValue;

                    angerText.text = npc.AngerLvl.ToString();
                    angerEffectText.text = FormatText(effectValue);
                }
            }
        }

        _lerpTimer = 0f;
        effectTexts.alpha = 1;
    }

    // Get projected energy or emotion values
    public int ProjectTraitEffect(LevelType levelType, int effectValue, ActionType actionType)
    {
        int addend = 0;

        // Energy
        if (levelType == LevelType.Energy)
        {
            // NPC x Strategy
            // Distraction
            if (actionType == ActionType.Distraction)
            {
                _traitEffectText = ComposeTooltipContent(TooltipType.Trait, levelType,
                                                         actionType, npc.DistractionEnergyRationale,
                                                         npc.DistractionEnergyAddend);
                addend += AddExtraEffect(effectValue, npc.DistractionEnergyAddend);
            }
            // Expression
            else if (actionType == ActionType.Expression)
            {
                _traitEffectText = ComposeTooltipContent(TooltipType.Trait, levelType,
                                                         actionType, npc.ExpressionEnergyRationale,
                                                         npc.ExpressionEnergyAddend);
                addend += AddExtraEffect(effectValue, npc.ExpressionEnergyAddend);
            }
            // Processing
            else if (actionType == ActionType.Processing)
            {
                _traitEffectText = ComposeTooltipContent(TooltipType.Trait, levelType,
                                                         actionType, npc.ProcessingEnergyRationale,
                                                         npc.ProcessingEnergyAddend);
                addend += AddExtraEffect(effectValue, npc.ProcessingEnergyAddend);
            }
            // Reappraisal
            else if (actionType == ActionType.Reappraisal)
            {
                _traitEffectText = ComposeTooltipContent(TooltipType.Trait, levelType,
                                                         actionType, npc.ReappraisalEnergyRationale,
                                                         npc.ReappraisalEnergyAddend);
                addend += AddExtraEffect(effectValue, npc.ReappraisalEnergyAddend);
            }
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
                    _traitEffectText = ComposeTooltipContent(TooltipType.Trait, levelType,
                                                             actionType, npc.DistractionEmotionRationale,
                                                             npc.DistractionEmotionAddend);
                    addend += AddExtraEffect(effectValue, npc.DistractionEmotionAddend);
                }
                // Expression
                else if (actionType == ActionType.Expression)
                {
                    _traitEffectText = ComposeTooltipContent(TooltipType.Trait, levelType,
                                                             actionType, npc.ExpressionEmotionRationale,
                                                             npc.ExpressionEmotionAddend);
                    addend += AddExtraEffect(effectValue, npc.ExpressionEmotionAddend);
                }
                // Processing
                else if (actionType == ActionType.Processing)
                {
                    _traitEffectText = ComposeTooltipContent(TooltipType.Trait, levelType,
                                                             actionType, npc.ProcessingEmotionRationale,
                                                             npc.ProcessingEmotionAddend);
                    addend += AddExtraEffect(effectValue, npc.ProcessingEmotionAddend);
                }
                // Reappraisal
                else if (actionType == ActionType.Reappraisal)
                {
                    _traitEffectText = ComposeTooltipContent(TooltipType.Trait, levelType,
                                                             actionType, npc.ReappraisalEmotionRationale,
                                                             npc.ReappraisalEmotionAddend);
                    addend += AddExtraEffect(effectValue, npc.ReappraisalEmotionAddend);
                }

                // NPC x Events
                else if (actionType == ActionType.None)
                {
                    // Joy
                    if (levelType == LevelType.Joy)
                    {
                        _traitEffectText = ComposeTooltipContent(TooltipType.Trait,
                                                                 levelType,
                                                                 actionType,
                                                                 npc.JoyRationale,
                                                                 npc.JoyAddend);
                        addend += AddExtraEffect(effectValue, npc.JoyAddend);
                    }
                    // Sadness
                    else if (levelType == LevelType.Sadness)
                    {
                        _traitEffectText = ComposeTooltipContent(TooltipType.Trait,
                                                                 levelType,
                                                                 actionType,
                                                                 npc.SadnessRationale,
                                                                 npc.SadnessAddend);
                        addend += AddExtraEffect(effectValue, npc.SadnessAddend);
                    }
                    // Fear
                    else if (levelType == LevelType.Fear)
                    {
                        _traitEffectText = ComposeTooltipContent(TooltipType.Trait,
                                                                 levelType,
                                                                 actionType,
                                                                 npc.FearRationale,
                                                                 npc.FearAddend);
                        addend += AddExtraEffect(effectValue, npc.FearAddend);
                    }
                    // Anger
                    else if (levelType == LevelType.Anger)
                    {
                        _traitEffectText = ComposeTooltipContent(TooltipType.Trait,
                                                                 levelType,
                                                                 actionType,
                                                                 npc.AngerRationale,
                                                                 npc.AngerAddend);
                        addend += AddExtraEffect(effectValue, npc.AngerAddend);
                    }
                }
            }
        }

        if (addend == 0)
        {
            _traitEffectText = null;
        }

        return effectValue + addend;
    }

    // Reset effect texts to null
    private void ResetEffectTexts()
    {
        energyEffectText.text = null;
        joyEffectText.text = null;
        sadnessEffectText.text = null;
        fearEffectText.text = null;
        angerEffectText.text = null;
    }

    // Project effect UI
    public void ProjectEffectUI(LevelType levelType, int projectedValue)
    {
        // Energy
        if (levelType == LevelType.Energy && projectedValue != npc.EnergyLvl)
        {
            energyProjectText.text = projectedValue.ToString();
            energySlash.enabled = true;

            if (projectedValue > npc.EnergyLvl)
            {
                energyProjectUp.gameObject.SetActive(true);
                energyProjectDown.gameObject.SetActive(false);
                energyProjectUp.SetValue(projectedValue);
            }
            else
            {
                energyProjectDown.gameObject.SetActive(true);
                energyProjectUp.gameObject.SetActive(false);
                energyProjectDown.SetValue(projectedValue);
            }
        }
        // Joy
        else if (levelType == LevelType.Joy && projectedValue != npc.JoyLvl)
        {
            joyProjectText.text = projectedValue.ToString();
            joySlash.enabled = true;
            
            if (projectedValue > npc.JoyLvl)
            {
                joyProjectUp.gameObject.SetActive(true);
                joyProjectDown.gameObject.SetActive(false);
                joyProjectUp.SetValue(projectedValue);
            }
            else
            {
                joyProjectDown.gameObject.SetActive(true);
                joyProjectUp.gameObject.SetActive(false);
                joyProjectDown.SetValue(projectedValue);
            }
        }
        // Sadness
        else if (levelType == LevelType.Sadness && projectedValue != npc.SadnessLvl)
        {
            sadnessProjectText.text = projectedValue.ToString();
            sadnessSlash.enabled = true;
            
            if (projectedValue > npc.SadnessLvl)
            {
                sadnessProjectUp.gameObject.SetActive(true);
                sadnessProjectDown.gameObject.SetActive(false);
                sadnessProjectUp.SetValue(projectedValue);
            }
            else
            {
                sadnessProjectDown.gameObject.SetActive(true);
                sadnessProjectUp.gameObject.SetActive(false);
                sadnessProjectDown.SetValue(projectedValue);
            }
        }
        // Fear
        else if (levelType == LevelType.Fear && projectedValue != npc.FearLvl)
        {
            fearProjectText.text = projectedValue.ToString();
            fearSlash.enabled = true;
            
            if (projectedValue > npc.FearLvl)
            {
                fearProjectUp.gameObject.SetActive(true);
                fearProjectDown.gameObject.SetActive(false);
                fearProjectUp.SetValue(projectedValue);
            }
            else
            {
                fearProjectDown.gameObject.SetActive(true);
                fearProjectUp.gameObject.SetActive(false);
                fearProjectDown.SetValue(projectedValue);
            }
        }
        // Anger
        else if (levelType == LevelType.Anger && projectedValue != npc.AngerLvl)
        {
            angerProjectText.text = projectedValue.ToString();
            angerSlash.enabled = true;
            
            if (projectedValue > npc.AngerLvl)
            {
                angerProjectUp.gameObject.SetActive(true);
                angerProjectDown.gameObject.SetActive(false);
                angerProjectUp.SetValue(projectedValue);
            }
            else
            {
                angerProjectDown.gameObject.SetActive(true);
                angerProjectUp.gameObject.SetActive(false);
                angerProjectDown.SetValue(projectedValue);
            }
        }
    }

    // Reset project effect UI
    public void ResetProjectedUI()
    {
        // Text
        energyProjectText.text = null;
        joyProjectText.text = null;
        sadnessProjectText.text = null;
        fearProjectText.text = null;
        angerProjectText.text = null;

        // Slash
        energySlash.enabled = false;
        joySlash.enabled = false;
        sadnessSlash.enabled = false;
        fearSlash.enabled = false;
        angerSlash.enabled = false;

        // Project Bars
        energyProjectUp.gameObject.SetActive(false);
        joyProjectUp.gameObject.SetActive(false);
        sadnessProjectUp.gameObject.SetActive(false);
        fearProjectUp.gameObject.SetActive(false);
        angerProjectUp.gameObject.SetActive(false);
        energyProjectDown.gameObject.SetActive(false);
        joyProjectDown.gameObject.SetActive(false);
        sadnessProjectDown.gameObject.SetActive(false);
        fearProjectDown.gameObject.SetActive(false);
        angerProjectDown.gameObject.SetActive(false);
    }

    /* ------------------------------- Coroutines ------------------------------- */
    // Apply energy and emotion effects on bars
    IEnumerator UpdateLevelBars()
    {
        while (true)
        {
            _lerpTimer += Time.deltaTime;
            _percentComplete = Mathf.Pow(_lerpTimer / _chipSpeed, 2);

            // Update energy
            if (EnergyBackBar.Value > npc.EnergyLvl)
            {
                EnergyFrontBar.SetValue(npc.EnergyLvl);
                EnergyBackBar.SetValue(Mathf.Lerp(EnergyBackBar.Value, npc.EnergyLvl, _percentComplete));
            }

            if (EnergyFrontBar.Value < npc.EnergyLvl)
            {
                EnergyBackBar.SetValue(npc.EnergyLvl);
                EnergyFrontBar.SetValue(Mathf.Lerp(EnergyFrontBar.Value, npc.EnergyLvl, _percentComplete));
            }

            // Update joy
            if (JoyBackBar.Value > npc.JoyLvl)
            {
                JoyFrontBar.SetValue(npc.JoyLvl);
                JoyBackBar.SetValue(Mathf.Lerp(JoyBackBar.Value, npc.JoyLvl, _percentComplete));
            }

            if (JoyFrontBar.Value < npc.JoyLvl)
            {
                JoyBackBar.SetValue(npc.JoyLvl);
                JoyFrontBar.SetValue(Mathf.Lerp(JoyFrontBar.Value, npc.JoyLvl, _percentComplete));
            }

            // Update sadness
            if (SadnessBackBar.Value > npc.SadnessLvl)
            {
                SadnessFrontBar.SetValue(npc.SadnessLvl);
                SadnessBackBar.SetValue(Mathf.Lerp(SadnessBackBar.Value, npc.SadnessLvl, _percentComplete));
            }

            if (SadnessFrontBar.Value < npc.SadnessLvl)
            {
                SadnessBackBar.SetValue(npc.SadnessLvl);
                SadnessFrontBar.SetValue(Mathf.Lerp(SadnessFrontBar.Value, npc.SadnessLvl, _percentComplete));
            }

            // Update fear
            if (FearBackBar.Value > npc.FearLvl)
            {
                FearFrontBar.SetValue(npc.FearLvl);
                FearBackBar.SetValue(Mathf.Lerp(FearBackBar.Value, npc.FearLvl, _percentComplete));
            }

            if (FearFrontBar.Value < npc.FearLvl)
            {
                FearBackBar.SetValue(npc.FearLvl);
                FearFrontBar.SetValue(Mathf.Lerp(FearFrontBar.Value, npc.FearLvl, _percentComplete));
            }

            // Update anger
            if (AngerBackBar.Value > npc.AngerLvl)
            {
                AngerFrontBar.SetValue(npc.AngerLvl);
                AngerBackBar.SetValue(Mathf.Lerp(AngerBackBar.Value, npc.AngerLvl, _percentComplete));
            }

            if (AngerFrontBar.Value < npc.AngerLvl)
            {
                AngerBackBar.SetValue(npc.AngerLvl);
                AngerFrontBar.SetValue(Mathf.Lerp(AngerFrontBar.Value, npc.AngerLvl, _percentComplete));
            }

            yield return null;
        }
    }

    // Fade out effects text UI
    IEnumerator FadeOutEffects()
    {
        while (true)
        {
            yield return new WaitUntil(() => effectTexts.alpha >= 0);

            effectTexts.alpha -= (Time.deltaTime - _fadeSpeed);

            if (effectTexts.alpha == 0)
            {
                ResetEffectTexts();
            }
        }
    }

    // Show indicator on negative or surplus energy
    IEnumerator ShowEnergySignal()
    {
        while (true)
        {
            yield return new WaitUntil(() => npc.EnergyLvl > 20 || npc.EnergyLvl < 0);

            // Show image
            EnergyBarGlow.enabled = true;

            // Change color
            if (npc.EnergyLvl > 20)
            {
                EnergyBarGlow.color = new Color32(0,171,109,255);
            }
            else if (npc.EnergyLvl < 0)
            {
                EnergyBarGlow.color = new Color32(249,151,60,255);
            }
        }
    }

    // Hide indicator on energy within bounds
    IEnumerator HideEnergySignal()
    {
        while (true)
        {
            yield return new WaitUntil(() => npc.EnergyLvl < 20 && npc.EnergyLvl > 0);

            // Hide image
            EnergyBarGlow.enabled = false;
        }
    }

    // Change NPC expressions
    IEnumerator ChangeExpression()
    {
        while (true)
        {
            // Joy
            if ((npc.JoyLvl > npc.SadnessLvl) &&
                (npc.JoyLvl > npc.FearLvl) &&
                (npc.JoyLvl > npc.AngerLvl))
            {
                illustrationImage.sprite = npc.JoyIllustration;
            }
            // Sadness
            else if ((npc.SadnessLvl > npc.JoyLvl) &&
                     (npc.SadnessLvl > npc.FearLvl) &&
                     (npc.SadnessLvl > npc.AngerLvl))
            {
                illustrationImage.sprite = npc.SadnessIllustration;
            }
            // Fear
            else if ((npc.FearLvl > npc.JoyLvl) &&
                     (npc.FearLvl > npc.SadnessLvl) &&
                     (npc.FearLvl > npc.AngerLvl))
            {
                illustrationImage.sprite = npc.FearIllustration;
            }
            // Anger
            else if ((npc.AngerLvl > npc.JoyLvl) &&
                     (npc.AngerLvl > npc.SadnessLvl) &&
                     (npc.AngerLvl > npc.FearLvl))
            {
                illustrationImage.sprite = npc.SadnessIllustration;
            }
            // Balanced
            else
            {
                illustrationImage.sprite = npc.Illustration;
            }

            yield return null;
        }
    }
}
