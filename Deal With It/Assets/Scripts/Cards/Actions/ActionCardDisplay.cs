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
        Action ThisCard = Owner.CardsInHand[CardNumber];

        if(ThisCard != null){

            CardTypeText.text = ThisCard.GetType().Name;
            CardNameText.text = ThisCard.CardName;
            CardDescriptionText.text = ThisCard.CardDescription;
            ActionTypeText.text = ThisCard.ActionType;

            EnergyText.text = FormatText(ThisCard.EnergyVal);
            EnergyImage.gameObject.SetActive(ShowImage(ThisCard.EnergyVal));

            JoyText.text = FormatText(ThisCard.JoyVal);
            JoyImage.gameObject.SetActive(ShowImage(ThisCard.JoyVal));

            SadnessText.text = FormatText(ThisCard.SadnessVal);
            SadnessImage.gameObject.SetActive(ShowImage(ThisCard.SadnessVal));

            FearText.text = FormatText(ThisCard.FearVal);
            FearImage.gameObject.SetActive(ShowImage(ThisCard.FearVal));

            AngerText.text = FormatText(ThisCard.AngerVal);
            AngerImage.gameObject.SetActive(ShowImage(ThisCard.AngerVal));
        }else{
        }
    }

    /* ---------------------------- Custom Functions ---------------------------- */
    // Returns null if value is 0 (for printing values of energy etc.)
    private string FormatText(int Value){
        if(Value == 0){
            return null;
        }else if(Value > 0){
            return "+" + Value.ToString();
        }else{
            return Value.ToString();
        }
    }

    // Show or hide image
    private bool ShowImage(int Value){
        string EmotionPoints = FormatText(Value);

        if(EmotionPoints == null){
            return false;
        }else{
            return true;
        }
    }
}
