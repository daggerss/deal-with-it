using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NPCDisplay : MonoBehaviour
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

    /* --------------------------------- Bar UI --------------------------------- */
    public LevelBar EnergyBar;
    public LevelBar JoyBar;
    public LevelBar SadnessBar;
    public LevelBar FearBar;
    public LevelBar AngerBar;

    /* --------------------------------- Methods -------------------------------- */
    // Start is called before the first frame update
    void Start()
    {
        // Set energy UI
        EnergyBar.SetMaxValue(20);
        EnergyBar.SetValue(20);

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

        // Set emotions UI
        JoyBar.SetMaxValue(13);
        SadnessBar.SetMaxValue(13);
        FearBar.SetMaxValue(13);
        AngerBar.SetMaxValue(13);

        JoyBar.SetValue(npc.JoyLvl);
        SadnessBar.SetValue(npc.SadnessLvl);
        FearBar.SetValue(npc.FearLvl);
        AngerBar.SetValue(npc.AngerLvl);

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
}
