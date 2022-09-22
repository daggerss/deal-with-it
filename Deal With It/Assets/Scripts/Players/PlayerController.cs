using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    // Player Variables
    public int PlayerNumber;
    private int SelectedCard = -1;

    // Misc
    private Vector2 OriginalButtonSize;

    // Array of cards in a player's hand (public if we're adding feature when other players can look into ur hand)
    public Action[] CardsInHand = new Action[5];
    public Text[] CardsInHandText = new Text[5];
    public Button[] CardsInHandButton = new Button[5];

    // ActionCardDeck
    private ActionCardDeck ActionCardDeck;

    // Round Counter
    private RoundController RoundController;
    private int CurrentRound = -1;

    // Start is called before the first frame update
    void Start()
    {
        // Initializing the ActionCardDeck
        ActionCardDeck = (ActionCardDeck)GameObject.FindGameObjectWithTag("Action Card Deck").GetComponent(typeof(ActionCardDeck));

        // Initializing the RoundController 
        RoundController = (RoundController)GameObject.FindGameObjectWithTag("Round Controller").GetComponent(typeof(RoundController));

        // Initializing OriginalButtonSize
        OriginalButtonSize = CardsInHandButton[0].transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log(RoundController.Round);
        // Debug.Log(CurrentRound);
        if(RoundController.Round != CurrentRound){
            CurrentRound = RoundController.Round;
            SelectedCard = -1;

            int IndexOfEmptyElement = Array.IndexOf(CardsInHand, null);

            // Makes sure that players have 5 cards every round
            // While there is an empty slot in the player's hand
            while(IndexOfEmptyElement != -1){
                Action PickedCard = ActionCardDeck.GetRandomCard();

                // Puts card in player's hand
                CardsInHand[IndexOfEmptyElement] = PickedCard;

                // Writes down the card name for a button
                CardsInHandText[IndexOfEmptyElement].text = PickedCard.CardName;

                IndexOfEmptyElement = Array.IndexOf(CardsInHand, null);
            }
        }
    }

    public void PlayCard(int CardIndex){
        // If a player clicks on a card twice it will deselect the card
        if(CardIndex == SelectedCard){
            SelectedCard = -1;
        // If a player clicks on a card slot with a value inside
        }else if(CardsInHand[CardIndex] != null){
            SelectedCard = CardIndex;
        // If a player clicks on a card slot with no value inside
        }else{
            // Do nothing
        }

        for(int i = 0; i < CardsInHandButton.Length; i++){
            if(SelectedCard == i){
                CardsInHandButton[i].transform.localScale = new Vector2(1.2f, 1.2f);
            }else{
                CardsInHandButton[i].transform.localScale = OriginalButtonSize;
            }
        }
    }
}
