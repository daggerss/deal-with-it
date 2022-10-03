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
    private int[] SelectedEventCards;

    // NPC 
    public NPCDisplay NPCDisplay;

    // Displays the current event card being played
    private Event CurrentEventCard;
    public TMP_Text CardTypeText, CardNameText, CardDescriptionText, EnergyText, JoyText, SadnessText, FearText, AngerText;
    public Image EnergyImage, JoyImage, SadnessImage, FearImage, AngerImage;
    public GameObject ThisObject;

    // Round Variables
    public GameObject RoundObject;
    private RoundController RoundController;
    private int CurrentRound = -1;

    /* ----------------------------- Default Methods ---------------------------- */
    // Start is called before the first frame update
    void Start()
    {
        // Initializing the RoundController variable to access the RoundController from the RoundController Object (kinda confusing to type it xd)
        RoundController = (RoundController)RoundObject.GetComponent(typeof(RoundController));

        // Initialize the NPC
        NPCDisplay = (NPCDisplay)GameObject.FindGameObjectWithTag("NPC").GetComponent(typeof(NPCDisplay));

        // Updating array size of SelectedEventCards to match the array size of the total number of EventCards
        Array.Resize(ref SelectedEventCards, EventCard.Length);
        // Set all elements in SelectedEventCards to -1 (Can't make it null)
        for(int i = 0; i < SelectedEventCards.Length; i++){
            SelectedEventCards[i] = -1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // If round is changed then select a new card from array
        if(RoundController.Round != CurrentRound){
            ThisObject.SetActive(true);
            CurrentRound = RoundController.Round;

            // Select New Card
            CurrentEventCard = EventCard[GetRandomCard()];
            //Format Text of New Card
            CardTypeText.text = CurrentEventCard.GetType().Name;
            CardNameText.text = CurrentEventCard.CardName;
            CardDescriptionText.text = CurrentEventCard.CardDescription;

            EnergyText.text = FormatText(CurrentEventCard.EnergyVal);
            EnergyImage.gameObject.SetActive(ShowImage(CurrentEventCard.EnergyVal));

            JoyText.text = FormatText(CurrentEventCard.JoyVal);
            JoyImage.gameObject.SetActive(ShowImage(CurrentEventCard.JoyVal));

            SadnessText.text = FormatText(CurrentEventCard.SadnessVal);
            SadnessImage.gameObject.SetActive(ShowImage(CurrentEventCard.SadnessVal));

            FearText.text = FormatText(CurrentEventCard.FearVal);
            FearImage.gameObject.SetActive(ShowImage(CurrentEventCard.FearVal));

            AngerText.text = FormatText(CurrentEventCard.AngerVal);
            AngerImage.gameObject.SetActive(ShowImage(CurrentEventCard.AngerVal));
        }
    }

    /* ----------------------------- Custom Methods ----------------------------- */
    // Returns random index of EventCards (Already programmed to not repeat cards until whole deck is used)
    private int GetRandomCard(){
        // Getting random number
        int output = UnityEngine.Random.Range(0, EventCard.Length);

        int IndexOfEmptyElement = Array.IndexOf(SelectedEventCards, -1);

        // If SelectedEventCards is not full
        if(IndexOfEmptyElement != -1){
            // Checking if the card has already been used
            while(Array.IndexOf(SelectedEventCards, output) != -1){
                output = UnityEngine.Random.Range(0, EventCard.Length);
            }
        // SelectedEventCards is full
        }else{
            // Clear SelectedEventCards Array
            for(int i = 0; i < SelectedEventCards.Length; i++){
                SelectedEventCards[i] = -1;
            }
            IndexOfEmptyElement = Array.IndexOf(SelectedEventCards, -1);
        }

        // Output
        SelectedEventCards[IndexOfEmptyElement] = output;
        return output;
    }

    // Hides the card (Event Effects are applied here so that changes can be seen (?))
    public void HideCard(){
        ThisObject.SetActive(false);

        // Apply Event Effects
        NPCDisplay.npc.EnergyLvl += CurrentEventCard.EnergyVal;
        // NPCDisplay.npc.JoyLvl += CurrentEventCard.JoyVal;
        // NPCDisplay.npc.SadnessLvl += CurrentEventCard.SadnessVal;
        // NPCDisplay.npc.FearLvl += CurrentEventCard.FearVal;
        // NPCDisplay.npc.AngerLvl += CurrentEventCard.AngerVal;

        Debug.Log(NPCDisplay.npc.EnergyLvl);
    }

    // Returns null if value is 0 (for printing values of energy etc.)
    private string FormatText(int Value){
        if(Value == 0){
            return null;
        }else{
            return Value.ToString();
        }
    }

    // Show or hide image
    private bool ShowImage(int Value){
        string EmotionPoints = FormatText(Value);

        if(EmotionPoints == null){
            return false;
        }else{
            return true;
        }
    }
}
