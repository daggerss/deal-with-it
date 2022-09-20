using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventCardDisplay : MonoBehaviour
{
    public Event[] EventCard;

    public Text NameText;

    // Round Variables
    public GameObject RoundObject;
    private RoundController RoundController;
    private int CurrentRound;

    // Start is called before the first frame update
    void Start()
    {
        RoundController = (RoundController)RoundObject.GetComponent(typeof(RoundController));
        CurrentRound = RoundController.Round;
    }

    // Update is called once per frame
    void Update()
    {
        // If round is changed then select a new card from array
        if(RoundController.Round != CurrentRound){
            // Select New Card
            CurrentRound = RoundController.Round;
            Debug.Log("Next Round From EventCardDisplay");
        }
    }
}
