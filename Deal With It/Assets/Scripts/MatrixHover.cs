using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MatrixHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // TODO: Change to more appropriate name (either this variable or this script)
    // * Leaning towards changing script/class to "BasicHover"
    public GameObject HoverPanel;

    public void OnPointerEnter(PointerEventData eventData){
        HoverPanel.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData){
        HoverPanel.SetActive(false);
    }
}
