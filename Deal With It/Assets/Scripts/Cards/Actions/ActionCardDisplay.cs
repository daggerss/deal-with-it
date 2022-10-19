using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ActionCardDisplay : CardDisplay
{
    public PlayerController Owner;
    public PlayedActionCardsDisplay Owner1;
    public int CardNumber;

    // UI Variables
    public TMP_Text CardTypeText, CardNameText, CardDescriptionText, EnergyText, JoyText, SadnessText, FearText, AngerText, ActionTypeText;
    public Image EnergyImage, JoyImage, SadnessImage, FearImage, AngerImage, ActionTypeImage;

    // Arrows UI
    public Image EnergyArrowUpImage, EnergyArrowDownImage;
    public Image JoyArrowUpImage, JoyArrowDownImage;
    public Image SadnessArrowUpImage, SadnessArrowDownImage;
    public Image FearArrowUpImage, FearArrowDownImage;
    public Image AngerArrowUpImage, AngerArrowDownImage;
    
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Action currentActionCard;

        // ActionCardDisplay is under PlayerController
        if(Owner != null){
            currentActionCard = Owner.CardsInHand[CardNumber];
        
        // ActionCardDisplay is under PlayedActionCardsDisplay
        }else if(Owner1 != null){
            currentActionCard = Owner1.PlayedActionCards[CardNumber];
        }else{
            currentActionCard = null;
        }

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

            EnergyArrowUpImage.gameObject.SetActive(ShouldShowArrow(0, currentActionCard.EnergyOriginalVal, currentActionCard.EnergyVal));
            JoyArrowUpImage.gameObject.SetActive(ShouldShowArrow(0, currentActionCard.JoyOriginalVal, currentActionCard.JoyVal));
            SadnessArrowUpImage.gameObject.SetActive(ShouldShowArrow(0, currentActionCard.SadnessOriginalVal, currentActionCard.SadnessVal));
            FearArrowUpImage.gameObject.SetActive(ShouldShowArrow(0, currentActionCard.FearOriginalVal, currentActionCard.FearVal));
            AngerArrowUpImage.gameObject.SetActive(ShouldShowArrow(0, currentActionCard.AngerOriginalVal, currentActionCard.AngerVal));

            EnergyArrowDownImage.gameObject.SetActive(ShouldShowArrow(1, currentActionCard.EnergyOriginalVal, currentActionCard.EnergyVal));
            JoyArrowDownImage.gameObject.SetActive(ShouldShowArrow(1, currentActionCard.JoyOriginalVal, currentActionCard.JoyVal));
            SadnessArrowDownImage.gameObject.SetActive(ShouldShowArrow(1, currentActionCard.SadnessOriginalVal, currentActionCard.SadnessVal));
            FearArrowDownImage.gameObject.SetActive(ShouldShowArrow(1, currentActionCard.FearOriginalVal, currentActionCard.FearVal));
            AngerArrowDownImage.gameObject.SetActive(ShouldShowArrow(1, currentActionCard.AngerOriginalVal, currentActionCard.AngerVal));
        }else{
        }
    }
}
