using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerCardHover : MonoBehaviour , IPointerEnterHandler, IPointerExitHandler
{
    Vector3 cardScale;
    Vector2 cardPosition;
    int cardIndexNumber;

    public PlayerController playerController;
    private RoundController roundController;

    void Start(){
        cardScale = this.transform.localScale;
        cardPosition = this.transform.position;
        cardIndexNumber = this.transform.GetSiblingIndex();

        roundController = (RoundController)GameObject.FindGameObjectWithTag("Round Controller").GetComponent(typeof(RoundController));
    }

    public void OnPointerEnter(PointerEventData eventData){
        if ((playerController.ActionCardProject == true) && (roundController.PlayerTurn != -1)){
            this.transform.localScale = new Vector3(1.5F, 1.5F, 1.5F);
            this.transform.position = new Vector2(this.transform.position.x, 350F);
            this.transform.SetSiblingIndex(5);
        }
    }

    public void OnPointerExit(PointerEventData eventData){
        this.transform.localScale = cardScale;
        this.transform.position = cardPosition;
        this.transform.SetSiblingIndex(cardIndexNumber);
    }
}