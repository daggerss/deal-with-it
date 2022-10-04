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
    public NPCDisplay NPCDisplay;

    // Displays the current event card being played
    private Event _currentEventCard;
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
        NPCDisplay = (NPCDisplay)GameObject.FindGameObjectWithTag("NPC").GetComponent(typeof(NPCDisplay));

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
            ThisObject.SetActive(true);
            _currentRound = _roundController.Round;

            // Select New Card
            _currentEventCard = EventCard[GetRandomCard()];
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

    // Hides the card (Event Effects are applied here so that changes can be seen (?))
    public void HideCard(){
        ThisObject.SetActive(false);

        // Apply Event Effects
        NPCDisplay.npc.ApplyEffect(NPC.LevelType.Energy, _currentEventCard.EnergyVal);
        NPCDisplay.npc.ApplyEffect(NPC.LevelType.Joy, _currentEventCard.JoyVal);
        NPCDisplay.npc.ApplyEffect(NPC.LevelType.Sadness, _currentEventCard.SadnessVal);
        NPCDisplay.npc.ApplyEffect(NPC.LevelType.Fear, _currentEventCard.FearVal);
        NPCDisplay.npc.ApplyEffect(NPC.LevelType.Anger, _currentEventCard.AngerVal);
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
