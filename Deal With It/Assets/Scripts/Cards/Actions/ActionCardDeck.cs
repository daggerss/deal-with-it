using System; // For Array Controllers
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionCardDeck : MonoBehaviour
{
    public Action[] Deck;
    public Action[] UsedCards;

    void Start()
    {
        // Updating array size of UsedCards to match the array size of the total number of ActionCards
        Array.Resize(ref UsedCards, Deck.Length);
        // Set all elements in UsedCards to -1 (Can't make it null)
        for(int i = 0; i < UsedCards.Length; i++){
            UsedCards[i] = null;
        }
    }

    // Returns random index of ActionCards (Already programmed to not repeat cards until whole deck is used)
    public Action GetRandomCard(){
        // Getting random number
        int output = UnityEngine.Random.Range(0, Deck.Length);

        while(Deck[output] == null){
            output = UnityEngine.Random.Range(0, Deck.Length);
        }

        Action chosenCard = Deck[output];
        Deck[output] = null;
        return chosenCard;

        // // // Index of empty slot in UsedCards
        // // int IndexOfEmptyElement = Array.IndexOf(UsedCards, null);

        // // // If UsedCards is not full
        // // if(IndexOfEmptyElement != -1){
        // //     // Checking if the card has already been used
        // //     while(Array.IndexOf(UsedCards, Deck[output]) != -1){
        // //         output = UnityEngine.Random.Range(0, Deck.Length);
        // //     }
        // // // UsedCards is full
        // // }else{
        // //     // Clear UsedCards Array
        // //     for(int i = 0; i < UsedCards.Length; i++){
        // //         UsedCards[i] = Deck[i];
        // //         UsedCards[i] = null;
        // //     }
        // //     IndexOfEmptyElement = Array.IndexOf(UsedCards, -1);
        // // }

        // // // Output
        // // Action returnedCard = Deck[output];
        // // UsedCards[IndexOfEmptyElement] = returnedCard;
        // // Deck[output] = null;

        // // return returnedCard;
    }
}
