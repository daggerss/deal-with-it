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
        Action currentActionCard = Owner.CardsInHand[CardNumber];

        if(currentActionCard != null){

            CardTypeText.text = currentActionCard.GetType().Name;
            CardNameText.text = currentActionCard.CardName;
            CardDescriptionText.text = currentActionCard.CardDescription;
            ActionTypeText.text = currentActionCard.CardActionType.ToString();

            EnergyText.text = FormatText(currentActionCard.EnergyVal);
            EnergyImage.gameObject.SetActive(ShowImage(currentActionCard.EnergyVal));

            JoyText.text = FormatText(currentActionCard.JoyVal);
            JoyImage.gameObject.SetActive(ShowImage(currentActionCard.JoyVal));

            SadnessText.text = FormatText(currentActionCard.SadnessVal);
            SadnessImage.gameObject.SetActive(ShowImage(currentActionCard.SadnessVal));

            FearText.text = FormatText(currentActionCard.FearVal);
            FearImage.gameObject.SetActive(ShowImage(currentActionCard.FearVal));

            AngerText.text = FormatText(currentActionCard.AngerVal);
            AngerImage.gameObject.SetActive(ShowImage(currentActionCard.AngerVal));
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
