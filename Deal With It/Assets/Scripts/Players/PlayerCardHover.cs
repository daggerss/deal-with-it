using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerCardHover : MonoBehaviour , IPointerEnterHandler, IPointerExitHandler
{
    Vector2 resolution;

    Vector3 originalScale;
    Vector2 originalPosition;
    int originalIndex;

    public PlayerController playerController;
    private RoundController roundController;

    void Start(){
        StartCoroutine(OnScreenChange());
        originalIndex = this.transform.GetSiblingIndex();

        roundController = (RoundController)GameObject.FindGameObjectWithTag("Round Controller").GetComponent(typeof(RoundController));
    }

    // Resave positions when screen resolution changes
    IEnumerator OnScreenChange()
    {
        SaveDimensions();

        while (true)
        {
            if (resolution.x != Screen.width || resolution.y != Screen.height)
            {
                SaveDimensions();
            }

            yield return new WaitForSeconds(0.5f);
        }
    }

    // Save screen resolution, card scale, and card position
    private void SaveDimensions()
    {
        resolution = new Vector2(Screen.width, Screen.height);
        originalScale = this.transform.localScale;
        originalPosition = this.transform.position;
    }

    public void OnPointerEnter(PointerEventData eventData){
        if ((playerController.ActionCardProject == true) && (roundController.PlayerTurn != -1)){
            this.transform.localScale = new Vector3(1.5F, 1.5F, 1.5F);
            this.transform.position = new Vector2(this.transform.position.x, Screen.height / 3);
            this.transform.SetSiblingIndex(5);
        }
    }

    public void OnPointerExit(PointerEventData eventData){
        this.transform.localScale = originalScale;
        this.transform.position = originalPosition;
        this.transform.SetSiblingIndex(originalIndex);
    }
}