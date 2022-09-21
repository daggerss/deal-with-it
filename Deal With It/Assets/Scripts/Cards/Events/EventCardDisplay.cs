using System; // For Array Controllers
using System.Linq; // For Count() method 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventCardDisplay : MonoBehaviour
{
    /* -------------------------------- Variables ------------------------------- */
    // Arrays storing event cards
    public Event[] EventCard;
    private int[] SelectedEventCards;

    // Displays the current event card being played
    private Event CurrentEventCard;
    public Text CurrentEventCardText;

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
            CurrentRound = RoundController.Round;

            // Select New Card
            CurrentEventCard = EventCard[GetRandomCard()];
            CurrentEventCardText.text = CurrentEventCard.CardName;
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
}
