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
    private Vector3[] OriginalButtonRotation = new Vector3[5];
    public Button ConfirmButton;
    public Button SwapButton;
    public Button SkipTurnButton;
    private bool _showSkipTurnButton = false;

    //public bool[] ActionCardProject = new bool[]{true, true, true, true, true};
    public bool ActionCardProject = true;

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

    // Tooltip Texts: In Order Combos
    private string _prevIOStrategyText;
    private string _nextIOStrategyText;
    private string _energyInOrderEffectText;
    private string _joyInOrderEffectText;
    private string _sadnessInOrderEffectText;
    private string _fearInOrderEffectText;
    private string _angerInOrderEffectText;

    // Tooltip Texts: At Least Combos
    private string _ALStrategyText;
    private string _energyAtLeastEffectText;
    private string _joyAtLeastEffectText;
    private string _sadnessAtLeastEffectText;
    private string _fearAtLeastEffectText;
    private string _angerAtLeastEffectText;

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

        // Initializing OrginalButtonRotation
        for (int i = 0; i < OriginalButtonRotation.Length; i++){
            OriginalButtonRotation[i] = CardsInHandButton[i].transform.eulerAngles;
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
            }else if (!_showSkipTurnButton){
                SkipTurnButton.interactable = true;
                _showSkipTurnButton = true;
            }
        }else{
            // So player can't play card when it's not their turn
            SkipTurnButton.interactable = false;
            ConfirmButton.interactable = false;
            SwapButton.interactable = false;
        }
    }

    /* ----------------------------- Custom Methods ----------------------------- */

    // Select Card
    public void SelectCard(int CardIndex){
        if(RoundController.PlayerTurn == PlayerNumber){ // You can only select card when it is your turn
            // If you select a different card decrease the count
            if(SelectedCard != -1 && SelectedCard != CardIndex){
                PlayedActionCards.RemoveActionType(CardsInHand[SelectedCard].CardActionType);
                CardsInHand[SelectedCard].Revert();
                CardsInHandButton[SelectedCard].gameObject.SetActive(true);
                ActionCardProject = false;
                
                Action projectedCard = CardsInHand[SelectedCard];
                PlayedActionCards.RemoveCard(projectedCard);

                Button projectedButton = CardsInHandButton[SelectedCard];
                projectedButton.transform.position = OriginalButtonPosition[SelectedCard];
                projectedButton.transform.localScale = OriginalButtonSize[SelectedCard]; 
                projectedButton.transform.eulerAngles = OriginalButtonRotation[SelectedCard];
            }

            // If a player clicks on a card twice it will deselect the card
            if(CardIndex == SelectedCard){
                // Hide confirm button
                SkipTurnButton.interactable = true;
                ConfirmButton.interactable = false;
                SwapButton.interactable = false;

                // Reset the card to original
                if (SelectedCard >= 0 && SelectedCard < CardsInHand.Length)
                {
                    CardsInHandButton[SelectedCard].gameObject.SetActive(true);
                    ActionCardProject = true;
                    PlayedActionCards.RemoveActionType(CardsInHand[SelectedCard].CardActionType);
                    PlayedActionCards.RevertAll();
                    CardsInHand[SelectedCard].Revert();
                    Action projectedCard = CardsInHand[SelectedCard];
                    PlayedActionCards.RemoveCard(projectedCard);


                    Button projectedButton = CardsInHandButton[SelectedCard];
                    projectedButton.transform.position = OriginalButtonPosition[SelectedCard];
                    projectedButton.transform.localScale = OriginalButtonSize[SelectedCard]; 
                    projectedButton.transform.eulerAngles = OriginalButtonRotation[SelectedCard];
                }
                // Deselect
                SelectedCard = -1;
            // If a player clicks on a card slot with a value inside
            }else if(CardsInHand[CardIndex] != null){
                // Show confirm button
                SkipTurnButton.interactable = false;
                ConfirmButton.interactable = true;
                SwapButton.interactable = true;

                SelectedCard = CardIndex;
                CardsInHandButton[SelectedCard].gameObject.SetActive(true);
                ActionCardProject = true;

                // Apply NPC trait effects
                Action projectedCard = CardsInHand[SelectedCard];
                ActionType cardActionType = projectedCard.CardActionType;

                // Project on PlayedActionCards
                PlayedActionCards.AddCard(projectedCard);

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

                // Update tooltips content: NPC Traits
                projectedCard.EnergyTraitEffectText = _energyTraitEffectText;
                projectedCard.JoyTraitEffectText = _joyTraitEffectText;
                projectedCard.SadnessTraitEffectText = _sadnessTraitEffectText;
                projectedCard.FearTraitEffectText = _fearTraitEffectText;
                projectedCard.AngerTraitEffectText = _angerTraitEffectText;

                // Save trait effects of the card on PlayedActionCards
                PlayedActionCards.SaveCard(projectedCard);

                // Add current card type to the card count
                PlayedActionCards.AddActionType(cardActionType);

                // Set all PlayedActionCards to original value before applying new effects
                PlayedActionCards.RevertAll();

                // Apply Combo effects
                projectedCard.EnergyVal = PlayedActionCards.ProjectComboEffect(LevelType.Energy, projectedCard.EnergyVal, cardActionType);
                _energyInOrderEffectText = PlayedActionCards.InOrderEffectText;
                _energyAtLeastEffectText = PlayedActionCards.AtLeastEffectText;
                
                projectedCard.JoyVal = PlayedActionCards.ProjectComboEffect(LevelType.Joy, projectedCard.JoyVal, cardActionType);
                _joyInOrderEffectText = PlayedActionCards.InOrderEffectText;
                _joyAtLeastEffectText = PlayedActionCards.AtLeastEffectText;
                
                projectedCard.SadnessVal = PlayedActionCards.ProjectComboEffect(LevelType.Sadness, projectedCard.SadnessVal, cardActionType);
                _sadnessInOrderEffectText = PlayedActionCards.InOrderEffectText;
                _sadnessAtLeastEffectText = PlayedActionCards.AtLeastEffectText;
                
                projectedCard.FearVal = PlayedActionCards.ProjectComboEffect(LevelType.Fear, projectedCard.FearVal, cardActionType);
                _fearInOrderEffectText = PlayedActionCards.InOrderEffectText;
                _fearAtLeastEffectText = PlayedActionCards.AtLeastEffectText;
                
                projectedCard.AngerVal = PlayedActionCards.ProjectComboEffect(LevelType.Anger, projectedCard.AngerVal, cardActionType);
                _angerInOrderEffectText = PlayedActionCards.InOrderEffectText;
                _angerAtLeastEffectText = PlayedActionCards.AtLeastEffectText;

                // Combo order for tooltips
                _prevIOStrategyText = PlayedActionCards.PrevIOStrategyText;
                _nextIOStrategyText = PlayedActionCards.NextIOStrategyText;
                _ALStrategyText = PlayedActionCards.ALStrategyText;

                // Check if values were canceled out
                projectedCard.UpdateValueCanceled();

                // Update tooltips content: In Order
                projectedCard.PrevIOStrategyText = _prevIOStrategyText;
                projectedCard.NextIOStrategyText = _nextIOStrategyText;
                projectedCard.EnergyInOrderEffectText = _energyInOrderEffectText;
                projectedCard.JoyInOrderEffectText = _joyInOrderEffectText;
                projectedCard.SadnessInOrderEffectText = _sadnessInOrderEffectText;
                projectedCard.FearInOrderEffectText = _fearInOrderEffectText;
                projectedCard.AngerInOrderEffectText = _angerInOrderEffectText;

                // Update tooltips content: At Least
                projectedCard.ALStrategyText = _ALStrategyText;
                projectedCard.EnergyAtLeastEffectText = _energyAtLeastEffectText;
                projectedCard.JoyAtLeastEffectText = _joyAtLeastEffectText;
                projectedCard.SadnessAtLeastEffectText = _sadnessAtLeastEffectText;
                projectedCard.FearAtLeastEffectText = _fearAtLeastEffectText;
                projectedCard.AngerAtLeastEffectText = _angerAtLeastEffectText;

            // If a player clicks on a card slot with no value inside
            }else{
                // Do nothing
            }

            // ! LEGACY hand of cards sliding up only
            // // Flare for selected card and non-flare for non selected cards
            // // for(int i = 0; i < CardsInHandButton.Length; i++){
            // //     Button currentButton = CardsInHandButton[i];
            // //     if(SelectedCard == i){
            // //         for (int j = 1; j < 6; j++){
            // //             if (RoundController.PlayerTurn == j){
            // //                 ActionCardProject = true;
            // //                 currentButton.transform.eulerAngles = new Vector3(0.0F, 0.0F,0.0F);
            // //                 currentButton.transform.localScale = new Vector3 (0.7F, 0.7F, 0.7F);
            // //                 currentButton.transform.position = new Vector2(PlayedActionCards.PlayedActionCardsButton[j-1].transform.position.x + 272F, PlayedActionCards.PlayedActionCardsButton[j-1].transform.position.y);
            // //             }
            // //         }
            // //     }else{
            // //         currentButton.transform.position = OriginalButtonPosition[i];
            // //         currentButton.transform.localScale = OriginalButtonSize[i]; 
            // //         currentButton.transform.eulerAngles = OriginalButtonRotation[i];
            // // Reset the card to original
            // //         CardsInHand[i].Revert();
            // //     }
            // // }
        }
    }

    // Play Card
    public void PlayCard(){
        // Hide confirm button
        SkipTurnButton.interactable = true;
        ConfirmButton.interactable = false;
        SwapButton.interactable = false;

        // No card picked
        if(SelectedCard == -1){
            Debug.Log("Player " + PlayerNumber + " played no card");
        }else{
            Action playedActionCard = CardsInHand[SelectedCard];

            // Save playedActionCard values to the PlayedActionCards
            PlayedActionCards.SaveCard(playedActionCard);

            Debug.Log("Player " + PlayerNumber + " played " + playedActionCard.CardActionType);

            // Remove card from hand
            CardsInHand[SelectedCard] = null;
            CardsInHandButton[SelectedCard].gameObject.SetActive(false);

            // Move played card button back to the original position
            CardsInHandButton[SelectedCard].transform.position = OriginalButtonPosition[SelectedCard];
        }

        RoundController.NextPlayer();
    }

    public void SwapCard(){
        // Hide swap button
        SkipTurnButton.interactable = true;
        ConfirmButton.interactable = false;
        SwapButton.interactable = false;

        // No card picked
        if(SelectedCard == -1){
            Debug.Log("Player " + PlayerNumber + " played no card");
        }else{
            Debug.Log("Player " + PlayerNumber + " swapped " + CardsInHand[SelectedCard].CardActionType);

            // Remove card from hand
            ActionCardDeck.PutCardBack(CardsInHand[SelectedCard]);
            CardsInHand[SelectedCard] = null;
            CardsInHandButton[SelectedCard].gameObject.SetActive(false);

            // Move played card button back to the original position
            CardsInHandButton[SelectedCard].transform.position = OriginalButtonPosition[SelectedCard];
        }

        RoundController.NextPlayer();
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
