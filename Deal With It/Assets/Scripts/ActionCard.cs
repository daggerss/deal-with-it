using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionCard : MonoBehaviour
{
    // /* -------------------------------- Card Name ------------------------------- */
    // // [SerializeField]
    // // private string _CardName;
    // // public string CardName => _CardName;

    // // /* ---------------------------- Card Description ---------------------------- */
    // // [SerializeField] 
    // // [TextArea(1, 3)]
    // // private string _CardDescription;
    // // public string CardDescription => _CardDescription;

    // /* ------------------------------- Test things ------------------------------ */
    // // public Card(string CardName, string CardDescription){
    // //     _CardName = CardName;
    // //     _CardDescription = CardDescription;
    // // }

    // /* ---------------------------- Card Use ---------------------------- */
    // [SerializeField]
    public bool hasBeenPlayed;
    public int handIndex;
    private GameManager gm;
    
    private void Start()
    {
        gm = FindObjectOfType<GameManager>();
    }

    public void ChooseCard()
    {
        if(hasBeenPlayed == false)
        {
            transform.position += Vector3.up * 35;
            hasBeenPlayed = true;
            gm.availableCardSlots[handIndex] = true;
            Invoke("MoveToCardPlayed", 2f);
        }
    }

    void MoveToCardPlayed()
    {
        gm.playingCards.Add(this);
        for(int i = 0; i < gm.availablePlayingSlots.Length; i++)
        {
            if(gm.availablePlayingSlots[i] == true)
            {
                transform.position = gm.playingSlots[i].position;
                gm.availablePlayingSlots[i] = false;
                return;
            }
        }
    }

    public void MoveToDiscardPile()
    {
        gm.discardPile.Add(this);
        gameObject.SetActive(false);
    }

    // public void Info(){
    //     Debug.Log("Card Name: " + _CardName);
    //     Debug.Log("Card Description: " + _CardDescription);
    // }
}
