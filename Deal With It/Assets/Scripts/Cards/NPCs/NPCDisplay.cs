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

    public LevelBar energyProjectLine;
    public LevelBar joyProjectLine;
    public LevelBar sadnessProjectLine;
    public LevelBar fearProjectLine;
    public LevelBar angerProjectLine;

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

        // Set project line
        energyProjectLine.SetMaxValue(20);
        joyProjectLine.SetMaxValue(13);
        sadnessProjectLine.SetMaxValue(13);
        fearProjectLine.SetMaxValue(13);
        angerProjectLine.SetMaxValue(13);

        energyProjectLine.SetValue(20);
        joyProjectLine.SetValue(npc.JoyLvl);
        sadnessProjectLine.SetValue(npc.SadnessLvl);
        fearProjectLine.SetValue(npc.FearLvl);
        angerProjectLine.SetValue(npc.AngerLvl);

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
        StartCoroutine(UpdateLevelBars());
        StartCoroutine(FadeOutEffects());
        StartCoroutine(ShowEnergySignal());
        StartCoroutine(HideEnergySignal());
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

                // NPC x Events
                else if (actionType == ActionType.None)
                {
                    // Joy
                    if (levelType == LevelType.Joy)
                    {
                        addend += AddExtraEffect(effectValue, npc.JoyAddend);
                    }
                    // Sadness
                    else if (levelType == LevelType.Sadness)
                    {
                        addend += AddExtraEffect(effectValue, npc.SadnessAddend);
                    }
                    // Fear
                    else if (levelType == LevelType.Fear)
                    {
                        addend += AddExtraEffect(effectValue, npc.FearAddend);
                    }
                    // Anger
                    else if (levelType == LevelType.Anger)
                    {
                        addend += AddExtraEffect(effectValue, npc.AngerAddend);
                    }
                }
            }
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
            energyProjectLine.gameObject.SetActive(true);
            energyProjectLine.SetValue(projectedValue);
        }
        // Joy
        else if (levelType == LevelType.Joy && projectedValue != npc.JoyLvl)
        {
            joyProjectText.text = projectedValue.ToString();
            joySlash.enabled = true;
            joyProjectLine.gameObject.SetActive(true);
            joyProjectLine.SetValue(projectedValue);
        }
        // Sadness
        else if (levelType == LevelType.Sadness && projectedValue != npc.SadnessLvl)
        {
            sadnessProjectText.text = projectedValue.ToString();
            sadnessSlash.enabled = true;
            sadnessProjectLine.gameObject.SetActive(true);
            sadnessProjectLine.SetValue(projectedValue);
        }
        // Fear
        else if (levelType == LevelType.Fear && projectedValue != npc.FearLvl)
        {
            fearProjectText.text = projectedValue.ToString();
            fearSlash.enabled = true;
            fearProjectLine.gameObject.SetActive(true);
            fearProjectLine.SetValue(projectedValue);
        }
        // Anger
        else if (levelType == LevelType.Anger && projectedValue != npc.AngerLvl)
        {
            angerProjectText.text = projectedValue.ToString();
            angerSlash.enabled = true;
            angerProjectLine.gameObject.SetActive(true);
            angerProjectLine.SetValue(projectedValue);
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

        // Project Line
        energyProjectLine.gameObject.SetActive(false);
        joyProjectLine.gameObject.SetActive(false);
        sadnessProjectLine.gameObject.SetActive(false);
        fearProjectLine.gameObject.SetActive(false);
        angerProjectLine.gameObject.SetActive(false);
    }

    /* ------------------------------- Coroutines ------------------------------- */
    // Apply energy and emotion effects on bars
    IEnumerator UpdateLevelBars()
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

        yield return new WaitForSeconds(1f);
    }

    // Fade out effects text UI
    IEnumerator FadeOutEffects()
    {
        yield return new WaitUntil(() => effectTexts.alpha >= 0);

        effectTexts.alpha -= (Time.deltaTime - _fadeSpeed);

        if (effectTexts.alpha == 0)
        {
            ResetEffectTexts();
        }
    }

    // Show indicator on negative or surplus energy
    IEnumerator ShowEnergySignal()
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

    // Hide indicator on energy within bounds
    IEnumerator HideEnergySignal()
    {
        yield return new WaitUntil(() => npc.EnergyLvl < 20 && npc.EnergyLvl > 0);

        // Hide image
        EnergyBarGlow.enabled = false;
    }
}
