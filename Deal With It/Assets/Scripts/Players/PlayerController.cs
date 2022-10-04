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
    public bool Playable;

    // Misc
    private Vector2[] OriginalButtonPosition = new Vector2[5];

    // Array of cards in a player's hand (public if we're adding feature when other players can look into ur hand)
    public Action[] CardsInHand = new Action[5];
    public Button[] CardsInHandButton = new Button[5];
    public Button ConfirmButton;

    // ActionCardDeck
    private ActionCardDeck ActionCardDeck;

    // Round Counter
    private RoundController RoundController;
    private int CurrentRound = -1;

    // NPC
    public NPCDisplay npcDisplay;

    // Start is called before the first frame update
    void Start()
    {
        // Initializing the ActionCardDeck 
        ActionCardDeck = (ActionCardDeck)GameObject.FindGameObjectWithTag("Action Card Deck").GetComponent(typeof(ActionCardDeck));

        // Initializing the RoundController 
        RoundController = (RoundController)GameObject.FindGameObjectWithTag("Round Controller").GetComponent(typeof(RoundController));

        // Initializing NPC
        npcDisplay = (NPCDisplay)GameObject.FindGameObjectWithTag("NPC").GetComponent(typeof(NPCDisplay));

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
                Action pickedCard = ActionCardDeck.GetRandomCard();

                // Puts card in player's hand
                CardsInHand[IndexOfEmptyElement] = pickedCard;

                // Check for empty slot again for the while loop
                IndexOfEmptyElement = Array.IndexOf(CardsInHand, null);
            }

            // Reset Cards to Original Position at the start of every round
            SelectCard(-1);
        }

        // If it is this player's turn
        if(RoundController.PlayerTurn == PlayerNumber){
            ConfirmButton.gameObject.SetActive(true);

            //If player is AI
            if(!Playable){
                /* -------------------------------------------------------------------------- */
                /* ------- Can we add a buffer here so the cards don't play instantly ------- */
                /* -------------------------------------------------------------------------- */

                // Random Number
                int rng = UnityEngine.Random.Range(0, CardsInHand.Length);
                SelectedCard = rng;
                PlayCard();
            }
        }else{
            // So player can't play card when it's not their turn
            ConfirmButton.gameObject.SetActive(false);
        }
    }

    /* ----------------------------- Custom Methods ----------------------------- */

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
                CurrentButton.transform.position = new Vector2(CurrentButton.transform.position.x, CurrentButton.transform.position.y + 50f);
            }else{
                CurrentButton.transform.position = OriginalButtonPosition[i];
            }
        }
    }

    // Play Card
    public void PlayCard(){
        if(SelectedCard == -1){
            // Do nothing skip turn
            Debug.Log("Player " + PlayerNumber + " played no card");
        }else{
            Action playedActionCard = CardsInHand[SelectedCard];

            Debug.Log("Player " + PlayerNumber + " played " + playedActionCard.CardName);

            // Remove card from hand
            CardsInHand[SelectedCard] = null;

            npcDisplay.ApplyEffect(LevelType.Energy, playedActionCard.EnergyVal, playedActionCard.CardActionType);
            npcDisplay.ApplyEffect(LevelType.Joy, playedActionCard.JoyVal, playedActionCard.CardActionType);
            npcDisplay.ApplyEffect(LevelType.Sadness, playedActionCard.SadnessVal, playedActionCard.CardActionType);
            npcDisplay.ApplyEffect(LevelType.Fear, playedActionCard.FearVal, playedActionCard.CardActionType);
            npcDisplay.ApplyEffect(LevelType.Anger, playedActionCard.AngerVal, playedActionCard.CardActionType);
        }

        RoundController.NextPlayer();
    }
}
