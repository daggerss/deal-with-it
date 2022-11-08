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
    public Image EnergyArrowUpImage, EnergyArrowDownImage, EnergyEqualImage;
    public Image JoyArrowUpImage, JoyArrowDownImage, JoyEqualImage;
    public Image SadnessArrowUpImage, SadnessArrowDownImage, SadnessEqualImage;
    public Image FearArrowUpImage, FearArrowDownImage, FearEqualImage;
    public Image AngerArrowUpImage, AngerArrowDownImage, AngerEqualImage;

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

            EnergyText.text = FormatText(currentActionCard.EnergyVal, currentActionCard.EnergyOriginalVal);
            EnergyImage.gameObject.SetActive(ShowImage(currentActionCard.EnergyVal, currentActionCard.EnergyOriginalVal));

            JoyText.text = FormatText(currentActionCard.JoyVal, currentActionCard.JoyOriginalVal);
            JoyImage.gameObject.SetActive(ShowImage(currentActionCard.JoyVal, currentActionCard.JoyOriginalVal));

            SadnessText.text = FormatText(currentActionCard.SadnessVal, currentActionCard.SadnessOriginalVal);
            SadnessImage.gameObject.SetActive(ShowImage(currentActionCard.SadnessVal, currentActionCard.SadnessOriginalVal));

            FearText.text = FormatText(currentActionCard.FearVal, currentActionCard.FearOriginalVal);
            FearImage.gameObject.SetActive(ShowImage(currentActionCard.FearVal, currentActionCard.FearOriginalVal));

            AngerText.text = FormatText(currentActionCard.AngerVal, currentActionCard.AngerOriginalVal);
            AngerImage.gameObject.SetActive(ShowImage(currentActionCard.AngerVal, currentActionCard.AngerOriginalVal));

            EnergyArrowUpImage.enabled = (currentActionCard.EnergyValChangeDir == 0);
            JoyArrowUpImage.enabled = (currentActionCard.JoyValChangeDir == 0);
            SadnessArrowUpImage.enabled = (currentActionCard.SadnessValChangeDir == 0);
            FearArrowUpImage.enabled = (currentActionCard.FearValChangeDir == 0);
            AngerArrowUpImage.enabled = (currentActionCard.AngerValChangeDir == 0);

            EnergyArrowDownImage.enabled = (currentActionCard.EnergyValChangeDir == 1);
            JoyArrowDownImage.enabled = (currentActionCard.JoyValChangeDir == 1);
            SadnessArrowDownImage.enabled = (currentActionCard.SadnessValChangeDir == 1);
            FearArrowDownImage.enabled = (currentActionCard.FearValChangeDir == 1);
            AngerArrowDownImage.enabled = (currentActionCard.AngerValChangeDir == 1);

            EnergyEqualImage.enabled = currentActionCard.EnergyValCanceled;
            JoyEqualImage.enabled = currentActionCard.JoyValCanceled;
            SadnessEqualImage.enabled = currentActionCard.SadnessValCanceled;
            FearEqualImage.enabled = currentActionCard.FearValCanceled;
            AngerEqualImage.enabled = currentActionCard.AngerValCanceled;
        }else{
        }
    }
}
