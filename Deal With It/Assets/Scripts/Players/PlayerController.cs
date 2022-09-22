using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    // Player Number
    public int PlayerNumber;

    // Array of cards in a player's hand (public if we're adding feature when other players can look into ur hand)
    public Action[] CardsInHand = new Action[5];
    public Text[] CardsInHandText = new Text[5];

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
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log(RoundController.Round);
        // Debug.Log(CurrentRound);
        if(RoundController.Round != CurrentRound){
            CurrentRound = RoundController.Round;

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
        Debug.Log("Player " + PlayerNumber + " has played the card " + CardsInHand[CardIndex].CardName);
    }
}
