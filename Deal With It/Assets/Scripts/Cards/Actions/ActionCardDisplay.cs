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
    private Action _currentActionCard;
    public Action CurrentActionCard => _currentActionCard;

    // UI Variables
    public TMP_Text CardTypeText, CardNameText, CardDescriptionText, EnergyText, JoyText, SadnessText, FearText, AngerText, ActionTypeText;
    public Image EnergyImage, JoyImage, SadnessImage, FearImage, AngerImage, ActionTypeImage;

    // Arrows UI
    public Image EnergyArrowUpImage, EnergyArrowDownImage, EnergyEqualImage;
    public Image JoyArrowUpImage, JoyArrowDownImage, JoyEqualImage;
    public Image SadnessArrowUpImage, SadnessArrowDownImage, SadnessEqualImage;
    public Image FearArrowUpImage, FearArrowDownImage, FearEqualImage;
    public Image AngerArrowUpImage, AngerArrowDownImage, AngerEqualImage;

    // Update is called once per frame
    void Update()
    {
        // ActionCardDisplay is under PlayerController
        if(Owner != null){
            _currentActionCard = Owner.CardsInHand[CardNumber];

        // ActionCardDisplay is under PlayedActionCardsDisplay
        }else if(Owner1 != null){
            _currentActionCard = Owner1.PlayedActionCards[CardNumber];
        }else{
            _currentActionCard = null;
        }

        if(_currentActionCard != null){
            CardTypeText.text = _currentActionCard.GetType().Name;
            CardNameText.text = _currentActionCard.CardName;
            CardDescriptionText.text = _currentActionCard.CardDescription;
            ActionTypeText.text = _currentActionCard.CardActionType.ToString();

            EnergyText.text = FormatText(_currentActionCard.EnergyVal, _currentActionCard.EnergyOriginalVal);
            EnergyImage.gameObject.SetActive(ShowImage(_currentActionCard.EnergyVal, _currentActionCard.EnergyOriginalVal));

            JoyText.text = FormatText(_currentActionCard.JoyVal, _currentActionCard.JoyOriginalVal);
            JoyImage.gameObject.SetActive(ShowImage(_currentActionCard.JoyVal, _currentActionCard.JoyOriginalVal));

            SadnessText.text = FormatText(_currentActionCard.SadnessVal, _currentActionCard.SadnessOriginalVal);
            SadnessImage.gameObject.SetActive(ShowImage(_currentActionCard.SadnessVal, _currentActionCard.SadnessOriginalVal));

            FearText.text = FormatText(_currentActionCard.FearVal, _currentActionCard.FearOriginalVal);
            FearImage.gameObject.SetActive(ShowImage(_currentActionCard.FearVal, _currentActionCard.FearOriginalVal));

            AngerText.text = FormatText(_currentActionCard.AngerVal, _currentActionCard.AngerOriginalVal);
            AngerImage.gameObject.SetActive(ShowImage(_currentActionCard.AngerVal, _currentActionCard.AngerOriginalVal));

            EnergyArrowUpImage.enabled = (_currentActionCard.EnergyValChangeDir == 0);
            JoyArrowUpImage.enabled = (_currentActionCard.JoyValChangeDir == 0);
            SadnessArrowUpImage.enabled = (_currentActionCard.SadnessValChangeDir == 0);
            FearArrowUpImage.enabled = (_currentActionCard.FearValChangeDir == 0);
            AngerArrowUpImage.enabled = (_currentActionCard.AngerValChangeDir == 0);

            EnergyArrowDownImage.enabled = (_currentActionCard.EnergyValChangeDir == 1);
            JoyArrowDownImage.enabled = (_currentActionCard.JoyValChangeDir == 1);
            SadnessArrowDownImage.enabled = (_currentActionCard.SadnessValChangeDir == 1);
            FearArrowDownImage.enabled = (_currentActionCard.FearValChangeDir == 1);
            AngerArrowDownImage.enabled = (_currentActionCard.AngerValChangeDir == 1);

            EnergyEqualImage.enabled = _currentActionCard.EnergyValCanceled;
            JoyEqualImage.enabled = _currentActionCard.JoyValCanceled;
            SadnessEqualImage.enabled = _currentActionCard.SadnessValCanceled;
            FearEqualImage.enabled = _currentActionCard.FearValCanceled;
            AngerEqualImage.enabled = _currentActionCard.AngerValCanceled;
        }
    }
}
