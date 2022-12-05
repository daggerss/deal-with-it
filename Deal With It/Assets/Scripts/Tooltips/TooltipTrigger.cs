using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [field: SerializeField] public TooltipType Type {get; set;}
    [field: SerializeField] public string Heading {get; set;}
    [field: SerializeField] public string PrevStrat {get; set;}
    [field: SerializeField] public string NextStrat {get; set;}

    [field: SerializeField] [TextArea(1,5)]
    private string _content;
    public string Content {get; set;}

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Delay
        Invoke("Trigger", 0.25f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipSystem.Hide(Type);
    }

    // Hide also when object is deactivated
    private void OnDisable()
    {
        TooltipSystem.Hide(Type);
    }

    private void Trigger()
    {
        TooltipSystem.Show(Type, Content, Heading, PrevStrat, NextStrat);
    }
}
