using System; // For Array Controllers
using System.Linq; // For Count() method
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EventCardDisplay : CardDisplay
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

    // Arrows UI
    public Image EnergyArrowUpImage, EnergyArrowDownImage;
    public Image JoyArrowUpImage, JoyArrowDownImage;
    public Image SadnessArrowUpImage, SadnessArrowDownImage;
    public Image FearArrowUpImage, FearArrowDownImage;
    public Image AngerArrowUpImage, AngerArrowDownImage;

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

            // Replenish energy
            // If negative
            if(npcDisplay.npc.EnergyLvl < 0){
                npcDisplay.npc.EnergyLvl += 20;

            // If not negative but less than 20
            }else if(npcDisplay.npc.EnergyLvl < 20)
            {
                npcDisplay.npc.EnergyLvl = 20;

            // 20 or more
            }else{
                // Nothing
            }

            npcDisplay.ApplyEffect(LevelType.Energy, 0);

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

        // Randomize card's emotion values
        _currentEventCard.SetRandomEmotions();

        if(_currentEventCard.Randomize){
            // Clone current event card so that it doesn't change the original values
            Event tempCurrentEventCard = (Event) _currentEventCard.Clone();
            _currentEventCard = tempCurrentEventCard;

            // Randomize
            _currentEventCard.RandomVariation();
        }

        _currentEventCard.SaveValues();

        // Apply NPC trait effects
        _currentEventCard.EnergyVal = npcDisplay.ProjectTraitEffect(LevelType.Energy, _currentEventCard.EnergyVal, ActionType.None);
        _currentEventCard.JoyVal = npcDisplay.ProjectTraitEffect(LevelType.Joy, _currentEventCard.JoyVal, ActionType.None);
        _currentEventCard.SadnessVal = npcDisplay.ProjectTraitEffect(LevelType.Sadness, _currentEventCard.SadnessVal, ActionType.None);
        _currentEventCard.FearVal = npcDisplay.ProjectTraitEffect(LevelType.Fear, _currentEventCard.FearVal, ActionType.None);
        _currentEventCard.AngerVal = npcDisplay.ProjectTraitEffect(LevelType.Anger, _currentEventCard.AngerVal, ActionType.None);

        //Format Text of New Card
        CardTypeText.text = _currentEventCard.GetType().Name;
        CardNameText.text = _currentEventCard.CardName;
        CardDescriptionText.text = _currentEventCard.CardDescription;

        EnergyText.text = FormatText(_currentEventCard.EnergyVal, _currentEventCard.EnergyOriginalVal);
        EnergyImage.gameObject.SetActive(ShowImage(_currentEventCard.EnergyVal, _currentEventCard.EnergyOriginalVal));

        JoyText.text = FormatText(_currentEventCard.JoyVal, _currentEventCard.JoyOriginalVal);
        JoyImage.gameObject.SetActive(ShowImage(_currentEventCard.JoyVal, _currentEventCard.JoyOriginalVal));

        SadnessText.text = FormatText(_currentEventCard.SadnessVal, _currentEventCard.SadnessOriginalVal);
        SadnessImage.gameObject.SetActive(ShowImage(_currentEventCard.SadnessVal, _currentEventCard.SadnessOriginalVal));

        FearText.text = FormatText(_currentEventCard.FearVal, _currentEventCard.FearOriginalVal);
        FearImage.gameObject.SetActive(ShowImage(_currentEventCard.FearVal, _currentEventCard.FearOriginalVal));

        AngerText.text = FormatText(_currentEventCard.AngerVal, _currentEventCard.AngerOriginalVal);
        AngerImage.gameObject.SetActive(ShowImage(_currentEventCard.AngerVal, _currentEventCard.AngerOriginalVal));

        EnergyArrowUpImage.enabled = (_currentEventCard.EnergyValChangeDir == 0);
        JoyArrowUpImage.enabled = (_currentEventCard.JoyValChangeDir == 0);
        SadnessArrowUpImage.enabled = (_currentEventCard.SadnessValChangeDir == 0);
        FearArrowUpImage.enabled = (_currentEventCard.FearValChangeDir == 0);
        AngerArrowUpImage.enabled = (_currentEventCard.AngerValChangeDir == 0);

        EnergyArrowDownImage.enabled = (_currentEventCard.EnergyValChangeDir == 1);
        JoyArrowDownImage.enabled = (_currentEventCard.JoyValChangeDir == 1);
        SadnessArrowDownImage.enabled = (_currentEventCard.SadnessValChangeDir == 1);
        FearArrowDownImage.enabled = (_currentEventCard.FearValChangeDir == 1);
        AngerArrowDownImage.enabled = (_currentEventCard.AngerValChangeDir == 1);

        // Extra Event Cards
        _extraEventCards += _currentEventCard.ExtraEventCards;
    }

    // Hides and applies the card
    public void ApplyCard(){
        ThisObject.SetActive(false);

        // Apply Event Effects
        npcDisplay.ApplyEffect(LevelType.Energy, _currentEventCard.EnergyVal);
        npcDisplay.ApplyEffect(LevelType.Joy, _currentEventCard.JoyVal);
        npcDisplay.ApplyEffect(LevelType.Sadness, _currentEventCard.SadnessVal);
        npcDisplay.ApplyEffect(LevelType.Fear, _currentEventCard.FearVal);
        npcDisplay.ApplyEffect(LevelType.Anger, _currentEventCard.AngerVal);

        _currentEventCard.Revert();

        if(_extraEventCards > 0){
            DrawCard();
            _extraEventCards--;
        }else{
            _roundController.NextPlayer();
        }
    }
}
