using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    /* -------------------------------- Variables ------------------------------- */
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
    public NPCDisplay NPCDisplay;

    // PlayedActionCards
    public PlayedActionCardsDisplay PlayedActionCards;
    private bool _aiActionCardPlayed = false;

    /* --------------------------------- Methods -------------------------------- */
    // Start is called before the first frame update
    void Start()
    {
        // Initializing the ActionCardDeck 
        ActionCardDeck = (ActionCardDeck)GameObject.FindGameObjectWithTag("Action Card Deck").GetComponent(typeof(ActionCardDeck));

        // Initializing the RoundController 
        RoundController = (RoundController)GameObject.FindGameObjectWithTag("Round Controller").GetComponent(typeof(RoundController));

        // Initializing NPC
        NPCDisplay = (NPCDisplay)GameObject.FindGameObjectWithTag("NPC").GetComponent(typeof(NPCDisplay));

        // Initializing PlayedActionCards
        PlayedActionCards = (PlayedActionCardsDisplay)GameObject.FindGameObjectWithTag("Played Action Cards Display").GetComponent(typeof(PlayedActionCardsDisplay));

        // Initializing OriginalButtonSize
        for(int i = 0; i < OriginalButtonPosition.Length; i++){
            OriginalButtonPosition[i] = CardsInHandButton[i].transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // New round
        if(RoundController.Round != CurrentRound){
            // Set Current round to RoundController.Round so the if doesn't repeat 
            CurrentRound = RoundController.Round;
            SelectedCard = -1;

            // Set _aiActionCardPlayed to false so that all of the AI are ready to play a card
            _aiActionCardPlayed = false;

            // Makes sure that players have 5 cards every round
            // While there is an empty slot in the player's hand
            int IndexOfEmptyElement = Array.IndexOf(CardsInHand, null);

            while(IndexOfEmptyElement != -1){
                // Pick a card
                Action pickedCard = ActionCardDeck.GetRandomCard();
                
                // Ensures cards don't repeat
                int indexOfPickedCard = Array.IndexOf(CardsInHand, pickedCard);

                // Card already in hand
                while(indexOfPickedCard != -1){
                    // Pick another card
                    pickedCard = ActionCardDeck.GetRandomCard();
                    indexOfPickedCard = Array.IndexOf(CardsInHand, pickedCard);
                }

                // Randomize card's emotion values
                pickedCard.SetRandomEmotions();

                // Make sure card is using its original values
                pickedCard.Revert();

                // Puts card in player's hand
                CardsInHand[IndexOfEmptyElement] = pickedCard;
                CardsInHandButton[IndexOfEmptyElement].gameObject.SetActive(true);

                // Check for empty slot again for the while loop
                IndexOfEmptyElement = Array.IndexOf(CardsInHand, null);
            }

            // Reset Cards to Original Position at the start of every round
            SelectCard(-1);
        }

        // If it is this player's turn
        if(RoundController.PlayerTurn == PlayerNumber && !_aiActionCardPlayed){

            //! LEGACY Skip if out of energy     
            // // if (NPCDisplay.npc.EnergyLvl < GetLowestEnergy())
            // // {
            // //     SelectedCard = -2;
            // //     PlayCard();
            // // }

            //If player is AI
            if(!Playable){
                //! CAN WE ADD DELAY HERE SO THE CARD DOESN'T PLAY RIGHT AWAY

                StartCoroutine(AIPlayActionCard());
            }

            // Allow user player to play
            else
            {
                ConfirmButton.gameObject.SetActive(true);
            }
        }else{
            // So player can't play card when it's not their turn
            ConfirmButton.gameObject.SetActive(false);
        }
    }

    /* ----------------------------- Custom Methods ----------------------------- */

    // Select Card
    public void SelectCard(int CardIndex){
        // If you select a different card decrease the count
        if(SelectedCard != -1 && SelectedCard != CardIndex){
            PlayedActionCards.RemoveCurrentCard(CardsInHand[SelectedCard].CardActionType);
        }

        // If a player clicks on a card twice it will deselect the card
        if(CardIndex == SelectedCard){
            // Reset the card to original
            if (SelectedCard >= 0 && SelectedCard < CardsInHand.Length)
            {
                PlayedActionCards.RemoveCurrentCard(CardsInHand[SelectedCard].CardActionType);
                PlayedActionCards.RevertAll();
                CardsInHand[SelectedCard].Revert();
            }
            // Deselect
            SelectedCard = -1;
        // If a player clicks on a card slot with a value inside
        }else if(CardsInHand[CardIndex] != null){
            SelectedCard = CardIndex;

            // Apply NPC trait effects
            Action projectedCard = CardsInHand[SelectedCard];
            ActionType cardActionType = projectedCard.CardActionType;

            projectedCard.EnergyVal = NPCDisplay.ProjectTraitEffect(LevelType.Energy, projectedCard.EnergyVal, cardActionType);
            projectedCard.JoyVal = NPCDisplay.ProjectTraitEffect(LevelType.Joy, projectedCard.JoyVal, cardActionType);
            projectedCard.SadnessVal = NPCDisplay.ProjectTraitEffect(LevelType.Sadness, projectedCard.SadnessVal, cardActionType);
            projectedCard.FearVal = NPCDisplay.ProjectTraitEffect(LevelType.Fear, projectedCard.FearVal, cardActionType);
            projectedCard.AngerVal = NPCDisplay.ProjectTraitEffect(LevelType.Anger, projectedCard.AngerVal, cardActionType);

            // Add current card type to the card count
            PlayedActionCards.AddCurrentCard(cardActionType);

            // Set all PlayedActionCards to original value before applying new effects
            PlayedActionCards.RevertAll();

            // int ProjectComboEffect(LevelType levelType, int effectValue, ActionType actionType);
            projectedCard.EnergyVal = PlayedActionCards.ProjectComboEffect(LevelType.Energy, projectedCard.EnergyVal, cardActionType);
            projectedCard.JoyVal = PlayedActionCards.ProjectComboEffect(LevelType.Joy, projectedCard.JoyVal, cardActionType);
            projectedCard.SadnessVal = PlayedActionCards.ProjectComboEffect(LevelType.Sadness, projectedCard.SadnessVal, cardActionType);
            projectedCard.FearVal = PlayedActionCards.ProjectComboEffect(LevelType.Fear, projectedCard.FearVal, cardActionType);
            projectedCard.AngerVal = PlayedActionCards.ProjectComboEffect(LevelType.Anger, projectedCard.AngerVal, cardActionType);
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
                // Reset the card to original
                CardsInHand[i].Revert();
            }
        }
    }

    // Play Card
    public void PlayCard(){
        // No card picked
        if(SelectedCard == -1){
            Debug.Log("Player " + PlayerNumber + " played no card");
        }
        //! LEGACY right now won't trigger
        // // else if (SelectedCard == -2){
        // //     // Energy exhausted force skip
        // //     Debug.Log("Out of energy! Player " + PlayerNumber + " is skipped.");
        // // }
        // Play card picked
        else{
            Action playedActionCard = CardsInHand[SelectedCard];

            // Add playedActionCard to the PlayedActionCards in the middle
            PlayedActionCards.AddPlayedActionCard(playedActionCard);

            Debug.Log("Player " + PlayerNumber + " played " + playedActionCard.CardActionType);

            // Remove card from hand
            CardsInHand[SelectedCard] = null;
            CardsInHandButton[SelectedCard].gameObject.SetActive(false);

            //! LEGACY Action cards are only gonna be applied at the end of the round
            NPCDisplay.ApplyEffect(LevelType.Energy, playedActionCard.EnergyVal, playedActionCard.CardActionType);
            NPCDisplay.ApplyEffect(LevelType.Joy, playedActionCard.JoyVal, playedActionCard.CardActionType);
            NPCDisplay.ApplyEffect(LevelType.Sadness, playedActionCard.SadnessVal, playedActionCard.CardActionType);
            NPCDisplay.ApplyEffect(LevelType.Fear, playedActionCard.FearVal, playedActionCard.CardActionType);
            NPCDisplay.ApplyEffect(LevelType.Anger, playedActionCard.AngerVal, playedActionCard.CardActionType);
        }

        RoundController.NextPlayer();
    }

    /* --------------------- Apply PlayedActionCards effects -------------------- */
    private void ApplyPlayedActionCardsEffects(){
        
    }

    /* --------------------- AI Play Action Card With Delay --------------------- */
    private IEnumerator AIPlayActionCard(){
        // _aiActionCardPlayed
        _aiActionCardPlayed = true;
        
        //Wait 
        yield return new WaitForSeconds(3f);

        // Random Number
        int rng = UnityEngine.Random.Range(-1, CardsInHand.Length);
        SelectedCard = rng;
        PlayCard();
    }

    //! LEGACY Get lowest energy cost in hand
    // // private int GetLowestEnergy()
    // // {
    // //     int lowest = -50;

    // //     foreach (Action card in CardsInHand)
    // //     {
    // //         if (card.EnergyVal > 0)
    // //         {
    // //             return -card.EnergyVal; // Always negative
    // //         }
    // //         else if (card.EnergyVal < 0 && card.EnergyVal > lowest)
    // //         {
    // //             lowest = card.EnergyVal;
    // //         }
    // //     }

    // //     return -lowest;
    // // }
}
