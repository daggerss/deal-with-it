using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    /* ---------------------------- Card Description ---------------------------- */
    [SerializeField] 
    public List<ActionCard> deck = new List<ActionCard>();
    public List<ActionCard> discardPile = new List<ActionCard>();
    public List<ActionCard> playingCards = new List<ActionCard>();

    public Transform[] cardSlots;
    public bool[] availableCardSlots;
    public Transform[] playingSlots;
    public bool[] availablePlayingSlots;

    public void DrawCard()
    {
        if (deck.Count >=1 )
        {
            ActionCard randCard = deck[Random.Range(0, deck.Count)];

            for (int i = 0; i < availableCardSlots.Length; i++)
            {
                if (availableCardSlots[i] == true)
                {
                    randCard.gameObject.SetActive(true);
                    randCard.handIndex = i;
                    randCard.transform.position = cardSlots[i].position;
                    availableCardSlots[i] = false;
                    deck.Remove(randCard);
                    return;
                }
            }
        }
    }

    public void ConfirmCard()
    {
        foreach(ActionCard card in playingCards) 
        {
            // Affect NPC here 

            card.MoveToDiscardPile();
        }
    }

}
