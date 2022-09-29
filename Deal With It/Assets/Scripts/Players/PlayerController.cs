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
    private Vector2[] OriginalButtonPosition = new Vector2[5];

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
        for(int i = 0; i < OriginalButtonPosition.Length; i++){
            OriginalButtonPosition[i] = CardsInHandButton[i].transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(RoundController.Round != CurrentRound){
            // Set Current round to roundcontroller.round so the if doesn't repeat 
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

                // Check for empty slot again for the while loop
                IndexOfEmptyElement = Array.IndexOf(CardsInHand, null);
            }
        }
    }

    // Check If Player Has Card Buttons
    private bool PlayerHasButtons(){
        foreach(Button btn in CardsInHandButton){
            if(btn != null){
                return true;
            }
        }

        return false;
    }

    // Select Card
    public void SelectCard(int CardIndex){
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

        // Flare for selected card and non-flare for non selected cards
        for(int i = 0; i < CardsInHandButton.Length; i++){
            Button CurrentButton = CardsInHandButton[i];
            if(SelectedCard == i){
                CurrentButton.transform.position = new Vector2(CurrentButton.transform.position.x, CurrentButton.transform.position.y + 2f);
            }else{
                CurrentButton.transform.position = OriginalButtonPosition[i];
            }
        }
    }
}
