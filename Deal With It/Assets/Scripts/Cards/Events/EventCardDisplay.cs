using System; // For Array Controllers
using System.Linq; // For Count() method 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EventCardDisplay : MonoBehaviour
{
    /* -------------------------------- Variables ------------------------------- */
    // Arrays storing event cards
    public Event[] EventCard;
    private int[] _selectedEventCards;

    // NPC 
    public NPCDisplay npcDisplay;

    // Displays the current event card being played
    private Event _currentEventCard;
    private int _extraEventCards = 0;
    public TMP_Text CardTypeText, CardNameText, CardDescriptionText, EnergyText, JoyText, SadnessText, FearText, AngerText;
    public Image EnergyImage, JoyImage, SadnessImage, FearImage, AngerImage;
    public GameObject ThisObject;

    // Round Variables
    public GameObject RoundObject;
    private RoundController _roundController;
    private int _currentRound = -1;

    /* ----------------------------- Default Methods ---------------------------- */
    // Start is called before the first frame update
    void Start()
    {
        // Initializing the _roundController variable to access the _roundController from the _roundController Object (kinda confusing to type it xd)
        _roundController = (RoundController)RoundObject.GetComponent(typeof(RoundController));

        // Initialize the NPC
        npcDisplay = (NPCDisplay)GameObject.FindGameObjectWithTag("NPC").GetComponent(typeof(NPCDisplay));

        // Updating array size of _selectedEventCards to match the array size of the total number of EventCards
        Array.Resize(ref _selectedEventCards, EventCard.Length);
        // Set all elements in _selectedEventCards to -1 (Can't make it null)
        for(int i = 0; i < _selectedEventCards.Length; i++){
            _selectedEventCards[i] = -1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // If round is changed then select a new card from array
        if(_roundController.Round != _currentRound){
            _currentRound = _roundController.Round;

            DrawCard();
        }
    }

    /* ----------------------------- Custom Methods ----------------------------- */
    // Returns random index of EventCards (Already programmed to not repeat cards until whole deck is used)
    private int GetRandomCard(){
        // Getting random number
        int output = UnityEngine.Random.Range(0, EventCard.Length);

        int IndexOfEmptyElement = Array.IndexOf(_selectedEventCards, -1);

        // If _selectedEventCards is not full
        if(IndexOfEmptyElement != -1){
            // Checking if the card has already been used
            while(Array.IndexOf(_selectedEventCards, output) != -1){
                output = UnityEngine.Random.Range(0, EventCard.Length);
            }
        // _selectedEventCards is full
        }else{
            // Clear _selectedEventCards Array
            for(int i = 0; i < _selectedEventCards.Length; i++){
                _selectedEventCards[i] = -1;
            }
            IndexOfEmptyElement = Array.IndexOf(_selectedEventCards, -1);
        }

        // Output
        _selectedEventCards[IndexOfEmptyElement] = output;
        return output;
    }

    // Draw Card
    private void DrawCard(){
        ThisObject.SetActive(true);
        // Select New Card
        _currentEventCard = EventCard[GetRandomCard()];

        if(_currentEventCard.Randomize){
            // Clone current event card so that it doesn't change the original values
            Event tempCurrentEventCard = (Event) _currentEventCard.Clone();
            _currentEventCard = tempCurrentEventCard;

            // Randomize
            _currentEventCard.RandomVariation();
        }

        //Format Text of New Card
        CardTypeText.text = _currentEventCard.GetType().Name;
        CardNameText.text = _currentEventCard.CardName;
        CardDescriptionText.text = _currentEventCard.CardDescription;

        EnergyText.text = FormatText(_currentEventCard.EnergyVal);
        EnergyImage.gameObject.SetActive(ShowImage(_currentEventCard.EnergyVal));

        JoyText.text = FormatText(_currentEventCard.JoyVal);
        JoyImage.gameObject.SetActive(ShowImage(_currentEventCard.JoyVal));

        SadnessText.text = FormatText(_currentEventCard.SadnessVal);
        SadnessImage.gameObject.SetActive(ShowImage(_currentEventCard.SadnessVal));

        FearText.text = FormatText(_currentEventCard.FearVal);
        FearImage.gameObject.SetActive(ShowImage(_currentEventCard.FearVal));

        AngerText.text = FormatText(_currentEventCard.AngerVal);
        AngerImage.gameObject.SetActive(ShowImage(_currentEventCard.AngerVal));

        // Extra Event Cards
        _extraEventCards += _currentEventCard.ExtraEventCards;
    }

    // Hides and applies the card
    public void ApplyCard(){
        ThisObject.SetActive(false);

        // Apply Event Effects
        npcDisplay.ApplyEffect(LevelType.Energy, _currentEventCard.EnergyVal, ActionType.None);
        npcDisplay.ApplyEffect(LevelType.Joy, _currentEventCard.JoyVal, ActionType.None);
        npcDisplay.ApplyEffect(LevelType.Sadness, _currentEventCard.SadnessVal, ActionType.None);
        npcDisplay.ApplyEffect(LevelType.Fear, _currentEventCard.FearVal, ActionType.None);
        npcDisplay.ApplyEffect(LevelType.Anger, _currentEventCard.AngerVal, ActionType.None);

        if(_extraEventCards > 0){
            DrawCard();
            _extraEventCards--;
        }else{
            _roundController.NextPlayer();
        }
    }

    // Returns null if value is 0 (for printing values of energy etc.)
    private string FormatText(int value){
        if(value == 0){
            return null;
        }else if(value > 0){
            return "+" + value.ToString();
        }else{
            return value.ToString();
        }
    }

    // Show or hide image
    private bool ShowImage(int value){
        string EmotionPoints = FormatText(value);

        if(EmotionPoints == null){
            return false;
        }else{
            return true;
        }
    }
}
