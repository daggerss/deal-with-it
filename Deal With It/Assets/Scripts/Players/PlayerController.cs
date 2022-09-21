 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Array of cards in a player's hand (public if we're adding feature when other players can look into ur hand)
    public Action[] CardsInHand = new Action[5];

    // 
    private ActionCardDeck ActionCardDeck;

    // Start is called before the first frame update
    void Start()
    {
        
        ActionCardDeck = (ActionCardDeck)GameObject.FindGameObjectWithTag("Action Card Deck").GetComponent(typeof(ActionCardDeck));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
