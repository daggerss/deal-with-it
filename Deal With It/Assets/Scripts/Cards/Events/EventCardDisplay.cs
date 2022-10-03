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
    private int[] selectedEventCards;

    // NPC 
    public NPCDisplay NPCDisplay;

    // Displays the current event card being played
    private Event currentEventCard;
    public TMP_Text CardTypeText, CardNameText, CardDescriptionText, EnergyText, JoyText, SadnessText, FearText, AngerText;
    public Image EnergyImage, JoyImage, SadnessImage, FearImage, AngerImage;
    public GameObject ThisObject;

    // Round Variables
    public GameObject RoundObject;
    private RoundController roundController;
    private int currentRound = -1;

    /* ----------------------------- Default Methods ---------------------------- */
    // Start is called before the first frame update
    void Start()
    {
        // Initializing the roundController variable to access the roundController from the roundController Object (kinda confusing to type it xd)
        roundController = (RoundController)RoundObject.GetComponent(typeof(RoundController));

        // Initialize the NPC
        NPCDisplay = (NPCDisplay)GameObject.FindGameObjectWithTag("NPC").GetComponent(typeof(NPCDisplay));

        // Updating array size of selectedEventCards to match the array size of the total number of EventCards
        Array.Resize(ref selectedEventCards, EventCard.Length);
        // Set all elements in selectedEventCards to -1 (Can't make it null)
        for(int i = 0; i < selectedEventCards.Length; i++){
            selectedEventCards[i] = -1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // If round is changed then select a new card from array
        if(roundController.Round != currentRound){
            ThisObject.SetActive(true);
            currentRound = roundController.Round;

            // Select New Card
            currentEventCard = EventCard[GetRandomCard()];
            //Format Text of New Card
            CardTypeText.text = currentEventCard.GetType().Name;
            CardNameText.text = currentEventCard.CardName;
            CardDescriptionText.text = currentEventCard.CardDescription;

            EnergyText.text = FormatText(currentEventCard.EnergyVal);
            EnergyImage.gameObject.SetActive(ShowImage(currentEventCard.EnergyVal));

            JoyText.text = FormatText(currentEventCard.JoyVal);
            JoyImage.gameObject.SetActive(ShowImage(currentEventCard.JoyVal));

            SadnessText.text = FormatText(currentEventCard.SadnessVal);
            SadnessImage.gameObject.SetActive(ShowImage(currentEventCard.SadnessVal));

            FearText.text = FormatText(currentEventCard.FearVal);
            FearImage.gameObject.SetActive(ShowImage(currentEventCard.FearVal));

            AngerText.text = FormatText(currentEventCard.AngerVal);
            AngerImage.gameObject.SetActive(ShowImage(currentEventCard.AngerVal));
        }
    }

    /* ----------------------------- Custom Methods ----------------------------- */
    // Returns random index of EventCards (Already programmed to not repeat cards until whole deck is used)
    private int GetRandomCard(){
        // Getting random number
        int output = UnityEngine.Random.Range(0, EventCard.Length);

        int IndexOfEmptyElement = Array.IndexOf(selectedEventCards, -1);

        // If selectedEventCards is not full
        if(IndexOfEmptyElement != -1){
            // Checking if the card has already been used
            while(Array.IndexOf(selectedEventCards, output) != -1){
                output = UnityEngine.Random.Range(0, EventCard.Length);
            }
        // selectedEventCards is full
        }else{
            // Clear selectedEventCards Array
            for(int i = 0; i < selectedEventCards.Length; i++){
                selectedEventCards[i] = -1;
            }
            IndexOfEmptyElement = Array.IndexOf(selectedEventCards, -1);
        }

        // Output
        selectedEventCards[IndexOfEmptyElement] = output;
        return output;
    }

    // Hides the card (Event Effects are applied here so that changes can be seen (?))
    public void HideCard(){
        ThisObject.SetActive(false);

        // Apply Event Effects
        NPCDisplay.ApplyEffect(NPCDisplay.LevelType.Energy, currentEventCard.EnergyVal);
        NPCDisplay.ApplyEffect(NPCDisplay.LevelType.Joy, currentEventCard.JoyVal);
        NPCDisplay.ApplyEffect(NPCDisplay.LevelType.Sadness, currentEventCard.SadnessVal);
        NPCDisplay.ApplyEffect(NPCDisplay.LevelType.Fear, currentEventCard.FearVal);
        NPCDisplay.ApplyEffect(NPCDisplay.LevelType.Anger, currentEventCard.AngerVal);

        // Debug.Log(NPCDisplay.npc.EnergyLvl);
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
