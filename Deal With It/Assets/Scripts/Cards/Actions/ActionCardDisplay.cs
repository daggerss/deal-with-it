using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ActionCardDisplay : MonoBehaviour
{
    public PlayerController Owner;
    public int CardNumber;

    // UI Variables
    public TMP_Text CardTypeText, CardNameText, CardDescriptionText, EnergyText, JoyText, SadnessText, FearText, AngerText, ActionTypeText;
    public Image EnergyImage, JoyImage, SadnessImage, FearImage, AngerImage, ActionTypeImage;
    

    //public Text CardNameText, CardDescriptionText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Action thisCard = Owner.CardsInHand[CardNumber];

        if(thisCard != null){

            CardTypeText.text = thisCard.GetType().Name;
            CardNameText.text = thisCard.CardName;
            CardDescriptionText.text = thisCard.CardDescription;
            ActionTypeText.text = thisCard.ActionType;

            EnergyText.text = FormatText(thisCard.EnergyVal);
            EnergyImage.gameObject.SetActive(ShowImage(thisCard.EnergyVal));

            JoyText.text = FormatText(thisCard.JoyVal);
            JoyImage.gameObject.SetActive(ShowImage(thisCard.JoyVal));

            SadnessText.text = FormatText(thisCard.SadnessVal);
            SadnessImage.gameObject.SetActive(ShowImage(thisCard.SadnessVal));

            FearText.text = FormatText(thisCard.FearVal);
            FearImage.gameObject.SetActive(ShowImage(thisCard.FearVal));

            AngerText.text = FormatText(thisCard.AngerVal);
            AngerImage.gameObject.SetActive(ShowImage(thisCard.AngerVal));
        }else{
        }
    }

    /* ---------------------------- Custom Functions ---------------------------- */
    // Returns null if value is 0 (for printing values of energy etc.)
    private string FormatText(int value){
        if(value == 0){
            return null;
        }else if(value > 0){
            return "+" + value.ToString();
        }else{
            return value.ToString();
        }
    }

    // Show or hide image
    private bool ShowImage(int value){
        string EmotionPoints = FormatText(value);

        if(EmotionPoints == null){
            return false;
        }else{
            return true;
        }
    }
}
