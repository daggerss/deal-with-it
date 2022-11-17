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
    [SerializeField] private GameObject _playerTurnIndicator;

    // Misc

    // Array of cards in a player's hand (public if we're adding feature when other players can look into ur hand)
    public Action[] CardsInHand = new Action[5];
    public Button[] CardsInHandButton = new Button[5];
    private Vector2[] OriginalButtonPosition = new Vector2[5];
    private Vector3[] OriginalButtonSize = new Vector3[5];
    public Button ConfirmButton;
    public Button SwapButton;

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

    // Tooltip Texts: NPC Traits
    private string _energyTraitEffectText;
    private string _joyTraitEffectText;
    private string _sadnessTraitEffectText;
    private string _fearTraitEffectText;
    private string _angerTraitEffectText;

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

        // Initializing OriginalButtonPosition
        for(int i = 0; i < OriginalButtonPosition.Length; i++){
            OriginalButtonPosition[i] = CardsInHandButton[i].transform.position;
        }

        // Initializing OriginalButtonSize
        for(int i = 0; i < OriginalButtonSize.Length; i++){
            OriginalButtonSize[i] = CardsInHandButton[i].transform.localScale;
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

        // Show or hide player turn indicator
        if (RoundController.PlayerTurn == PlayerNumber)
        {
            _playerTurnIndicator.SetActive(true);
        }
        else
        {
            _playerTurnIndicator.SetActive(false);
        }

        // If it is this player's turn
        if(RoundController.PlayerTurn == PlayerNumber && !_aiActionCardPlayed){            
            //If player is AI
            if(!Playable){
                StartCoroutine(AIPlayActionCard());
            }
            // Allow user player to play
            else
            {
                ConfirmButton.gameObject.SetActive(true);
                SwapButton.gameObject.SetActive(true);
            }

            // Timer Countdown
            // ! WIP
            // if(!RoundController.CountdownActive){
            //     Debug.Log("CountdownActive = false");
            //     StartCoroutine(RoundController.Skip);
            // }
        }else{
            // So player can't play card when it's not their turn
            ConfirmButton.gameObject.SetActive(false);
            SwapButton.gameObject.SetActive(false);
        }
    }

    /* ----------------------------- Custom Methods ----------------------------- */

    // Select Card
    public void SelectCard(int CardIndex){
        if(RoundController.PlayerTurn == PlayerNumber){ // You can only select card when it is your turn
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
                _energyTraitEffectText = NPCDisplay.TraitEffectText;

                projectedCard.JoyVal = NPCDisplay.ProjectTraitEffect(LevelType.Joy, projectedCard.JoyVal, cardActionType);
                _joyTraitEffectText = NPCDisplay.TraitEffectText;

                projectedCard.SadnessVal = NPCDisplay.ProjectTraitEffect(LevelType.Sadness, projectedCard.SadnessVal, cardActionType);
                _sadnessTraitEffectText = NPCDisplay.TraitEffectText;

                projectedCard.FearVal = NPCDisplay.ProjectTraitEffect(LevelType.Fear, projectedCard.FearVal, cardActionType);
                _fearTraitEffectText = NPCDisplay.TraitEffectText;

                projectedCard.AngerVal = NPCDisplay.ProjectTraitEffect(LevelType.Anger, projectedCard.AngerVal, cardActionType);
                _angerTraitEffectText = NPCDisplay.TraitEffectText;

                // Check if values were changed
                projectedCard.UpdateValueChanges();

                // Update tooltips content
                projectedCard.EnergyTraitEffectText = _energyTraitEffectText;
                projectedCard.JoyTraitEffectText = _joyTraitEffectText;
                projectedCard.SadnessTraitEffectText = _sadnessTraitEffectText;
                projectedCard.FearTraitEffectText = _fearTraitEffectText;
                projectedCard.AngerTraitEffectText = _angerTraitEffectText;

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

                // Check if values were canceled out
                projectedCard.UpdateValueCanceled();

            // If a player clicks on a card slot with no value inside
            }else{
                // Do nothing
            }

            // Flare for selected card and non-flare for non selected cards
            for(int i = 0; i < CardsInHandButton.Length; i++){
                Button currentButton = CardsInHandButton[i];
                if(SelectedCard == i){
                    currentButton.transform.position = new Vector2(currentButton.transform.position.x, 350F);
                    currentButton.transform.localScale = new Vector3(1.5F, 1.5F, 1.5F);
                }else{
                    currentButton.transform.position = OriginalButtonPosition[i];
                    currentButton.transform.localScale = OriginalButtonSize[i]; 
                    // Reset the card to original
                    CardsInHand[i].Revert();
                }
            }
        }
    }

    // Play Card
    public void PlayCard(){
        // Hide confirm button
        ConfirmButton.gameObject.SetActive(false);
        SwapButton.gameObject.SetActive(false);

        // No card picked
        if(SelectedCard == -1){
            Debug.Log("Player " + PlayerNumber + " played no card");
        }else{
            Action playedActionCard = CardsInHand[SelectedCard];

            // Add playedActionCard to the PlayedActionCards in the middle
            PlayedActionCards.AddPlayedActionCard(playedActionCard);

            Debug.Log("Player " + PlayerNumber + " played " + playedActionCard.CardActionType);

            // Remove card from hand
            CardsInHand[SelectedCard] = null;
            CardsInHandButton[SelectedCard].gameObject.SetActive(false);

            // Move played card button back to the original position
            CardsInHandButton[SelectedCard].transform.position = OriginalButtonPosition[SelectedCard];
        }

        RoundController.NextPlayer();
        // ! WIP
        // StopCoroutine(RoundController.Skip);
    }

    public void SwapCard(){
        // Hide swap button
        ConfirmButton.gameObject.SetActive(false);
        SwapButton.gameObject.SetActive(false);

        // No card picked
        if(SelectedCard == -1){
            Debug.Log("Player " + PlayerNumber + " played no card");
        }else{
            Debug.Log("Player " + PlayerNumber + " swapped " + CardsInHand[SelectedCard].CardActionType);

            // Remove card from hand
            CardsInHand[SelectedCard] = null;
            CardsInHandButton[SelectedCard].gameObject.SetActive(false);

            // Move played card button back to the original position
            CardsInHandButton[SelectedCard].transform.position = OriginalButtonPosition[SelectedCard];
        }

        RoundController.NextPlayer();
        // ! WIP
        // StopCoroutine(RoundController.Skip);
    }

    /* --------------------- AI Play Action Card With Delay --------------------- */
    private IEnumerator AIPlayActionCard(){
        // _aiActionCardPlayed
        _aiActionCardPlayed = true;

        //Wait
        yield return new WaitForSeconds(3f);

        // Random Number
        int rng = UnityEngine.Random.Range(-1, CardsInHand.Length);

        // TODO Remove temporary
        // // int rng = 0;
        // // for(int i = 0; i < CardsInHand.Length; i++){
        // //     if(CardsInHand[i].CardActionType == ActionType.Distraction){
        // //         rng = i;
        // //         break;
        // //     }
        // // }

        SelectCard(rng);
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
